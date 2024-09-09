using EduGraf.Tensors;
using System;

namespace EduGraf.ToolsAndShapes;

// Contains functions that help to compute normals and tangents.
public static class Normals
{
    private readonly ref struct Matrix23
    {
        private float[,] Elements { get; }

        public Matrix23(float[,] elements)
        {
            Elements = elements;
        }

        public Matrix23(Vector3 row0, Vector3 row1)
        {
            Elements = new float[2, 3];
            Elements[0, 0] = row0.X;
            Elements[0, 1] = row0.Y;
            Elements[0, 2] = row0.Z;
            Elements[1, 0] = row1.X;
            Elements[1, 1] = row1.Y;
            Elements[1, 2] = row1.Z;
        }

        public float this[int row, int col] => Elements[row, col];
    }

    // Return the constructed normals.
    public static float[] FromNonIndexedVertices(float[] positions)
    {
        var normals = new float[positions.Length];
        int count = positions.Length / 3;
        for (int i = 0; i < count; i += 3)
        {
            var a = GetPoint3(positions, i + 0);
            var b = GetPoint3(positions, i + 1);
            var c = GetPoint3(positions, i + 2);
            var n = GetN(a, b, c);
            for (int j = i; j < i + 3; j++)
            {
                int index = 3 * j;
                normals[index + 0] = n[0];
                normals[index + 1] = n[1];
                normals[index + 2] = n[2];
            }
        }

        return normals;
    }

    // Return the constructed tangents.
    public static (float[], float[]) TangentsFromNonIndexedVertices(float[] positions, float[] uv)
    {
        var tangents = new float[positions.Length];
        var biTangents = new float[positions.Length];
        int count = positions.Length / 3;
        for (int i = 0; i < count; i += 3)
        {
            var t = GetPoint2(uv, i);
            var duv = Matrix2.CreateByRows(
                GetPoint2(uv, i + 1) - t,
                GetPoint2(uv, i + 2) - t);

            var p = GetPoint3(positions, i);
            var dp = new Matrix23(
                GetPoint3(positions, i + 1) - p,
                GetPoint3(positions, i + 2) - p);

            var inverted = duv.Inverted();
            var tb = Mul(inverted, dp);
            var il0 = 1 / GetRowLength(tb, 0);
            var il1 = 1 / GetRowLength(tb, 1);

            for (int j = i; j < i + 3; j++)
            {
                int index = 3 * j;
                tangents[index + 0] = il0 * tb[0, 0];
                tangents[index + 1] = il0 * tb[0, 1];
                tangents[index + 2] = il0 * tb[0, 2];
                biTangents[index + 0] = il1 * tb[1, 0];
                biTangents[index + 1] = il1 * tb[1, 1];
                biTangents[index + 2] = il1 * tb[1, 2];
            }
        }

        return (tangents, biTangents);
    }

    private static float GetRowLength(Matrix23 tb, int row)
    {
        float tb00 = tb[row, 0];
        float tb01 = tb[row, 1];
        float tb02 = tb[row, 2];
        return MathF.Sqrt(tb00 * tb00 + tb01 * tb01 + tb02 * tb02);
    }

    private static Matrix23 Mul(Matrix2 l, Matrix23 r)
    {
        float[,] result = new float[2, 3];

        for (int row = 0; row < 2; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                float sum = 0;
                for (int k = 0; k < 2; k++)
                {
                    sum += l[row, k] * r[k, col];
                }
                result[row, col] = sum;
            }
        }

        return new Matrix23(result);
    }

    // Return the constructed normals.
    public static float[] FromTriangles<TIndex>(float[] positions, TIndex[] triangles)
        where TIndex : struct
    {
        var normals = new float[positions.Length];

        for (int i = 0; i < triangles.Length; i += 3)
        {
            var vertex0 = triangles[i + 0];
            var vertex1 = triangles[i + 1];
            var vertex2 = triangles[i + 2];
            var a = GetPoint3(positions, Convert.ToInt64(vertex0));
            var b = GetPoint3(positions, Convert.ToInt64(vertex1));
            var c = GetPoint3(positions, Convert.ToInt64(vertex2));
            var n = GetN(a, b, c);
            AddVector(vertex0, normals, n);
            AddVector(vertex1, normals, n);
            AddVector(vertex2, normals, n);
        }

        for (int i = 0; i < normals.Length / 3; i++)
        {
            int index = 3 * i;
            float x = normals[index];
            float y = normals[index + 1];
            float z = normals[index + 2];
            float l = MathF.Sqrt(x * x + y * y + z * z);
            normals[index + 0] /= l;
            normals[index + 1] /= l;
            normals[index + 2] /= l;
        }

        return normals;
    }

    private static Vector3 GetN(Point3 a, Point3 b, Point3 c)
    {
        return Vector3.Normalize(Vector3.Cross(b - a, c - a));
    }

    private static void AddVector<TIndex>(TIndex vertex, float[] vertices, Vector3 value)
        where TIndex : struct
    {
        var vi = 3 * Convert.ToInt32(vertex);
        vertices[vi + 0] += value[0];
        vertices[vi + 1] += value[1];
        vertices[vi + 2] += value[2];
    }

    private static Point2 GetPoint2(float[] values, long offset)
    {
        long index = 2 * offset;
        return new Point2(values[index + 0], values[index + 1]);
    }

    private static Point3 GetPoint3(float[] values, long offset)
    {
        long index = 3 * offset;
        return new Point3(values[index + 0], values[index + 1], values[index + 2]);
    }
}