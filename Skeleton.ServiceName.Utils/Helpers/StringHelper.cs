using System.Globalization;
using System.Linq;
using System.Text;

namespace Skeleton.ServiceName.Utils.Helpers
{
    public static class StringHelper
    {
        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(
                  text.Normalize(NormalizationForm.FormD)
                  .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                                UnicodeCategory.NonSpacingMark)
                ).Normalize(NormalizationForm.FormC);
        }
    }
}
