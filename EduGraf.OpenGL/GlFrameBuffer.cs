using EduGraf.OpenGL.Enums;
using System;

namespace EduGraf.OpenGL;

internal class GlFrameBuffer : IDisposable
{
    private readonly GlApi _api;
    internal readonly uint Handle;

    public GlFrameBuffer(GlApi api)
    {
        _api = api;
        Handle = api.GenFramebuffer();
        api.BindFramebuffer(GlFramebufferTarget.Framebuffer, Handle);
    }

    public virtual void Dispose()
    {
        _api.Invoke(() =>
        {
            _api.BindFramebuffer(GlFramebufferTarget.Framebuffer, Handle);
            _api.DeleteFramebuffer(Handle);
            _api.BindFramebuffer(GlFramebufferTarget.Framebuffer, 0);
        });
    }
}