namespace CBT.FlyText.Configuration;

/// <summary>
/// FlyTextMessageConfiguration configures the fly text message.
/// </summary>
public class FlyTextMessageConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextMessageConfiguration"/> class.
    /// </summary>
    /// <param name="prefix">What to prefix the message with.</param>
    /// <param name="suffix">What to postfix the message with.</param>
    /// <param name="format">Whether or not to format the string.</param>
    public FlyTextMessageConfiguration(string prefix, string suffix, bool format)
    {
        this.Prefix = prefix;
        this.Suffix = suffix;
        this.Format = format;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextMessageConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Configuration to copy from.</param>
    public FlyTextMessageConfiguration(FlyTextMessageConfiguration toCopy)
    {
        this.Prefix = toCopy.Prefix;
        this.Suffix = toCopy.Suffix;
        this.Format = toCopy.Format;
    }

    /// <summary>
    /// Gets or sets the message prefix.
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// Gets or sets the message suffix.
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the string should be formatted.
    /// </summary>
    public bool Format { get; set; }
}