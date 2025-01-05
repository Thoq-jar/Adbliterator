using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Adbliterator.utilities.config;

public static class Config {
    public static Settings? Settings;
    public static AdListConfig? AdList;
    
    public static void Load(string configDir) {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var settingsPath = Path.Combine(configDir, "config.yaml");
        var adListPath = Path.Combine(configDir, "ad_list.yaml");

        if (!File.Exists(settingsPath) || !File.Exists(adListPath)) {
            throw new FileNotFoundException("Required configuration files not found in the specified directory.");
        }

        var settingsYaml = File.ReadAllText(settingsPath);
        var settingsRoot = deserializer.Deserialize<Dictionary<string, Settings>>(settingsYaml);

        if (!settingsRoot.TryGetValue("SETTINGS", out var value)) {
            throw new KeyNotFoundException("SETTINGS key not found in config.yaml.");
        }

        Settings = value;

        var adListYaml = File.ReadAllText(adListPath);
        AdList = deserializer.Deserialize<AdListConfig>(adListYaml);

        if (Settings.Debug) {
            misc.Logger.Log($"Loaded configuration from: {configDir}");
        }
    }
}