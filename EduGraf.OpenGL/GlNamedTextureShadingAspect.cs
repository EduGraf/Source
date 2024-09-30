namespace EduGraf.OpenGL;

// This represents an aspect using a texture that can be referred to in the shader by name and maps the texture-sampler parameter name to texture handle.
public sealed class GlNamedTextureShadingAspect(string name, GlTextureHandle handle) : GlTextureShadingAspect(handle)
{
    // Activates the texture(s).
    protected internal override void Apply()
    {
        base.Apply();
        Shading!.Set(name, Unit!.Value, false);
    }
}