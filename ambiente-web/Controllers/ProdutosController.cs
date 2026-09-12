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
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .ToListAsync();
            return View(produtos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null) return NotFound();

            return View(produto);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoriaId"] = new SelectList(await _context.Categorias.ToListAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            ModelState.Remove("Categoria");
            if (ModelState.IsValid)
            {
                produto.Categoria = null;
                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(await _context.Categorias.ToListAsync(), "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null) return NotFound();

            ViewData["CategoriaId"] = new SelectList(await _context.Categorias.ToListAsync(), "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto)
        {
            if (id != produto.Id) return NotFound();

            ModelState.Remove("Categoria");
            if (ModelState.IsValid)
            {
                var existing = await _context.Produtos.FindAsync(id);
                if (existing == null) return NotFound();

                existing.Nome = produto.Nome;
                existing.Descricao = produto.Descricao;
                existing.Preco = produto.Preco;
                existing.Estoque = produto.Estoque;
                existing.EstoqueMinimo = produto.EstoqueMinimo;
                existing.CodigoBarras = produto.CodigoBarras;
                existing.CategoriaId = produto.CategoriaId;
                existing.Ativo = produto.Ativo;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(await _context.Categorias.ToListAsync(), "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null) return NotFound();

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}