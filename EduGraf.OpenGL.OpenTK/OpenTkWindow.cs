using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using EduGraf.Cameras;
using EduGraf.UI;
using MouseButton = EduGraf.UI.MouseButton;

namespace EduGraf.OpenGL.OpenTK;

public sealed class OpenTkWindow : UI.Window, IDisposable
{
    private static readonly Dictionary<Keys, ConsoleKey> KeyMapping;

    static OpenTkWindow()
    {
        KeyMapping = new();
        foreach (Keys key in Enum.GetValues(typeof(Keys)))
        {
            string keyName = Enum.GetName(typeof(Keys), key)!;
            if (Enum.TryParse(typeof(ConsoleKey), keyName, out var value))
            {
                KeyMapping.Add(key, (ConsoleKey)value);
            }
            else
            {
                switch (key)
                {
                    case Keys.KeyPad0:
                        KeyMapping.Add(key, ConsoleKey.NumPad0);
                        break;
                    case Keys.KeyPad1:
                        KeyMapping.Add(key, ConsoleKey.NumPad1);
                        break;
                    case Keys.KeyPad2:
                        KeyMapping.Add(key, ConsoleKey.NumPad2);
                        break;
                    case Keys.KeyPad3:
                        KeyMapping.Add(key, ConsoleKey.NumPad3);
                        break;
                    case Keys.KeyPad4:
                        KeyMapping.Add(key, ConsoleKey.NumPad4);
                        break;
                    case Keys.KeyPad5:
                        KeyMapping.Add(key, ConsoleKey.NumPad5);
                        break;
                    case Keys.KeyPad6:
                        KeyMapping.Add(key, ConsoleKey.NumPad6);
                        break;
                    case Keys.KeyPad7:
                        KeyMapping.Add(key, ConsoleKey.NumPad7);
                        break;
                    case Keys.KeyPad8:
                        KeyMapping.Add(key, ConsoleKey.NumPad8);
                        break;
                    case Keys.KeyPad9:
                        KeyMapping.Add(key, ConsoleKey.NumPad9);
                        break;
                    case Keys.Down:
                        KeyMapping.Add(key, ConsoleKey.DownArrow);
                        break;
                    case Keys.Up:
                        KeyMapping.Add(key, ConsoleKey.UpArrow);
                        break;
                    case Keys.Left:
                        KeyMapping.Add(key, ConsoleKey.LeftArrow);
                        break;
                    case Keys.Right:
                        KeyMapping.Add(key, ConsoleKey.RightArrow);
                        break;
                    case Keys.Space:
                        KeyMapping.Add(key, ConsoleKey.Spacebar);
                        break;
                }
            }
        }
    }

    private readonly GameWindow _window;
    private PolygonMode _polygonMode;

    public OpenTkWindow(
        string title,
        OpenTkGraphic graphic,
        int width,
        int height,
        Camera camera,
        params Action<IInputEvent>[] eventHandlers)
        : this(title, graphic, width, height, 60, false, camera, eventHandlers)
    {
    }

    public OpenTkWindow(
        string title,
        OpenTkGraphic graphic,
        int width,
        int height,
        int targetFrameRate,
        bool antiAliased,
        Camera camera,
        params Action<IInputEvent>[] eventHandlers)
        : base(width, height, camera, eventHandlers)
    {
        var settings = new GameWindowSettings
        {
            UpdateFrequency = targetFrameRate
        };
        var nativeSettings = new NativeWindowSettings
        {
            ClientSize = new Vector2i(width, height),
            Title = title,
            RedBits = 8,
            GreenBits = 8,
            BlueBits = 8,
            AlphaBits = 8,
            NumberOfSamples = antiAliased ? 4 : 1
        };
        _window = new GameWindow(settings, nativeSettings);
        if (antiAliased)
        {
            GL.Enable(EnableCap.Multisample);
        }
        _polygonMode = PolygonMode.Fill;

        _window.Resize += OnResize;
        _window.UpdateFrame += OnUpdateFrame;
        _window.RenderFrame += OnRenderFrame;
        _window.KeyDown += OnKeyDown;
        _window.KeyUp += OnKeyUp;
        _window.MouseDown += OnMouseButtonDown;
        _window.MouseUp += OnMouseButtonUp;
        _window.MouseMove += OnMouseMove;
        _window.MouseWheel += OnMouseScroll;
    }

    public override void Show(Rendering rendering)
    {
        base.Show(rendering);
        _window.Run();
    }

    protected override void OnLoad()
    {
        _window.Title += $" (OpenGL Version: {GL.GetString(StringName.Version)})";
        base.OnLoad();
    }

    private void OnResize(ResizeEventArgs e)
    {
        Width = e.Width;
        Height = e.Height;
    }

    private void OnUpdateFrame(FrameEventArgs e)
    {
        OnUpdateFrame();
    }

    private void OnRenderFrame(FrameEventArgs e)
    {
        OnRenderFrame();
        _window.SwapBuffers();
    }

    private void OnKeyDown(KeyboardKeyEventArgs e)
    {
        if (!KeyMapping.TryGetValue(e.Key, out var key)) return;

        OnKeyDown(key);
        if (key == ConsoleKey.G)
        {
            TogglePolygonMode();
            GL.PolygonMode(TriangleFace.FrontAndBack, _polygonMode);
        }
    }

    private void OnKeyUp(KeyboardKeyEventArgs e)
    {
        if (!KeyMapping.TryGetValue(e.Key, out var key)) return;

        OnKeyUp(key);
        if (key == ConsoleKey.Escape) _window.Close();
    }

    private void OnMouseButtonDown(MouseButtonEventArgs e)
    {
        OnMouseButtonDown(GetMouseButton(e));
    }

    private void OnMouseButtonUp(MouseButtonEventArgs e)
    {
        OnMouseButtonUp(GetMouseButton(e));
    }

    private void OnMouseMove(MouseMoveEventArgs e)
    {
        OnMouseMove(e.X, e.Y);
    }

    private void OnMouseScroll(MouseWheelEventArgs e)
    {
        OnMouseScroll(e.Offset.X, e.OffsetY);
    }

    private static MouseButton GetMouseButton(MouseButtonEventArgs e)
    {
        return e.Button switch
        {
            global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left => MouseButton.Left,
            global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle => MouseButton.Middle,
            global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right => MouseButton.Right,
            _ => MouseButton.None
        };
    }

    private void TogglePolygonMode()
    {
        _polygonMode = _polygonMode switch
        {
            PolygonMode.Line => PolygonMode.Fill,
            PolygonMode.Fill => PolygonMode.Line,
            _ => throw new ArgumentOutOfRangeException(nameof(_polygonMode), _polygonMode, "not supported polygon mode")
        };
    }

    public void Dispose()
    {
        _window.Resize -= OnResize;
        _window.UpdateFrame -= OnUpdateFrame;
        _window.RenderFrame -= OnRenderFrame;
        _window.KeyDown -= OnKeyDown;
        _window.MouseMove -= OnMouseMove;
        _window.MouseDown -= OnMouseButtonDown;
        _window.MouseUp -= OnMouseButtonUp;
        _window.MouseWheel -= OnMouseScroll;
        _window.Dispose();
    }
}