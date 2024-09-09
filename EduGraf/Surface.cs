using EduGraf.Geometries;

namespace EduGraf;

// This is the abstraction for all surfaces.
// Refer to the programming concepts documentation to understand how the different concepts work together.
public abstract class Surface
{
    // of this surface.
    public Shading Shading { get; }

    // of this surface.
    public IGeometry Geometry { get; }

    // Create a new surface.
    protected Surface(Shading shading, IGeometry geometry)
    {
        Shading = shading;
        Geometry = geometry;
    }
}