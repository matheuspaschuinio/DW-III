using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VasosInteligentes.Data;
using VasosInteligentes.Models;

namespace VasosInteligentes.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var usuariosDb = _userManager.Users.ToList();
            var usuariosView = usuariosDb.Select(u => new User
            {
                Id = u.Id.ToString(),
                Email = u.Email,
                Celular = u.PhoneNumber,
                Nome = u.Nome
            }).ToList();

            return View(usuariosView);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id);
            if(appUser == null)
            {
                return NotFound();
            }

            var userView = new User
            {
                Id = appUser.Id.ToString(),
                Nome = appUser.Nome,
                Email = appUser.Email,
                Celular = appUser.PhoneNumber
            };

            return View(userView);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Celular,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    PhoneNumber = user.Celular,
                    Nome = user.Nome
                };

                var result = await _userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Usuario");
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id);
            if(appUser == null)
            {
                return NotFound();
            }

            var userView = new User
            {
                Id = appUser.Id.ToString(),
                Email = appUser.Email,
                Celular = appUser.PhoneNumber,
                Nome = appUser.Nome
            };

            return View(userView);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Celular,Email,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(id);
                if(appUser == null)
                {
                    return NotFound();
                }

                appUser.Email = user.Email;
                appUser.UserName = user.Email;
                appUser.PhoneNumber = user.Celular;
                appUser.Nome = user.Nome;

                var result = await _userManager.UpdateAsync(appUser);

                if(result.Succeeded && !string.IsNullOrEmpty(user.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                    await _userManager.ResetPasswordAsync(appUser, token, user.Password);
                }

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var userView = new User
            {
                Id = appUser.Id.ToString(),
                Email = appUser.Email,
                Celular = appUser.PhoneNumber
            };

            return View(userView);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);

            if (appUser != null)
            {
                await _userManager.DeleteAsync(appUser);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
