namespace Scroll;

using System;

using Dalamud.Game;

internal class PluginAddressResolver : BaseAddressResolver
{
    public IntPtr AddScreenLog { get; set; }

    protected override void Setup64Bit(ISigScanner scanner)
    {
        this.AddScreenLog = scanner.ScanText("E8 ?? ?? ?? ?? BF ?? ?? ?? ?? EB 39");

        Service.PluginLog.Debug($"{nameof(this.AddScreenLog)} 0x{this.AddScreenLog:X}");
    }
}