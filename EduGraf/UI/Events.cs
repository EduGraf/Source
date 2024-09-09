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
public class KeyInputEvent : InputEvent
{
    // if the key is going down or up.
    public Pressing Pressing { get; }

    // that was pressed.
    public ConsoleKey Key { get; }
 
    // Create a new event.
    public KeyInputEvent(Pressing pressing, ConsoleKey key)
    {
        Pressing = pressing;
        Key = key;
    }
}

// This is the concrete class for mouse button events.
public class MouseButtonEvent : InputEvent
{
    // if the button is going down or up.
    public Pressing Pressing { get; }

    // that was pressed.
    public MouseButton Button { get; }

    // Create a new event.
    public MouseButtonEvent(Pressing pressing, MouseButton button) 
    {
        Pressing = pressing;
        Button = button;
    }
}

// This is the concrete class for mouse move events.
public class MouseMoveEvent : InputEvent
{
    // absolute position in pixels.
    public float X { get; }
    // absolute position in pixels.
    public float Y { get; }  

    // Create a new event.
    public MouseMoveEvent(float x, float y) 
    {
        X = x;
        Y = y;
    }
}

// This is the concrete class for mouse scroll events.
public class MouseScrollEvent : InputEvent
{
    // relative position in pixels.
    public float DeltaX { get; }
    // relative position in pixels.
    public float DeltaY { get; }

    // Create a new event.
    public MouseScrollEvent(float deltaX, float deltaY)
    {
        DeltaX = deltaX;
        DeltaY = deltaY;
    }
}