namespace CBT.Helpers;

using System.Collections.Generic;
using System.Linq;
using CBT.Types;

/// <summary>
/// Initializes a new instance of the <see cref="QuadTree"/> class.
/// </summary>
/// <remarks>
/// We need a quad tree because the naive implementation wasn't fucking working.
/// </remarks>
/// <param name="level">Level.</param>
/// <param name="bounds">Bounds.</param>
public class QuadTree(int level, Rectangle bounds)
{
    private const int MaxEvents = 10;
    private const int MaxLevels = 5;

    private readonly int level = level;
    private readonly List<FlyTextEvent> events = [];
    private readonly Rectangle bounds = bounds;
    private readonly QuadTree[] nodes = new QuadTree[4];

    /// <summary>
    /// Clear the tree.
    /// </summary>
    public void Clear()
    {
        this.events.Clear();
        for (int i = 0; i < this.nodes.Length; i++)
        {
            if (this.nodes[i] != null)
            {
                this.nodes[i].Clear();
                this.nodes[i] = null!;
            }
        }
    }

    /// <summary>
    /// Insert a new event into the tree.
    /// </summary>
    /// <param name="e">Event.</param>
    public void Insert(FlyTextEvent e)
    {
        if (this.nodes[0] != null)
        {
            var index = this.GetIndex(e);
            if (index != -1)
            {
                this.nodes[index].Insert(e);
                return;
            }
        }

        this.events.Add(e);

        if (this.events.Count > MaxEvents && this.level < MaxLevels)
        {
            if (this.nodes[0] == null)
            {
                this.Split();
            }

            var i = 0;
            while (i < this.events.Count)
            {
                int index = this.GetIndex(this.events[i]);
                if (index != -1)
                {
                    this.nodes[index].Insert(this.events[i]);
                    this.events.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }

    /// <summary>
    /// Retreieve a copy of potential collisions.
    /// </summary>
    /// <param name="potentialCollisions">Events.</param>
    /// <param name="e">Event.</param>
    /// <returns>Potential collisions for this event.</returns>
    public List<FlyTextEvent> Retrieve(List<FlyTextEvent> potentialCollisions, FlyTextEvent e)
    {
        var index = this.GetIndex(e);
        if (index != -1 && this.nodes[0] != null)
        {
            this.nodes[index].Retrieve(potentialCollisions, e);
        }

        // Only interested in collisions where animation kinds are the same.
        potentialCollisions.AddRange(this.events.Where(p => p.Config.Animation.Kind == e.Config.Animation.Kind));

        return potentialCollisions;
    }

    private void Split()
    {
        var subWidth = this.bounds.Width / 2;
        var subHeight = this.bounds.Height / 2;
        var x = this.bounds.X;
        var y = this.bounds.Y;

        this.nodes[0] = new QuadTree(this.level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
        this.nodes[1] = new QuadTree(this.level + 1, new Rectangle(x, y, subWidth, subHeight));
        this.nodes[2] = new QuadTree(this.level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
        this.nodes[3] = new QuadTree(this.level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
    }

    private int GetIndex(FlyTextEvent e)
    {
        var index = -1;
        var rect = new Rectangle(e.Position.X, e.Position.Y, e.Size.X, e.Size.Y);
        var verticalMidpoint = this.bounds.X + (this.bounds.Width / 2);
        var horizontalMidpoint = this.bounds.Y + (this.bounds.Height / 2);

        var topQuadrant = rect.Y < horizontalMidpoint && rect.Y + rect.Height < horizontalMidpoint;
        var bottomQuadrant = rect.Y > horizontalMidpoint;

        if (rect.X < verticalMidpoint && rect.X + rect.Width < verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 1;
            }
            else if (bottomQuadrant)
            {
                index = 2;
            }
        }
        else if (rect.X > verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 0;
            }
            else if (bottomQuadrant)
            {
                index = 3;
            }
        }

        return index;
    }
}