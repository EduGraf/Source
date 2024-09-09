using EduGraf.Tensors;

namespace EduGraf;

// This is a mathematical representation of the view.
public class View
{
    private Vector3 _lookOut;

    // from which the view is.
    public Point3 Position { get; set; }

    // into the direction
    public Vector3 LookOut
    {
        get => _lookOut;
        set => _lookOut = Vector3.Normalize(value);
    }

    // vertical direction.
    public Vector3 Up { get; set; }

    // Create a new view.
    public View(Point3 position, Vector3 lookOut, Vector3 up)
    {
        Position = position;
        LookOut = lookOut;
        Up = up;
    }

    // Get the rotation matrix corresponding to the view.
    public Matrix4 GetViewMatrix() => Matrix4.GetView(Position, LookOut, Up);
}