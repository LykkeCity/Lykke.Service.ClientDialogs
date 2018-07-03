using JetBrains.Annotations;
using Lykke.Service.ClientDialogs.Client.Models.ConditionParameters;
using Newtonsoft.Json;

namespace Lykke.Service.ClientDialogs.Client.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
    [PublicAPI]
    public static class StringExtensions
    {
        /// <summary>
        /// Returns typed condition parameter object from string
        /// </summary>
        /// <param name="src"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetParameters<T>(this string src) where T : BaseConditionParameters
        {
            return string.IsNullOrEmpty(src)
                ? default(T)
                : JsonConvert.DeserializeObject<T>(src);
        }
    }
}
