namespace CBT.Helpers;

using System.Collections.Generic;
using Newtonsoft.Json;

public record Manifest
{
    /// <summary>
    /// Gets or initializes the Author of the plugin.
    /// </summary>
    [JsonProperty]
    public string? Author { get; init; }

    /// <summary>
    /// Gets or initializes the Name of the plugin.
    /// </summary>
    [JsonProperty]
    public string? Name { get; init; }

    /// <summary>
    /// Gets or initializes the Punchline of the plugin.
    /// </summary>
    [JsonProperty]
    public string? Punchline { get; init; }

    /// <summary>
    /// Gets or initializes the Description for the plugin.
    /// </summary>
    [JsonProperty]
    public string? Description { get; init; }

    /// <summary>
    /// Gets or initializes the RepoURL for the plugin.
    /// </summary>
    [JsonProperty]
    public string? RepoURL { get; init; }

    /// <summary>
    /// Gets or initializes the IconURL for the plugin.
    /// </summary>
    [JsonProperty]
    public string? IconURL { get; init; }

    /// <summary>
    /// Gets or initializes the Image URLs for the plugin.
    /// </summary>
    [JsonProperty]
    public List<string>? ImageURLs { get; init; }

    /// <summary>
    /// Gets or initializes the Dalamud API level for the plugin.
    /// </summary>
    [JsonProperty]
    public int? DalamuAPILevel { get; init; }

    /// <summary>
    /// Gets or initializes the Tags for the plugin.
    /// </summary>
    [JsonProperty]
    public List<string>? Tags { get; init; }
}