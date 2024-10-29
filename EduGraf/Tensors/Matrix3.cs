using System;

namespace EduGraf.Tensors;

// This represents a matrix in 4d homogeneous space.
public class Matrix3 : Tensor
{
    public Matrix3(params float[] elements)
        : base(elements)
    {
        if (elements.Length != 9) throw new ArgumentOutOfRangeException(nameof(elements));
    }

    public static readonly Matrix3 Identity = new(
        1, 0, 0,
        0, 1, 0,
        0, 0, 1
    );

    // Create a new transformation matrix scaling in all axis directions by the same factor.
    public static Matrix3 Scale(float factor) => Scale(factor, factor);

    // Create a new transformation matrix scaling by varying factors in the different axis directions.
    public static Matrix3 Scale(float x, float y)
    {
        return new Matrix3(
            x, 0, 0,
            0, y, 0,
            0, 0, 1);
    }

    // Create a new scaling matrix from a vector.
    public static Matrix3 Scale(Vector2 factor) => Scale(factor.X, factor.Y);

    // Create a new transformation matrix representing translation by a vector.
    public static Matrix3 Translation(Vector2 delta)
    {
        return new Matrix3(
            1, 0, 0,
            0, 1, 0,
            delta.X, delta.Y, 1);
    }

    // Create a new transformation matrix representing rotation around the z axis.
    public static Matrix3 Rotation(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix3(
            +cos, sin, 0,
            -sin, cos, 0,
               0,   0, 1);
    }

    public static Matrix3 operator *(Matrix3 l, Matrix3 r)
    {
        float[] result = new float[9];

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                float sum = 0;
                for (int k = 0; k < 3; k++) sum += l[row, k] * r[k, col];
                result[3 * row + col] = sum;
            }
        }

        return new Matrix3(result);
    }

    public static Vector3 operator *(Matrix3 l, Vector3 r)
    {
        return new Vector3(
            l[0, 0] * r.X + l[0, 1] * r.Y + l[0, 2] * r.Z,
            l[1, 0] * r.X + l[1, 1] * r.Y + l[1, 2] * r.Z,
            l[2, 0] * r.X + l[2, 1] * r.Y + l[2, 2] * r.Z);
    }

    // Return matrix-elements.
    public float this[int row, int col]
    {
        get => Elements[3 * row + col];
        set => Elements[3 * row + col] = value;
    }

    // Ditto.
    public Matrix3 Transposed()
    {
        return new Matrix3(
            Elements[0], Elements[3], Elements[6],
            Elements[1], Elements[4], Elements[7],
            Elements[2], Elements[5], Elements[8]);
    }

    public override string ToString() => $"[{string.Join(' ', Elements)}]";
}