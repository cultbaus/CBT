namespace CBT;

using CBT.FlyText;
using CBT.Helpers;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

/// <summary>
/// Dalamud plugin services.
/// </summary>
internal class Service
{
    /// <summary>
    /// Gets or sets the Dalamud plugin configuration.
    /// </summary>
    internal static PluginConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Dalamud plugin address resolver.
    /// </summary>
    internal static PluginAddressResolver Address { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CBT Plugin manager.
    /// </summary>
    internal static PluginManager Manager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CBT FlyText receiver.
    /// </summary>
    internal static FlyTextReceiver Receiver { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CBT Font manager.
    /// </summary>
    internal static FontManager Fonts { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CBT Manifest manager.
    /// </summary>
    internal static ManifestManager Manifest { get; set; } = null!;

    /// <summary>
    /// Gets the Dalamud ClientState.
    /// </summary>
    [PluginService]
    internal static IClientState ClientState { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud ClientState.
    /// </summary>
    [PluginService]
    internal static ICommandManager CommandManager { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud ClientState.
    /// </summary>
    [PluginService]
    internal static IDalamudPluginInterface Interface { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud FlyTextGui.
    /// </summary>
    [PluginService]
    internal static IFlyTextGui FlyTextGui { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud Framework.
    /// </summary>
    [PluginService]
    internal static IFramework Framework { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud GameGui.
    /// </summary>
    [PluginService]
    internal static IGameGui GameGui { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud PluginLog.
    /// </summary>
    [PluginService]
    internal static IPluginLog PluginLog { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud TargetManager.
    /// </summary>
    [PluginService]
    internal static ITargetManager TargetManager { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud TextureProvider.
    /// </summary>
    [PluginService]
    internal static ITextureProvider TextureProvider { get; private set; } = null!;
}