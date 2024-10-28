using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This is the base class for texture materials.
public class ColorTextureMaterial(float roughness, float metalness, TextureHandle handle)
    : Material
{
    [Data] private float Rough { get; } = roughness;
    [Data] private float Metal { get; } = metalness;

    // to the texture.
    [Data] public TextureHandle Handle { get; } = handle;

    public override Expression<Func<float>> Roughness => () => Rough;
    public override Expression<Func<float>> Metalness => () => Metal;

    // the texture coordinates.
    protected Vector2 SurfaceTextureUv => throw new NotSupportedException("not callable");
    // the color at the given coordinates.
    protected Color3 Texture(Vector2 uv) => throw new NotSupportedException("not callable");
    public override Expression<Func<Color3>> Color => () => Texture(SurfaceTextureUv);
}
