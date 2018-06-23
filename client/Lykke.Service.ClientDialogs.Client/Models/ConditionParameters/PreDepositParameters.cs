using System.Collections.Generic;

namespace Lykke.Service.ClientDialogs.Client.Models.ConditionParameters
{
    public class PreDepositParameters : BaseConditionParameters
    {
        public IReadOnlyList<string> AssetIds { get; set; }
    }
}
