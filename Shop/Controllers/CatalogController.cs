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
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext dbContext = new ApplicationDbContext();
        private readonly ApplicationUserManager userManager;

        public CatalogController()
        {
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
        }

        private bool IsAdmin
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return userManager.IsInRole(User.Identity.GetUserId(), "admin");
                return false;
            }
        }

        #region Index
        public ActionResult Index()
        {
            ViewBag.IsAdmin = IsAdmin;
            return View(dbContext.Items);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Item item)
        {
            if (!ModelState.IsValid)
                return View(item);

            item.Id = Guid.NewGuid();
            dbContext.Items.Add(item);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public ActionResult Edit(Guid id)
        {
            Item item = dbContext.Items.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            if (!ModelState.IsValid)
                return View(item);

            Item dbItem = dbContext.Items.Find(item.Id);
            dbItem.Code = item.Code;
            dbItem.Name = item.Name;
            dbItem.Category = item.Category;
            dbItem.Price = item.Price;
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public JsonResult Delete(Guid id)
        {
            Item item = dbContext.Items.Find(id);
            dbContext.Items.Remove(item);
            dbContext.SaveChanges();
            return Json("ok");
        }
        #endregion

    }
}