using System;
using System.Collections.Generic;
using System.Linq;

namespace EduGraf.UI;

// This is the platform independent window abstraction.
public abstract class Window
{
    private readonly List<Action<InputEvent>> _handlers;

    // the width of the window in pixels.
    public int Width { get; protected set; }

    // the height of the window in pixels.
    public int Height { get; protected set; }

    // in float pixels.
    public virtual (float x, float y) MousePosition { get; private set; }

    // in unknown unit.
    public virtual (float x, float y) MouseWheel { get; private set; }

    // the displayed rendering.
    protected Rendering? Rendering { get; private set; }

    // Create a new window.
    protected Window(int width /* in pixels */, int height /* in pixels */, params Action<InputEvent>[] handlers /* window event handlers */)
    {
        Width = width;
        Height = height;

        _handlers = handlers.ToList();
    }

    // Display the rendering.
    public virtual void Show(Rendering rendering)
    {
        // TODO: _handlers.Add(rendering);

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
        Rendering!.Graphic.Render(this, Rendering!);
    }

    // Called by the framework when a keyboard-key is pressed delegating the handling to the camera and rendering.
    protected void OnKeyDown(ConsoleKey key)
    {
        Handle(new KeyInputEvent(Pressing.Down, key));
    }

    // Called by the framework when a keyboard-key is released delegating the handling to the camera and rendering.
    protected void OnKeyUp(ConsoleKey key)
    {
        Handle(new KeyInputEvent(Pressing.Up, key));
    }

    // Called by the framework when a mouse-button is pressed delegating the handling to the camera and rendering.
    protected void OnMouseButtonDown(MouseButton button)
    {
        Handle(new MouseButtonEvent(Pressing.Down, button));
    }

    // Called by the framework when a mouse-button is released delegating the handling to the camera and rendering.
    protected void OnMouseButtonUp(MouseButton button)
    {
        Handle(new MouseButtonEvent(Pressing.Up, button));
    }

    // Called by the framework when the mouse is moved delegating the handling to the camera and rendering.
    protected void OnMouseMove(float x, float y)
    {
        MousePosition = (x, y);
        Handle(new MouseMoveEvent(x, y));
    }

    // Called by the framework when the mouse-wheel is turned delegating the handling to the camera and rendering.
    protected void OnMouseScroll(float deltaX, float deltaY)
    {
        MouseWheel = (MouseWheel.x + deltaX, MouseWheel.y + deltaY);
        Handle(new MouseScrollEvent(deltaX, deltaY));
    }

    private void Handle(InputEvent e)
    {
        foreach (var handler in _handlers) handler(e);
    }
}