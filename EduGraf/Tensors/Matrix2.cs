using System;

namespace EduGraf.Tensors;

// This represents a matrix in 2d space.
public class Matrix2 : Tensor
{
    // Create a new 2d rotation matrix.
    public static Matrix2 Rotation(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix2(
            cos, sin,
            -sin, cos);
    }

    public Matrix2(float e00, float e01, float e10, float e11)
        : base(e00, e01, e10, e11)
    {
    }

    // Ditto.
    public static Matrix2 CreateByColumns(Vector2 col0, Vector2 col1)
    {
        return new Matrix2(
            col0.X, col1.X,
            col0.Y, col1.Y);
    }

    // Ditto.
    public static Matrix2 CreateByRows(Vector2 row0, Vector2 row1)
    {
        return new Matrix2(
            row0.X, row0.Y,
            row1.X, row1.Y);
    }

    // Return matrix-elements.
    public float this[int row, int col] => Elements[2 * row + col];

    // Ditto.
    public Matrix2 Transposed()
    {
        return new Matrix2(
            Elements[0], Elements[2],
            Elements[1], Elements[3]);
    }

    // Return the inverted matrix.
    public Matrix2 Inverted()
    {
        float det = Elements[0] * Elements[3] - Elements[1] * Elements[2];
        return det * new Matrix2(Elements[3], -Elements[1], -Elements[2], Elements[0]);
    }

    public static Matrix2 operator *(float scalar, Matrix2 m)
    {
        return new Matrix2(
            scalar * m.Elements[0],
            scalar * m.Elements[1],
            scalar * m.Elements[2],
            scalar * m.Elements[3]);
    }
}