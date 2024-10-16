using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that is parallel in all space.
public class ParallelLight(Color3 color, Vector3 direction) : Light
{
    // of the light.
    [Data] public Color3 Color { get; } = color;

    // direction.
    [Data] public Vector3 Heading { get; } = Vector3.Normalize(direction);

    public override Expression<Func<Vector3>> Direction => () => Heading;
    public override Expression<Func<Color3>> Immission => () => MathF.Max(-ValueOf(Direction) * SurfaceNormal, 0) * Color;
}