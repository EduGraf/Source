using EduGraf.OpenGL.Enums;
using System;

namespace EduGraf.OpenGL;

internal sealed class GlIndexBuffer(GlApi api, Array data) : IDisposable
{
    internal readonly uint Handle = GlBuffer.CreateBuffer(api, data, GlBufferTarget.ElementArrayBuffer);
    internal readonly int Length = data.Length;

    public void Dispose()
    {
        api.Invoke(() => api.DeleteBuffer(Handle));
    }
}