using EduGraf.Tensors;

namespace EduGraf.Cameras;

// This is the abstraction for all projections. It is a mere collection of properties.
public abstract class Projection
{
    // distance of the view-frustum plane
    public float Near { get; }

    // distance of the view-frustum plane
    public float Far { get; }

    protected Projection(float near, float far)
    {
        Near = near;
        Far = far;
    }

    // Return the projection matrix.
    public abstract Matrix4 GetMatrix(float aspect);
}

// Ditto.
public class PerspectiveProjection : Projection
{
    // the field of view y angle in radians.
    public float FovY { get; }

    // Create a new perspective projection.
    public PerspectiveProjection(float near, float far, float fovY)
        : base(near, far)
    {
        FovY = fovY;
    }

    // Ditto.
    public override Matrix4 GetMatrix(float aspect /* projection plane width divided by height */)
    {
        return Matrix4.GetPerspectiveProjection(FovY, aspect, Near, Far);
    }
}

// Prototype.
public class OffCenterPerspectiveProjection : Projection
{
    // -
    public Vector3 Center { get; set; }
    // -
    public Vector3 PositiveX { get; set; }
    // -
    public Vector3 PositiveY { get; set; }
    // -
    public bool ToNear { get; }
    // -
    public Matrix4 View { get; set; }

    // -
    public OffCenterPerspectiveProjection(float near, float far, Vector3 center, Vector3 positiveX, Vector3 positiveY, bool toNear, Matrix4 view)
        : base(near, far)
    {
        Center = center;
        PositiveX = positiveX;
        PositiveY = positiveY;
        ToNear = toNear;
        View = view;
    }

    // -
    public override Matrix4 GetMatrix(float aspect)
    {
        return Matrix4.GetOffCenterProjection(Center, aspect * PositiveX, PositiveY, Near, Far, ToNear, View);
    }
}

// Ditto.
public class OrthographicProjection : Projection
{
    // that is applied in the transformation to the projection plan.
    public float Scale { get; set; }

    // Create a new orthographic projection.
    public OrthographicProjection(float near, float far, float scale)
        : base(near, far)
    {
        Scale = scale;
    }

    // Ditto.
    public override Matrix4 GetMatrix(float aspect)
    {
        return Matrix4.GetOrthographicProjection(Scale * aspect, Scale, Near, Far);
    }
}
