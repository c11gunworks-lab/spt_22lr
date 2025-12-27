using Microsoft.Extensions.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using System.Reflection;
using Range = SemanticVersioning.Range;

// Changed namespace to match the mod's purpose
namespace Spt22lr;

public record ModMetadata : AbstractModMetadata
{
    // Updated ModGuid for consistency
    public override string ModGuid { get; init; } = "com.c11.spt_22lr"; 
    public override string Name { get; init; } = ".22 Long Rifle";
    public override string Author { get; init; } = "C11";
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");

    public override Range SptVersion { get; init; } = new("^4.0.8");

    public override string License { get; init; } = "MIT";
    public override bool? IsBundleMod { get; init; } = true;

    public override Dictionary<string, Range>? ModDependencies { get; init; } = new()
    {
        { "com.wtt.commonlib", new Range("~2.0.7") }
    };

    public override string? Url { get; init; }
    public override List<string>? Contributors { get; init; }
    public override List<string>? Incompatibilities { get; init; }
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
// Renamed the class for clarity (e.g., "Plugin")
public class Plugin(
    WTTServerCommonLib.WTTServerCommonLib wttCommon,
    // Corrected the ILogger generic type to use this class
    ILogger<Plugin> log 
) : IOnLoad
{
    public async Task OnLoad()
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Log resource names once while wiring things up
        foreach (var name in assembly.GetManifestResourceNames())
            // Updated log prefix for consistency
            log.LogDebug("[spt_22lr] Embedded resource: {Res}", name);

        // WTT ingestion
        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);
        await wttCommon.CustomLocaleService.CreateCustomLocales(assembly);

        log.LogInformation("Loaded your Plinking Dreams");
        await Task.CompletedTask;
    }
}