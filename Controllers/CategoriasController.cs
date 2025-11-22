using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTrack.Data;
using FinTrack.Models;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
=======
using System.Collections.Generic;
>>>>>>> backup-minhas-alteracoes
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace FinTrack.Controllers
{
<<<<<<< HEAD
    [Authorize] // Exige login para acessar qualquer ação
=======
>>>>>>> backup-minhas-alteracoes
    public class CategoriasController : Controller
    {
        private readonly FinTrackContext _context;

        public CategoriasController(FinTrackContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
        // INDEX -----------------------------------------------------------
        // Lista categorias com filtro opcional por Tipo (Receita/Despesa)
        public async Task<IActionResult> Index(int? tipoId)
        {
            // Lista de Tipos (Enum) para o filtro dropdown
=======
        // INDEX
        public async Task<IActionResult> Index(int? tipoId)
        {
>>>>>>> backup-minhas-alteracoes
            ViewData["Tipos"] = Enum.GetValues(typeof(TipoCategoria))
                .Cast<TipoCategoria>()
                .Select(t => new SelectListItem
                {
                    Value = ((int)t).ToString(),
                    Text = t.ToString()
                }).ToList();

            var categorias = _context.Categorias.AsQueryable();

<<<<<<< HEAD
            // Aplica o filtro se o tipoId for válido
=======
>>>>>>> backup-minhas-alteracoes
            if (tipoId.HasValue && Enum.IsDefined(typeof(TipoCategoria), tipoId.Value))
            {
                var tipoFiltro = (TipoCategoria)tipoId.Value;
                categorias = categorias.Where(c => c.Tipo == tipoFiltro);

                ViewData["SelectedTipoId"] = tipoId.Value;
            }

            return View(await categorias.ToListAsync());
        }

<<<<<<< HEAD
        // DETAILS ---------------------------------------------------------
=======
        // DETAILS (GET)

>>>>>>> backup-minhas-alteracoes
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
<<<<<<< HEAD
                .FirstOrDefaultAsync(c => c.Id == id);
=======
                .FirstOrDefaultAsync(m => m.Id == id);
>>>>>>> backup-minhas-alteracoes

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

<<<<<<< HEAD
        // CREATE (GET) ----------------------------------------------------
=======
        // CREATE (GET)

>>>>>>> backup-minhas-alteracoes
        public IActionResult Create()
        {
            return View();
        }

<<<<<<< HEAD
        // CREATE (POST) ---------------------------------------------------
=======
        // CREATE (POST)

>>>>>>> backup-minhas-alteracoes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Tipo")] Categoria categoria)
        {
<<<<<<< HEAD
            // Verificação de duplicidade Nome + Tipo
=======
>>>>>>> backup-minhas-alteracoes
            var existeDuplicata = await _context.Categorias
                .AnyAsync(c => c.Nome == categoria.Nome && c.Tipo == categoria.Tipo);

            if (existeDuplicata)
            {
<<<<<<< HEAD
                ModelState.AddModelError("Nome", "Já existe uma categoria com este Nome e Tipo.");
=======
                ModelState.AddModelError("Nome", "Já existe uma categoria com este Nome e Tipo (Receita/Despesa).");
>>>>>>> backup-minhas-alteracoes
            }

            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

<<<<<<< HEAD
        // EDIT (GET) ------------------------------------------------------
=======
        // EDIT (GET)

>>>>>>> backup-minhas-alteracoes
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
<<<<<<< HEAD

            return View(categoria);
        }

        // EDIT (POST) -----------------------------------------------------
=======
            return View(categoria);
        }

        // EDIT (POST)

>>>>>>> backup-minhas-alteracoes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Tipo")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

<<<<<<< HEAD
            // Verificação de duplicidade Nome + Tipo no Edit
            var existeDuplicata = await _context.Categorias
                .AnyAsync(c => c.Id != categoria.Id &&
                               c.Nome == categoria.Nome &&
                               c.Tipo == categoria.Tipo);

            if (existeDuplicata)
            {
                ModelState.AddModelError("Nome", "Já existe uma categoria com este Nome e Tipo.");
            }

=======
>>>>>>> backup-minhas-alteracoes
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
<<<<<<< HEAD

                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // DELETE (GET) ----------------------------------------------------
=======
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // DELETE (GET)

>>>>>>> backup-minhas-alteracoes
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
<<<<<<< HEAD
                .FirstOrDefaultAsync(c => c.Id == id);
=======
                .FirstOrDefaultAsync(m => m.Id == id);
>>>>>>> backup-minhas-alteracoes

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

<<<<<<< HEAD
        // DELETE (POST) ---------------------------------------------------
=======
        // DELETE (POST)

>>>>>>> backup-minhas-alteracoes
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
<<<<<<< HEAD

            return RedirectToAction(nameof(Index));
        }
    }
}
=======
            return RedirectToAction(nameof(Index));
        }
    }
}
>>>>>>> backup-minhas-alteracoes
