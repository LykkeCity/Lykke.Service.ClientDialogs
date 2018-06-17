using Lykke.Service.ClientDialogs.Client.Models.ConditionParameters;
using Newtonsoft.Json;

namespace Lykke.Service.ClientDialogs.Client.Extensions
{
    public static class StringExtensions
    {
        public static T GetParameters<T>(this string src) where T : BaseConditionParameters
        {
            return string.IsNullOrEmpty(src)
                ? default(T)
                : JsonConvert.DeserializeObject<T>(src);
        }
    }
}
