namespace CBT.FlyText.Types;

/// <summary>
/// ActionEventKind are derived from the Action Event Handler on the target of a FlyTextEvent.
/// This is, clearly, not exhaustive.
/// </summary>
public enum ActionEventKind
{
    /// <summary>
    /// None.
    /// </summary>
    None = 0,

    /// <summary>
    /// Damage event. Applies to both abilities and auto attacks. Has some additional parameters which coincide with criticals/dh/dhc.
    /// </summary>
    Damage = (byte)3,

    /// <summary>
    /// Healing events, includes HpDrain. Has some additional parameters which coincide with criticals/dh/dhc.
    /// </summary>
    Healing = (byte)4,

    /// <summary>
    /// PlayerAppliedDebuff - maybe. Some ambiguity around this.
    /// </summary>
    PlayerAppliedDebuff = (byte)14,

    /// <summary>
    /// PlayerAppliedBuff - maybe. Some ambiguity around this.
    /// </summary>
    PlayerAppliedBuff = (byte)15,

    /// <summary>
    /// Some sort of combo action. Has some additional parameters which coincide with combo state.
    /// </summary>
    ComboAction = (byte)27,

    /// <summary>
    /// Changes to the state of the Job Gauge.
    /// </summary>
    JobGauge = (byte)62,
}