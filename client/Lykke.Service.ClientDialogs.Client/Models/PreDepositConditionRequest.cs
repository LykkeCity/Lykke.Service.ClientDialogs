using System.Collections.Generic;

namespace Lykke.Service.ClientDialogs.Client.Models
{
    public class PreDepositConditionRequest
    {
        public string DialogId { get; set; }
        public IReadOnlyList<string> AssetIds { get; set; }
    }
}
