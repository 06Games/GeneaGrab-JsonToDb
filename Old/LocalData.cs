using Newtonsoft.Json;

namespace GeneaGrab_JsonToDb.Old;

public static class LocalData
{
    private static string AppName => "GeneaGrab";

    /// <summary>Application appdata folder</summary>
    /// <returns>
    /// On Windows: %localappdata%\GeneaGrab
    /// On MacOS and Linux: ~/.local/share/GeneaGrab
    /// </returns>
    public static readonly string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);
    private static readonly DirectoryInfo RegistriesFolder = new(Path.Combine(AppData, "Registries"));

    public static async Task<List<Registry>> LoadDataAsync()
    {
        var registries = new List<Registry>();
        foreach (var reg in Directory.EnumerateFiles(RegistriesFolder.FullName, "Registry.json", SearchOption.AllDirectories).AsParallel())
        {
            Console.WriteLine("Processing " + reg);
            var data = await File.ReadAllTextAsync(reg);
            var registry = JsonConvert.DeserializeObject<Registry>(data);

            if (registry is null) Console.WriteLine("couldn't parse " + reg);
            registries.Add(registry!);
        }
        return registries;
    }
}
