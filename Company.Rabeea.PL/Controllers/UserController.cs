using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Company.Rabeea.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Company.Rabeea.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController(UserManager<AppUser> userManager) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturn> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users  = _userManager.Users.Select(U => new UserToReturn
                {
                    Id = U.Id,
                    UserName = U.UserName!,
                    Email = U.Email!,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Roles = _userManager.GetRolesAsync(U).Result

                });
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturn
                {
                    Id = U.Id,
                    UserName = U.UserName!,
                    Email = U.Email!,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Roles = _userManager.GetRolesAsync(U).Result

                }).Where(u => u.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (string.IsNullOrEmpty(id)) return BadRequest("Invalid Id");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound(new { StatusCode = 404, message = $"User with id: {id} is not found" });
            var dto = new UserToReturn
            {
                Id = user.Id,
                UserName= user.UserName!,
                Email = user.Email!,
                FirstName= user.FirstName,
                LastName = user.LastName,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturn model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operations");
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return BadRequest("Invalid Operations");
                user.UserName = model.UserName!;
                user.Email = model.Email!;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result =await _userManager.UpdateAsync(user);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return BadRequest("Invalid Operations");
            var result = await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
