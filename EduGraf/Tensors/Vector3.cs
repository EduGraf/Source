using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This represents a displacement in 3d-space.
public readonly struct Vector3(float x, float y, float z) : ITensor, IEquatable<Vector3>
{
    public static readonly Vector3 Zero = new(0, 0, 0);
    public static readonly Vector3 UnitX = new(1, 0, 0);
    public static readonly Vector3 UnitY = new(0, 1, 0);
    public static readonly Vector3 UnitZ = new(0, 0, 1);

    [Data] public float X { get; } = x; // -coordinate

    [Data] public float Y { get; } = y; // -coordinate

    [Data] public float Z { get; } = z; // -coordinate

    public float[] Elements { get; } = [x, y, z]; // see interface property

    // Get the coordinate-value by index.
    public float this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    // Return a homogeneous vector with the given additional component.
    public Vector4 Extend(float w) => new(X, Y, Z, w);

    // euclidean length of this vector.
    public float Length { get; } = MathF.Sqrt(x*x + y*y + z*z);

    public static Vector3 operator +(Vector3 l, Vector3 r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);

    public static Vector3 operator -(Vector3 v) => new(-v.X, -v.Y, -v.Z);

    public static Vector3 operator -(Vector3 l, Vector3 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);

    public static Vector3 operator *(float scalar, Vector3 v) => new(scalar * v.X, scalar * v.Y, scalar * v.Z);

    public static Vector3 operator *(Vector3 v, float scalar) => new(scalar * v.X, scalar * v.Y, scalar * v.Z);

    public static float operator *(Vector3 l, Vector3 r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z;

    public static bool operator ==(Vector3 l, Vector3 r) => l.X == r.X && l.Y == r.Y && l.Z == r.Z;

    public static bool operator !=(Vector3 l, Vector3 r) => !(l == r);

    // Return the cross product of this vector with r.
    public static Vector3 Cross(Vector3 l, Vector3 r) =>
        new(
            l.Y * r.Z - l.Z * r.Y,
            l.Z * r.X - l.X * r.Z,
            l.X * r.Y - l.Y * r.X);

    // Return a new vector in the same direction with length 1.
    public static Vector3 Normalize(Vector3 vector) => 1 / vector.Length * vector;

    // Return the incoming direction with the normal.
    public static Vector3 Reflect(Vector3 direction, Vector3 normal)
    {
        if (MathF.Abs(normal.Length - 1f) > Math.Precision) throw new InvalidOperationException("vector must be normalized");

        return direction - 2 * (direction * normal) * normal;
    }

    public bool Equals(Vector3 other) => 
        X.Equals(other.X) && 
        Y.Equals(other.Y) && 
        Z.Equals(other.Z);

    public override bool Equals(object? obj) => 
        !ReferenceEquals(null, obj) && 
        obj.GetType() == GetType() && 
        Equals((Vector3)obj);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => $"({X}, {Y}, {Z})";
}