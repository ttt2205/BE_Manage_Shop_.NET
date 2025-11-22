using Manage_Store.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Manage_Store.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Inventory> Inventory => Set<Inventory>();
        public DbSet<Promotion> Promotions => Set<Promotion>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<AuditSessions> AuditSessions => Set<AuditSessions>();
        public DbSet<InventoryAuditItem> InventoryAuditItems => Set<InventoryAuditItem>();
    }
}
