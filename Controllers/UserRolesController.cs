using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Enums;
using SMS.Models;

namespace SMS.Controllers
{
    [Authorize(Roles= "Admin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = await _userManager.Users.ToListAsync();
            List<UserRolesViewModel> userRoleViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisModel = new UserRolesViewModel();
                thisModel.UserId = user.Id;
                thisModel.UserName = user.UserName;
                thisModel.Email = user.Email;
                thisModel.FirstName = user.FirstName;
                thisModel.MiddleName = user.MiddleName;
                thisModel.LastName = user.LastName;
                thisModel.Roles = await GetUserRoles(user);
                userRoleViewModel.Add(thisModel);
            }
            return View(userRoleViewModel);
        }

        private async Task<IEnumerable<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            foreach (IdentityRole role in roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                { userRolesViewModel.Selected = true; }
                else { userRolesViewModel.Selected = false; }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
