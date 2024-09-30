using System;

namespace EduGraf.Geometries;

// This annotation can be set on properties of objects IGeometry to declare their geometric dimensionality.
// This information is required by the OpenGL backend.
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class DimensionAttribute(int n) : Attribute
{
    // the number of dimensions.
    public int N { get; } = n;
}