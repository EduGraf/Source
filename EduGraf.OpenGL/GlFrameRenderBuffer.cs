using EduGraf.OpenGL.Enums;
using System;

namespace EduGraf.OpenGL;

internal sealed class GlFrameRenderBuffer : GlFrameBuffer
{
    private readonly GlApi _api;
    private readonly uint _handle;

    public GlFrameRenderBuffer(GlApi api, int width, int height, GlRenderbufferStorage storage, GlFramebufferAttachment attachment)
        : base(api)
    {
        _api = api;

        _handle = api.GenRenderbuffer();
        api.BindRenderbuffer(GlRenderbufferTarget.Renderbuffer, _handle);
        api.RenderbufferStorage(GlRenderbufferTarget.Renderbuffer, storage, width, height);
        api.FramebufferRenderbuffer(GlFramebufferTarget.Framebuffer, attachment, GlRenderbufferTarget.Renderbuffer, _handle);

        var errorCode = api.CheckFramebufferStatus(GlFramebufferTarget.Framebuffer);
        if (errorCode != GlFramebufferErrorCode.FramebufferComplete)
        {
            throw new InvalidOperationException($"framebuffer not configured correctly, error code {errorCode}");
        }
    }

    public override void Dispose()
    {
        _api.Invoke(() => _api.DeleteRenderbuffer(_handle));
        base.Dispose();
    }
}