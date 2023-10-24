using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infra.Data
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        protected Context()
        {

        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(255)");

            modelBuilder.Properties<DateTime>(c => c.HasColumnType("timestamp"));

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);

            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            base.OnModelCreating(modelBuilder);
        }
    }
}
