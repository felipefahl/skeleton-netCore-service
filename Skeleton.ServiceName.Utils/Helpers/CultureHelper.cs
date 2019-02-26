using Skeleton.ServiceName.Utils.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;

namespace Skeleton.ServiceName.Utils
{
    public static class CultureHelper
    {
        // Valid cultures
        private static readonly List<string> _validCultures = new List<string> {
            "af", "af-ZA", "sq", "sq-AL", "gsw-FR", "am-ET", "ar", "ar-DZ", "ar-BH", "ar-EG",
            "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA", "ar-OM", "ar-QA", "ar-SA",
            "ar-SY", "ar-TN", "ar-AE", "ar-YE", "hy", "hy-AM", "as-IN", "az", "az-Cyrl-AZ",
            "az-Latn-AZ", "ba-RU", "eu", "eu-ES", "be", "be-BY", "bn-BD", "bn-IN", "bs-Cyrl-BA", "bs-Latn-BA",
            "br-FR", "bg", "bg-BG", "ca", "ca-ES", "zh-HK", "zh-MO", "zh-CN", "zh-Hans", "zh-SG", "zh-TW",
            "zh-Hant", "co-FR", "hr", "hr-HR", "hr-BA", "cs", "cs-CZ", "da", "da-DK", "prs-AF", "div",
            "div-MV", "nl", "nl-BE", "nl-NL", "en", "en-AU", "en-BZ", "en-CA", "en-029", "en-IN", "en-IE",
            "en-JM", "en-MY", "en-NZ", "en-PH", "en-SG", "en-ZA", "en-TT", "en-GB", "en-US", "en-ZW", "et", "et-EE", "fo", "fo-FO",
            "fil-PH", "fi", "fi-FI", "fr", "fr-BE", "fr-CA", "fr-FR", "fr-LU", "fr-MC", "fr-CH", "fy-NL", "gl", "gl-ES", "ka", "ka-GE",
            "de", "de-AT", "de-DE", "de-LI", "de-LU", "de-CH", "el", "el-GR", "kl-GL", "gu", "gu-IN", "ha-Latn-NG", "he", "he-IL", "hi", "hi-IN",
            "hu", "hu-HU", "is", "is-IS", "ig-NG", "id", "id-ID", "iu-Latn-CA", "iu-Cans-CA", "ga-IE", "xh-ZA", "zu-ZA", "it", "it-IT", "it-CH", "ja",
            "ja-JP", "kn", "kn-IN", "kk", "kk-KZ", "km-KH", "qut-GT", "rw-RW", "sw", "sw-KE", "kok", "kok-IN", "ko", "ko-KR", "ky", "ky-KG", "lo-LA", "lv",
            "lv-LV", "lt", "lt-LT", "wee-DE", "lb-LU", "mk", "mk-MK", "ms", "ms-BN", "ms-MY", "ml-IN", "mt-MT", "mi-NZ", "arn-CL", "mr", "mr-IN", "moh-CA",
            "mn", "mn-MN", "mn-Mong-CN", "ne-NP", "no", "nb-NO", "nn-NO", "oc-FR", "or-IN", "ps-AF", "fa", "fa-IR", "pl", "pl-PL", "pt", "pt-BR", "pt-PT",
            "pa", "pa-IN", "quz-BO", "quz-EC", "quz-PE", "ro", "ro-RO", "rm-CH", "ru", "ru-RU", "smn-FI", "smj-NO", "smj-SE", "se-FI", "se-NO", "se-SE",
            "sms-FI", "sma-NO", "sma-SE", "sa", "sa-IN", "sr", "sr-Cyrl-BA", "sr-Cyrl-SP", "sr-Latn-BA", "sr-Latn-SP", "nso-ZA", "tn-ZA", "si-LK", "sk",
            "sk-SK", "sl", "sl-SI", "es", "es-AR", "es-BO", "es-CL", "es-CO", "es-CR", "es-DO", "es-EC", "es-SV", "es-GT", "es-HN", "es-MX", "es-NI",
            "es-PA", "es-PY", "es-PE", "es-PR", "es-ES", "es-US", "es-UY", "es-VE", "sv", "sv-FI", "sv-SE", "syr", "syr-SY", "tg-Cyrl-TJ", "tzm-Latn-DZ",
            "ta", "ta-IN", "tt", "tt-RU", "te", "te-IN", "th", "th-TH", "bo-CN", "tr", "tr-TR", "tk-TM", "ug-CN", "uk", "uk-UA", "wen-DE", "ur", "ur-PK", "uz",
            "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "vi-VN", "cy-GB", "wo-SN", "sah-RU", "ii-CN", "yo-NG" };

        private static ResourceManager resource = new ResourceManager(typeof(Global));

        // Include ONLY cultures you are implementing
        public static readonly List<string> _cultures = new List<string> {
            "en-US",  // first culture is the DEFAULT
            "pt-BR",
            "es-ES"
        };

        public static IList<CultureInfo> GetCultures()
        {

            IList<CultureInfo> Cultures = new List<CultureInfo>
            {
                {  new CultureInfo("en-US") },
                {  new CultureInfo("pt-BR") },
                {  new CultureInfo("es-ES") }
            };

            return Cultures;
        }

        public static IDictionary<string, string> GetLanguages()
        {

            IDictionary<string, string> Cultures = new Dictionary<string, string>
            {
                {   "en-US", resource.GetString("enUS") },
                {   "pt-BR", resource.GetString("ptBR") },
                {   "es-ES",resource.GetString("esES")  }
            };

            return Cultures;
        }

        /// <summary>
        /// Returns the name based in iso code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetLanguageByCode(string code)
        {
            KeyValuePair<string, string>? language = GetLanguages().FirstOrDefault(l => l.Key == code);

            return language.Value.Value;
        }

        /// <summary>
        /// Returns the iso code based in name
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetCodeByLanguage(string language)
        {
            KeyValuePair<string, string>? kvp = GetLanguages().FirstOrDefault(l => l.Value == language);
            return kvp.Value.Key;
        }

        /// <summary>
        /// Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "en-US"
        /// </summary>
        /// <param name="name" />Culture's name (e.g. en-US)</param>
        public static string GetImplementedCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture();

            if (!_validCultures.Any(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                return GetDefaultCulture();

            if (_cultures.Any(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                return name;

            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB", 
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)  
            var n = GetNeutralCulture(name);
            foreach (var c in _cultures.Where(c => c.StartsWith(n)))
                return c;

            return GetDefaultCulture();
        }

        /// <summary>
        /// Returns default culture name which is the first name decalared (e.g. en-US)
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return _cultures[0];
        }

        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public static string GetNeutralCulture(string name)
        {
            if (name.Length < 2)
                return name;
            return name.Substring(0, 2);
        }

        public static string GetGlobalResource(string name, string culture)
        {
            if (String.IsNullOrEmpty(culture))
            {
                culture = "pt-BR";
            }
            try
            {
                var resourceManager = new ResourceManager(typeof(Global));
                try
                {
                    var ci = new CultureInfo(culture);
                    return resourceManager.GetString(name, ci);
                }
                catch
                {
                    return resourceManager.GetString(name);
                }
            }
            catch
            {
                return name;
            }
        }

        public static string GetGlobalResources(string culture, params string[] names)
        {
            var lst = names.Select(name => GetGlobalResource(name, culture)).ToList();
            return String.Join(" ", lst);
        }

        public static string GetDateString(DateTime date, string culture)
        {
            try
            {
                var info = new CultureInfo(culture).DateTimeFormat;
                return date.ToString("d", info);
            }
            catch (Exception)
            {
                return date.ToString("d");
            }
        }
    }
}
