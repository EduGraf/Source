using EduGraf.OpenGL.Enums;
using System;

namespace EduGraf.OpenGL;

// This is the OpenGL texture-handle implementation.
public class GlTextureHandle(GlApi api, uint handle) : TextureHandle, IDisposable
{
    // id of the texture on the GPU
    protected internal uint Handle { get; } = handle;

    // Activates the texture on the selected unit.
    public void Activate(int unit)
    {
        api.ActiveTexture(GlTextureUnit.Texture0 + unit);
        api.BindTexture(GlTextureTarget.Texture2D, Handle);
    }

    // Deactivates the texture.
    public void Deactivate()
    {
        api.ClearTexture();
    }

    public void Dispose()
    {
        api.Invoke(() => api.DeleteTexture(Handle));
    }
}