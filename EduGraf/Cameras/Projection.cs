using EduGraf.Tensors;

namespace EduGraf.Cameras;

// This is the abstraction for all projections. It is a mere collection of properties.
public abstract class Projection(float near, float far)
{
    // distance of the view-frustum plane
    public float Near { get; } = near;

    // distance of the view-frustum plane
    public float Far { get; } = far;

    // Return the projection matrix.
    public abstract Matrix4 GetMatrix(float aspect);
}

// Represents a centered perspective projection.
public class PerspectiveProjection(float near, float far, float fovY) : Projection(near, far)
{
    // the field of view y angle in radians
    public float FovY { get; } = fovY;

    // See overridden method.
    public override Matrix4 GetMatrix(float aspect /* projection plane width divided by height */)
    {
        return Matrix4.GetPerspectiveProjection(FovY, aspect, Near, Far);
    }
}

// Represents an off-center perspective projection.
public class OffCenterPerspectiveProjection(float near, float far, bool toNear, Point3 center, Vector3 right, Vector3 up) : Projection(near, far)
{
    // of the projection plane
    public Point3 Center { get; } = center;

    // point on the border of the plane
    public Vector3 Right { get; } = right;

    // point on the border of the plane
    public Vector3 Up { get; } = up;

    // Project to near-plane if true, else to the far-plane
    public bool ToNear { get; } = toNear;

    // position
    public Point3 Camera { private get; set; } = Point3.Origin;

    // See overridden method.
    public override Matrix4 GetMatrix(float aspect) => Matrix4.GetOffCenterProjection(Camera, Center, Right, Up, Near, Far, ToNear);
}

// Represents an orthographic projection.
public class OrthographicProjection(float near, float far, float scale) : Projection(near, far)
{
    // applied in the transformation to the projection plane
    public float Scale { get; set; } = scale;

    // See overridden method.
    public override Matrix4 GetMatrix(float aspect) => Matrix4.GetOrthographicProjection(Scale * aspect, Scale, Near, Far);
}
