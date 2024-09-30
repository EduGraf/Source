using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents the specular part of e.g. the phong model.
public class SpecularMaterial(Point3 cameraPosition, float shininess) : Material
{
    // current position.
    [Data] public Point3 CameraPosition { get; set; } = cameraPosition;

    // the sharpness of the point reflection.
    [Data] public float Shininess { get; } = shininess;

    public override Expression<Func<Color4>> Remission => () =>
        new Color4(
            MathF.Pow(
               MathF.Max(
                   Vector3.Normalize(CameraPosition - SurfacePosition) * 
                   Vector3.Reflect(ValueOf(Light.Direction), SurfaceNormal), 
                   0),
               Shininess) * 
            ValueOf(Light.Immission),
            1);
}