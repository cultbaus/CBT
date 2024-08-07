namespace CBT.Helpers;

using System.Collections.Generic;
using Dalamud.Interface.Utility;

/// <summary>
/// Quad Tree manager. This should be smarter than it is, but for now, it just is.
/// </summary>
public class QuadTreeManager
{
    private static readonly Dictionary<(uint Target, bool Reversed), QuadTree> Cache = [];

    /// <summary>
    /// Get a quad tree for a target.
    /// </summary>
    /// <param name="objectID">Target ID.</param>
    /// <param name="reversed">Animation direction of the tree.</param>
    /// <returns>A quadtree instance.</returns>
    public static QuadTree GetQuadTree(uint objectID, bool reversed)
    {
        if (!Cache.TryGetValue((Target: objectID, Reversed: reversed), out var quadTree))
        {
            var size = ImGuiHelpers.MainViewport.Size;
            quadTree = new QuadTree(0, new Rectangle(0, 0, size.X / 10, size.Y / 10));
            Cache[(Target: objectID, Reversed: reversed)] = quadTree;
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