using EduGraf.Tensors;
using EduGraf.UI;
using System;

namespace EduGraf.Cameras;

// This camera can fly around in the scene like a little plane. Refer to the UI documentation about how to control it.
// The camera is perspective by default.
public class FlyCamera(View view, Projection? projection = default)
    : Camera(view, projection ?? new PerspectiveProjection(0.1f, 100, MathF.PI / 4))
{
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
            var delta = UpdateMouse((mme.X, mme.Y));

            if (PressedButtons.TryGetSingle() == MouseButton.Left)
            {
                Yaw += delta.x * RotateSensitivity;
                Pitch = Limit(Pitch - delta.y * MoveSensitivity, MathF.PI / 2);
            }
        }

        if (e is MouseScrollEvent mse) View.Position -= mse.DeltaY * View.LookOut;
    }

    private Vector3 GetHorizontalDelta()
    {
        return Vector3.Normalize(FlySpeed * Vector3.Cross(View.LookOut, Vector3.UnitY));
    }
}