using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that originates from a point.
public class PointLight : Light
{
    // of light.
    [Data] public Color3 Color { get; }

    // of the origin.
    [Data] public Point3 Position { get; set; }

    public PointLight(Color3 color, Point3 position)
    {
        Position = position;
        Color = color;
    }

    // delta from the light to the surface position.
    [Calc] public Expression<Func<Vector3>> LightToSurface => () => SurfacePosition - Position;

    public override Expression<Func<Vector3>> Direction => () => Vector3.Normalize(ValueOf(LightToSurface));

    public override Expression<Func<Color3>> Immission => () =>
        1
        / Vector3.Dot(ValueOf(LightToSurface), ValueOf(LightToSurface))
        * MathF.Max(Vector3.Dot(-ValueOf(Direction), SurfaceNormal), 0)
        * Color;
}