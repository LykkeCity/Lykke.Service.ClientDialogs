using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ClientDialogs.Models
{
    public class DeleteDialogRequest
    {
        public string ClientId { get; set; }
        [Required]
        public string DialogId { get; set; }
    }
}
