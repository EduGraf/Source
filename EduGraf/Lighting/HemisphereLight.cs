using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that uniformly comes from a hemisphere.
public class HemisphereLight(Color3 color, Vector3 sky) : Light
{
    // of the light.
    [Data] public Color3 Color { get; } = color;

    // the direction to the sky.
    [Data] public Vector3 Sky { get; } = sky;

    public override Expression<Func<Vector3>> Direction => () => Vector3.Zero;

    public override Expression<Func<Color3>> Immission => () => (1 - MathF.Acos(Sky * SurfaceNormal) / MathF.PI) * Color;
}