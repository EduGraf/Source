using EduGraf.Tensors;
using System;

namespace EduGraf;

// This class contains frequently used values and functions for geometric tranformations.
public static class Space
{
    public static readonly Point2 Origin2 = new(0, 0);
    public static readonly Point3 Origin3 = new(0, 0, 0);

    public static readonly Vector2 Zero2 = new(0, 0);
    public static readonly Vector2 Unit2X = new(1, 0);
    public static readonly Vector2 Unit2Y = new(0, 1);

    public static readonly Vector3 Zero3 = new(0, 0, 0);
    public static readonly Vector3 Unit3X = new(1, 0, 0);
    public static readonly Vector3 Unit3Y = new(0, 1, 0);
    public static readonly Vector3 Unit3Z = new(0, 0, 1);

    public static readonly Vector4 Zero4 = new(0, 0, 0, 0);
    public static readonly Matrix4 Identity4 = new(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );

    // Create a new 2d rotation matrix.
    public static Matrix2 Rotation2(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix2(
                 cos, sin,
                -sin, cos);
    }

    // Create a new transformation matrix scaling in all axis directions by the same factor.
    public static Matrix4 Scale4(float factor) => Scale4(factor, factor, factor);

    // Create a new transformation matrix scaling by varying factors in the different axis directions.
    public static Matrix4 Scale4(float x, float y, float z)
    {
        return new Matrix4(
            x, 0, 0, 0,
            0, y, 0, 0,
            0, 0, z, 0,
            0, 0, 0, 1);
    }

    // Create a new scaling matrix from a vector.
    public static Matrix4 Scale4(Vector3 factor) => Scale4(factor.X, factor.Y, factor.Z);

    // Create a new transformation matrix representing translation by a vector.
    public static Matrix4 Translation4(Vector3 delta)
    {
        return new Matrix4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            delta.X, delta.Y, delta.Z, 1);
    }

    // Create a new transformation matrix representing rotation around the x axis.
    public static Matrix4 RotationX4(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix4(
            1, 0, 0, 0,
            0, cos, sin, 0,
            0, -sin, cos, 0,
            0, 0, 0, 1);
    }

    // Create a new transformation matrix representing rotation around the y axis.
    public static Matrix4 RotationY4(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix4(
            cos, 0, -sin, 0,
              0, 1, 0, 0,
            sin, 0, cos, 0,
              0, 0, 0, 1);
    }

    // Create a new transformation matrix representing rotation around the z axis.
    public static Matrix4 RotationZ4(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix4(
             cos, sin, 0, 0,
            -sin, cos, 0, 0,
               0, 0, 1, 0,
               0, 0, 0, 1);
    }
}
