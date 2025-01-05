namespace Adbliterator.utilities.misc;

public abstract class Logger {
    private static string Colorize(string text, string color) {
        return color.ToLower() switch {
            "red" => $"\e[31m{text}\e[0m",
            "yellow" => $"\e[33m{text}\e[0m",
            "blue" => $"\e[34m{text}\e[0m",
            "magenta" => $"\e[35m{text}\e[0m",
            "gray" => $"\e[90m{text}\e[0m",
            _ => text
        };
    }

    private static string GetPrefix(string logKind) {
        return logKind.ToLower() switch {
            "info" => Colorize("[ ", "gray") + Colorize("INFO", "blue") + Colorize(" ] ", "gray"),
            "warning" => Colorize("[ ", "gray") + Colorize("WARN", "yellow") + Colorize(" ] ", "gray"),
            "error" => Colorize("[ ", "gray") + Colorize("ERROR", "red") + Colorize(" ] ", "gray"),
            "blocked" => Colorize("[ ", "gray") + Colorize("BLOCKED", "red") + Colorize(" ] ", "gray"),
            "allowed" => Colorize("[ ", "gray") + Colorize("ALLOWED", "magenta") + Colorize(" ] ", "gray"),
            "screening" => Colorize("[ ", "gray") + Colorize("SCREENING", "magenta") + Colorize(" ] ", "gray"),
            _ => ""
        };
    }

    public static void Section(string sectionName) => Console.WriteLine($"+-+-+- {sectionName} -+-+-+");
    public static void Log(string text) => Console.WriteLine(GetPrefix("info") + text);
    public static void Warn(string warning) => Console.WriteLine(GetPrefix("warning") + warning);
    public static void Error(string error) => Console.WriteLine(GetPrefix("error") + error);
    public static void Blocked(string address) => Console.WriteLine(GetPrefix("blocked") + address);
    public static void Allowed(string address) => Console.WriteLine(GetPrefix("allowed") + address);
    public static void Screening(string address) => Console.WriteLine(GetPrefix("screening") + address);
}