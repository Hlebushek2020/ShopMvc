using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class BasketController : Controller
    {
        private readonly ApplicationDbContext dbContext = new ApplicationDbContext();
        private readonly ApplicationUserManager userManager;

        public BasketController()
        {
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
        }

        private bool IsAdmin
        {
            get => userManager.IsInRole(User.Identity.GetUserId(), "admin");
        }

        private const string CookieName = "Basket";

        #region Index
        public ActionResult Index()
        {
            // create list orderItems
            return View();
        }
        #endregion

        #region Add
        public JsonResult Add(Guid id, int count)
        {
            string stringId = id.ToString();
            HttpCookie basketCookie = null;
            if (Request.Cookies[CookieName] != null)
            {
                basketCookie = Request.Cookies[CookieName];
                Request.Cookies.Remove(CookieName);
            }
            else
                basketCookie = new HttpCookie(CookieName) { Expires = DateTime.Now.AddDays(7) };
            if (basketCookie.Values[stringId] != null)
            {
                count = Convert.ToInt32(basketCookie.Values[stringId]) + count;
                basketCookie.Values[stringId] = count.ToString();
            }
            else
                basketCookie.Values.Add(stringId, count.ToString());

            Request.Cookies.Add(basketCookie);

            return Json(count);
        }
        #endregion
    }
}