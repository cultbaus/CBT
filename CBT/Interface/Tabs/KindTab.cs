namespace CBT.Interface.Tabs;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBT.FlyText.Configuration;
using CBT.Helpers;
using CBT.Types;

/// <summary>
/// CatgeoryTab configures settings for FlyTextKinds.
/// </summary>
public class KindTab : Tab<FlyTextKind>
{
    private static readonly List<FlyTextKind> KindPickerValues = [.. FlyTextKindExtension.GetAll()];

    private static FlyTextKind currentKind = FlyTextKindExtension.GetAll().First();

    private static Dictionary<FlyTextKind, FlyTextConfiguration>? tmpConfig;

    /// <inheritdoc/>
    public override string Name => TabKind.Kind.ToString();

    /// <inheritdoc/>
    public override TabKind Kind => TabKind.Kind;

    /// <inheritdoc/>
    protected override Dictionary<FlyTextKind, FlyTextConfiguration> TmpConfig
    {
        get =>
            tmpConfig ??= Service.Configuration.FlyTextKinds
                .ToDictionary(
                    entry => entry.Key,
                    entry => new FlyTextConfiguration(entry.Value));

        set
        {
            tmpConfig = value;
            Service.Configuration.FlyTextKinds = tmpConfig;
        }
    }

    /// <inheritdoc />
    protected override Dictionary<FlyTextKind, FlyTextConfiguration> Configuration
    {
        get => Service.Configuration.FlyTextKinds;
    }

    /// <inheritdoc/>
    protected override FlyTextKind Current
    {
        get => currentKind;
        set => currentKind = value;
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        Artist.DrawTitle("Kind Configuration Settings");

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

        Artist.DrawSeperator();
        Artist.ColoredButton("Save##Kind", sameLine: false, ButtonColors, this.OnSave);
    }

    /// <inheritdoc/>
    public override void OnClose()
    {
        this.ResetTmp();
        ResetCurrent();
    }

    /// <inheritdoc/>
    public override void ResetTmp()
    {
        this.TmpConfig = Service.Configuration.FlyTextKinds
            .ToDictionary(
                entry => entry.Key,
                entry => new FlyTextConfiguration(entry.Value));
    }

    private static void ResetCurrent()
    {
        currentKind = FlyTextKindExtension.GetAll().First();
    }

    private void OnSave()
    {
        this.TmpConfig.Keys.ToList().ForEach(kind =>
        {
            if (this.TmpConfig.TryGetValue(kind, out var currentConfig))
            {
                Service.Configuration.FlyTextKinds[kind] = new FlyTextConfiguration(currentConfig);
            }
        });

        this.ResetTmp();
    }
}