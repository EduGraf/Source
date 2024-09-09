using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that uniformly comes from a hemisphere.
public class HemisphereLight : Light
{
    // of the light.
    [Data] public Color3 Color { get; }

    // the direction to the sky.
    [Data] public Vector3 Sky { get; }

    public HemisphereLight(Color3 color, Vector3 sky)
    {
        Sky = sky;
        Color = color;
    }

    public override Expression<Func<Vector3>> Direction => () => Space.Zero3;

    public override Expression<Func<Color3>> Immission => () => (1 - MathF.Acos(Vector3.Dot(Sky, SurfaceNormal)) / MathF.PI) * Color;
}