namespace CBT;

using System.IO;
using System.Reflection;

using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

using CBT.FlyText;
using CBT.Interface;
using CBT.Helpers;

internal sealed partial class Plugin : IDalamudPlugin
{
    private static readonly string command = "/cbt";
    private static readonly string name = "CBT";

    private readonly WindowSystem windowSystem;
    private readonly ConfigWindow configWindow;

    public Plugin(IDalamudPluginInterface pluginInterface, ISigScanner sigScanner, IGameInteropProvider gameInteropProvider)
    {
        pluginInterface.Create<Service>();

        var overlayWindow = new OverlayWindow();
        var artist = new FlyTextArtist();
        var assemblyLocation = Service.Interface.AssemblyLocation.DirectoryName != null
            ? Service.Interface.AssemblyLocation.DirectoryName + "\\"
            : Assembly.GetExecutingAssembly().Location;

        this.configWindow = new ConfigWindow(name);
        this.windowSystem = new WindowSystem(name);

        windowSystem.AddWindow(configWindow);
        windowSystem.AddWindow(overlayWindow);

        Service.Address = new PluginAddressResolver();
        Service.Address.Setup(sigScanner);
        Service.CommandManager.AddHandler(command, new CommandInfo(this.OnCommand)
        {
            HelpMessage = "Open a window to edit CBT settings.",
            ShowInHelp = true,
        });
        Service.Configuration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
        Service.Fonts = new FontManager(Path.GetDirectoryName(assemblyLocation) + "\\Media\\Fonts\\");
        Service.Interface.UiBuilder.OpenConfigUi += this.OnOpenConfigUi;
        Service.Interface.UiBuilder.Draw += windowSystem.Draw;
        Service.Manager = new PluginManager(artist);
        Service.Receiver = new FlyTextReceiver(gameInteropProvider);
    }

    public void Dispose()
    {
        Service.Receiver.Dispose();
        Service.Manager.Dispose();
        Service.Interface.UiBuilder.Draw -= windowSystem.Draw;
        Service.Interface.UiBuilder.OpenConfigUi -= this.OnOpenConfigUi;
        Service.Fonts.Dispose();
        Service.CommandManager.RemoveHandler(command);
    }

    private void OnOpenConfigUi()
        => this.configWindow.IsOpen = true;

    private void OnCommand(string command, string arguments)
        => this.configWindow.Toggle();
}