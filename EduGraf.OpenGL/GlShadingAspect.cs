using System;

namespace EduGraf.OpenGL;

// A shading can have multiple visual properties or aspects that can be defined separately.
public abstract class GlShadingAspect : IDisposable
{
    // the aspect belongs to.
    protected internal GlShading? Shading { get; internal set; }

    // Apply this effect for the following actions.
    protected internal abstract void Apply();

    // Unapply this effect for the following actions.
    protected internal abstract void UnApply();

    // all used disposables and resources.
    public abstract void Dispose();
}