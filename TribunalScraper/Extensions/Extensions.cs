using System.Globalization;

namespace TribunalScraper.Extensions;

public static class Extensions
{
    public static string ToDatetime(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return null;

        if (DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            return date.ToString("yyyy-MM-dd");
        }
        else
        {
            return null;
        }
    }
}