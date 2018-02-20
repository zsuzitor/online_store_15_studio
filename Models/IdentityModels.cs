﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace online_store.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public byte[] Image { get; set; }
        public string Info { get; set; }

        public  void Eq(ApplicationUser a)
        {
            Name = a.Name;
            Age = a.Age;
           Info = a.Info;
            return;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Object_os> Objects { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Mark_for_comment> Mark_for_comment { get; set; }
        public DbSet<Connect_image> Images { get; set; }
        public DbSet<Connect_basket> Baskets { get; set; }
       
        public DbSet<Follow_object> Follow_objects { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Purchase_connect> Purchases_connect { get; set; }
        public DbSet<Section_main_header> Section_in_main_header { get; set; }

        public DbSet<Application_phone> Application_phone_comm { get; set; }
        


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}