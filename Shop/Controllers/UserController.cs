using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext = new ApplicationDbContext();

        #region Index
        public ActionResult Index()
        {
            return View(dbContext.Users);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserCreateViewModel userCreateViewModel)
        {
            if (userCreateViewModel.IsAdmin)
            {
                userCreateViewModel.Name = "NOUSE";
                userCreateViewModel.Code = "1212-2000";
            }

            if (!ModelState.IsValid)
                return View(userCreateViewModel);

            // User create
            ApplicationUser applicationUser = new ApplicationUser
            {
                Email = userCreateViewModel.Email,
                UserName = userCreateViewModel.Email,
                PhoneNumber = userCreateViewModel.Phone
            };
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            IdentityResult identityUser = userManager.Create(applicationUser, userCreateViewModel.Password);
            // User add role 
            if (identityUser.Succeeded)
            {
                string role = "user";
                if (userCreateViewModel.IsAdmin)
                    role = "admin";
                userManager.AddToRole(applicationUser.Id.ToString(), role);
            }
            // Customer attached to user
            if (userCreateViewModel.IsAdmin == false)
            {
                dbContext.Customers.Add(new Customer
                {
                    Id = Guid.Parse(applicationUser.Id),
                    Name = userCreateViewModel.Name,
                    Code = userCreateViewModel.Code,
                    Address = userCreateViewModel.Address,
                    Discount = userCreateViewModel.Discount
                });
            }

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public ActionResult Edit(Guid id)
        {
            ApplicationUser applicationUser = dbContext.Users.Find(id.ToString());
            Customer customer = dbContext.Customers.Find(id);
            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = id,
                Phone = applicationUser.PhoneNumber,
                IsAdmin = false
            };
            if (customer != null)
            {
                userEditViewModel.Name = customer.Name;
                userEditViewModel.Address = customer.Address;
                userEditViewModel.Code = customer.Code;
                userEditViewModel.Discount = customer.Discount;
            }
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
            if (userManager.IsInRole(id.ToString(), "admin"))
            {
                userEditViewModel.IsAdmin = true;
                userEditViewModel.Name = "NOUSE";
                userEditViewModel.Code = "1212-2000";
            }
            return View(userEditViewModel);
        }

        [HttpPost]
        public ActionResult Edit(UserEditViewModel userEditViewModel)
        {
            if (!ModelState.IsValid)
                return View(userEditViewModel);

            if (!string.IsNullOrEmpty(userEditViewModel.Password))
            {
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
                userManager.RemovePassword(userEditViewModel.Id.ToString());
                userManager.AddPassword(userEditViewModel.Id.ToString(), userEditViewModel.Password);
            }
            if (userEditViewModel.IsAdmin == false)
            {
                Customer customer = dbContext.Customers.Find(userEditViewModel.Id);
                customer.Name = userEditViewModel.Name;
                customer.Address = userEditViewModel.Address;
                customer.Code = userEditViewModel.Code;
                customer.Discount = userEditViewModel.Discount;

                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public JsonResult Delete(Guid id)
        {
            if (User.Identity.GetUserId() == id.ToString())
                return Json("Невозможно удалить себя же");

            // Delete user
            ApplicationUser user = dbContext.Users.Find(id.ToString());
            dbContext.Users.Remove(user);
            // Delete customer attached to user
            Customer customer = dbContext.Customers.Find(id);
            if (customer != null)
                dbContext.Customers.Remove(customer);

            dbContext.SaveChanges();

            return Json("ok");
        }
        #endregion

    }
}