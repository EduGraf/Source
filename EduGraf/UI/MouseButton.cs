using System;

namespace EduGraf.UI;

// Platform independent mouse buttons.
[Flags]
public enum MouseButton
{
    None = 0,
    Left = 1,
    Middle = 2,
    Right = 4
}

internal static class MouseButtonExtensions
{
    public static MouseButton TryGetSingle(this MouseButton mouse)
    {
        return mouse switch
        {
            MouseButton.Left => MouseButton.Left,
            MouseButton.Middle => MouseButton.Middle,
            MouseButton.Right => MouseButton.Right,
            _ => MouseButton.None
        };
    }
}