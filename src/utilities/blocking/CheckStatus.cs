using Adbliterator.utilities.config;
using Adbliterator.utilities.domains;
using Adbliterator.utilities.misc;

namespace Adbliterator.utilities.blocking;

public class CheckStatus {
    public static bool IsBlocked(string domainName) {
        Console.WriteLine();
        Logger.Section(domainName);
        if (domainName.EndsWith(".lan", StringComparison.OrdinalIgnoreCase)) domainName = domainName[..^4];

        Logger.Screening(domainName);

        if (Config.AdList != null) {
            if (Config.AdList.KnownHosts.Any(host =>
                    host.Equals(domainName, StringComparison.OrdinalIgnoreCase))) {
                Logger.Blocked($"Exact match found in KnownHosts: {domainName}");
                return true;
            }

            if (Config.AdList.AdKeywords.Any(keyword =>
                    domainName.Contains(keyword, StringComparison.OrdinalIgnoreCase))) {
                Logger.Blocked($"Keyword match found in AdKeywords: {domainName}");
                return true;
            }

            var path = Parser.ExtractPathFromDomain(domainName);
            if (Config.AdList.KnownPaths.Any(knownPath =>
                    path.StartsWith(knownPath, StringComparison.OrdinalIgnoreCase))) {
                Logger.Blocked($"Prefix match found in KnownPaths: {path}");
                return true;
            }
        }

        Logger.Allowed($"No match found for: {domainName}");
        return false;
    }
}