namespace CBT.FlyText;

using System;
using CBT.Attributes;
using CBT.FlyText.Types;

/// <summary>
/// FlyTextKinds implements methods on the FlyTextKind enum.
/// </summary>
public static class FlyTextKindMethods
{
    /// <summary>
    /// Gets the category for a given FlyTextKind.
    /// </summary>
    /// <param name="kind">FlyTextKind enum.</param>
    /// <returns>Category for which the kind is a member.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the kind has no category.</exception>
    public static FlyTextCategory GetCategory(this FlyTextKind kind)
    {
        var attr = typeof(FlyTextKind)
            .GetMember(kind.ToString())[0]
            .GetCustomAttributes(typeof(FlyTextCategoryAttribute), false);

        return attr.Length > 0
            ? ((FlyTextCategoryAttribute)attr[0]).Category
            : throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
    }

    /// <summary>
    /// Checks if a kind is in a category.
    /// </summary>
    /// <param name="kind">FlyTextKind to look up.</param>
    /// <param name="category">FlyTextCategory to check if kind is a member of.</param>
    /// <returns>True if the kind is a member of that category.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the category is a group.</exception>
    public static bool InCategory(this FlyTextKind kind, FlyTextCategory category)
        => category.IsCategory()
            ? GetCategory(kind) == category
            : throw new ArgumentOutOfRangeException(nameof(category), category, null);

    /// <summary>
    /// Checks if a kind is in a group.
    /// </summary>
    /// <param name="kind">FlyTextKind to look up.</param>
    /// <param name="group">FlyTextCategory to check if kind is a member of.</param>
    /// <returns>True if the kind is a member of that group.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the group is a category.</exception>
    public static bool InGroup(this FlyTextKind kind, FlyTextCategory group)
        => group.IsGroup()
            ? GetCategory(kind).HasFlag(group)
            : throw new ArgumentOutOfRangeException(nameof(group), group, null);
}