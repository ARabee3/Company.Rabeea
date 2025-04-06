using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Rabeea.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturn model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded) return RedirectToAction(nameof(Index));
                }
                
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturn> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturn
                {
                    Id = U.Id,
                    Name = U.Name

                });
            }
            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturn
                {
                    Id = U.Id,
                    Name = U.Name

                }).Where(u => u.Name.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (string.IsNullOrEmpty(id)) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { StatusCode = 404, message = $"Role with id: {id} is not found" });
            var dto = new RoleToReturn
            {
                Id = role.Id,
                Name = role.Name!
            };
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturn model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operations");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operations");
                var roleResult = await _roleManager.FindByNameAsync(model.Name);
                if(roleResult is null)
                {
                    role.Name = model.Name!;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded) 
                        return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Invalid Operations");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var user = await _roleManager.FindByIdAsync(id);
            if (user is null) return BadRequest("Invalid Operations");
            var result = await _roleManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return BadRequest();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound("No Role Found with this id");

            ViewData["RoleId"] = roleId;
            var usersInRole = new List<UserInRole>();
            var users = _userManager.Users.ToList();
            foreach(var user in users)
            {
                var userInRole = new UserInRole
                {
                    UserId = user.Id,
                    UserName = user.UserName!
                };
                if (await _userManager.IsInRoleAsync(user, role.Name!)) userInRole.IsSelected = true;
                else userInRole.IsSelected = false;
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRole> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null) {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser,role.Name))
                        {
                             await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (! user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    } 
                }
                return RedirectToAction(nameof(Edit), new { id = roleId }); 
            }
            ModelState.AddModelError("", "Invalid Operation");
            return View(users);
        }
    }
}
