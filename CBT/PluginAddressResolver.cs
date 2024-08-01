namespace CBT;

using System;

using Dalamud.Game;

/// <summary>
/// Dalamud address resolver.
/// </summary>
public class PluginAddressResolver : BaseAddressResolver
{
    /// <summary>
    /// Gets the adress of AddScreenLog.
    /// </summary>
    public IntPtr AddScreenLog { get; private set; }

    /// <inheritdoc/>
    protected override void Setup64Bit(ISigScanner scanner)
    {
        this.AddScreenLog = scanner.ScanText("E8 ?? ?? ?? ?? BF ?? ?? ?? ?? EB 39");

        Service.PluginLog.Debug($"{nameof(this.AddScreenLog)} 0x{this.AddScreenLog:X}");
    }
}