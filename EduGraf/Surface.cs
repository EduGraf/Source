using EduGraf.Geometries;

namespace EduGraf;

// This is the base class of all surfaces.
// Refer to the programming concepts documentation to understand how the different concepts work together.
public abstract class Surface(Shading shading, IGeometry geometry)
{
    public Shading Shading { get; } = shading; // of this surface

    public IGeometry Geometry { get; } = geometry; // of this surface
}