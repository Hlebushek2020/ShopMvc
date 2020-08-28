using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class DataBaseManager : IDisposable
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        private UserRepository userRepository;
        private CustomerRepository customerRepository;
        private OrderRepository orderRepository;
        private OrderItemRepository orderItemRepository;
        private ItemRepository itemRepository;

        public bool Disposed { get; private set; } = false;

        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(dbContext);
                return userRepository;
            }
        }
        
        public CustomerRepository Customers
        {
            get
            {
                if (customerRepository == null)
                    customerRepository = new CustomerRepository(dbContext);
                return customerRepository;
            }
        }

        public OrderRepository Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(dbContext);
                return orderRepository;
            }
        }

        public OrderItemRepository OrderItems
        {
            get
            {
                if (orderItemRepository == null)
                    orderItemRepository = new OrderItemRepository(dbContext);
                return orderItemRepository;
            }
        }

        public ItemRepository Items
        {
            get
            {
                if (itemRepository == null)
                    itemRepository = new ItemRepository(dbContext);
                return itemRepository;
            }
        }

        public void SaveChanges() =>
            dbContext.SaveChanges();


        public void Dispose()
        {
            if (Disposed == false)
            {
                dbContext.Dispose();
                Disposed = true;
            } 
        }
    }
}