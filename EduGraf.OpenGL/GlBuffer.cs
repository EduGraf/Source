using EduGraf.OpenGL.Enums;
using System;
using System.Runtime.InteropServices;

namespace EduGraf.OpenGL;

internal static class GlBuffer
{
    internal static uint CreateBuffer(GlApi api, Array data, GlBufferTarget target)
    {
        int sizeOfT;
        var elementType = data.GetType().GetElementType();
        if (elementType == typeof(double) ||
            elementType == typeof(long) ||
            elementType == typeof(ulong)) sizeOfT = 8;
        else if (elementType == typeof(float) ||
                 elementType == typeof(int) ||
                 elementType == typeof(uint)) sizeOfT = 4;
        else if (elementType == typeof(short) ||
                 elementType == typeof(ushort)) sizeOfT = 2;
        else if (elementType == typeof(sbyte) ||
                 elementType == typeof(byte)) sizeOfT = 1;
        else
            throw new NotSupportedException("only numeric types allowed");

        uint handle = api.GenBuffer();
        api.BindBuffer(target, handle);
        var hostHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            api.BufferData(target, data.Length * sizeOfT, hostHandle.AddrOfPinnedObject());
        }
        finally
        {
            hostHandle.Free();
        }

        long bufferSize = api.GetBufferParameter(target, GlBufferParameterName.BufferSize);
        if (data.Length * sizeOfT != bufferSize)
        {
            throw new InvalidOperationException("array not uploaded correctly");
        }

        api.BindBuffer(target, 0);

        return handle;
    }
}