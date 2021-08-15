using FluentValidation.Results;
using KSE.Core.DomainObjets;
using KSE.Core.Interfaces;
using KSE.Core.Mediatr;
using KSE.Core.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Client.Data
{
    public class ClientContext : DbContext, IUnitOfWork
    {
        private readonly IMediatrHandler _mediatrHandler;
        public ClientContext(DbContextOptions<ClientContext> options, IMediatrHandler mediatrHandler) : base(options)
        {
            _mediatrHandler = mediatrHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Models.Client> Client { get; set; }
        public DbSet<Models.Address> Address { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientContext).Assembly);

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(255)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientContext).Assembly);
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
            var success = await base.SaveChangesAsync() > 0;
            if (success) await _mediatrHandler.SendEvent(this);

            return success;
        }
    }

    public static class MediatorExtension
    {
        public static async Task SendEvent<T>(this IMediatrHandler mediatr, T context) where T : DbContext
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notification != null && x.Entity.Notification.Any());

            var domainEvents = domainEntities.SelectMany(x => x.Entity.Notification).ToList();

            domainEntities.ToList().ForEach(x => x.Entity.DisposeEvent());

            var tasks = domainEvents.Select(async (domainEvents) =>
            {
                await mediatr.PublishEvent(domainEvents);
            });

            await Task.WhenAll(tasks);
        }
    }
}
