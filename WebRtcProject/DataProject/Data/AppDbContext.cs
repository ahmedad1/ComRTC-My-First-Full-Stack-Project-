using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data
{
    public class AppDbContext:IdentityDbContext<User>
    {   public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<BlackListJwt> BlackListJwts { get; set; }
        public DbSet<Verification>Verifications { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
       public  DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Product> Products { get; set; }
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Verification>().HasOne(x => x.User).WithOne(x => x.VerificationCode).HasForeignKey<Verification>(x=>x.UserId);

            modelBuilder.Entity<RefreshToken>().HasOne(x => x.User).WithOne(x => x.RefreshToken).HasForeignKey<RefreshToken>(x=>x.UserId);
            modelBuilder.Entity<Group>().HasMany(x => x.Users).WithMany(x => x.Groups).UsingEntity<UserGroups>();
            modelBuilder.Entity<Group>().ToTable("Groups").Property(x=>x.GroupName).HasColumnType("varchar");
            modelBuilder.Entity<UserConnection>().HasOne(x => x.User).WithMany(x => x.UserConnections).HasForeignKey(x=>x.UserId);
            modelBuilder.Entity<UserConnection>().ToTable("UserConnections");

            modelBuilder.Entity<UserConnection>().HasKey(x => x.ConnectionId);
            modelBuilder.Entity<BlackListJwt>().Property(x => x.TokenString).HasColumnType("varchar").HasMaxLength(600);
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "user", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() });
          
            foreach(var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetColumnName()!="FullName")
                    {
                        property.SetColumnType("varchar");
                        property.SetMaxLength(600);
                    }
                }
            }
            modelBuilder.Entity<IdentityToken>(x =>
            {
                x.HasOne(x => x.User).WithOne(x => x.IdentityToken).HasForeignKey<IdentityToken>(x => x.UserId);
                x.HasKey(x => new { x.UserId, x.Token });
                x.Property(x => x.Token).HasColumnType("nvarchar").HasMaxLength(44);
                
            });

        }
    }
}
