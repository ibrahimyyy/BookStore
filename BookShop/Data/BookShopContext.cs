using BookShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data
{
    public class BookShopContext : IdentityDbContext<ApplicationUser> //i inheritance from (IdentityDbContext) because (IdentityDbContext) is inherit from (DbContext) so (IdentityDbContext) has more properity to use it like the security and signup and login , and i user this (IdentityDbContext<ApplicationUser>) because i'm using a custom (IdentityUser) Class his name is ApplicationUser.
    {
        public BookShopContext(DbContextOptions<BookShopContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Books>().HasKey(b => b.Id).HasName("Book_ID");//this can change column name which i want by select this (b=>b.@@@).HasName("laplalapla".)
            //modelBuilder.Entity<Books>().HasData(new Books {Id=50,Name="Geo",Author="Bero", }); //this can do insert data in my database using fluintAPI.(simply i'm inserting a record in Book Table).
        }
        public DbSet<Books> Books { get; set; } //Create a Table his name is Books.
        public DbSet<BookGallery> BookGallery { get; set; }//create a Table his name is BookGallery.
        public DbSet<Language> Language { get; set; } //Create a Table his name is Language.
        //(DbSet) this mean i create a table on the database it's name is Books
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //optionsBuilder.UseSqlServer("Server=LENOVO; Database=BookShop; Integrated security=True;");
        //this can take parameter first one is (Server) i put "LENOVO" because i use a local server (My PC it's now a server) or i can put the IP of DataBase.
        //the second one is (DataBase) it's mean the name of DataBase.
        //the third one is (Integrated security) it's mean for ssecurity if my database has locked so i should to put the username and password here to access to the DataBase.
        // in the end i can declear this method in (Startup.cs) so there is no need to declear it here.
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
