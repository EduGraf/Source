using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This represents the specular part of e.g. the phong model.
public class SpecularMaterial : Material
{
    // current position.
    [Data] public Point3 CameraPosition { get; set; }

    // the sharpness of the point reflection.
    [Data] public float Shininess { get; }

    public SpecularMaterial(Point3 cameraPosition, float shininess)
    {
        CameraPosition = cameraPosition;
        Shininess = shininess;
    }

    public override Expression<Func<Color4>> Remission => () =>
        new Color4(
            MathF.Pow(
               MathF.Max(
                   Vector3.Dot(
                       Vector3.Normalize(CameraPosition - SurfacePosition),
                       Vector3.Reflect(ValueOf(Light.Direction),
                           SurfaceNormal)), 0),
               Shininess) * ValueOf(Light.Immission),
            1);
}