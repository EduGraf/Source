using EduGraf.Cameras;
using System;

namespace EduGraf.UI;

// This is the platform independent window abstraction.
public abstract class Window
{
    private readonly Action<IInputEvent>[] _handlers;

    public int Width { get; protected set; } // of the window in pixels

    public int Height { get; protected set; } // of the window in pixels

    public virtual (float x, float y) MousePosition { get; private set; } // in float pixels

    public virtual (float x, float y) MouseWheel { get; private set; } // in unknown unit

    protected Rendering? Rendering { get; private set; } // displayed

    protected Camera Camera { get; } // to produce the rendering

    // Create a new window.
    protected Window(
        int width,     // in pixels
        int height,    // in pixels
        Camera camera, // handles events if handlers are not passed explicitly
        params Action<IInputEvent>[] handlers) // window event handlers
    {
        Width = width;
        Height = height;
        Camera = camera;

        _handlers = handlers.Length == 0
            ? [ camera.Handle ]
            : handlers;
    }

    // Display the rendering.
    public virtual void Show(Rendering rendering)
    {
        Rendering = rendering;
        OnLoad();
    }

    // Do not call this from application programs.
    protected virtual void OnLoad()
    {
        Rendering!.OnLoad(this);
    }

    // Do not call this from application programs.
    protected void OnUpdateFrame()
    {
        Rendering!.OnUpdateFrame(this);
    }

    // Do not call this from application programs.
    protected void OnRenderFrame()
    {
        Rendering!.Graphic.Render(this, Rendering!, Camera);
    }

    // The framework calls this method when a keyboard-key is pressed delegating the handling to the camera and rendering.
    protected void OnKeyDown(ConsoleKey key)
    {
        Handle(new KeyInputEvent(Pressing.Down, key));
    }

    // The framework calls this method when a keyboard-key is released delegating the handling to the camera and rendering.
    protected void OnKeyUp(ConsoleKey key)
    {
        Handle(new KeyInputEvent(Pressing.Up, key));
    }

    // The framework calls this method when a mouse-button is pressed delegating the handling to the camera and rendering.
    protected void OnMouseButtonDown(MouseButton button)
    {
        Handle(new MouseButtonEvent(Pressing.Down, button));
    }

    // The framework calls this method when a mouse-button is released delegating the handling to the camera and rendering.
    protected void OnMouseButtonUp(MouseButton button)
    {
        Handle(new MouseButtonEvent(Pressing.Up, button));
    }

    // The framework calls this method when the mouse is moved delegating the handling to the camera and rendering.
    protected void OnMouseMove(float x, float y)
    {
        MousePosition = (x, y);
        Handle(new MouseMoveEvent(x, y));
    }

    // The framework calls this method when the mouse-wheel is turned delegating the handling to the camera and rendering.
    protected void OnMouseScroll(float deltaX, float deltaY)
    {
        MouseWheel = (MouseWheel.x + deltaX, MouseWheel.y + deltaY);
        Handle(new MouseScrollEvent(deltaX, deltaY));
    }

    private void Handle(IInputEvent e)
    {
        foreach (var handler in _handlers) handler(e);
    }
}