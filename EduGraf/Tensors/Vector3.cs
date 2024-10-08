﻿using System;

namespace EduGraf.Tensors;

// This represents a displacement in 3d-space.
public class Vector3(float x, float y, float z) : Coordinate3(x, y, z)
{
    public static readonly Vector3 Zero = new(0, 0, 0);
    public static readonly Vector3 UnitX = new(1, 0, 0);
    public static readonly Vector3 UnitY = new(0, 1, 0);
    public static readonly Vector3 UnitZ = new(0, 0, 1);

    // ditto.
    public Vector2 Xy => new(X, Y);
    // ditto.
    public Vector2 Xz => new(X, Z);
    // ditto.
    public Vector2 Yz => new(Y, Z);

    // Return a homogeneous vector with the given additional component.
    public Vector4 Extend(float w) => new(X, Y, Z, w);

    // Return the euclidean length of this vector.
    public float Length() => MathF.Sqrt(this * this);

    // Return the cross product of this vector with r.
    public static Vector3 Cross(Vector3 l, Vector3 r)
    {
        return new Vector3(
            l.Y * r.Z - l.Z * r.Y,
            l.Z * r.X - l.X * r.Z,
            l.X * r.Y - l.Y * r.X);
    }

    // Return a new vector in the same direction with length 1.
    public static Vector3 Normalize(Vector3 vector) => 1 / vector.Length() * vector;

    // Return the incoming direction with the normal.
    public static Vector3 Reflect(Vector3 direction, Vector3 normal)
    {
        if (Math.Abs(normal.Length() - 1f) > 1e-6) throw new InvalidOperationException("vector must be normalized");

        return direction - 2 * (direction * normal) * normal;
    }

    public static Vector3 operator +(Vector3 l, Vector3 r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);

    public static Vector3 operator -(Vector3 v) => new(-v.X, -v.Y, -v.Z);

    public static Vector3 operator -(Vector3 l, Vector3 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);

    public static Vector3 operator *(float scalar, Vector3 v) => new(scalar * v.X, scalar * v.Y, scalar * v.Z);

    public static Vector3 operator *(Vector3 v, float scalar) => new(scalar * v.X, scalar * v.Y, scalar * v.Z);

    public static float operator *(Vector3 l, Vector3 r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z;
}