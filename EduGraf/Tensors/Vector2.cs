using System;

namespace EduGraf.Tensors;

// This represents a displacement in 2d-space.
public class Vector2(float x, float y) : Coordinate2(x, y)
{
    public static readonly Vector2 Zero = new(0, 0);
    public static readonly Vector2 UnitX = new(1, 0);
    public static readonly Vector2 UnitY = new(0, 1);


    // Return the euclidean length of this vector.
    public float Length() => MathF.Sqrt(Dot(this, this));

    // Ditto.
    public static float Dot(Vector2 l, Vector2 r) => l.X * r.X + l.Y * r.Y;

    // Return a new vector in the same direction with length 1.
    public static Vector2 Normalize(Vector2 vector) => 1 / vector.Length() * vector;

    public static Vector2 operator +(Vector2 l, Vector2 r) => new(l.X + r.X, l.Y + r.Y);

    public static Vector2 operator -(Vector2 l, Vector2 r) => new(l.X - r.X, l.Y - r.Y);

    public static Vector2 operator *(float scalar, Vector2 v) => new(scalar * v.X, scalar * v.Y);

    public static Vector2 operator *(Vector2 v, float scalar) => new(scalar * v.X, scalar * v.Y);

    public static Vector2 operator *(Vector2 l, Vector2 r) => new(l.X * r.X, l.Y * r.Y);
}