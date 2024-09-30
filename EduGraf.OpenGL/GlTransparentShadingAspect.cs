using EduGraf.OpenGL.Enums;

namespace EduGraf.OpenGL;

// This aspect enables transparent objects.
public sealed class GlTransparentShadingAspect(bool enableTransparency) : GlShadingAspect
{
    // Apply this effect for the following actions.
    protected internal override void Apply()
    {
        if (enableTransparency)
        {
            Shading!.Api.Enable(GlCap.Blend);
            Shading!.Api.BlendFunc(GlBlendingFactor.SrcAlpha, GlBlendingFactor.OneMinusSrcAlpha);
        }
    }

    // Unapply this effect for the following actions.
    protected internal override void UnApply()
    {
        if (enableTransparency) Shading!.Api.Disable(GlCap.Blend);
    }

    public override void Dispose() { }
}