namespace CBT.Attributes;

using System;
using CBT.Types;

/// <summary>
/// FlyTextCategoryAttribute decorates a <see cref="FlyTextKind"/> with a Category.
/// </summary>
/// <param name="category">FlyTextCategory which is a collection of kinds.</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FlyTextCategoryAttribute(FlyTextCategory category) : Attribute
{
    /// <summary>
    /// Gets the FlyTextCategory.
    /// </summary>
    public FlyTextCategory Category { get; } = category;
}