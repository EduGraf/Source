using EduGraf.Tensors;
using EduGraf.UI;
using System;

namespace EduGraf.Cameras;

// This is the camera abstraction. The camera is oriented such that the x-z plane is horizontal.
public class Camera
{
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
        return Math.Min(Math.Max(value, -range), range);
    }

    // Return the world positions of the projection plane in unit distance from the camera position. This is useful to calculate the view-frustum.
    public (
        Point3 topLeft,
        Point3 topRight,
        Point3 bottomLeft,
        Point3 bottomRight
        ) GetProjectionPlane(float aspect /* the relation of projection width over height. */)
    {
        var unitY = Space.Unit3Y;
        var lookAt = View.LookOut;
        var up = Vector3.Normalize(unitY - Vector3.Dot(unitY, lookAt) * lookAt);
        var right = Vector3.Cross(lookAt, up);
        var center = View.Position + lookAt;
        float halfWidth = aspect * HalfProjectionHeight;

        var tl = center + HalfProjectionHeight * up - halfWidth * right;
        var tr = center + HalfProjectionHeight * up + halfWidth * right;
        var bl = center - HalfProjectionHeight * up - halfWidth * right;
        var br = center - HalfProjectionHeight * up + halfWidth * right;

        return (tl, tr, bl, br);
    }

    // Hande the input event. Pass this to the window inorder to receive the events.
    public virtual void Handle(InputEvent e)
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