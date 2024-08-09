namespace CBT.FlyText;

using System;
using System.Collections.Concurrent;
using CBT.Types;

/// <summary>
/// Object pool for to re-use memory for FlyText Events and hopefully not thrash the gc.
/// </summary>
public class FlyTextPool : IDisposable
{
    private readonly ConcurrentBag<FlyTextEvent> pool = [];

    /// <summary>
    /// Get a fly text event instance.
    /// </summary>
    /// <returns>A flytext event.</returns>
    public FlyTextEvent Get()
        => this.pool.TryTake(out var e) ? e : new FlyTextEvent();

    /// <summary>
    /// Put a fly text event back into the pool.
    /// </summary>
    /// <param name="e">fly text event.</param>
    public void Put(FlyTextEvent e)
    {
        e.Reset();
        this.pool.Add(e);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.pool.Clear();
        GC.SuppressFinalize(this);
    }
}