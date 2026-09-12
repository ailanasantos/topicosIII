using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ambiente_web.Data;
using ambiente_web.Models;

namespace ambiente_web.Controllers
{
    public class VendasController : Controller
    {
        private readonly AppDbContext _context;

        public VendasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vendas = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                .OrderByDescending(v => v.DataVenda)
                .ToListAsync();
            return View(vendas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                    .ThenInclude(iv => iv.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null) return NotFound();

            return View(venda);
        }

        public async Task<IActionResult> Pdv()
        {
            ViewBag.Produtos = await _context.Produtos
                .Where(p => p.Ativo && p.Estoque > 0)
                .OrderBy(p => p.Nome)
                .ToListAsync();
            ViewBag.Clientes = await _context.Clientes
                .OrderBy(c => c.Nome)
                .ToListAsync();
            return View(new Venda());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarVenda(int? clienteId, string formaPagamento, string itensJson)
        {
            if (string.IsNullOrEmpty(itensJson))
            {
                return BadRequest("Nenhum item na venda.");
            }

            var itens = System.Text.Json.JsonSerializer.Deserialize<List<ItemVendaInput>>(itensJson);
            if (itens == null || !itens.Any())
            {
                return BadRequest("Nenhum item na venda.");
            }

            var venda = new Venda
            {
                DataVenda = DateTime.Now,
                ClienteId = clienteId,
                FormaPagamento = formaPagamento ?? "Dinheiro",
                Status = "Concluída",
                Total = 0
            };

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            decimal total = 0;
            foreach (var item in itens)
            {
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto == null) continue;

                var itemVenda = new ItemVenda
                {
                    VendaId = venda.Id,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = produto.Preco
                };

                produto.Estoque -= item.Quantidade;
                _context.Produtos.Update(produto);

                total += itemVenda.Subtotal;
                _context.ItensVenda.Add(itemVenda);
            }

            venda.Total = total;
            _context.Vendas.Update(venda);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = venda.Id });
        }

        public async Task<IActionResult> Relatorios()
        {
            var hoje = DateTime.Today;
            var mesAtual = new DateTime(hoje.Year, hoje.Month, 1);

            ViewBag.VendasHoje = await _context.Vendas
                .Where(v => v.DataVenda.Date == hoje && v.Status == "Concluída")
                .CountAsync();
            ViewBag.TotalHoje = await _context.Vendas
                .Where(v => v.DataVenda.Date == hoje && v.Status == "Concluída")
                .SumAsync(v => v.Total);
            ViewBag.VendasMes = await _context.Vendas
                .Where(v => v.DataVenda >= mesAtual && v.Status == "Concluída")
                .CountAsync();
            ViewBag.TotalMes = await _context.Vendas
                .Where(v => v.DataVenda >= mesAtual && v.Status == "Concluída")
                .SumAsync(v => v.Total);
            ViewBag.TotalProdutos = await _context.Produtos.CountAsync();
            ViewBag.ProdutosEstoqueBaixo = await _context.Produtos
                .Where(p => p.Ativo && p.Estoque <= p.EstoqueMinimo)
                .CountAsync();
            ViewBag.TotalClientes = await _context.Clientes.CountAsync();

            var vendasRecentes = await _context.Vendas
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.DataVenda)
                .Take(10)
                .ToListAsync();
            ViewBag.VendasRecentes = vendasRecentes;

            var produtosMaisVendidos = await _context.ItensVenda
                .Include(iv => iv.Produto)
                .GroupBy(iv => iv.Produto.Nome)
                .Select(g => new { Nome = g.Key, Quantidade = g.Sum(iv => iv.Quantidade) })
                .OrderByDescending(x => x.Quantidade)
                .Take(10)
                .ToListAsync();
            ViewBag.ProdutosMaisVendidos = produtosMaisVendidos;

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas
                .Include(v => v.Itens)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda != null)
            {
                foreach (var item in venda.Itens)
                {
                    var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                    if (produto != null)
                    {
                        produto.Estoque += item.Quantidade;
                        _context.Produtos.Update(produto);
                    }
                }

                _context.ItensVenda.RemoveRange(venda.Itens);
                _context.Vendas.Remove(venda);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

    public class ItemVendaInput
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}