using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// See base class.
public abstract class StandardMaterial(float roughness, float metalness) : Material
{
    [Data] protected float Rough { get; } = roughness; // see base-class property
    [Data] protected float Metal { get; } = metalness; // see base-class property

    public override Expression<Func<float>> Roughness => () => Rough; // see overridden property

    public override Expression<Func<float>> Metalness => () => Metal; // see overridden property
}