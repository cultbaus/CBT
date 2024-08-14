namespace CBT;

using System;
using Dalamud.Game;

/// <summary>
/// Dalamud address resolver.
/// </summary>
public class PluginAddressResolver : BaseAddressResolver
{
    /// <summary>
    /// Gets the address of AddScreenLog.
    /// </summary>
    public IntPtr AddScreenLog { get; private set; }

    /// <summary>
    /// Gets the address of ReceiveActionEffect.
    /// </summary>
    public IntPtr ReceiveActionEffect { get; private set; }

    /// <inheritdoc/>
    protected override void Setup64Bit(ISigScanner scanner)
    {
        this.AddScreenLog = scanner.ScanText("E8 ?? ?? ?? ?? BF ?? ?? ?? ?? EB 39");

        this.ReceiveActionEffect = scanner.ScanText("40 55 56 57 41 54 41 55 41 56 48 8D AC 24");

        Service.PluginLog.Debug($"{nameof(this.AddScreenLog)}           0x{this.AddScreenLog:X}");
        Service.PluginLog.Debug($"{nameof(this.ReceiveActionEffect)}    0x{this.ReceiveActionEffect:X}");
    }
}