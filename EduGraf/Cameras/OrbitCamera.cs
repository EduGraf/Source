using EduGraf.Tensors;
using EduGraf.UI;
using System;

namespace EduGraf.Cameras;

// This camera orbits around the given center on the given radius. Refer to the UI documentation about how to control it.
public class OrbitCamera : Camera
{
    private float _radius;
    private Vector2 _mousePosition;

    // this camera looks at and orbits around.
    public Point3 Center { get; private set; }

    // Create a new camera.
    public OrbitCamera(Point3 position /* where the camera is located */, Point3 center, Projection? projection = default /* by default perspective */)
        : base(
            new View(position, center - position, Space.Unit3Y), 
            projection ?? new PerspectiveProjection(0.1f, 100, MathF.PI / 4))
    {
        _radius = (center - position).Length();
        _mousePosition = new(0, 0);
        Center = center;
    }

    // Hande the input event. Pass this to the window inorder to receive the events.
    public override void Handle(InputEvent e)
    {
        base.Handle(e);

        if (e is KeyInputEvent ke)
        {
            switch (ke.Key)
            {
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                case ConsoleKey.PageUp:
                case ConsoleKey.E:
                    Zoom(-1);
                    break;

                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                case ConsoleKey.PageDown:
                case ConsoleKey.Q:
                    Zoom(+1);
                    break;
            }
        }

        if (e is MouseMoveEvent mme)
        {
            float x = mme.X;
            float y = mme.Y;

            const float rotateSensitivity = 1f / 200;
            const float moveSensitivity = 2 * rotateSensitivity;

            var newPosition = new Vector2(x, y);
            var delta = _mousePosition - newPosition;
            _mousePosition = newPosition;

            switch (PressedButtons.TryGetSingle())
            {
                case MouseButton.Left:
                    Pitch += rotateSensitivity * delta.Y;
                    Yaw += rotateSensitivity * delta.X;
                    break;

                case MouseButton.Right:
                    var lookAtX = Vector3.Cross(View.LookOut, Space.Unit3Y);
                    var lookAtY = Vector3.Cross(View.LookOut, lookAtX);
                    Center += moveSensitivity * (delta.X * lookAtX + delta.Y * lookAtY);
                    break;
            }

            UpdatePosition();
        }

        if (e is MouseScrollEvent mse) Zoom(mse.DeltaY);
    }

    private void Zoom(float value)
    {
        _radius += value;

        if (Projection is OrthographicProjection orthographic)
        {
            const float sensitivity = 1f / 10f;
            orthographic.Scale *= 1 + sensitivity * value;
        }

        UpdatePosition();
    }

    private void UpdatePosition() => View.Position = Center - _radius * View.LookOut;
}