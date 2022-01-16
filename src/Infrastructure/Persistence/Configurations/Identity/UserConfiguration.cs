using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.ToTable("Users");

            b.Property(e => e.Id)
                .IsRequired();

            b.Property(e => e.Username)
                .IsRequired();

            b.Property(e => e.Name)
                .HasMaxLength(25);

            b.Property(e => e.Surname)
                .HasMaxLength(25);

            b.Property(e => e.PhoneNumber)
                .HasMaxLength(50);

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
    }
}