using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext dbContext = new ApplicationDbContext();
        private readonly ApplicationUserManager userManager;

        public OrderController()
        {
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
        }

        private bool IsAdmin
        {
            get => userManager.IsInRole(User.Identity.GetUserId(), "admin");
        }

        #region Index
        public ActionResult Index(string statusFilter)
        {
            ViewBag.IsAdmin = IsAdmin;

            IEnumerable<Order> orders = null;
            if (IsAdmin)
                orders = dbContext.Orders;
            else
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                Customer customer = dbContext.Customers.Find(userId);
                if (string.IsNullOrEmpty(statusFilter))
                    orders = customer.Orders;
                else
                    orders = customer.Orders.TakeWhile(order => order.Status == statusFilter);
            }

            return View(orders);
        }
        #endregion

        #region Confirm
        public ActionResult Confirm(Guid id)
        {
            ConfirmViewModel confirmViewModel = new ConfirmViewModel { Id = id };
            return View(confirmViewModel);
        }

        [HttpPost]
        public ActionResult Confirm(ConfirmViewModel confirmViewModel)
        {
            if (!ModelState.IsValid)
                return View(confirmViewModel);

            Order order = dbContext.Orders.Find(confirmViewModel.Id);
            order.ShipmentDate = confirmViewModel.ShipmentDate;
            order.Status = "Выполняется";
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Close
        [HttpPost]
        public JsonResult Close(Guid id)
        {
            Order order = dbContext.Orders.Find(id);
            order.Status = "Выполнен";
            dbContext.SaveChanges();

            return Json("ok");
        }
        #endregion

        #region Delete
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            Order order = dbContext.Orders.Find(id);
            if (order.Status == "Новый")
            {
                dbContext.Orders.Remove(order);
                dbContext.SaveChanges();
            }
            else
                return Json("Невозможно удалить заказ с текущим статусом");

            return Json("ok");
        }
        #endregion

    }
}