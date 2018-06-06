using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.ClientDialogs.Client.Models
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
