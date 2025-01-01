using System;

namespace EduGraf.Geometries;

// This annotation can be set on properties of objects IGeometry to declare their geometric dimensionality.
// This information is required by the OpenGL backend.
[AttributeUsage(AttributeTargets.Property)]
public class DimensionAttribute(int n) : Attribute
{
    public int N { get; } = n; // the number of dimensions
}