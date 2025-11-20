using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Controllers
{
    [Authorize]
    public class TransacoesController(FinTrackContext context, UserManager<Usuario> userManager) : Controller
    {
        private readonly FinTrackContext _context = context;
        private readonly UserManager<Usuario> _userManager = userManager;

        // VIEWBAG
        private async Task PopularSelectListsAsync(string usuarioId, Transacao? transacao = null)
        {
            ViewBag.Contas = new SelectList(
                await _context.Contas.Where(c => c.UsuarioId == usuarioId).ToListAsync(),
                "Id",
                "Nome",
                transacao?.ContaId
            );

            ViewBag.Categorias = new SelectList(
                await _context.Categorias.ToListAsync(),
                "Id",
                "Nome",
                transacao?.CategoriaId
            );

            ViewBag.TiposPagamento = new SelectList(
                await _context.TiposPagamento.ToListAsync(),
                "Id",
                "Nome",
                transacao?.TipoPagamentoId
            );

            ViewBag.TiposTransacao = new SelectList(
                Enum.GetValues(typeof(Transacao.TipoTransacao))
            );
        }

        // INDEX (SELECT)

        public async Task<IActionResult> Index(
            DateTime? dataInicio,
            DateTime? dataFim,
            int? categoriaId,
            int? contaId,
            int? tipoPagamentoId,
            Transacao.TipoTransacao? tipo)
        {
            var usuarioId = _userManager.GetUserId(User);

            if (usuarioId is null)
                return Unauthorized();

            var query = _context.Transacoes
                .Include(t => t.Conta)
                .Include(t => t.Categoria)
                .Include(t => t.TipoPagamento)
                .Where(t => t.UsuarioId == usuarioId)
                .AsQueryable();

            // FILTROS
            if (dataInicio.HasValue)
                query = query.Where(t => t.Data >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(t => t.Data <= dataFim.Value);

            if (categoriaId.HasValue)
                query = query.Where(t => t.CategoriaId == categoriaId.Value);

            if (contaId.HasValue)
                query = query.Where(t => t.ContaId == contaId.Value);

            if (tipoPagamentoId.HasValue)
                query = query.Where(t => t.TipoPagamentoId == tipoPagamentoId.Value);

            if (tipo.HasValue)
                query = query.Where(t => t.Tipo == tipo.Value);

            // DROPDOWNS
            await PopularSelectListsAsync(usuarioId);

            var transacoes = await query
                .OrderByDescending(t => t.Data)
                .ToListAsync();

            return View(transacoes);
        }

        // DETAILS

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var usuarioId = _userManager.GetUserId(User);

            var transacao = await _context.Transacoes
                .Include(t => t.Conta)
                .Include(t => t.Categoria)
                .Include(t => t.TipoPagamento)
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);

            if (transacao == null) return NotFound();

            return View(transacao);
        }

        // CREATE (GET)

        public async Task<IActionResult> Create()
        {
            var usuarioId = _userManager.GetUserId(User);

            if (usuarioId is null)
                return Unauthorized();

            await PopularSelectListsAsync(usuarioId);

            return View();
        }

        // CREATE (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transacao transacao)
        {
            var usuarioId = _userManager.GetUserId(User);

            if (usuarioId is null)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                await PopularSelectListsAsync(usuarioId, transacao);
                return View(transacao);
            }

            if (usuarioId is null)
                return Unauthorized();

            transacao.UsuarioId = usuarioId;

            _context.Add(transacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuarioId = _userManager.GetUserId(User);

            if (usuarioId is null)
                return Unauthorized();

            var transacao = await _context.Transacoes
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);

            if (transacao == null) return NotFound();

            await PopularSelectListsAsync(usuarioId, transacao);

            return View(transacao);
        }

        // EDIT (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Transacao transacao)
        {
            if (id != transacao.Id) return NotFound();

            var usuarioId = _userManager.GetUserId(User);

            if (usuarioId is null)
                return Unauthorized();

            var transacaoDb = await _context.Transacoes
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);

            if (transacaoDb == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                await PopularSelectListsAsync(usuarioId, transacao);
                return View(transacao);
            }

            if (usuarioId is null)
                return Unauthorized();

            transacao.UsuarioId = usuarioId;

            _context.Update(transacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET)

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuarioId = _userManager.GetUserId(User);

            var transacao = await _context.Transacoes
                .Include(t => t.Conta)
                .Include(t => t.Categoria)
                .Include(t => t.TipoPagamento)
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);

            if (transacao == null) return NotFound();

            return View(transacao);
        }

        // DELETE (POST)

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioId = _userManager.GetUserId(User);

            var transacao = await _context.Transacoes
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);

            if (transacao == null) return Unauthorized();

            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
