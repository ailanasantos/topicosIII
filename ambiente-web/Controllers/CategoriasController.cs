using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ambiente_web.Data;
using ambiente_web.Models;

namespace ambiente_web.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.Include(c => c.Produtos).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            ModelState.Remove("Produtos");
            if (ModelState.IsValid)
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoria categoria)
        {
            if (id != categoria.Id) return NotFound();

            ModelState.Remove("Produtos");
            if (ModelState.IsValid)
            {
                var existing = await _context.Categorias.FindAsync(id);
                if (existing == null) return NotFound();

                existing.Nome = categoria.Nome;
                existing.Descricao = categoria.Descricao;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}