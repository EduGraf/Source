using System;

namespace EduGraf.Tensors;

// This represents a matrix in 4d homogeneous space.
public class Matrix4 : Tensor
{
    public Matrix4(params float[] elements)
        : base(elements)
    {
        if (elements.Length != 16) throw new ArgumentOutOfRangeException(nameof(elements));
    }

    public static Matrix4 operator *(Matrix4 l, Matrix4 r)
    {
        float[] result = new float[16];

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                float sum = 0;
                for (int k = 0; k < 4; k++)
                {
                    sum += l[row, k] * r[k, col];
                }
                result[4 * row + col] = sum;
            }
        }

        return new Matrix4(result);
    }

    public static Vector4 operator *(Matrix4 l, Vector4 r)
    {
        return new Vector4(
            l[0, 0] * r.X + l[0, 1] * r.Y + l[0, 2] * r.Z + l[0, 3] * r.W,
            l[1, 0] * r.X + l[1, 1] * r.Y + l[1, 2] * r.Z + l[1, 3] * r.W,
            l[2, 0] * r.X + l[2, 1] * r.Y + l[2, 2] * r.Z + l[2, 3] * r.W,
            l[3, 0] * r.X + l[3, 1] * r.Y + l[3, 2] * r.Z + l[3, 3] * r.W);
    }

    // Return matrix-elements.
    public float this[int row, int col]
    {
        get => Elements[4 * row + col];
        set => Elements[4 * row + col] = value;
    }

    // Ditto.
    public Matrix4 Transposed()
    {
        return new Matrix4(
            Elements[0], Elements[4], Elements[8], Elements[12],
            Elements[1], Elements[5], Elements[9], Elements[13],
            Elements[2], Elements[6], Elements[10], Elements[14],
            Elements[3], Elements[7], Elements[11], Elements[15]);
    }

    public Vector3 GetRow3(int row) => new(this[row, 0], this[row, 1], this[row, 2]);

    // Ditto.
    public static Matrix4 GetView(Point3 eye, Vector3 lookAt, Vector3 up)
    {
        var right = Vector3.Normalize(Vector3.Cross(lookAt, up));
        var orthoUp = Vector3.Cross(right, lookAt);
        var transI = new Matrix4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            -eye.X, -eye.Y, -eye.Z, 1);
        // negate lookAt to switch handedness
        var rotI = new Matrix4(
            right.X, orthoUp.X, -lookAt.X, 0,
            right.Y, orthoUp.Y, -lookAt.Y, 0,
            right.Z, orthoUp.Z, -lookAt.Z, 0,
            0, 0, 0, 1);
        return transI * rotI;
    }

    // Ditto.
    public static Matrix4 GetOrthographicProjection(float width, float height, float near, float far)
    {
        float negDepth = near - far;
        return new Matrix4(
            new float[16])
        {
            [0, 0] = 2 / width,
            [1, 1] = 2 / height,
            [2, 2] = 2 / negDepth,
            [3, 2] = (near + far) / negDepth,
            [3, 3] = 1
        };
    }

    // Ditto.
    public static Matrix4 GetPerspectiveProjection(float fovY, float aspect, float near, float far)
    {
        float scale = 1 / MathF.Tan(0.5f * fovY);
        return new Matrix4(
            new float[16])
        {
            [0, 0] = scale / aspect, // scale the x coordinates of the projected point 
            [1, 1] = scale, // scale the y coordinates of the projected point 
            [2, 2] = (near + far) / (near - far), // remap z to [0,1] 
            [3, 2] = 2 * near * far / (near - far), // remap z [0,1] 
            [2, 3] = -1 // set w = -z 
        };
    }

    // Ditto.
    public static Matrix4 GetOffCenterProjection(
        Vector3 center,
        Vector3 positiveX,
        Vector3 positiveY,
        float near,
        float far,
        bool toNear,
        Matrix4 view)
    {
        var left = center - positiveX;
        var right = center + positiveX;
        var bot = center - positiveY;
        var top = center + positiveY;

        var view0 = view.GetRow3(0);
        var view1 = view.GetRow3(1);
        return GetOffCenterPerspectiveProjection(
            Vector3.Dot(view0, left),   // (view * left).X
            Vector3.Dot(view0, right),  // (view * right).X
            Vector3.Dot(view1, bot),    // (view * bot).Y
            Vector3.Dot(view1, top),    // (view * top).Y
            near, far, toNear);
    }

    // Ditto.
    public static Matrix4 GetOffCenterPerspectiveProjection(float left, float right, float bottom, float top, float near, float far, bool toNear)
    {
        float deltaX = right - left;
        float deltaY = top - bottom;
        float deltaZ = far - near;
        var pPlane = toNear ? near : far;
        return new Matrix4(new float[16])
        {
            [0, 0] = 2 * pPlane / deltaX,
            [1, 1] = 2 * pPlane / deltaY,
            [2, 0] = (right + left) / deltaX,
            [2, 1] = (top + bottom) / deltaY,
            [2, 2] = -(toNear ? far : near) / deltaZ, // remap z to [0,1] 
            [2, 3] = -1, // set w = -z 
            [3, 2] = -2 * near * far / deltaZ // remap z [0,1] 
        };
    }

    public override string ToString() => $"[{string.Join(' ', Elements)}]";
}