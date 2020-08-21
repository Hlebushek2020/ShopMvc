namespace Shop.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Shop.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shop.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Shop.Models.ApplicationDbContext";
        }

        protected override void Seed(Shop.Models.ApplicationDbContext context)
        {
            IdentityRole adminRole = new IdentityRole { Name = "admin" };
            IdentityRole userRole = new IdentityRole { Name = "user" };
            ApplicationUser adminUser = new ApplicationUser
            {
                Email = "administrator@shop.com",
                UserName = "administrator@shop.com"
            };
            string password = "Adm1n$shop";


            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            roleManager.Create(adminRole);
            roleManager.Create(userRole);
            IdentityResult result = userManager.Create(adminUser, password);
            if (result.Succeeded)
                userManager.AddToRole(adminUser.Id.ToString(), adminRole.Name);
            context.SaveChanges();


            base.Seed(context);

        }
    }
}
