using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// Standard material with color-texture.
public class TextureMaterial(float roughness, float metalness, TextureHandle handle)
    : StandardMaterial(roughness, metalness)
{
    [Data] public TextureHandle Handle { get; } = handle; // to the texture

    public override Expression<Func<Color3>> Color => () => Texture(SurfaceTextureUv); // see overridden property

    // the texture coordinates at the surface position
    protected (float u, float v) SurfaceTextureUv => throw new NotSupportedException("not callable");

    // Get the texture color at  given coordinates.
    protected Color3 Texture((float u, float v) uv) => throw new NotSupportedException("not callable");
}
