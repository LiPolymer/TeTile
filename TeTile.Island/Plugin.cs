using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Extensions.Registry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TeTile.Island.Services;
using TeTile.Island.SettingsPages;

namespace TeTile.Island;

[PluginEntrance]
// ReSharper disable once UnusedType.Global
public class Plugin : PluginBase {
    public override void Initialize(HostBuilderContext context,IServiceCollection services) {
        TeTileUpdaterService.ConfigDir = PluginConfigFolder;
        services.AddHostedService<TeTileUpdaterService>();
        services.AddSettingsPage<SettingsPage>();
    }
}