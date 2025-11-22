using GymMangementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                 .HasColumnType("varchar")
                 .HasMaxLength(50);

            builder.Property(x=> x.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            builder.Property(X => X.Phone)
                 .HasColumnType("varchar")
                 .HasMaxLength(11);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidEmailCheck", "Email like '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserValidPhoneCheck", "Phone like '01%' and phone Not Like '%[^0-9]%'");
            });

            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasIndex(x=>x.Phone).IsUnique();
            builder.OwnsOne(x => x.Address, AdressBuilder =>
            {
                AdressBuilder.Property(x => x.Street)
                .HasColumnName("Street")
                .HasColumnType("varchar")
                .HasMaxLength(30);

                AdressBuilder.Property(x => x.City)
                .HasColumnName("City")
                .HasColumnType("varchar")
                .HasMaxLength(30);
                AdressBuilder.Property(x => x.BuildingNumber)
                .HasColumnName("BuildingNumber");
            }
            );
        }
    }
}
