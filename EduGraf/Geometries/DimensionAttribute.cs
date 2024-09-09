using System;

namespace EduGraf.Geometries;

// This annotation can be set on properties of objects IGeometry to declare their geometric dimensionality.
// This information is required by the OpenGL backend.
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class DimensionAttribute : Attribute
{
    // the number of dimensions.
    public int N { get; }

    // This constructor is used by the C# annotation framework.
    public DimensionAttribute(int n /* the number of dimensions. */)
    {
        N = n;
    }
}