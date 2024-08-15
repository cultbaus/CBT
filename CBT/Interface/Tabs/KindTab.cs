namespace CBT.Interface.Tabs;

using System.Collections.Generic;
using System.Linq;
using CBT.Types;

/// <summary>
/// CatgeoryTab configures settings for FlyTextKinds.
/// </summary>
public class KindTab : Tab
{
    private static readonly List<FlyTextKind> KindPickerValues = [.. FlyTextKindExtension.GetAll().Where(FlyTextKindExtension.UnusedKindPredicate).OrderBy(k => k.ToString())];

    private static FlyTextKind currentKind = FlyTextKindExtension.GetAll().First();

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
        currentKind = FlyTextKindExtension.GetAll().First();
    }
}