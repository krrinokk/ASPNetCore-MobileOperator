using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using ASPNetCoreApp.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using DAL.Models;

namespace ASPNetCoreApp.DAL.Models
{
    public partial class OperatorContext : IdentityDbContext<User>
    // IdentityDbContext<User>
    {
        protected readonly IConfiguration Configuration;
        public OperatorContext(DbContextOptions<OperatorContext> options, IConfiguration configuration)
        : base(options)
        {
            Configuration = configuration;
        }

         public OperatorContext()
    
        { }
        //#endregion

        public virtual DbSet<Тариф> Тариф { get; set; }
        public virtual DbSet<Dogovor> Dogovor { get; set; }
        public virtual DbSet<Клиент> Клиент { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

       => optionsBuilder.UseSqlServer("Server=LAPTOP-QBUR001L\\SQLEXPRESS;Database=operator;Trusted_Connection=True;Encrypt=False");

        protected override void OnModelCreating(ModelBuilder
        modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Тариф>(entity =>
            {
                entity.Property(e => e.Код_тарифа).IsRequired();
            });
            modelBuilder.Entity<Dogovor>(entity =>
            {
                entity.HasOne(d => d.Тариф)
                .WithMany(p => p.Dogovor)
                .HasForeignKey(d => d.Код_тарифа_FK);
            });

            modelBuilder.Entity<Клиент>(entity =>
            {
                entity.Property(e => e.Номер_клиента).IsRequired();
            });
            modelBuilder.Entity<Dogovor>(entity =>
            {
                entity.HasOne(d => d.Клиент)
                .WithMany(p => p.Dogovor)
                .HasForeignKey(d => d.Номер_клиента_FK);
            });


        }
    }
}
