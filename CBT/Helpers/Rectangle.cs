namespace CBT.Helpers;

using System.Numerics;

/// <summary>
/// Initializes a new instance of the <see cref="Rectangle"/> class.
/// </summary>
/// <param name="x">X size.</param>
/// <param name="y">Y size.</param>
/// <param name="width">Width.</param>
/// <param name="height">Height.</param>
public class Rectangle(float x, float y, float width, float height)
{
    /// <summary>
    /// Gets or sets the X.
    /// </summary>
    public float X { get; set; } = x;

    /// <summary>
    /// Gets or sets the Y.
    /// </summary>
    public float Y { get; set; } = y;

    /// <summary>
    /// Gets or sets the Width.
    /// </summary>
    public float Width { get; set; } = width;

    /// <summary>
    /// Gets or sets the Height.
    /// </summary>
    public float Height { get; set; } = height;

    /// <summary>
    /// Checks if a point is within a rectangle.
    /// </summary>
    /// <param name="point">Point.</param>
    /// <returns>A bool indicating if a point is within a rectangle.</returns>
    public bool Contains(Vector2 point)
        => point.X >= this.X && point.X < this.X + this.Width &&
               point.Y >= this.Y && point.Y < this.Y + this.Height;

    /// <summary>
    /// Checks if two rectangles intersect.
    /// </summary>
    /// <param name="other">The other rectangle.</param>
    /// <returns>A bool indicating if they overlap.</returns>
    public bool Intersects(Rectangle other)
        => !(this.X + this.Width < other.X ||
                 this.X > other.X + other.Width ||
                 this.Y + this.Height < other.Y ||
                 this.Y > other.Y + other.Height);
}