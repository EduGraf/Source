using System;

namespace EduGraf.UI;

// The direction of the key or button changing.
public enum Pressing
{
    Down,
    Up
}

// This is the abstract base class for event handling.
public interface IInputEvent;

// This is the concrete class for key events.
public readonly struct KeyInputEvent(Pressing pressing, ConsoleKey key) : IInputEvent
{
    public Pressing Pressing { get; } = pressing; // if the key is going down or up

    public ConsoleKey Key { get; } = key; // that was pressed
}

// This is the concrete class for mouse button events.
public readonly struct MouseButtonEvent(Pressing pressing, MouseButton button) : IInputEvent
{
    public Pressing Pressing { get; } = pressing; // if the button is going down or up

    public MouseButton Button { get; } = button; // that was pressed
}

// This is the concrete class for mouse move events.
public readonly struct MouseMoveEvent(float x, float y) : IInputEvent
{
    public float X { get; } = x; // absolute position in pixels

    public float Y { get; } = y; // absolute position in pixels
}

// This is the concrete class for mouse scroll events.
public readonly struct MouseScrollEvent(float deltaX, float deltaY) : IInputEvent
{
    public float DeltaX { get; } = deltaX; // relative position in pixels

    public float DeltaY { get; } = deltaY; // relative position in pixels
}