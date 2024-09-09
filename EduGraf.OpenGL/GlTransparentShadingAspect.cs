using EduGraf.OpenGL.Enums;

namespace EduGraf.OpenGL;

// This aspect enables transparent objects.
public sealed class GlTransparentShadingAspect : GlShadingAspect
{
    private readonly bool _enableTransparency;

    // Create a new aspect.
    public GlTransparentShadingAspect(bool enableTransparency)
    {
        _enableTransparency = enableTransparency;
    }

    // Apply this effect for the following actions.
    protected internal override void Apply()
    {
        if (_enableTransparency)
        {
            Shading!.Api.Enable(GlCap.Blend);
            Shading!.Api.BlendFunc(GlBlendingFactor.SrcAlpha, GlBlendingFactor.OneMinusSrcAlpha);
        }
    }

    // Unapply this effect for the following actions.
    protected internal override void UnApply()
    {
        if (_enableTransparency) Shading!.Api.Disable(GlCap.Blend);
    }

    public override void Dispose() { }
}