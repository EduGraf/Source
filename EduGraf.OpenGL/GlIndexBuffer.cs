using EduGraf.OpenGL.Enums;
using System;

namespace EduGraf.OpenGL;

internal sealed class GlIndexBuffer : IDisposable
{
    private readonly GlApi _api;
    internal readonly uint Handle;
    internal readonly int Length;

    public GlIndexBuffer(GlApi api, Array data)
    {
        _api = api;
        Handle = GlBuffer.CreateBuffer(api, data, GlBufferTarget.ElementArrayBuffer);
        Length = data.Length;
    }

    public void Dispose()
    {
        _api.Invoke(() => _api.DeleteBuffer(Handle));
    }
}