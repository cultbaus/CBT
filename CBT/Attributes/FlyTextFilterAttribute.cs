namespace CBT.Attributes;

using System;
using CBT.Types;

/// <summary>
/// FlyTextFilter denotes if this text should always be filtered for a certain entity.
/// </summary>
public enum FlyTextFilter
{
    /// <summary>
    /// No-op.
    /// </summary>
    None,

    /// <summary>
    /// If the element should filter for self. This is probably unused.
    /// </summary>
    Self,

    /// <summary>
    /// If the element should filter for party members.
    /// </summary>
    Party,

    /// <summary>
    /// If the element should filter for party enemies.
    /// </summary>
    Enemy,
}

/// <summary>
/// FlyTextFilterAttribute decorates a <see cref="FlyTextKind"/> with a Filter.
/// </summary>
/// <param name="filter">The kind of filter.</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class FlyTextFilterAttribute(FlyTextFilter[] filter) : Attribute
{
    /// <summary>
    /// Gets the FlyTextFilter.
    /// </summary>
    public FlyTextFilter[] Filter { get; } = filter;
}