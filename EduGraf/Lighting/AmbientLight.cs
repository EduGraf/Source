using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light from any direction.
public class AmbientLight : Light
{
    // of the light.
    [Data] public Color3 Color { get; }

    public AmbientLight(Color3 color)
    {
        Color = color;
    }

    public override Expression<Func<Vector3>> Direction => () => Space.Zero3;

    [Calc] public override Expression<Func<Color3>> Immission => () => Color;
}