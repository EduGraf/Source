using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents light that is parallel in all space.
public class ParallelLight(Color3 color, Vector3 direction) : Light(color)
{
    // in which the light travels.
    [Data] protected Vector3 Direction { get; } = Vector3.Normalize(direction);

    public override Expression<Func<Material, Color3>> Remission => material => MathF.Max(-Direction * SurfaceNormal, 0) * ValueOf(BaseRemission);
}