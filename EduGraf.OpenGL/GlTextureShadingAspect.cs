namespace EduGraf.OpenGL;

// This represents an aspect using a texture and maps the texture-sampler parameter name to texture handle.
public class GlTextureShadingAspect(GlTextureHandle handle) : GlShadingAspect
{
    // ditto.
    protected GlTextureHandle Handle { get; } = handle;

    // acquired for the texture.
    protected int? Unit { get; private set; }

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