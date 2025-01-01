using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This represents a displacement in 4d-space, i.e. homogeneous coordinates.
public readonly struct Vector4(float x, float y, float z, float w) : ITensor, IEquatable<Vector4>
{
    public static readonly Vector4 Zero = new(0, 0, 0, 0);
    public static readonly Vector4 UnitX = new(1, 0, 0, 0);
    public static readonly Vector4 UnitY = new(0, 1, 0, 0);
    public static readonly Vector4 UnitZ = new(0, 0, 1, 0);
    public static readonly Vector4 UnitW = new(0, 0, 0, 1);

    [Data] public float X { get; } = x; // -coordinate

    [Data] public float Y { get; } = y; // -coordinate

    [Data] public float Z { get; } = z; // -coordinate

    [Data] public float W { get; } = w; // -coordinate

    public float[] Elements { get; } = [x, y, z, w]; // see interface property

    // the euclidean length of this vector
    public float Length { get; } = MathF.Sqrt(x*x + y*y + z*z + w*w);

    // Return a new vector in the same direction with length 1.
    public static Vector4 Normalize(Vector4 vector) => 1 / vector.Length * vector;

    public static Vector4 operator -(Vector4 v) => new(v.X, v.Y, v.Z, v.W);

    public static Vector4 operator +(Vector4 l, Vector4 r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z, l.W + r.W);

    public static Vector4 operator -(Vector4 l, Vector4 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z, l.W - r.W);

    public static Vector4 operator *(float scalar, Vector4 v) => new(scalar * v.X, scalar * v.Y, scalar * v.Z, scalar * v.W);

    public static Vector4 operator *(Vector4 v, float scalar) => new(scalar * v.X, scalar * v.Y, scalar * v.Z, scalar * v.W);

    public static Vector4 operator *(Vector4 l, Vector4 r) => new(l.X * r.X, l.Y * r.Y, l.Z * r.Z, l.W * r.W);

    public static Vector4 operator *(Vector4 l, Matrix4 r) => new(
        l.X * r[0, 0] + l.Y * r[1, 0] + l.Z * r[2, 0] + l.W * r[3, 0],
        l.X * r[0, 1] + l.Y * r[1, 1] + l.Z * r[2, 1] + l.W * r[3, 1],
        l.X * r[0, 2] + l.Y * r[1, 2] + l.Z * r[2, 2] + l.W * r[3, 2],
        l.X * r[0, 3] + l.Y * r[1, 3] + l.Z * r[2, 3] + l.W * r[3, 3]);

    public static bool operator ==(Vector4 l, Vector4 r) => l.X == r.X && l.Y == r.Y && l.Z == r.Z && l.W == r.W;

    public static bool operator !=(Vector4 l, Vector4 r) => !(l == r);

    // Ditto.
    public static float Dot(Vector4 l, Vector4 r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z + l.W * r.W;
    
    // Get this vector without the homogeneous coordinate.
    public Vector3 Xyz => new(X, Y, Z);

    // Get the coordinate-value by index.
    public float this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        3 => W,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    public bool Equals(Vector4 other) => 
        X.Equals(other.X) && 
        Y.Equals(other.Y) && 
        Z.Equals(other.Z) && 
        W.Equals(other.W);

    public override bool Equals(object? obj) => 
        !ReferenceEquals(null, obj) && 
        obj.GetType() == GetType() && 
        Equals((Vector4)obj);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public override string ToString() => $"({X}, {Y}, {Z}, {W})";
}