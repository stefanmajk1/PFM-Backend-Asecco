using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFMBackend.Database.Entities;

namespace PFMBackend.Database.Configurations
{
    //konfiguracija za netitet CategoryEntity
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        //metoda koja konfigurise entitet
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("categories");//tabela

            builder.HasKey(c => c.Code);//primarni kljuc
            builder.Property(c => c.Code).HasMaxLength(32).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(256);
            builder.Property(c => c.ParentCode).HasMaxLength(32);
        }
    }
}
