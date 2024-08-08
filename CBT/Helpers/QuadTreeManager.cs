namespace CBT.Helpers;

using System;
using System.Collections.Generic;
using Dalamud.Interface.Utility;

/// <summary>
/// Quad Tree manager. This should be smarter than it is, but for now, it just is.
/// </summary>
public class QuadTreeManager : IDisposable
{
    private readonly Dictionary<uint, QuadTree> cache = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadTreeManager"/> class.
    /// </summary>
    public QuadTreeManager()
    {
        this.cache = [];
    }

    /// <summary>
    /// Get a quad tree for a target.
    /// </summary>
    /// <param name="objectID">Target ID.</param>
    /// <returns>A quadtree instance.</returns>
    public QuadTree GetQuadTree(uint objectID)
    {
        if (!this.cache.TryGetValue(objectID, out var quadTree))
        {
            var size = ImGuiHelpers.MainViewport.Size;
            quadTree = new QuadTree(0, new Rectangle(0, 0, size.X, size.Y));
            this.cache[objectID] = quadTree;
        }

        return quadTree;
    }

    /// <summary>
    /// Clear the quad trees and the cache. This is not the best way, we can do better.
    /// </summary>
    public void Clear()
    {
        foreach (var quadTree in this.cache.Values)
        {
            quadTree.Clear();
        }

        this.cache.Clear();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Clear();
        GC.SuppressFinalize(this);
    }
}