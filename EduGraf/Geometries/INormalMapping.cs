namespace EduGraf.Geometries;

// This interface declares a geometry to include explicitly defined normals.
public interface INormalMapping : IGeometry
{
    // unrolled vector coordinates (x, y , z).
    float[] Normal { get; }
}