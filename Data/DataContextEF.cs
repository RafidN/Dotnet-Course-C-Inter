

using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HelloWorld.Data
{
    public class DataContextEF : DbContext
    {
        public DbSet<Computer>? Computer { get; set; }

        private string _connectionString;

        public DataContextEF(IConfiguration config)
        {
            ArgumentNullException.ThrowIfNull(config);
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                // options.UseSqlServer("Server=localhost;Database=DotNetCourseDatabase;Trusted_connection=false;TrustServerCertificate=True;User Id=sa;Password=SQLConnect1!;",
                options.UseSqlServer(_connectionString,
                    options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<Computer>()
                // .HasNoKey()
                .HasKey(c => c.ComputerId);
                // .ToTable("Computer", "TutorialAppSchema");
                // .ToTable("TableName", "SchemaName");
        }
        

    }
}