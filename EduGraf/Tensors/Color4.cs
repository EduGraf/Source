using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This represents a color with transparency that can be sent to the GPU.
public class Color4(float r, float g, float b, float a) : Tensor(r, g, b, a), IColor, IEquatable<Color4>
{
    // red in the range of 0..1.
    [Data] public float R { get; } = r;

    // green in the range of 0..1.
    [Data] public float G { get; } = g;

    // blue in the range of 0..1.
    [Data] public float B { get; } = b;

    // transparency in the range of 0..1.
    [Data] public float A { get; } = a;

    // Creates a new instance.
    public Color4(Color3 c, float a)
        : this(c.R, c.G, c.B, a)
    {
    }

    // Strips transparency from the color.
    public Color3 Color3 => new(R, G, B);

    public static Color4 operator -(Color4 c) => new(-c.R, -c.G, -c.B, -c.A);

    public static Color4 operator *(float scalar, Color4 c) => new(scalar * c.R, scalar * c.G, scalar * c.B, scalar * c.A);

    public static Color4 operator *(Color4 c, float scalar) => new(scalar * c.R, scalar * c.G, scalar * c.B, scalar * c.A);

    public static Color4 operator *(Color4 l, Color4 r) => new(l.R * r.R, l.G * r.G, l.B * r.B, l.A * r.A);

    public static Color4 operator +(Color4 l, Color4 r) => new(l.R + r.R, l.G + r.G, l.B + r.B, l.A + r.A);

    public static Color4 operator -(Color4 l, Color4 r) => new(l.R - r.R, l.G - r.G, l.B - r.B, l.A - r.A);

    public bool Equals(Color4? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Color4)obj);
    }

    public override int GetHashCode() => HashCode.Combine(R, G, B, A);
}