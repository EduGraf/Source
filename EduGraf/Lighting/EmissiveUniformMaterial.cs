using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents a material displayed in the given color, i.e. not interacting with light sources.
public class EmissiveUniformMaterial : Material
{
    // of the material.
    [Data] public Color4 Color { get; }

    public EmissiveUniformMaterial(Color4 color)
    {
        Color = color;
        SemiTransparent = color.A < 1;
    }

    public override Expression<Func<Color4>> Remission => () => Color;
}