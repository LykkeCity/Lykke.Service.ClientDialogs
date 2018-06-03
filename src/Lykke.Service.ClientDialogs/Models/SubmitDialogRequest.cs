using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ClientDialogs.Models
{
    public class SubmitDialogRequest
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string DialogId { get; set; }
        [Required]
        public string ActionId { get; set; }
    }
}
