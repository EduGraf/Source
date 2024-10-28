using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that uniformly comes from a hemisphere.
public class HemisphereLight(Color3 color, Vector3 sky) : Light(color)
{
    // the direction to the sky.
    [Data] protected Vector3 Sky { get; } = sky;

    public override Expression<Func<Material, Color3>> Remission => material => 
        (1 - MathF.Acos(Sky * SurfaceNormal) / MathF.PI) *
        ValueOf(BaseRemission);
}