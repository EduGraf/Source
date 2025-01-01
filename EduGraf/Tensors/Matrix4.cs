using System;
using System.Linq;

namespace EduGraf.Tensors;

// This represents a matrix in 4d homogeneous space.
public readonly struct Matrix4 : ITensor, IEquatable<Matrix4>
{
    public float[] Elements { get; } // see interface property

    // Create a matrix with 16 elements.
    public Matrix4(params float[] elements)
    {
        if (elements.Length != 16) throw new ArgumentOutOfRangeException(nameof(elements));
        Elements = elements;

        Transposed = new(() => new(
            elements[0], elements[4], elements[8], elements[12],
            elements[1], elements[5], elements[9], elements[13],
            elements[2], elements[6], elements[10], elements[14],
            elements[3], elements[7], elements[11], elements[15]));
    }
    
    public static readonly Matrix4 Identity = new(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );

    // Create a new transformation matrix scaling in all axis directions by the same factor.
    public static Matrix4 Scale(float factor) => Scale(factor, factor, factor);

    // Create a new transformation matrix scaling by varying factors in the different axis directions.
    public static Matrix4 Scale(float x, float y, float z)
    {
        return new Matrix4(
            x, 0, 0, 0,
            0, y, 0, 0,
            0, 0, z, 0,
            0, 0, 0, 1);
    }

    // Create a new scaling matrix from a vector.
    public static Matrix4 Scale(Vector3 factor) => Scale(factor.X, factor.Y, factor.Z);

    // Create a new transformation matrix representing translation by a vector.
    public static Matrix4 Translation(Vector3 delta)
    {
        return new Matrix4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            delta.X, delta.Y, delta.Z, 1);
    }

    // Create a new transformation matrix representing rotation around the x-axis.
    public static Matrix4 RotationX(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix4(
            1,    0, 0, 0,
            0, +cos, sin, 0,
            0, -sin, cos, 0,
            0,    0, 0, 1);
    }

    // Create a new transformation matrix representing rotation around the y-axis.
    public static Matrix4 RotationY(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix4(
            cos, 0, -sin, 0,
              0, 1,    0, 0,
            sin, 0, +cos, 0,
              0, 0,    0, 1);
    }

    // Create a new transformation matrix representing rotation around the z-axis.
    public static Matrix4 RotationZ(float angle /* in radians */)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Matrix4(
             cos, sin, 0, 0,
            -sin, cos, 0, 0,
               0, 0, 1, 0,
               0, 0, 0, 1);
    }

    public static Matrix4 operator *(Matrix4 l, Matrix4 r)
    {
        float[] result = new float[16];

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                float sum = 0;
                for (int k = 0; k < 4; k++) sum += l[row, k] * r[k, col];
                result[4 * row + col] = sum;
            }
        }

        return new Matrix4(result);
    }

    public static Vector4 operator *(Matrix4 l, Vector4 r) => new(
        l[0, 0] * r.X + l[0, 1] * r.Y + l[0, 2] * r.Z + l[0, 3] * r.W,
        l[1, 0] * r.X + l[1, 1] * r.Y + l[1, 2] * r.Z + l[1, 3] * r.W,
        l[2, 0] * r.X + l[2, 1] * r.Y + l[2, 2] * r.Z + l[2, 3] * r.W,
        l[3, 0] * r.X + l[3, 1] * r.Y + l[3, 2] * r.Z + l[3, 3] * r.W);

    public static Matrix4 operator *(float s, Matrix4 r) => new(
        s * r[0, 0], s * r[0, 1], s * r[0, 2], s * r[0, 3],
        s * r[1, 0], s * r[1, 1], s * r[1, 2], s * r[1, 3],
        s * r[2, 0], s * r[2, 1], s * r[2, 2], s * r[2, 3],
        s * r[3, 0], s * r[3, 1], s * r[3, 2], s * r[3, 3]);

    // Return matrix-elements.
    public float this[int row, int col]
    {
        get => Elements[4 * row + col];
        set => Elements[4 * row + col] = value;
    }

    // of this matrix
    public Lazy<Matrix4> Transposed { get; }

    // Returns the matrix corresponding to the given view.
    public static Matrix4 GetView(View view)
    {
        var eye = view.Position;
        var lookOut = view.LookOut;
        var right = Vector3.Normalize(Vector3.Cross(view.LookOut, view.Up));
        var orthoUp = Vector3.Cross(right, lookOut);
        var transI = new Matrix4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            -eye.X, -eye.Y, -eye.Z, 1);
        // negate lookOut to switch handedness
        var rotI = new Matrix4(
            right.X, orthoUp.X, -lookOut.X, 0,
            right.Y, orthoUp.Y, -lookOut.Y, 0,
            right.Z, orthoUp.Z, -lookOut.Z, 0,
            0, 0, 0, 1);
        return transI * rotI;
    }

    // Returns the orthographic projection-matrix.
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

    // Returns the perspective projection-matrix.
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

    // Returns the intersection of the plane with the ray staring from the camera with direction n.
    public static Point3 GetProjectionCenter(Point3 camera, Vector3 n, Point3 plane, out float s)
    {
        // pc = c - s*n;
        // pc*n = p*n
        // (c - s*n)*n = p*n
        // c*n - s*n*n = p*n
        // s = c*n - p*n = (c - p) * n
        s = (camera.Vector - plane.Vector) * n;
        return camera - s * n;
    }

    // Returns the perspective projection-matrix in case the camera view is not orthogonal and centered with respect to the projection plane.
    public static Matrix4 GetOffCenterProjection(
        Point3 camera,
        Point3 mid,
        Vector3 right,
        Vector3 up,
        float near,
        float far,
        bool toNear)
    {
        var n = Vector3.Normalize(Vector3.Cross(right, up));
        var pjc = GetProjectionCenter(camera, n, mid, out _);
        var off = mid - pjc;

        float right2 = right * right;
        float ldh = off * right / right2;
        float l = ldh - 1;
        float r = ldh + 1;
        float halfWidth = -MathF.Sqrt(right2);
        l *= halfWidth;
        r *= halfWidth;

        float up2 = up * up;
        float ldv = off * up / up2;
        float b = ldv - 1;
        float t = ldv + 1;
        float halfHeight = MathF.Sqrt(up2);
        b *= halfHeight;
        t *= halfHeight;

        return GetOffCenterPerspectiveProjection(l, r, b, t, near, far, toNear);
    }

    private static Matrix4 GetOffCenterPerspectiveProjection(float left, float right, float bottom, float top, float near, float far, bool toNear)
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
            [3, 2] = -near * far / deltaZ // remap z [0,1] 
        };
    }

    public override string ToString() =>
        $"[ {this[0, 0]} {this[0, 1]} {this[0, 2]} {this[0, 3]} ]\r\n" +
        $"[ {this[1, 0]} {this[1, 1]} {this[1, 2]} {this[1, 3]} ]\r\n" +
        $"[ {this[2, 0]} {this[2, 1]} {this[2, 2]} {this[2, 3]} ]\r\n" +
        $"[ {this[3, 0]} {this[3, 1]} {this[3, 2]} {this[3, 3]} ]\r\n";

    public bool Equals(Matrix4 other)
    {
        for (int i = 0; i < Elements.Length; i++)
        {
            if (Elements[i] - other.Elements[i] > Math.Precision) return false;
        }
        return true;
    }

    public override bool Equals(object? obj) => !ReferenceEquals(null, obj) && Equals((Matrix4)obj);

    public override int GetHashCode() => Elements.GetHashCode();
}