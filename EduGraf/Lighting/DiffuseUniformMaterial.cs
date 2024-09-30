using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents a material of a uniform color that is purely diffuse, i.e. lambertian.
public class DiffuseUniformMaterial(Color4 color) : EmissiveUniformMaterial(color)
{
    public override Expression<Func<Color4>> Remission => () => new Color4(ValueOf(Light.Immission), 1) * Color;
}