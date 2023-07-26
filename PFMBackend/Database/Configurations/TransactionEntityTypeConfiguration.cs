using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFMBackend.Database.Entities;

namespace PFMBackend.Database.Configurations
{
    //klasa koja konfigurise entitet 'TransactionEntity'
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {

        //metoda koja konfigurise entitet
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");//tabela

            builder.HasKey(t => t.Id);//primarni kljuc
            builder.Property(t => t.Id).HasMaxLength(32).Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);
            builder.Property(t => t.BeneficiaryName).HasMaxLength(256);
            builder.Property(t => t.Date).IsRequired();
            builder.Property(t => t.Direction).HasConversion<string>().IsRequired();
            builder.Property(t => t.Amount).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(1024);
            builder.Property(t => t.Currency).HasMaxLength(3).IsFixedLength<string>(true).IsRequired();
            builder.Property(t => t.Mcc);
            builder.Property(t => t.Kind).IsRequired().HasConversion<string>();
        }
    }
}
