using EduGraf.Geometries;

namespace EduGraf;

// This is the abstraction for all surfaces.
// Refer to the programming concepts documentation to understand how the different concepts work together.
public abstract class Surface(Shading shading, IGeometry geometry)
{
    // of this surface.
    public Shading Shading { get; } = shading;

    // of this surface.
    public IGeometry Geometry { get; } = geometry;
}