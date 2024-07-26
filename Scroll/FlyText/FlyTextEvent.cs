namespace Scroll.Flytext;

using Dalamud.Game.Gui.FlyText;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

using Scroll.FlyText;

internal unsafe class FlyTextEvent
{
    internal FlyTextEvent(FlyTextKind kind, Character* target, Character* source, int option, int actionKind, int actionID, int val1, int val2, int val3, int val4)
    {
        FlyTextConfiguration config = Service.Configuration.FlyText[kind];

        this.Config = new FlyTextConfiguration(
            config.Font.Name,
            config.Font.Size,
            config.Font.Color,
            config.Outline.Size,
            config.Outline.Color,
            config.Animation.Kind,
            config.Animation.Duration);
        this.Target = target;
        this.Source = source;
        this.Option = option;
        this.ActionKind = actionKind;
        this.ActionID = actionID;
        this.Value1 = val1;
        this.Value2 = val2;
        this.Value3 = val3;
        this.Value4 = val4;
    }

    public FlyTextConfiguration Config { get; set; }

    public Character* Target { get; set; }

    public Character* Source { get; set; }

    public int Option { get; set; }

    public int ActionKind { get; set; }

    public int ActionID { get; set; }

    public int Value1 { get; set; }

    public int Value2 { get; set; }

    public int Value3 { get; set; }

    public int Value4 { get; set; }
}