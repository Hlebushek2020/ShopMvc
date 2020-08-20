﻿using Microsoft.AspNet.Identity;
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
            List<OrderItem> orderItems = new List<OrderItem>();
            if (Request.Cookies[CookieName] != null)
            {
                HttpCookie basketCookie = Request.Cookies[CookieName];
                string[] keys = basketCookie.Values.AllKeys;
                for (int i = 0; i < keys.Length; i++)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.ItemsCount = Convert.ToInt32(basketCookie.Values[keys[i]]);
                    orderItem.Item = dbContext.Items.Find(Guid.Parse(keys[i]));
                    if (orderItem.Item != null)
                        orderItems.Add(orderItem);
                }
            }
            return View(orderItems);
        }
        #endregion

        #region Confirm
        public ActionResult Confirm()
        {
            if (Request.Cookies[CookieName] == null)
                return RedirectToAction("Index");

            HttpCookie basketCookie = Request.Cookies[CookieName];
            string[] keys = basketCookie.Values.AllKeys;
            Order order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.Parse(User.Identity.GetUserId()),
                OrderDate = DateTime.Now,
                Status = "Новый",
                OrderItems = new List<OrderItem>()
            };
            for (int i = 0; i < keys.Length; i++)
                order.OrderItems.Add(new OrderItem
                {
                    Order = order,
                    ItemsCount = Convert.ToInt32(basketCookie.Values[keys[i]]),
                    Item = dbContext.Items.Find(Guid.Parse(keys[i]))
                });

            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            Request.Cookies.Remove(CookieName);

            return View(order.OrderNumber);
        }
        #endregion

        #region Add
        public JsonResult Add(Guid id)
        {
            string stringId = id.ToString();
            int count = 1;
            HttpCookie basketCookie;
            if (Request.Cookies[CookieName] != null)
            {
                basketCookie = Request.Cookies[CookieName];
                Request.Cookies.Remove(CookieName);
            }
            else
                basketCookie = new HttpCookie(CookieName) { Expires = DateTime.Now.AddDays(7) };
            if (basketCookie.Values[stringId] != null)
            {
                count += Convert.ToInt32(basketCookie.Values[stringId]);
                basketCookie.Values[stringId] = count.ToString();
            }
            else
                basketCookie.Values.Add(stringId, count.ToString());

            Request.Cookies.Add(basketCookie);

            return Json(count);
        }
        #endregion

        #region Reduce
        public JsonResult Reduce(Guid id)
        {
            string stringId = id.ToString();
            int count = 1;
            HttpCookie basketCookie;
            if (Request.Cookies[CookieName] != null)
            {
                basketCookie = Request.Cookies[CookieName];
                Request.Cookies.Remove(CookieName);
            }
            else
                basketCookie = new HttpCookie(CookieName) { Expires = DateTime.Now.AddDays(7) };
            if (basketCookie.Values[stringId] != null)
            {
                count -= Convert.ToInt32(basketCookie.Values[stringId]);
                if (count <= 0)
                    basketCookie.Values.Remove(stringId);
                else
                    basketCookie.Values[stringId] = count.ToString();
            }

            Request.Cookies.Add(basketCookie);

            return Json(count);

        }
        #endregion

    }
}