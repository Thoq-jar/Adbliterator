﻿using Adbliterator.utilities.config;
using Adbliterator.utilities.misc;

namespace Adbliterator;

internal static class Program {
    public static void Main(string[] args) {
        Logger.Info("Starting Adbliterator...");

        var configDir = ".";

        for (var i = 0; i < args.Length; i++) {
            if (args[i] != "--config-dir" || i + 1 >= args.Length) continue;
            configDir = args[i + 1];
            break;
        }

        if (!Directory.Exists(configDir)) {
            Logger.Warn($"Config directory \"{configDir}\" does not exist.");
            Logger.Error("Adbliterator encountered a fatal error!");
            return;
        }

        try {
            Config.Load(configDir);
            Server.Start();
        }
        catch (Exception ex) {
            Logger.Error($"Failed to start proxy. {ex.Message}");
            if (ex.Message == "Permission denied") {
                Logger.Log("Is the address already in use?");
            }
        }
    }
}