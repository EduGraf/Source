namespace EduGraf.OpenGL;

// This represents an aspect using a texture.
public class GlTextureShadingAspect : GlShadingAspect
{
    // ditto.
    protected GlTextureHandle Handle { get; }

    // acquired for the texture.
    protected int? Unit { get; private set; }

    // Create a new aspect.
    public GlTextureShadingAspect(GlTextureHandle handle) // maps the texture-sampler parameter name to texture handle.
    {
        Handle = handle;
    }

    // Activates the texture(s).
    protected internal override void Apply()
    {
        Unit = Shading!.Api.AcquireTextureUnit();
        Handle.Activate(Unit.Value);
    }

    // Deactivates the texture(s).
    protected internal override void UnApply()
    {
        Handle.Deactivate();
        Shading!.Api.ReleaseTextureUnit(Unit!.Value);
    }

    // Ditto.
    public override void Dispose() => Handle.Dispose();
}