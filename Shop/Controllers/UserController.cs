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
    }
}