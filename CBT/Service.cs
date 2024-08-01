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
public class Service
{
    /// <summary>
    /// Gets or sets the Dalamud plugin configuration.
    /// </summary>
    public static PluginConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Dalamud plugin address resolver.
    /// </summary>
    public static PluginAddressResolver Address { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CBT Plugin manager.
    /// </summary>
    public static PluginManager Manager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CBT FlyText receiver.
    /// </summary>
    public static FlyTextReceiver Receiver { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CBT Font manager.
    /// </summary>
    public static FontManager Fonts { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CBT Manifest manager.
    /// </summary>
    public static ManifestManager Manifest { get; set; } = null!;

    /// <summary>
    /// Gets the Dalamud ClientState.
    /// </summary>
    [PluginService]
    public static IClientState ClientState { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud ClientState.
    /// </summary>
    [PluginService]
    public static ICommandManager CommandManager { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud ClientState.
    /// </summary>
    [PluginService]
    public static IDalamudPluginInterface Interface { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud FlyTextGui.
    /// </summary>
    [PluginService]
    public static IFlyTextGui FlyTextGui { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud Framework.
    /// </summary>
    [PluginService]
    public static IFramework Framework { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud GameGui.
    /// </summary>
    [PluginService]
    public static IGameGui GameGui { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud PluginLog.
    /// </summary>
    [PluginService]
    public static IPluginLog PluginLog { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud TargetManager.
    /// </summary>
    [PluginService]
    public static ITargetManager TargetManager { get; private set; } = null!;

    /// <summary>
    /// Gets the Dalamud TextureProvider.
    /// </summary>
    [PluginService]
    public static ITextureProvider TextureProvider { get; private set; } = null!;
}