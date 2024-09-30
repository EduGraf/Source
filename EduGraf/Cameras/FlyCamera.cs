using EduGraf.Tensors;
using EduGraf.UI;
using System;

namespace EduGraf.Cameras;

// This camera can fly around in the scene like a little plane. Refer to the UI documentation about how to control it.
// The camera is perspective by default.
public class FlyCamera(View view, Projection? projection = default)
    : Camera(view, projection ?? new PerspectiveProjection(0.1f, 100, MathF.PI / 4))
{
    private const float FlySpeed = 1f / 10;
    private const float MoveSensitivity = 1 / 250f;

    private (float x, float y) _mousePosition = (float.NaN, float.NaN);

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
                    View.Position += FlySpeed * View.LookOut;
                    break;

                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                case ConsoleKey.PageDown:
                case ConsoleKey.Q:
                    View.Position -= FlySpeed * View.LookOut;
                    break;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    View.Position -= GetHorizontalDelta();
                    break;

                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    View.Position += GetHorizontalDelta();
                    break;

                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    View.Position += FlySpeed * Vector3.UnitY;
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    View.Position -= FlySpeed * Vector3.UnitY;
                    break;
            }
        }

        if (e is MouseMoveEvent mme)
        {
            float x = mme.X;
            float y = mme.Y;

            float deltaX = float.IsNaN(_mousePosition.x) ? 0 : x - _mousePosition.x;
            float deltaY = float.IsNaN(_mousePosition.y) ? 0 : y - _mousePosition.y;
            _mousePosition = (x, y);

            if (PressedButtons.TryGetSingle() == MouseButton.Left)
            {
                Yaw += deltaX * MoveSensitivity;
                Pitch = Limit(Pitch - deltaY * MoveSensitivity, MathF.PI / 2);
            }
        }

        if (e is MouseScrollEvent mse) View.Position -= mse.DeltaY * View.LookOut;
    }

    private Vector3 GetHorizontalDelta()
    {
        return Vector3.Normalize(FlySpeed * Vector3.Cross(View.LookOut, Vector3.UnitY));
    }
}