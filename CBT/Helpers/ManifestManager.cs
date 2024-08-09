namespace CBT.Helpers;

using System;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// Initializes a new instance of the <see cref="ManifestManager"/> class.
/// </summary>
/// <remarks>
/// ManifestManager loads the Plugin manifest from the assembly directory.
/// </remarks>
/// <param name="manifestPath">Path to the Assembly directory where manifest is located.</param>
public class ManifestManager(string manifestPath) : IDisposable
{
    /// <summary>
    /// Gets the PluginManifest.
    /// </summary>
    public Manifest? Manifest { get; private set; } = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(manifestPath)) ?? new Manifest();

    /// <inheritdoc/>
    public void Dispose()
    {
    }
}