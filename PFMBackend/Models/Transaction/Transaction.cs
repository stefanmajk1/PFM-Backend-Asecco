using PFMBackend.Models.Transaction.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PFMBackend.Models.Transaction
{
    //ovo je klasa koja predstavlja pojedinacnu bankovnu transakciju
    public class Transaction
    {
        [Required]
        [ReadOnly(true)]//samo citljivo
        public string Id { get; set; }

        public string BeneficiaryName { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]//format godine
        public DateTime Date { get; set; }

        [Required]
        public DirectionsEnum Directions { get; set; }

        [Required]
        public double Amount { get; set; }

        public string Description { get; set; }

        [MinLength(3)]//minimalna duzina
        [MaxLength(3)]//maksimalna duzina
        [Required]
        public string Currency { get; set; }

        public MccCodeEnum Mcc { get; set; }

        [Required]
        public TransactionKindsEnum Kind { get; set; }

        [ReadOnly(true)]//samo citljivo
        public string CatCode { get; set; }
    }
}
