using EduGraf.Tensors;
using System;

namespace EduGraf.Lighting;

// This is the base class for texture materials.
public abstract class TextureMaterial(TextureHandle handle) : Material
{
    // reference to the texture.
    [Data]
    public TextureHandle Handle { get; } = handle;

    // the texture coordinates.
    protected Vector2 SurfaceTextureUv => throw new NotSupportedException("not callable");

    // Get the color at the given coordinates.
    protected Color4 Texture(Vector2 uv) => throw new NotSupportedException("not callable");
}
