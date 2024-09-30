using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that originates from a point.
public class PointLight(Color3 color, Point3 position) : Light
{
    // of light.
    [Data] public Color3 Color { get; } = color;

    // of the origin.
    [Data] public Point3 Position { get; set; } = position;

    // delta from the light to the surface position.
    [Calc] public Expression<Func<Vector3>> LightToSurface => () => SurfacePosition - Position;

    public override Expression<Func<Vector3>> Direction => () => Vector3.Normalize(ValueOf(LightToSurface));

    public override Expression<Func<Color3>> Immission => () =>
        1
        / (ValueOf(LightToSurface) * ValueOf(LightToSurface))
        * MathF.Max(-ValueOf(Direction) * SurfaceNormal, 0)
        * Color;
}