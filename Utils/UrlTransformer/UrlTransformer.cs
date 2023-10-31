using System.Text.RegularExpressions;

namespace WebApplication1.Utils.UrlTransformer
{
    public class UrlTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null)
            {
                return null;
            }

            return Regex
                .Replace(
                    value.ToString()!,
                    "([a-z])([A-Z])",
                    "$1-$2",
                    RegexOptions.CultureInvariant,
                    TimeSpan.FromMilliseconds(100)
                )
                .ToLowerInvariant();
        }
    }
}
