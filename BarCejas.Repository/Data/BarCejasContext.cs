using Microsoft.EntityFrameworkCore;
using BarCejas.Core.Entities;
using System.Reflection;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BarCejas.Infrastructure.Data
{
    public partial class BarCejasContext : DbContext
    {
        public BarCejasContext()
        {
        }

        public BarCejasContext(DbContextOptions<BarCejasContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        //public virtual DbSet<Security> Securities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
