namespace CBT;

using System.IO;
using System.Reflection;
using CBT.FlyText;
using CBT.Helpers;
using CBT.Interface;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

/// <summary>
/// Plugin is the main entrypoint for CBT.
/// </summary>
internal sealed partial class Plugin : IDalamudPlugin
{
    private static readonly string Command = "/cbt";

    private static readonly string Name = "CBT";

    private readonly WindowSystem windowSystem;

    private readonly ConfigWindow configWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class.
    /// </summary>
    /// <param name="pluginInterface">Dalamud plugin interface.</param>
    /// <param name="sigScanner">Dalamus signature scanner.</param>
    /// <param name="gameInteropProvider">Dalamud game interop provider.</param>
    public Plugin(IDalamudPluginInterface pluginInterface, ISigScanner sigScanner, IGameInteropProvider gameInteropProvider)
    {
        pluginInterface.Create<Service>();

        var overlayWindow = new OverlayWindow();
        var artist = new FlyTextArtist();
        var assemblyLocation = Service.Interface.AssemblyLocation.DirectoryName != null
            ? Service.Interface.AssemblyLocation.DirectoryName + "\\"
            : Assembly.GetExecutingAssembly().Location;

        this.configWindow = new ConfigWindow(Name);
        this.windowSystem = new WindowSystem(Name);

        this.windowSystem.AddWindow(this.configWindow);
        this.windowSystem.AddWindow(overlayWindow);

        Service.Address = new PluginAddressResolver();
        Service.Address.Setup(sigScanner);
        Service.CommandManager.AddHandler(Command, new CommandInfo(this.OnCommand)
        {
            HelpMessage = "Open a window to edit CBT settings.",
            ShowInHelp = true,
        });
        Service.Configuration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
        Service.Fonts = new FontManager(Path.GetDirectoryName(assemblyLocation) + "\\Media\\Fonts\\");
        Service.Interface.UiBuilder.OpenConfigUi += this.OnOpenConfigUi;
        Service.Interface.UiBuilder.OpenMainUi += this.OnOpenConfigUi;
        Service.Interface.UiBuilder.Draw += this.windowSystem.Draw;
        Service.Manager = new PluginManager(artist);
        Service.Manifest = new ManifestManager(assemblyLocation + Path.GetFileNameWithoutExtension(Service.Interface.AssemblyLocation.FullName) + ".json");
        Service.Receiver = new FlyTextReceiver(gameInteropProvider);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Service.Receiver.Dispose();
        Service.Manager.Dispose();
        Service.Manifest.Dispose();
        Service.Fonts.Dispose();
        Service.Interface.UiBuilder.Draw -= this.windowSystem.Draw;
        Service.Interface.UiBuilder.OpenMainUi -= this.OnOpenConfigUi;
        Service.Interface.UiBuilder.OpenConfigUi -= this.OnOpenConfigUi;
        Service.CommandManager.RemoveHandler(Command);
    }

    /// <summary>
    /// Sets the state of the configuration window when opened.
    /// </summary>
    private void OnOpenConfigUi()
        => this.configWindow.IsOpen = true;

    /// <summary>
    /// OnCommand reacts to commands received via <see cref="Command"/>.
    /// </summary>
    /// <param name="command">The command received.</param>
    /// <param name="arguments">Arguments received alongside the command.</param>
    private void OnCommand(string command, string arguments)
        => this.configWindow.Toggle();
}