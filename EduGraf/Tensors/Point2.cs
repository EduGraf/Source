﻿namespace EduGraf.Tensors;

// This represents an absolute position in 2d-space.
public class Point2(float x, float y) : Coordinate2(x, y)
{
    public static readonly Point2 Origin = new(0, 0);

    public Point2(Vector2 vector)
        : this(vector.X, vector.Y)
    {
    }

    // Convert absolute to relative.
    public Vector2 Vector => new(X, Y);

    public static Point2 operator +(Point2 l, Vector2 r) => new(l.X + r.X, l.Y + r.Y);

    public static Point2 operator -(Point2 l, Vector2 r) => new(l.X - r.X, l.Y - r.Y);

    public static Vector2 operator -(Point2 l, Point2 r) => new(l.X - r.X, l.Y - r.Y);

    // Interpolate linearly.
    public static Point2 Combine(float wa /* weight of a, the weight of b is implicitly 1-wa. */, Point2 a, Point2 b)
    {
        return a + wa * (b - a);
    }
}
