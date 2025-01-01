using EduGraf.Tensors;
using EduGraf.UI;
using System;

namespace EduGraf.Cameras;

// This camera orbits around the given center on the given radius.
// Refer to the UI documentation about how to control it.
// Pass in its position and the center around which it orbits.
// The camera is perspective by default.
public class OrbitCamera(Point3 position, Point3 center, Vector3 up, Projection? projection = default)
    : Camera(
        new View(position, center - position, up),
        projection ?? new PerspectiveProjection(0.1f, 100, MathF.PI / 4))
{
    private float _radius = (center - position).Length;

    public Point3 Center { get; private set; } = center; // this camera looks at and orbits around

    // See overridden method.
    public override void Handle(IInputEvent e)
    {
        base.Handle(e);

        if (e is KeyInputEvent ke)
        {
            switch (ke.Key)
            {
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                case ConsoleKey.E:
                    Zoom(true);
                    break;

                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                case ConsoleKey.Q:
                    Zoom(false);
                    break;
            }
        }

        if (e is MouseMoveEvent mme)
        {
            var delta = UpdateMouse((mme.X, mme.Y));

            switch (PressedButtons.TryGetSingle())
            {
                case MouseButton.Left:
                    Pitch += RotateSensitivity * delta.y;
                    Yaw += RotateSensitivity * delta.x;
                    break;

                case MouseButton.Right:
                    var lookOutX = Vector3.Cross(View.LookOut, Vector3.UnitY);
                    var lookOutY = Vector3.Cross(View.LookOut, lookOutX);
                    Center += MoveSensitivity * (delta.x * lookOutX + delta.y * lookOutY);
                    break;
            }

            UpdatePosition();
        }

        if (e is MouseScrollEvent mse) Zoom(mse.DeltaY > 0);
    }

    private void Zoom(bool closer)
    {
        float scale = closer ? ZoomPercentage : 1 / ZoomPercentage;
        _radius *= scale;

        if (Projection is OrthographicProjection orthographic) orthographic.Scale *= scale;

        UpdatePosition();
    }

    private void UpdatePosition() => View.Position = Center - _radius * View.LookOut;
}