using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace FinTrack.Controllers
{
    [Authorize] // Exige login para acessar qualquer ação
    public class CategoriasController : Controller
    {
        private readonly FinTrackContext _context;

        public CategoriasController(FinTrackContext context)
        {
            _context = context;
        }

        // INDEX -----------------------------------------------------------
        // Lista categorias com filtro opcional por Tipo (Receita/Despesa)
        public async Task<IActionResult> Index(int? tipoId)
        {
            // Lista de Tipos (Enum) para o filtro dropdown
            ViewData["Tipos"] = Enum.GetValues(typeof(TipoCategoria))
                .Cast<TipoCategoria>()
                .Select(t => new SelectListItem
                {
                    Value = ((int)t).ToString(),
                    Text = t.ToString()
                }).ToList();

            var categorias = _context.Categorias.AsQueryable();

            // Aplica o filtro se o tipoId for válido
            if (tipoId.HasValue && Enum.IsDefined(typeof(TipoCategoria), tipoId.Value))
            {
                var tipoFiltro = (TipoCategoria)tipoId.Value;
                categorias = categorias.Where(c => c.Tipo == tipoFiltro);

                ViewData["SelectedTipoId"] = tipoId.Value;
            }

            return View(await categorias.ToListAsync());
        }

        // DETAILS ---------------------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // CREATE (GET) ----------------------------------------------------
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST) ---------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Tipo")] Categoria categoria)
        {
            // Verificação de duplicidade Nome + Tipo
            var existeDuplicata = await _context.Categorias
                .AnyAsync(c => c.Nome == categoria.Nome && c.Tipo == categoria.Tipo);

            if (existeDuplicata)
            {
                ModelState.AddModelError("Nome", "Já existe uma categoria com este Nome e Tipo.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // EDIT (GET) ------------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // EDIT (POST) -----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Tipo")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            // Verificação de duplicidade Nome + Tipo no Edit
            var existeDuplicata = await _context.Categorias
                .AnyAsync(c => c.Id != categoria.Id &&
                               c.Nome == categoria.Nome &&
                               c.Tipo == categoria.Tipo);

            if (existeDuplicata)
            {
                ModelState.AddModelError("Nome", "Já existe uma categoria com este Nome e Tipo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categorias.Any(e => e.Id == categoria.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // DELETE (GET) ----------------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // DELETE (POST) ---------------------------------------------------
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
