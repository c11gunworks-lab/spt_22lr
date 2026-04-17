using Microsoft.Extensions.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using System.Reflection;
using Range = SemanticVersioning.Range;

namespace Spt22lr;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.c11.spt22lr"; 
    public override string Name { get; init; } = ".22 Long Rifle";
    public override string Author { get; init; } = "C11";
    public override SemanticVersioning.Version Version { get; init; } = new("1.5.0");

    public override Range SptVersion { get; init; } = new("^4.0.10");

    public override string License { get; init; } = "MIT";
    public override bool? IsBundleMod { get; init; } = true;

    public override Dictionary<string, Range>? ModDependencies { get; init; } = new()
    {
        { "com.wtt.commonlib", new Range("~2.0.18") }
    };

    public override string? Url { get; init; }
    public override List<string>? Contributors { get; init; }
    public override List<string>? Incompatibilities { get; init; }
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
public class Plugin(
    WTTServerCommonLib.WTTServerCommonLib wttCommon,
    ILogger<Plugin> log 
) : IOnLoad
{
    public async Task OnLoad()
    {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var name in assembly.GetManifestResourceNames())
            log.LogDebug("[spt_22lr] Embedded resource: {Res}", name);

        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);
        await wttCommon.CustomLocaleService.CreateCustomLocales(assembly);
    	await wttCommon.CustomAssortSchemeService.CreateCustomAssortSchemes(assembly);


        log.LogInformation("Loaded your Plinking Dreams");
        await Task.CompletedTask;
    }
}