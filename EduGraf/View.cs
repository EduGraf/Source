using EduGraf.Tensors;

namespace EduGraf;

// This is a mathematical representation of a view.
public class View
{
    private Vector3 _lookOut;
    private Vector3 _up;

    // from where the view looks
    public Point3 Position { get; set; }

    // from the position
    public Vector3 LookOut
    {
        get => _lookOut;
        set => _lookOut = Vector3.Normalize(value);
    }

    // vertical direction
    public Vector3 Up
    {
        get => _up;
        set => _up = Vector3.Normalize(value);
    }

    // Create a new view.
    public View(Point3 position, Vector3 lookOut, Vector3 up)
    {
        Position = position;
        LookOut = lookOut;
        Up = up;
    }

    // Get the rotation matrix corresponding to the view.
    public Matrix4 GetMatrix() => Matrix4.GetView(this);
}