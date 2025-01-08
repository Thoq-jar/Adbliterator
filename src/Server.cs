using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Adbliterator.utilities.blocking;
using Adbliterator.utilities.config;
using Adbliterator.utilities.domains;
using Adbliterator.utilities.misc;

namespace Adbliterator;

public abstract class Server {
    public static void Start() {
        var dnsPort = Config.Settings?.Port ?? 53;

        Logger.Info("Starting Adbliterator DNS Server...");

        var upstreamDns = Config.Settings?.Dns.Primary;

        using var udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, dnsPort));
        using var forwardClient = new UdpClient();

        Logger.Info($"DNS Proxy server is now listening :{dnsPort}...");
        var cts = new CancellationTokenSource();

        var stopWatch = new Stopwatch();
        var firstCancelPress = false;

        Console.CancelKeyPress += (_, e) => {
            if (!firstCancelPress) {
                Console.WriteLine();
                Logger.Info("Shutting down DNS Proxy (press Ctrl + C again to force quit)...");
                firstCancelPress = true;
                stopWatch.Start();
                cts.Cancel();
                e.Cancel = true;
            }
            else {
                if (stopWatch.Elapsed.TotalSeconds < 3) {
                    Console.WriteLine();
                    Logger.Error("Force quitting DNS Proxy...");
                    stopWatch.Stop();
                    Environment.Exit(0);
                }
                else {
                    Console.WriteLine();
                    Logger.Info("Restarting shutdown timer. Press Ctrl + C again to force exit.");
                    stopWatch.Restart();
                    firstCancelPress = true;
                    cts.Cancel();
                    e.Cancel = true;
                }
            }
        };

        try {
            while (!cts.Token.IsCancellationRequested) {
                var remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                var requestData = udpClient.Receive(ref remoteEndpoint);

                var domainName = Parser.ParseDomainName(requestData);

                if (CheckStatus.IsBlocked(domainName)) {
                    Logger.Blocked($"Blocked domain: {domainName}");
                    ResponseHandler.SendBlockedResponse(udpClient, remoteEndpoint, requestData);
                    continue;
                }

                Logger.Info($"Requesting resolution for: {domainName}");

                try {
                    if (upstreamDns != null) {
                        var upstreamEndpoint = new IPEndPoint(IPAddress.Parse(upstreamDns), 53);
                        forwardClient.Send(requestData, requestData.Length, upstreamEndpoint);
                    }

                    var upstreamResponseEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    var resolvedData = forwardClient.Receive(ref upstreamResponseEndpoint);

                    udpClient.Send(resolvedData, resolvedData.Length, remoteEndpoint);
                    Logger.Info($"Resolved domain: {domainName}");
                }
                catch (Exception ex) {
                    Logger.Error($"Upstream DNS resolution error: {ex.Message}");
                }
            }
        }
        finally {
            Logger.Info("DNS Proxy has stopped.");
        }
    }
}