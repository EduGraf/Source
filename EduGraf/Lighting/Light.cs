using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This is the base class for all lights.
public abstract class Light : LightingBase
{
    // the direction in which the light travels.
    [Calc] public abstract Expression<Func<Vector3>> Direction { get; }

    // the color and intensity of light coming to a surface point.
    [Calc] public abstract Expression<Func<Color3>> Immission { get; }
}