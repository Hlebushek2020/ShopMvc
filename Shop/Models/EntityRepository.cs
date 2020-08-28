using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetWhere(Func<T, bool> predicate);
        T Get(Guid id);
        T Get(string id);
        void Add(T item);
        void Update(T item);
        void Remove(Guid id);
    }

    interface IUser
    {
        bool Add(ApplicationUser user, string password, string role);
    }

    public class UserRepository : IRepository<ApplicationUser>, IUser
    {
        private ApplicationDbContext dbContext;
        public ApplicationUserManager Manager { get; private set; }

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            Manager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
        }

        public void Add(ApplicationUser item) =>
            dbContext.Users.Add(item);

        public bool Add(ApplicationUser user, string password, string role)
        {
            IdentityResult identityUser = Manager.Create(user, password);
            // User add role 
            if (identityUser.Succeeded)
            {
                Manager.AddToRole(user.Id, role);
                return true;
            }
            return false;
        }

        public ApplicationUser Get(Guid id) => Get(id.ToString());

        public ApplicationUser Get(string id) =>
            dbContext.Users.Find(id);

        public IEnumerable<ApplicationUser> GetAll() =>
            dbContext.Users;

        public IEnumerable<ApplicationUser> GetWhere(Func<ApplicationUser, bool> predicate) =>
            dbContext.Users.Where(predicate);

        public void Remove(Guid id)
        {
            ApplicationUser applicationUser = dbContext.Users.Find(id.ToString());
            if (applicationUser != null)
                dbContext.Users.Remove(applicationUser);
        }

        public void Update(ApplicationUser item) =>
            dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
    }

    public class CustomerRepository : IRepository<Customer>
    {
        private ApplicationDbContext dbContext;

        public CustomerRepository(ApplicationDbContext dbContext) =>
            this.dbContext = dbContext;

        public void Add(Customer item) =>
            dbContext.Customers.Add(item);

        public Customer Get(Guid id) =>
            dbContext.Customers.Find(id);

        public Customer Get(string id) => Get(Guid.Parse(id));

        public IEnumerable<Customer> GetAll() =>
            dbContext.Customers;

        public IEnumerable<Customer> GetWhere(Func<Customer, bool> predicate) =>
            dbContext.Customers.Where(predicate);

        public void Remove(Guid id)
        {
            Customer customer = dbContext.Customers.Find(id);
            if (customer != null)
                dbContext.Customers.Remove(customer);
        }

        public void Update(Customer item) =>
            dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
    }

    public class OrderRepository : IRepository<Order>
    {
        private ApplicationDbContext dbContext;

        public OrderRepository(ApplicationDbContext dbContext) =>
            this.dbContext = dbContext;

        public void Add(Order item) =>
            dbContext.Orders.Add(item);

        public Order Get(Guid id) =>
            dbContext.Orders.Find(id);

        public Order Get(string id) => Get(Guid.Parse(id));

        public IEnumerable<Order> GetAll() =>
            dbContext.Orders;

        public IEnumerable<Order> GetWhere(Func<Order, bool> predicate) =>
            dbContext.Orders.Where(predicate);

        public void Remove(Guid id)
        {
            Order order = dbContext.Orders.Find(id);
            if (order != null)
                dbContext.Orders.Remove(order);
        }

        public void Update(Order item) =>
            dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
    }

    public class OrderItemRepository : IRepository<OrderItem>
    {
        private ApplicationDbContext dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext) =>
            this.dbContext = dbContext;

        public void Add(OrderItem item) =>
            dbContext.OrderItems.Add(item);

        public OrderItem Get(Guid id) =>
            dbContext.OrderItems.Find(id);

        public OrderItem Get(string id) => Get(Guid.Parse(id));

        public IEnumerable<OrderItem> GetAll() =>
            dbContext.OrderItems;

        public IEnumerable<OrderItem> GetWhere(Func<OrderItem, bool> predicate) =>
            dbContext.OrderItems.Where(predicate);

        public void Remove(Guid id)
        {
            OrderItem orderItem = dbContext.OrderItems.Find(id);
            if (orderItem != null)
                dbContext.OrderItems.Remove(orderItem);
        }

        public void Update(OrderItem item) =>
            dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
    }

    public class ItemRepository : IRepository<Item>
    {
        private ApplicationDbContext dbContext;

        public ItemRepository(ApplicationDbContext dbContext) =>
            this.dbContext = dbContext;

        public void Add(Item item) =>
            dbContext.Items.Add(item);

        public Item Get(Guid id) =>
            dbContext.Items.Find(id);

        public Item Get(string id) => Get(Guid.Parse(id));

        public IEnumerable<Item> GetAll() =>
            dbContext.Items;

        public IEnumerable<Item> GetWhere(Func<Item, bool> predicate) =>
            dbContext.Items.Where(predicate);

        public void Remove(Guid id)
        {
            Item item = dbContext.Items.Find(id);
            if (item != null)
                dbContext.Items.Remove(item);
        }

        public void Update(Item item) =>
            dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
    }
}