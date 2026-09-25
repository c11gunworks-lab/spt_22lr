using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;

namespace Spt22lr;

// Preload + 1: one step ahead of True North (Preload + 2), which uses this mod's items
[Injectable(TypePriority = OnLoadOrder.Preload + 1), UsedImplicitly]
public class Plugin(
    WTTServerCommonLib.WTTServerCommonLib wttCommon,
    ILogger<Plugin> log
) : IOnLoad
{
    public async Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var name in assembly.GetManifestResourceNames())
        {
            log.LogDebug("[spt_22lr] Embedded resource: {Res}", name);
        }

        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);
        await wttCommon.CustomLocaleService.CreateCustomLocales(assembly);
        await wttCommon.CustomAssortSchemeService.CreateCustomAssortSchemes(assembly);

        log.LogInformation("Loaded your Plinking Dreams");
    }
}
