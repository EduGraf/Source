using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light from any direction.
public class AmbientLight(Color3 color) : Light
{
    // of the light.
    [Data] public Color3 Color { get; } = color;

    public override Expression<Func<Vector3>> Direction => () => Vector3.Zero;

    [Calc] public override Expression<Func<Color3>> Immission => () => Color;
}