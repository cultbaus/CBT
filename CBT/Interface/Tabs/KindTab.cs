namespace CBT.Interface.Tabs;

using System.Collections.Generic;
using System.Linq;
using CBT.FlyText.Configuration;
using CBT.Types;

/// <summary>
/// CatgeoryTab configures settings for FlyTextKinds.
/// </summary>
public class KindTab : Tab
{
    private static readonly List<FlyTextKind> KindPickerValues = [.. FlyTextKindExtension.GetAll()];

    private static FlyTextKind currentKind = FlyTextKindExtension.GetAll().First();

    private static Dictionary<FlyTextKind, FlyTextConfiguration>? tmpConfig;

    /// <inheritdoc/>
    public override string Name => "Configuration";

    /// <inheritdoc/>
    protected override FlyTextKind Current
    {
        get => currentKind;
        set => currentKind = value;
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        GuiArtist.DrawTitle("Kind Configuration Settings");

        this.DrawCurrentConfigurations(KindPickerValues);

        if (this.CurrentEnabled)
        {
            this.DrawFontConfigurations();

            if (this.Current.InCategory(FlyTextCategory.AbilityDamage))
            {
                this.DrawPositionalPickers();
            }

            this.DrawIconConfigurations();
            this.DrawAnimationConfigurations();
        }

        GuiArtist.DrawSeperator();
    }

    /// <inheritdoc/>
    public override void OnClose()
    {
        ResetCurrent();
    }

    private static void ResetCurrent()
    {
        currentKind = FlyTextKindExtension.GetAll().First();
    }
}