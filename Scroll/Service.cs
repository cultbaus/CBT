namespace Scroll;

using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

using Scroll.FlyText;
using Scroll.Helpers;

internal class Service
{
    internal static PluginConfiguration Configuration { get; set; } = null!;

    internal static PluginAddressResolver Address { get; set; } = null!;

    internal static PluginManager Manager { get; set; } = null!;

    internal static FlyTextReceiver Receiver { get; set; } = null!;

    internal static FontManager Fonts { get; set; } = null!;

    [PluginService]
    internal static IClientState ClientState { get; private set; } = null!;

    [PluginService]
    internal static ICommandManager CommandManager { get; private set; } = null!;

    [PluginService]
    internal static IDalamudPluginInterface Interface { get; private set; } = null!;

    [PluginService]
    internal static IFlyTextGui FlyTextGui { get; private set; } = null!;

    [PluginService]
    internal static IFramework Framework { get; private set; } = null!;

    [PluginService]
    internal static IGameGui GameGui { get; private set; } = null!;

    [PluginService]
    internal static IPluginLog PluginLog { get; private set; } = null!;

    [PluginService]
    internal static ITargetManager TargetManager { get; private set; } = null!;
}