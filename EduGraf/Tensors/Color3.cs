using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This represents a color that can be sent to the GPU.
public class Color3 : Tensor, IEquatable<Color3>
{
    // red in the range of 0..1.
    [Data] public float R { get; }

    // green in the range of 0..1.
    [Data] public float G { get; }

    // blue in the range of 0..1.
    [Data] public float B { get; }

    public Color3(float r, float g, float b)
        : base(r, g, b)
    {
        R = r;
        G = g;
        B = b;
    }

    public static Color3 operator -(Color3 c) => new(-c.R, -c.G, -c.B);

    public static Color3 operator *(float scalar, Color3 c) => new(scalar * c.R, scalar * c.G, scalar * c.B);

    public static Color3 operator *(Color3 c, float scalar) => new(scalar * c.R, scalar * c.G, scalar * c.B);

    public static Color3 operator *(Color3 l, Color3 r) => new(l.R * r.R, l.G * r.G, l.B * r.B);

    public static Color3 operator +(Color3 l, Color3 r) => new(l.R + r.R, l.G + r.G, l.B + r.B);

    public static Color3 operator -(Color3 l, Color3 r) => new(l.R - r.R, l.G - r.G, l.B - r.B);

    public bool Equals(Color3? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Color3)obj);
    }

    public override int GetHashCode() => HashCode.Combine(R, G, B);
}