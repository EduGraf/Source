using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that is parallel in all space.
public class ParallelLight : Light
{
    // direction.
    [Data] public Vector3 Heading { get; }

    // of the light.
    [Data] public Color3 Color { get; }

    public ParallelLight(Color3 color, Vector3 direction)
    {
        Heading = Vector3.Normalize(direction);
        Color = color;
    }

    public override Expression<Func<Vector3>> Direction => () => Heading;
    public override Expression<Func<Color3>> Immission => () => MathF.Max(Vector3.Dot(-ValueOf(Direction), SurfaceNormal), 0) * Color;
}