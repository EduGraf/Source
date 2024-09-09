using EduGraf.Tensors;
using System;
using System.Linq.Expressions;

namespace EduGraf.Lighting;

// This is the abstract base class for lights and materials. The methods must only be used to describe light and material properties.
public abstract class LightingBase
{
    // the current point on the surface.
    protected Point3 SurfacePosition => throw new NotSupportedException("not callable");

    // the current surface normal.
    protected Vector3 SurfaceNormal => throw new NotSupportedException("not callable");

    // refer to a calculated property.
    protected T ValueOf<T>(Expression<Func<T>> expression) => throw new NotSupportedException("not callable");

    // Create a new 2d vector.
    protected Vector2 VectorOf(float x, float y) => throw new NotSupportedException("not callable");

    // Create a new 3d vector.
    protected Vector3 VectorOf(float x, float y, float z) => throw new NotSupportedException("not callable");

    // Create a new 4d vector.
    protected Vector4 VectorOf(float x, float y, float z, float w) => throw new NotSupportedException("not callable");

    // Create a new color without transparency.
    protected Color3 ColorOf(float r, float g, float b) => throw new NotSupportedException("not callable");

    // Create a new color with transparency.
    protected Color4 ColorOf(float r, float g, float b, float a) => throw new NotSupportedException("not callable");

    // Add transparency to a color.
    protected Color4 ColorOf(Color3 col, float a) => throw new NotSupportedException("not callable");
}
