using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This is the base class for Vector4 and Point4.
public abstract class Coordinate4 : Tensor, IEquatable<Coordinate4>
{
    // ditto.
    [Data] public float X { get; }
    // ditto.
    [Data] public float Y { get; }
    // ditto.
    [Data] public float Z { get; }
    // ditto.
    [Data] public float W { get; }

    protected Coordinate4(float x, float y, float z, float w)
        : base(x, y, z, w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }



    public bool Equals(Coordinate4? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Coordinate4)obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);
}