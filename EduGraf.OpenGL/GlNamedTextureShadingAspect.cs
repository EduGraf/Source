namespace EduGraf.OpenGL;

// This represents an aspect using a texture that can be referred to in the shader by name.
public sealed class GlNamedTextureShadingAspect : GlTextureShadingAspect
{
    private readonly string _name;

    // Create a new aspect.
    public GlNamedTextureShadingAspect(string name, GlTextureHandle handle) // maps the texture-sampler parameter name to texture handle.
        : base(handle)
    {
        _name = name;
    }

    // Activates the texture(s).
    protected internal override void Apply()
    {
        base.Apply();
        Shading!.Set(_name, Unit!.Value, false);
    }
}