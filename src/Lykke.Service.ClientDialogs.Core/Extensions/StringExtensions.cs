using Newtonsoft.Json;

namespace Lykke.Service.ClientDialogs.Core.Extensions
{
    public static class StringExtensions
    {
        public static T GetConditionParameters<T>(this string src)
        {
            return string.IsNullOrEmpty(src)
                ? default(T)
                : JsonConvert.DeserializeObject<T>(src);
        }
    }
}
