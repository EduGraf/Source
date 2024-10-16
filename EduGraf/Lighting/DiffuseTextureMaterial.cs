using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents a texture material that does not interact with light.
public class DiffuseTextureMaterial(TextureHandle handle) : TextureMaterial(handle)
{
    public override Expression<Func<Color4>> Remission => () => new Color4(ValueOf(Light.Immission), 1) * Texture(SurfaceTextureUv);
}