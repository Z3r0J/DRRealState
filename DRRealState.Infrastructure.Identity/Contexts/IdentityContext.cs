using DRRealState.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DRRealState.Infrastructure.Identity.Contexts
{
    public class IdentityContext : IdentityDbContext<RealStateUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Fluent API
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Identity");

            builder.Entity<RealStateUser>(entity => {

                entity.ToTable(name: "Users");
                            
            });

            builder.Entity<IdentityRole>(entity => {

                entity.ToTable(name: "Roles");
                            
            });

            builder.Entity<IdentityUserRole<string>>(entity => {

                entity.ToTable(name: "UserRoles");
                            
            });
            
            builder.Entity<IdentityUserLogin<string>>(entity => {

                entity.ToTable(name: "UserLogins");
                            
            });
        }
    }
}
