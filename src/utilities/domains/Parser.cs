using System.Text;

namespace Adbliterator.utilities.domains;

public class Parser {
    public static string ParseDomainName(byte[] requestData) {
        var position = 12;
        var domainName = new StringBuilder();

        while (position < requestData.Length) {
            var segmentLength = requestData[position++];

            if (segmentLength == 0) break;

            if (position + segmentLength > requestData.Length) {
                throw new Exception("Invalid DNS request format.");
            }

            var segment = Encoding.ASCII.GetString(requestData, position, segmentLength);
            position += segmentLength;

            if (domainName.Length > 0)
                domainName.Append('.');

            domainName.Append(segment);
        }

        return domainName.ToString();
    }

    public static string ExtractPathFromDomain(string domainName) {
        var domainParts = domainName.Split('/', 2);
        return domainParts.Length > 1 ? "/" + domainParts[1] : "/";
    }
}