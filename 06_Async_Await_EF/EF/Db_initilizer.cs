using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_Async_Await_EF.EF
{
    public class Db_initilizer : DbContext
    {
        public Db_initilizer()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source = DESKTOP-3P42OP0\SQLEXPRESS;
                                          Initial Catalog = Library_SP;
                                          Integrated Security = True;
                                          Connect Timeout = 2;
                                          Trust Server Certificate = True; 
                                          Application Intent = ReadWrite;
                                          Multi Subnet Failover = False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>()
                .HasOne(a => a.Author)
                .WithMany(b => b.Books)
                .HasForeignKey(a => a.AuthorId);
        }
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

    }
}
