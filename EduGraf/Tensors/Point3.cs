using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This represents an absolute position in 3d-space.
public readonly struct Point3(float x, float y, float z) : ITensor, IEquatable<Point3>
{
    public static readonly Point3 Origin = new(0, 0, 0);

    [Data] public float X { get; } = x; // -coordinate

    [Data] public float Y { get; } = y; // -coordinate

    [Data] public float Z { get; } = z; // -coordinate

    public float[] Elements { get; } = [x, y, z]; // see interface property

    public Point3(Vector3 vector) : this(vector.X, vector.Y, vector.Z) {}

    public static Point3 operator +(Point3 l, Vector3 r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);

    public static Point3 operator -(Point3 l, Vector3 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);

    public static Vector3 operator -(Point3 l, Point3 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);

    public static bool operator ==(Point3 l, Point3 r) => l.X == r.X && l.Y == r.Y && l.Z == r.Z;

    public static bool operator !=(Point3 l, Point3 r) => !(l == r);

    // as direction from origin
    public Vector3 Vector { get; } = new(x, y, z);

    // Get the coordinate-value by index.
    public float this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    // Interpolate linearly.
    public static Point3 Combine(float wa /* weight of a, wb = 1-wa. */, Point3 a, Point3 b) => a + wa * (b - a);

    // Get equally weighted center.
    public static Point3 Center(Point3 a, Point3 b, Point3 c) => a + 1f / 3 * (b - a) + 1f / 3 * (c - a);
    
    public bool Equals(Point3 other) => 
        X.Equals(other.X) && 
        Y.Equals(other.Y) && 
        Z.Equals(other.Z);

    public override bool Equals(object? obj) =>
        !ReferenceEquals(null, obj) && 
        obj.GetType() == GetType() && 
        Equals((Point3)obj);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => Vector.ToString();
}
