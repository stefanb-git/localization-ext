using Microsoft.AspNetCore.Http;
using stefanb.git.extensions.localization;
using System.Linq;

namespace BMW.CHRiS.Common.Extensions
{
    public static class LocalizationExtension
    {
        /// <summary>
        /// Returns the most suitable locale based on a set of supported languages and the weighted quality value passed in the Accept-Language header
        /// </summary>
        /// <param name="supportedCultures">Array of supported locales with the first element being the default language,
        /// <example>e.g. [ "en-US", "en-GB", "es-ES", "zh-CN", "de-DE" ]</example>
        /// </param>
        /// <returns>locale as string</returns>
        public static string GetPreferredLanguage(this HttpRequest request, string[] supportedCultures)
        {
            var languages = request.GetTypedHeaders()?.AcceptLanguage?.ToArray();
            return CultureHelper.GetPreferredCulture(languages, supportedCultures);
        }
    }
}
