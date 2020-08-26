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
        private readonly DataBaseManager dbManager = new DataBaseManager();

        private bool IsAdmin
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return dbManager.Users.Manager.IsInRole(User.Identity.GetUserId(), "admin");
                return false;
            }
        }

        #region Index
        public ActionResult Index()
        {
            ViewBag.IsAdmin = IsAdmin;
            return View(dbManager.Items.GetAll());
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
            dbManager.Items.Add(item);
            dbManager.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public ActionResult Edit(Guid id)
        {
            Item item = dbManager.Items.Get(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            if (!ModelState.IsValid)
                return View(item);

            dbManager.Items.Update(item);
            dbManager.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public JsonResult Delete(Guid id)
        {
            dbManager.Items.Remove(id);
            dbManager.SaveChanges();

            return Json("ok");
        }
        #endregion

    }
}