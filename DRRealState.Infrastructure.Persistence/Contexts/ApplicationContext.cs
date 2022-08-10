using DRRealState.Core.Domain.Common;
using DRRealState.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.Modified = DateTime.Now;
                        entry.Entity.ModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }



        protected override void OnModelCreating(ModelBuilder builder) {
            //Fluent API

            builder.HasDefaultSchema("DRRealState");
            #region Tables

            builder.Entity<PropertiesType>().ToTable("PropertyType");

            builder.Entity<Upgrade>().ToTable("Upgrade");

            builder.Entity<SaleType>().ToTable("SaleType");

            builder.Entity<Estate>().ToTable("Estate");

            builder.Entity<EstateFavorite>().ToTable("Estate_Favorite");

            builder.Entity<Gallery>().ToTable("Gallery");

            builder.Entity<Upgrade_Estate>().ToTable("Upgrade_Estate");

            #endregion

            #region Primary Key


            builder.Entity<PropertiesType>()
                .HasKey(x=>x.Id);

            builder.Entity<Upgrade>()
                .HasKey(x => x.Id);

            builder.Entity<SaleType>()
                .HasKey(x => x.Id);

            builder.Entity<Estate>()
                .HasKey(x => x.Id);

            builder.Entity<EstateFavorite>()
                .HasKey(x => x.Id);

            builder.Entity<Gallery>()
                .HasKey(x => x.GalleryId);

            builder.Entity<Upgrade_Estate>()
                .HasKey(x => x.Id);

            #endregion

            #region RelationShips

            builder.Entity<Estate>()
                .HasMany(x => x.Gallery)
                .WithOne(x => x.Estate)
                .HasForeignKey(x => x.EstateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Estate>()
                .HasMany(x => x.Favorites)
                .WithOne(x => x.Estate)
                .HasForeignKey(x => x.EstateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Estate>()
                .HasMany(x => x.Upgrade)
                .WithOne(x => x.Estate)
                .HasForeignKey(x => x.EstateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Upgrade>()
                .HasMany(x => x.Estates)
                .WithOne(x => x.Upgrade)
                .HasForeignKey(x => x.UpgradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PropertiesType>()
                .HasMany(x => x.Estates)
                .WithOne(x => x.PropertiesType)
                .HasForeignKey(x=>x.PropertyTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SaleType>()
                .HasMany(x => x.Estates)
                .WithOne(x => x.SaleType)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Properties

            #region Estate

            builder.Entity<Estate>()
                .Property(x => x.BathroomQuantity)
                .IsRequired();

            builder.Entity<Estate>()
                .Property(x => x.Code)
                .IsRequired();

            builder.Entity<Estate>()
                .Property(x => x.BedRoomQuantity)
                .IsRequired();

            builder.Entity<Estate>()
                .Property(x => x.Description)
                .IsRequired();

            builder.Entity<Estate>()
                .Property(x => x.Ubication)
                .IsRequired();

            builder.Entity<Estate>()
                .Property(x => x.SizeInMeters)
                .IsRequired();            

            builder.Entity<Estate>()
                .Property(x => x.Price)
                .IsRequired();

            #endregion

            #region Properties Type

            builder.Entity<PropertiesType>()
                .Property(x => x.Name)
                .IsRequired();
            
            builder.Entity<PropertiesType>()
                .Property(x => x.Description)
                .IsRequired();

            #endregion

            #region Sale Type

            builder.Entity<SaleType>()
                .Property(x => x.Name)
                .IsRequired();
            
            builder.Entity<SaleType>()
                .Property(x => x.Description)
                .IsRequired();

            #endregion

            #region Upgrade

            builder.Entity<Upgrade>()
                .Property(x => x.Name)
                .IsRequired();
            
            builder.Entity<Upgrade>()
                .Property(x => x.Description)
                .IsRequired();

            #endregion

            #region Estate Favorite

            builder.Entity<EstateFavorite>()
                .Property(x => x.EstateId)
                .IsRequired();
            
            builder.Entity<EstateFavorite>()
                .Property(x => x.ClientId)
                .IsRequired();

            #endregion

            #region Upgrade Estate

            builder.Entity<Upgrade_Estate>()
                .Property(x => x.EstateId)
                .IsRequired();
            
            builder.Entity<Upgrade_Estate>()
                .Property(x => x.UpgradeId)
                .IsRequired();

            #endregion

            #endregion

        }
    }
}
