using System;
using System.Linq.Expressions;
using EduGraf.Tensors;

namespace EduGraf.Lighting;

// Represents a material that has the same properties anywhere on its surface.
public class UniformMaterial(float roughness, float metalness, Color3 color) : Material
{
    [Data] private float Rough { get; } = roughness;
    [Data] private float Metal { get; } = metalness;
    [Data] private Color3 Col { get; } = color;

    public override Expression<Func<float>> Roughness => () => Rough;
    public override Expression<Func<float>> Metalness => () => Metal;
    public override Expression<Func<Color3>> Color => () => Col;
}