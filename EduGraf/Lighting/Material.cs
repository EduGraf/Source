using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This is the base class for all materials
public abstract class Material : LightingBase
{
    // of the material with 0 = fully smooth and 1 = fully rough
    [Calc] public abstract Expression<Func<float>> Roughness { get; }

    // of the material with 0 = non-metal and 1 = metal
    [Calc] public abstract Expression<Func<float>> Metalness { get; }

    // of the material
    [Calc] public abstract Expression<Func<Color3>> Color { get; }
}