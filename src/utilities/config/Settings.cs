// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global
namespace Adbliterator.utilities.config;

public class Settings {
    public bool Debug { get; set; }
    public DnsConfig Dns { get; set; } = new();
}
