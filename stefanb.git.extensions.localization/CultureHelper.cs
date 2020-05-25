using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stefanb.git.extensions.localization
{
    public static class CultureHelper
    {
        /// <summary>
        /// Returns the most suitable locale based on a set of supported languages and the weighted quality value passed in the Accept-Language header
        /// </summary>
        /// <param name="userLanguages">Array of accepted languages optionally weighted,
        /// <example>e.g. en-NZ,en;q=0.9,en-US;q=0.8,de-DE,de;q=0.6</example>
        /// </param>
        /// <param name="supportedCultures">Array of supported locales with the first element being the default language,
        /// <example>e.g. [ "en-US", "en-GB", "es-ES", "zh-CN", "de-DE" ]</example>
        /// </param>
        /// <returns>locale as string</returns>
        public static string GetPreferredCulture(StringWithQualityHeaderValue[] userLanguages, string[] supportedCultures)
        {
            if (userLanguages == null)
            {
                return GetDefaultCulture(supportedCultures);
            }

            var languages = userLanguages.OrderByDescending(x => x.Quality ?? 1).Select(l => l.Value.Value);

            // First iteration check exact match, or if neutral language (e.g. "en"), return first supported culture
            foreach (var language in languages)
            {
                if (supportedCultures.Contains(language))
                {
                    return language;
                }

                if (IsNeutralLanguage(language))
                {
                    foreach (var c in supportedCultures)
                    {
                        if (c.StartsWith(language))
                            return c;
                    }
                }
            }

            // Second iteration check if another language of same neutral culture is supported
            foreach (var language in languages)
            {
                var neutralLang = GetNeutralCulture(language);

                foreach (var c in supportedCultures)
                {
                    if (c.StartsWith(neutralLang))
                        return c;
                }
            }

            return GetDefaultCulture(supportedCultures);
        }


        #region Private Methods

        private static string GetDefaultCulture(string[] supportedCultures)
        {
            return supportedCultures[0]; // return Default culture
        }

        private static string GetNeutralCulture(string name)
        {
            if (name.Length < 2)
                return name;

            return name.Substring(0, 2); // Read first two chars only. E.g. "en", "es"
        }

        private static bool IsNeutralLanguage(string language)
        {
            return !language.Contains("-");
        }

        #endregion
    }
}
