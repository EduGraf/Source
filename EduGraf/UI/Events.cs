using System;

namespace EduGraf.UI;

// The direction of the key or button changing.
public enum Pressing
{
    Down,
    Up
}

// This is the abstract base class for event handling.
public abstract class InputEvent {}

// This is the concrete class for key events.
public class KeyInputEvent(Pressing pressing, ConsoleKey key) : InputEvent
{
    // if the key is going down or up.
    public Pressing Pressing { get; } = pressing;

    // that was pressed.
    public ConsoleKey Key { get; } = key;
}

// This is the concrete class for mouse button events.
public class MouseButtonEvent(Pressing pressing, MouseButton button) : InputEvent
{
    // if the button is going down or up.
    public Pressing Pressing { get; } = pressing;

    // that was pressed.
    public MouseButton Button { get; } = button;
}

// This is the concrete class for mouse move events.
public class MouseMoveEvent(float x, float y) : InputEvent
{
    // absolute position in pixels.
    public float X { get; } = x;

    // absolute position in pixels.
    public float Y { get; } = y;
}

// This is the concrete class for mouse scroll events.
public class MouseScrollEvent(float deltaX, float deltaY) : InputEvent
{
    // relative position in pixels.
    public float DeltaX { get; } = deltaX;

    // relative position in pixels.
    public float DeltaY { get; } = deltaY;
}