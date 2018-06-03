using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    /// <summary>
    /// Type of the action element
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        /// <summary>
        /// Button
        /// </summary>
        Submit,
        
        /// <summary>
        /// Checkbox
        /// </summary>
        Checkbox
    }
}
