using System;
using EduGraf.Tensors;
using EduGraf.UI;

namespace EduGraf.Cameras;

// This camera pans around orthogonal to the initial lookout.
// This is useful for renderings that should look two-dimensional.
// Refer to the UI documentation about how to control it.
public class PanCamera(View view, Projection? projection = default)
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

            if (PressedButtons.TryGetSingle() == MouseButton.Left)
            {
                var lookOutX = Vector3.Cross(View.LookOut, Vector3.UnitY);
                var lookOutY = Vector3.Cross(View.LookOut, lookOutX);
                View.Position += MoveSensitivity * (delta.x * lookOutX + delta.y * lookOutY);
            }
        }

        if (e is MouseScrollEvent mse) Zoom(mse.DeltaY > 0);
    }

    private void Zoom(bool closer)
    { 
        if (Projection is OrthographicProjection orthographic)
        {
            orthographic.Scale *= closer ? ZoomPercentage : 1 / ZoomPercentage;
        }
        else
        {
            var delta = FlySpeed * View.LookOut;
            if (!closer) delta = -delta;
            View.Position += delta;
        }
    }
}