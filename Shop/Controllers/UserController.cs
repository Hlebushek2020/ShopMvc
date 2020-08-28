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
        private readonly DataBaseManager dbManager = new DataBaseManager();

        #region Index
        public ActionResult Index()
        {
            return View(dbManager.Users.GetAll());
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
            // User role
            string role = "user";
            if (userCreateViewModel.IsAdmin)
                role = "admin";
            // Add user
            dbManager.Users.Add(applicationUser, userCreateViewModel.Password, role);
            // Customer attached to user
            if (userCreateViewModel.IsAdmin == false)
            {
                dbManager.Customers.Add(new Customer
                {
                    Id = Guid.Parse(applicationUser.Id),
                    Name = userCreateViewModel.Name,
                    Code = userCreateViewModel.Code,
                    Address = userCreateViewModel.Address,
                    Discount = userCreateViewModel.Discount
                });
            }

            dbManager.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public ActionResult Edit(Guid id)
        {
            ApplicationUser applicationUser = dbManager.Users.Get(id);
            Customer customer = dbManager.Customers.Get(id);
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
            if (dbManager.Users.Manager.IsInRole(id.ToString(), "admin"))
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
                dbManager.Users.Manager.RemovePassword(userEditViewModel.Id.ToString());
                dbManager.Users.Manager.AddPassword(userEditViewModel.Id.ToString(), userEditViewModel.Password);
            }
            if (userEditViewModel.IsAdmin == false)
            {
                ApplicationUser user = dbManager.Users.Get(userEditViewModel.Id);
                user.PhoneNumber = userEditViewModel.Phone;
                Customer customer = dbManager.Customers.Get(userEditViewModel.Id);
                customer.Name = userEditViewModel.Name;
                customer.Address = userEditViewModel.Address;
                customer.Code = userEditViewModel.Code;
                customer.Discount = userEditViewModel.Discount;

                dbManager.SaveChanges();
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
            dbManager.Users.Remove(id);
            // Delete customer attached to user
            dbManager.Customers.Remove(id);

            dbManager.SaveChanges();

            return Json("ok");
        }
        #endregion

    }
}