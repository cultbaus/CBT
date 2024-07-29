namespace Scroll;

using System;

using Dalamud.Game;

internal class PluginAddressResolver : BaseAddressResolver
{
    protected override void Setup64Bit(ISigScanner scanner)
    {
        this.AddScreenLog = scanner.ScanText("E8 ?? ?? ?? ?? BF ?? ?? ?? ?? EB 39");

        Service.PluginLog.Debug($"{nameof(this.AddScreenLog)} 0x{this.AddScreenLog:X}");
    }

    internal IntPtr AddScreenLog { get; set; }
}