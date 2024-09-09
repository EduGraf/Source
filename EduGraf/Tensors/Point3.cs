namespace EduGraf.Tensors;

// This represents an absolute position in 3d-space.
public class Point3 : Coordinate3
{
    public Point3(float x, float y, float z)
        : base(x, y, z)
    {
    }

    public Point3(Vector3 vector)
        : base(vector.X, vector.Y, vector.Z)
    {
    }

    // Convert absolute to relative.
    public Vector3 Vector => new(X, Y, Z);

    public static Point3 operator +(Point3 l, Vector3 r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);

    public static Point3 operator -(Point3 l, Vector3 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);

    public static Vector3 operator -(Point3 l, Point3 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);

    // Interpolate linearly.
    public static Point3 Combine(float wa /* weight of a, the weight of b is implicitly 1-wa. */, Point3 a, Point3 b)
    {
        return a + wa * (b - a);
    }

    // Get equally weighted center.
    public static Point3 Center(Point3 a, Point3 b, Point3 c) => a + 1f / 3 * (b - a) + 1f / 3 * (c - a);
}
