// ReSharper disable UnusedMember.Global
// ReSharper disable CollectionNeverUpdated.Global
// Resharper disable ClassNeverInstantiated.Global
// Resharper disable AutoPropertyCanBeMadeGetOnly.Global
using YamlDotNet.Serialization;

namespace Adbliterator.utilities.config;

public class AdListConfig {
    [YamlMember(Alias = "AD_KEYWORDS", ApplyNamingConventions = false)]
    public List<string> AdKeywords { get; set; } = [];

    [YamlMember(Alias = "KNOWN_HOSTS", ApplyNamingConventions = false)]
    public List<string> KnownHosts { get; set; } = [];

    [YamlMember(Alias = "KNOWN_PATHS", ApplyNamingConventions = false)]
    public List<string> KnownPaths { get; set; } = [];

    [YamlMember(Alias = "FLASH_EXTENSIONS", ApplyNamingConventions = false)]
    public List<string> FlashExtensions { get; set; } = [];

    [YamlMember(Alias = "FLASH_MIME_TYPES", ApplyNamingConventions = false)]
    public List<string> FlashMimeTypes { get; set; } = [];
}
