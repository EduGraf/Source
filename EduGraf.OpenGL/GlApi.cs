using EduGraf.OpenGL.Enums;
using EduGraf.Tensors;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EduGraf.OpenGL;

// This is the generic low-level OpenGL api abstraction. Consult OpenGL documentation for details about abstracted the methods.
public abstract class GlApi
{
    private readonly Queue<Action> _queue = new();

    // Execute an action on the device.
    public void Invoke(Action action)
    {
        _queue.Enqueue(action);
    }

    internal void ExecutePending()
    {
        while (_queue.Count > 0)
        {
            _queue.Dequeue().Invoke();
        }
    }

    private readonly BitArray _textureUnitsInUse = new(64);

    // Shading aspects working with textures need to acquire a unit to handle the texture when applied.
    public int AcquireTextureUnit()
    {
        for (int unit = 0; unit < _textureUnitsInUse.Length; unit++)
        {
            if (!_textureUnitsInUse[unit])
            {
                _textureUnitsInUse[unit] = true;
                return unit;
            }
        }

        throw new InvalidOperationException("run out of texture units");
    }

    // Shading aspects need to release acquired texture units when unapplied.
    public void ReleaseTextureUnit(int unit)
    {
        if (!_textureUnitsInUse[unit]) throw new InvalidOperationException("release of not acquired texture unit");
        _textureUnitsInUse[unit] = false;
    }

    internal void CheckAllTextureUnitReleased()
    {
        foreach (bool unit in _textureUnitsInUse)
        {
            if (unit) throw new InvalidOperationException("texture units are not acquired or released correctly");
        }
    }

    [OmitInDocumentation] protected internal abstract void Viewport(int x, int y, int width, int height);
    [OmitInDocumentation] protected internal abstract void ClearColor(float r, float g, float b, float a);
    [OmitInDocumentation] protected internal abstract void Clear(GlBufferBit bufferBit);

    [OmitInDocumentation] protected internal abstract uint GenVertexArray();
    [OmitInDocumentation] protected internal abstract uint GenBuffer();
    [OmitInDocumentation] protected internal abstract void BufferData(GlBufferTarget target, int size, nint data);
    [OmitInDocumentation] protected internal abstract long GetBufferParameter(GlBufferTarget target, GlBufferParameterName name);
    [OmitInDocumentation] protected internal abstract void FloatVertexAttribPointer(uint index, int size, int stride, int offset);
    [OmitInDocumentation] protected internal abstract void BindVertexArray(uint array);
    [OmitInDocumentation] protected internal abstract void BindBuffer(GlBufferTarget target, uint buffer);
    [OmitInDocumentation] protected internal abstract void EnableVertexAttribArray(uint array);
    [OmitInDocumentation] protected internal abstract void DrawTriangles(int count);
    [OmitInDocumentation] protected internal abstract void DrawIndexedTriangles<T>(int count, nint indices);
    [OmitInDocumentation] protected internal abstract void Enable(GlCap cap);
    [OmitInDocumentation] protected internal abstract void Disable(GlCap cap);
    [OmitInDocumentation] protected internal abstract void BlendFunc(GlBlendingFactor srcFactor, GlBlendingFactor dstFactor);

    [OmitInDocumentation] protected internal abstract uint GenTexture();
    [OmitInDocumentation] protected internal abstract void BindTexture(GlTextureTarget target, uint texture);
    [OmitInDocumentation] protected internal abstract void TexImage2D(GlTextureTarget target, int level, int width, int height, int border, GlPixelFormat format, GlPixelType type, nint pixels);
    [OmitInDocumentation] protected internal abstract void GenerateMipmap(GlTextureTarget target);

    [OmitInDocumentation] protected internal abstract void TextureParameterI(GlTextureTarget target, GlTextureParameterName name, GlTextureParameter parameter);
    [OmitInDocumentation] protected internal abstract void ActiveTexture(GlTextureUnit unit);
    [OmitInDocumentation] protected internal abstract void DeleteTexture(uint texture);

    [OmitInDocumentation]
    protected internal void ClearTexture()
    {
        BindTexture(GlTextureTarget.Texture2D, 0);
    }

    [OmitInDocumentation] protected internal abstract void GetTexImage(GlTextureTarget target, int level, GlPixelFormat format, GlPixelType type, nint pixels);

    [OmitInDocumentation] protected internal abstract uint GenFramebuffer();
    [OmitInDocumentation] protected internal abstract void BindFramebuffer(GlFramebufferTarget target, uint framebuffer);
    [OmitInDocumentation] protected internal abstract void DeleteFramebuffer(uint framebuffer);
    [OmitInDocumentation] protected internal abstract void FramebufferTexture2D(GlFramebufferTarget target, GlFramebufferAttachment attachment, GlTextureTarget texTarget, uint texture, int level);
    [OmitInDocumentation] protected internal abstract uint GenRenderbuffer();
    [OmitInDocumentation] protected internal abstract void BindRenderbuffer(GlRenderbufferTarget target, uint renderbuffer);
    [OmitInDocumentation] protected internal abstract void RenderbufferStorage(GlRenderbufferTarget target, GlRenderbufferStorage internalFormat, int width, int height);
    [OmitInDocumentation] protected internal abstract void FramebufferRenderbuffer(GlFramebufferTarget target, GlFramebufferAttachment attachment, GlRenderbufferTarget renderbufferTarget, uint renderbuffer);
    [OmitInDocumentation] protected internal abstract GlFramebufferErrorCode CheckFramebufferStatus(GlFramebufferTarget target);
    [OmitInDocumentation] protected internal abstract void DeleteRenderbuffer(uint renderbuffer);
    [OmitInDocumentation] protected internal abstract void DrawBuffer(GlDrawBufferMode buffer);
    [OmitInDocumentation] protected internal abstract void ReadBuffer(GlReadBufferMode src);
    [OmitInDocumentation] protected internal abstract void DrawBuffers(GlFramebufferAttachment[] attachments);

    [OmitInDocumentation] protected internal abstract uint Compile(string vertexShaderSource, string fragmentShaderSource, string fragColorChannel, List<string> errors);
    [OmitInDocumentation] protected internal abstract void UseProgram(uint shaderProgram);
    [OmitInDocumentation] protected internal abstract void DeleteProgram(uint handle);

    [OmitInDocumentation] protected internal abstract void DeleteVertexArray(uint array);
    [OmitInDocumentation] protected internal abstract void DeleteBuffer(uint buffer);
    [OmitInDocumentation] protected internal abstract uint GetAttributeLocation(uint shader, string attributeParameter);
    [OmitInDocumentation] protected internal abstract uint GetUniformLocation(uint shader, string name);

    [OmitInDocumentation] protected internal abstract void Uniform1(uint location, int value);
    [OmitInDocumentation] protected internal abstract void Uniform1(uint location, float value);
    [OmitInDocumentation] protected internal abstract void Uniform2(uint location, float v1, float v2);
    [OmitInDocumentation] protected internal abstract void Uniform3(uint location, float v1, float v2, float v3);
    [OmitInDocumentation] protected internal abstract void Uniform4(uint location, float v1, float v2, float v3, float v4);
    [OmitInDocumentation] protected internal abstract void UniformMatrix2(uint location, bool transpose, Matrix2 value);
    [OmitInDocumentation] protected internal abstract void UniformMatrix3(uint location, bool transpose, Matrix3 value);
    [OmitInDocumentation] protected internal abstract void UniformMatrix4(uint location, bool transpose, Matrix4 value);
    [OmitInDocumentation] protected internal abstract void Uniform1(uint location, float[] values);
    [OmitInDocumentation] protected internal abstract void Uniform2(uint location, float[] values);
    [OmitInDocumentation] protected internal abstract void Uniform3(uint location, float[] values);
    [OmitInDocumentation] protected internal abstract void Uniform4(uint location, float[] values);
}