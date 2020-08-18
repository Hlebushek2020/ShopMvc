using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Shop.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class Customer
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Не указан заказчик")]
        [Display(Name = "Заказчик")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан код заказчика")]
        [Display(Name = "Код заказчика")]
        [RegularExpression(@"[0-9]{4}-[0-9]{4}", ErrorMessage = "Код заказчика не соответствует шаблону: ХХХХ-ГГГГ, где Х – число, ГГГГ – год в котором зарегистрирован заказчик")]
        public string Code { get; set; }

        [Display(Name = "Адрес заказчика")]
        public string Address { get; set; }

        [Display(Name = "Скидка (%)")]
        public double? Discount { get; set; }

        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public Guid Id { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Не указана дата создания заказа")]
        [Display(Name = "Дата создания")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Дата доставки")]
        [DataType(DataType.Date)]
        public DateTime? ShipmentDate { get; set; }

        [Display(Name = "Номер заказа")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber { get; set; }

        [Display(Name = "Состояние заказа")]
        public string Status { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public Guid Id { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        public Item Item { get; set; }

        [Display(Name = "Количество товара")]
        [Required(ErrorMessage = "Не указано количество товара")]
        public int ItemsCount { get; set; }

        [Display(Name = "Цена за единицу")]
        [Required(ErrorMessage = "Не указана цена за 1 шт. товара")]
        public double ItemPrice { get; set; }
    }

    public class Item
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Не указан код товара")]
        [Display(Name = "Код товара")]
        [RegularExpression(@"[0-9]{2}-[0-9]{4}-[A-Z]{2}[0-9]{2}", ErrorMessage = "Код товара не соответствует шаблону: XX-XXXX-YYXX, где Х – число, Y - заглавная буква английского алфавита")]
        public string Code { get; set; }

        [Display(Name = "Наименование товара")]
        public string Name { get; set; }

        [Display(Name = "Цена за единицу")]
        public double? Price { get; set; }

        [Display(Name = "Категория товара")]
        [MaxLength(30, ErrorMessage = "Привышена максимальная длина в 30 символов")]
        public string Category { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Item> Items { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}