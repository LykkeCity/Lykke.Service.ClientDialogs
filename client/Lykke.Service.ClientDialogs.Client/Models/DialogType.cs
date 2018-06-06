using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.ClientDialogs.Client.Models
{
    /// <summary>
    /// Type of the dialog
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DialogType
    {
        /// <summary>
        /// Information
        /// </summary>
        Info,
        
        /// <summary>
        /// Warning
        /// </summary>
        Warning,
        
        /// <summary>
        /// Question
        /// </summary>
        Question
    }
}
