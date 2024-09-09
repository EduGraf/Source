using System;

namespace EduGraf.Tensors;

// This represents a displacement in 4d-space, i.e. homogeneous coordinates.
public class Vector4 : Coordinate4
{
    public Vector4(float x, float y, float z, float w)
        : base(x, y, z, w)
    {
    }

    // Get the coordinate-value by index.
    public float this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        3 => W,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    // Get this vector without the homogeneous coordinate.
    public Vector3 Xyz => new(X, Y, Z);

    // Return the euclidean length of this vector.
    public float Length() => MathF.Sqrt(Dot(this, this));

    // Ditto.
    public static float Dot(Vector4 l, Vector4 r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z + l.W * r.W;

    // Return a new vector in the same direction with length 1.
    public static Vector4 Normalize(Vector4 vector) => 1 / vector.Length() * vector;

    public static Vector4 operator -(Vector4 v) => new(v.X, v.Y, v.Z, v.W);

    public static Vector4 operator +(Vector4 l, Vector4 r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z, l.W + r.W);

    public static Vector4 operator -(Vector4 l, Vector4 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z, l.W - r.W);

    public static Vector4 operator *(float scalar, Vector4 v) => new(scalar * v.X, scalar * v.Y, scalar * v.Z, scalar * v.W);

    public static Vector4 operator *(Vector4 v, float scalar) => new(scalar * v.X, scalar * v.Y, scalar * v.Z, scalar * v.W);

    public static Vector4 operator *(Vector4 l, Vector4 r) => new(l.X * r.X, l.Y * r.Y, l.Z * r.Z, l.W * r.W);

    public static Vector4 operator *(Vector4 l, Matrix4 r)
    {
        return new Vector4(
            l.X * r[0, 0] + l.Y * r[1, 0] + l.Z * r[2, 0] + l.W * r[3, 0],
            l.X * r[0, 1] + l.Y * r[1, 1] + l.Z * r[2, 1] + l.W * r[3, 1],
            l.X * r[0, 2] + l.Y * r[1, 2] + l.Z * r[2, 2] + l.W * r[3, 2],
            l.X * r[0, 3] + l.Y * r[1, 3] + l.Z * r[2, 3] + l.W * r[3, 3]);
    }
}