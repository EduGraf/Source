using EduGraf.Lighting;
using System;

namespace EduGraf.Tensors;

// This represents a color that can be sent to the GPU.
public readonly struct Color3(float r, float g, float b) : ITensor, IColor, IEquatable<Color3>
{
    [Data] public float R { get; } = r; // red in the range of 0..1

    [Data] public float G { get; } = g; // green in the range of 0..1

    [Data] public float B { get; } = b; // blue in the range of 0..1

    public float[] Elements { get; } = [r, g, b]; // see interface property

    public static Color3 operator -(Color3 c) => new(-c.R, -c.G, -c.B);

    public static Color3 operator *(float scalar, Color3 c) => new(scalar * c.R, scalar * c.G, scalar * c.B);

    public static Color3 operator *(Color3 c, float scalar) => new(scalar * c.R, scalar * c.G, scalar * c.B);

    public static Color3 operator *(Color3 l, Color3 r) => new(l.R * r.R, l.G * r.G, l.B * r.B);

    public static Color3 operator +(Color3 l, Color3 r) => new(l.R + r.R, l.G + r.G, l.B + r.B);

    public static Color3 operator -(Color3 l, Color3 r) => new(l.R - r.R, l.G - r.G, l.B - r.B);

    public bool Equals(Color3 other) => 
        R.Equals(other.R) && 
        G.Equals(other.G) && 
        B.Equals(other.B);

    public override bool Equals(object? obj) => 
        !ReferenceEquals(null, obj) && 
        obj.GetType() == GetType() && 
        Equals((Color3)obj);

    public override int GetHashCode() => HashCode.Combine(R, G, B);

    public override string ToString() => $"({R}, {G}, {B})";
}