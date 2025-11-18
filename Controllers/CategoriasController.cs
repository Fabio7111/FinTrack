using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTrack.Data;
using FinTrack.Models;

namespace FinTrack.Controllers
{
    // Classe principal do Controller
    public class CategoriasController : Controller
    {
        private readonly FinTrackContext _context;

        // Construtor com injeção de dependência
        public CategoriasController(FinTrackContext context)
        {
            _context = context;
        }

        // GET: Categorias (Lista de Categorias)
        public async Task<IActionResult> Index()
        {
            // Busca a lista de forma assíncrona
            return View(await _context.Categorias.ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca a categoria no banco
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Tipo")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
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

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Tipo")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
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

        // GET: Categorias/Delete/5 (Confirmação)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca a categoria para exibir na tela de confirmação
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5 (Execução da Exclusão)
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