using System.ComponentModel.DataAnnotations;

namespace PFMBackend.Commands
{
    public class TransactionCategorizeCommand
    {
        [Required]
        public string Catcode { get; set; }
    }
}
