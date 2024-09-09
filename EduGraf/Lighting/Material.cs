using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This is the base class for all materials
public abstract class Material : LightingBase
{
    // tells the rendering engine that the material is semi-transparent, by default it is assumed to be non-transparent.
    public bool SemiTransparent { get; init; }

    // the current light source.
    public Light Light => throw new NotSupportedException("not callable");

    // The light that goes out as a reaction to incoming light.
    // The reaction to each incoming light is calculated separately and added by the framework.
    [Calc] public abstract Expression<Func<Color4>> Remission { get; }
}