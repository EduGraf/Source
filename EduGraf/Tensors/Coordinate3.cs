using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This is the base class for Vector3 and Point3.
public abstract class Coordinate3 : Tensor, IEquatable<Coordinate3>
{
    // ditto.
    [Data] public float X { get; }
    // ditto.
    [Data] public float Y { get; }
    // ditto.
    [Data] public float Z { get; }

    protected Coordinate3(float x, float y, float z)
        : base(x, y, z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(Coordinate3? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Coordinate3)obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
}