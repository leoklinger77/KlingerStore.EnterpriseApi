using FluentValidation.Results;
using KSE.Core.Interfaces;
using KSE.Core.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Payment.Data
{
    public class PaymentContext : DbContext, IUnitOfWork
    {        
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {            
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Models.Payment> Payment { get; set; }
        public DbSet<Models.Transaction> Transaction { get; set; }
        public DbSet<Models.TaxaTransaction> TaxaTransaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentContext).Assembly);

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(255)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentContext).Assembly);
        }
        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("InsertDate") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("InsertDate").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("InsertDate").IsModified = false;
                }
            }
            try
            {
                return await base.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }
    }
}
