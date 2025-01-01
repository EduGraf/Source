using EduGraf.Tensors;
using EduGraf.UI;
using System;

namespace EduGraf.Cameras;

// This is the camera abstraction. The camera is oriented such that the x-z plane is horizontal.
public class Camera
{
    protected const float RotateSensitivity = 1f / 200;
    protected const float MoveSensitivity = 2 * RotateSensitivity;
    protected const float ZoomPercentage = 0.95f;
    protected const float FlySpeed = 1f / 5;

    private static readonly float HalfProjectionHeight = MathF.Tan(MathF.PI / 8);

    // that is applied
    public Projection Projection { get; }

    private float _pitch;
    // of this camera.
    public float Pitch
    {
        get => _pitch;
        protected set
        {
            _pitch = Limit(value, 0.499999f * MathF.PI);
            UpdateLookAt();
        }
    }

    private float _yaw;
    // of this camera.
    public float Yaw
    {
        get => _yaw;
        protected set
        {
            _yaw = value;
            UpdateLookAt();
        }
    }

    // from this camera.
    public View View { get; }

    // the set of mouse buttons that is currently pressed.
    protected MouseButton PressedButtons { get; private set; }

    private (float x, float y)? _mousePosition;

    // Create a new camera.
    public Camera(View view, Projection projection)
    {
        View = view;
        Projection = projection;
        var lookOut = view.LookOut;
        _pitch = MathF.Asin(lookOut.Y);
        _yaw = 
            MathF.Sign(lookOut.Z) * 
            MathF.Acos(lookOut.X) / 
            MathF.Cos(lookOut.Y); // division by 0 is not a problem, since lookOut.Y < Pi/2
    }

    // Return the delta to the old and store the new position.
    protected (float x, float y) UpdateMouse((float x, float y) newPosition)
    {
        (float x, float y) delta = _mousePosition.HasValue
            ? (_mousePosition.Value.x - newPosition.x,
               _mousePosition.Value.y - newPosition.y)
            : (0, 0);
        _mousePosition = newPosition;
        return delta;
    }

    private void UpdateLookAt()
    {
        View.LookOut = Vector3.Normalize(
            new Vector3(
                MathF.Cos(_pitch) * MathF.Cos(_yaw),
                MathF.Sin(_pitch),
                MathF.Cos(_pitch) * MathF.Sin(_yaw))
            );
    }

    // Limit the value in the given range plus and minus around zero.
    protected static float Limit(float value, float range)
    {
        return MathF.Min(MathF.Max(value, -range), range);
    }

    // Return the world positions of the projection plane in unit distance from the camera position. This is useful to calculate the view-frustum.
    public (
        Point3 topLeft,
        Point3 topRight,
        Point3 bottomLeft,
        Point3 bottomRight) 
        GetProjectionPlane(float aspect /* the relation of projection width over height. */)
    {
        var unitY = Vector3.UnitY;
        var lookOut = View.LookOut;
        var up = Vector3.Normalize(unitY - unitY * lookOut * lookOut);
        var right = Vector3.Cross(lookOut, up);
        var center = View.Position + lookOut;
        float halfWidth = aspect * HalfProjectionHeight;

        var tl = center + HalfProjectionHeight * up - halfWidth * right;
        var tr = center + HalfProjectionHeight * up + halfWidth * right;
        var bl = center - HalfProjectionHeight * up - halfWidth * right;
        var br = center - HalfProjectionHeight * up + halfWidth * right;

        return (tl, tr, bl, br);
    }

    // The window calls this method to pass events.
    public virtual void Handle(IInputEvent e)
    {
        if (e is MouseButtonEvent mbe)
        {
            switch (mbe.Pressing)
            {
                case Pressing.Down:
                    PressedButtons |= mbe.Button;
                    break;
                case Pressing.Up:
                    PressedButtons &= ~mbe.Button;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mbe.Pressing));
            }
        }
    }
}