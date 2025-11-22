using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController(
        UserManager<Usuario> userManager,
        RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly UserManager<Usuario> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        // INDEX

        public IActionResult Index()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }


        // EDIT - GET

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(usuario);

            ViewBag.Roles = roles;
            ViewBag.UserRole = userRoles.FirstOrDefault();

            return View(usuario);
        }


        // EDIT - POST

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string nomeCompleto, string role)
        {
            if (id == null) return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            usuario.NomeCompleto = nomeCompleto;
            await _userManager.UpdateAsync(usuario);

            var userRoles = await _userManager.GetRolesAsync(usuario);
            await _userManager.RemoveFromRolesAsync(usuario, userRoles);

            if (!string.IsNullOrEmpty(role))
                await _userManager.AddToRoleAsync(usuario, role);

            return RedirectToAction(nameof(Index));
        }

        // DELETE - GET

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // DELETE - POST

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            // impedir apagar o admin principal
            var email = usuario.Email?.ToLower();
            if (email == "admin@fintrack.com")
                return Forbid();

            await _userManager.DeleteAsync(usuario);

            return RedirectToAction(nameof(Index));
        }
    }
}
