using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This is the base class for Vector2 and Point2.
public abstract class Coordinate2 : Tensor, IEquatable<Coordinate2>
{
    // ditto.
    [Data] public float X { get; }
    // ditto.
    [Data] public float Y { get; }

    protected Coordinate2(float x, float y)
        : base(x, y)
    {
        X = x;
        Y = y;
    }

    // Get the coordinate-value by index.
    public float this[int index] => index switch
    {
        0 => X,
        1 => Y,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    public bool Equals(Coordinate2? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Coordinate2)obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
}