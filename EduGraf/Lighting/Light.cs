using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This is the base class for all lights.
public abstract class Light(Color3 color) : LightingBase
{
    // of the light.
    [Data] public Color3 Color { get; } = color;

    // of the light.
    [Data] protected float Brightness { get; } = 0.25f*color.R + 0.65f*color.G + 0.1f*color.B;

    // access to base property is not supported for expression trees.
    [Calc] protected Expression<Func<Material, Color3>> BaseRemission => material =>
        ValueOf(material.Metalness) * Brightness * ValueOf(material.Color) +
        (1 - ValueOf(material.Metalness)) * Color * ValueOf(material.Color);

    // the color of light remitted at any surface point.
    [Calc] public virtual Expression<Func<Material, Color3>> Remission => BaseRemission;
}