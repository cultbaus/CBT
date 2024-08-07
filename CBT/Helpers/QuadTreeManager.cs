namespace CBT.Helpers;

using System.Collections.Generic;
using Dalamud.Interface.Utility;

/// <summary>
/// Quad Tree manager. This should be smarter than it is, but for now, it just is.
/// </summary>
public class QuadTreeManager
{
    private static readonly Dictionary<uint, QuadTree> Cache = [];

    /// <summary>
    /// Get a quad tree for a target.
    /// </summary>
    /// <param name="objectID">Target ID.</param>
    /// <returns>A quadtree instance.</returns>
    public static QuadTree GetQuadTree(uint objectID)
    {
        if (!Cache.TryGetValue(objectID, out var quadTree))
        {
            var size = ImGuiHelpers.MainViewport.Size;
            quadTree = new QuadTree(0, new Rectangle(0, 0, size.X, size.Y));
            Cache[objectID] = quadTree;
        }

        return quadTree;
    }

    /// <summary>
    /// Clear the quad trees and the cache. This is not the best way, we can do better.
    /// </summary>
    public static void Clear()
    {
        foreach (var quadTree in Cache.Values)
        {
            quadTree.Clear();
        }

        Cache.Clear();
    }
}