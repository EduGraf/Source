using EduGraf.OpenGL.Enums;
using System;

namespace EduGraf.OpenGL;

// This is the OpenGL texture-handle implementation.
public class GlTextureHandle : TextureHandle, IDisposable
{
    private readonly GlApi _api;

    // id of the texture on the GPU
    protected internal uint Handle { get; }

    // Create a new handle for the texture.
    public GlTextureHandle(GlApi api, uint handle)
    {
        _api = api;
        Handle = handle;
    }

    // Activates the texture on the selected unit.
    public void Activate(int unit)
    {
        _api.ActiveTexture(GlTextureUnit.Texture0 + unit);
        _api.BindTexture(GlTextureTarget.Texture2D, Handle);
    }

    // Deactivates the texture.
    public void Deactivate()
    {
        _api.ClearTexture();
    }

    public void Dispose()
    {
        _api.Invoke(() => _api.DeleteTexture(Handle));
    }
}