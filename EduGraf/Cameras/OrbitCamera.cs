using EduGraf.Tensors;
using EduGraf.UI;
using System;

namespace EduGraf.Cameras;

// This camera orbits around the given center on the given radius. Refer to the UI documentation about how to control it.
// Pass in its position and the center around which it orbits. The camera is perspective by default.
public class OrbitCamera(Point3 position, Point3 center, Projection? projection = default)
    : Camera(
        new View(position, center - position, Vector3.UnitY),
        projection ?? new PerspectiveProjection(0.1f, 100, MathF.PI / 4))
{
    private float _radius = (center - position).Length();
    private Vector2 _mousePosition = new(0, 0);

    // this camera looks at and orbits around.
    public Point3 Center { get; private set; } = center;

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
                    var lookOutX = Vector3.Cross(View.LookOut, Vector3.UnitY);
                    var lookOutY = Vector3.Cross(View.LookOut, lookOutX);
                    Center += moveSensitivity * (delta.X * lookOutX + delta.Y * lookOutY);
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