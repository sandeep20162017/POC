using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using System.Linq;
using YourNamespace.Models;

namespace YourNamespace
{
    public class ManageUserModel : PageModel
    {
        private readonly YourDbContext _context;

        public ManageUserModel(YourDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Initialization code if needed
        }

        public JsonResult OnPostGetUsers([DataSourceRequest] DataSourceRequest request)
        {
            var users = _context.Users.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                UserName = u.UserName
            }).ToList();

            return new JsonResult(users.ToDataSourceResult(request));
        }

        public JsonResult OnPostCreateUser([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    UserName = user.UserName
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                user.UserId = newUser.UserId;
            }

            return new JsonResult(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdateUser([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
                if (existingUser != null)
                {
                    existingUser.UserName = user.UserName;
                    _context.SaveChanges();
                }
            }

            return new JsonResult(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDeleteUser([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                _context.Users.Remove(existingUser);
                _context.SaveChanges();
            }

            return new JsonResult(new[] { user }.ToDataSourceResult(request, ModelState));
        }
    }
}