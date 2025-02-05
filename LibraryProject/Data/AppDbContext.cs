using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Data
{
    public class AppDbContext : DbContext
    {

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-4F33AQO\\SQLEXPRESS;Database=LibraryProject;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanItem> LoanItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Loan>()
                .HasOne(x => x.Borrower)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.BorrowerId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<LoanItem>()
                .HasOne(x => x.Loan)
                .WithMany(x => x.LoanItems)
                .HasForeignKey(x => x.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<LoanItem>()
                .HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookId);
               
        }
    }

}

