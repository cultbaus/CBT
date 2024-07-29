namespace Scroll;

using System.IO;
using System.Reflection;

using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

using Scroll.FlyText;
using Scroll.Interface;
using Scroll.Helpers;

internal sealed partial class Plugin : IDalamudPlugin
{
    private static readonly string Command = "/scroll";
    private static readonly string Name = "Scroll";
    private readonly WindowSystem windowSystem;
    private readonly ConfigWindow configWindow;
    private readonly OverlayWindow overlayWindow;
    private readonly FlyTextArtist artist;
    private readonly string assemblyLocation;

    public Plugin(IDalamudPluginInterface pluginInterface, ISigScanner sigScanner, IGameInteropProvider gameInteropProvider)
    {
        pluginInterface.Create<Service>();


        Service.Configuration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();

        Service.Address = new PluginAddressResolver();
        Service.Address.Setup(sigScanner);

        this.configWindow = new ConfigWindow();
        this.overlayWindow = new OverlayWindow();

        this.windowSystem = new WindowSystem(Name);
        this.windowSystem.AddWindow(this.configWindow);
        this.windowSystem.AddWindow(this.overlayWindow);

        this.artist = new FlyTextArtist();

        this.assemblyLocation = Service.Interface.AssemblyLocation.DirectoryName != null
            ? Service.Interface.AssemblyLocation.DirectoryName + "\\"
            : Assembly.GetExecutingAssembly().Location;

        Service.Fonts = new FontManager(Path.GetDirectoryName(this.assemblyLocation) + "\\Media\\Fonts\\");
        Service.Receiver = new FlyTextReceiver(gameInteropProvider);
        Service.Manager = new PluginManager(artist);

        Service.Interface.UiBuilder.OpenConfigUi += this.OnOpenConfigUi;
        Service.Interface.UiBuilder.Draw += this.windowSystem.Draw;

        Service.CommandManager.AddHandler(Command, new CommandInfo(this.OnCommand)
        {
            HelpMessage = "Open a window to edit Scroll settings.",
            ShowInHelp = true,
        });
    }

    public void Dispose()
    {
        Service.CommandManager.RemoveHandler(Command);

        Service.Interface.UiBuilder.OpenConfigUi -= this.OnOpenConfigUi;
        Service.Interface.UiBuilder.Draw -= this.windowSystem.Draw;

        Service.Manager.Dispose();
        Service.Receiver.Dispose();

        Service.Fonts.Dispose();
    }

    private void OnOpenConfigUi()
        => this.configWindow.IsOpen = true;

    private void OnCommand(string command, string arguments)
        => this.configWindow.Toggle();
}