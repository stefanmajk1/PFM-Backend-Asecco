using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFMBackend.Database.Entities;

namespace PFMBackend.Database.Configurations
{
    //klasa za konfiguraciju entiteta 'SplitEntity'
    public class SplitEntityTypeConfiguration : IEntityTypeConfiguration<SplitEntity>
    {
        //metoda koja konfigurise entitet
        public void Configure(EntityTypeBuilder<SplitEntity> builder)
        {
            builder.ToTable("splits");//tabela

            builder.HasKey(s => new { s.TransactionId, s.Catcode });//primarni kljuc
            builder.Property(s => s.Catcode).HasMaxLength(32);
            builder.Property(s => s.Amount).IsRequired();
            builder.Property(s => s.TransactionId).HasMaxLength(32);
        }
    }
}
