using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Controllers
{
    [Authorize]
    public class TiposPagamentoController(FinTrackContext context) : Controller
    {
        private readonly FinTrackContext _context = context;

        // INDEX -------------------------------------------------------

        public async Task<IActionResult> Index()
        {
            var tiposPagamento = await _context.TiposPagamento
                .OrderBy(tp => tp.Nome)
                .ToListAsync();

            return View(tiposPagamento);
        }

        // CREATE (GET) -----------------------------------------------

        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST) ----------------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoPagamento tipoPagamento)
        {
            if (!ModelState.IsValid)
            {
                // Se Nome estiver vazio ou inválido, volta pra tela
                return View(tipoPagamento);
            }

            _context.Add(tipoPagamento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET) -------------------------------------------------

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoPagamento = await _context.TiposPagamento
                .FirstOrDefaultAsync(tp => tp.Id == id);

            if (tipoPagamento == null)
            {
                return NotFound();
            }

            return View(tipoPagamento);
        }

        // EDIT (POST) -----------------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoPagamento tipoPagamento)
        {
            if (id != tipoPagamento.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // Nome vazio ou inválido → volta pra tela
                return View(tipoPagamento);
            }

            _context.Update(tipoPagamento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET) ----------------------------------------------

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoPagamento = await _context.TiposPagamento
                .FirstOrDefaultAsync(tp => tp.Id == id);

            if (tipoPagamento == null)
            {
                return NotFound();
            }

            return View(tipoPagamento);
        }

        // DELETE (POST) ---------------------------------------------

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoPagamento = await _context.TiposPagamento
                .FirstOrDefaultAsync(tp => tp.Id == id);

            if (tipoPagamento == null)
            {
                return NotFound();
            }

            _context.TiposPagamento.Remove(tipoPagamento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
