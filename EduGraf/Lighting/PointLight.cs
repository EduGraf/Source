using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that originates from a point.
public class PointLight(Color3 color, float intensity, Point3 position) : Light(color)
{
    // factor.
    [Data] protected float Intensity { get; } = intensity;

    // of its origin.
    [Data] public Point3 Position { get; set; } = position;

    // delta from the light to the surface position.
    [Calc] protected Expression<Func<Vector3>> LightToSurface => () => SurfacePosition - Position;

    // normalized LightToSurface.
    [Calc] protected Expression<Func<Vector3>> Direction => () => Vector3.Normalize(ValueOf(LightToSurface));

    public override Expression<Func<Material, Color3>> Remission => material =>
        (1 - 
            (1 - MathF.Min(1, Intensity / LengthOf(LightToSurface) * MathF.Max(-ValueOf(Direction) * SurfaceNormal, 0))) *
            (1 - MathF.Pow(MathF.Max(Vector3.Normalize(CameraPosition - SurfacePosition) * Vector3.Reflect(ValueOf(Direction), SurfaceNormal), 0), 4 + (1 - ValueOf(material.Roughness)) * 128))
        ) *
        ValueOf(BaseRemission);
}