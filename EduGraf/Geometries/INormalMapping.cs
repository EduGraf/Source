namespace EduGraf.Geometries;

// This interface declares a geometry to include explicitly defined normals.
public interface INormalMapping : IGeometry
{
    public float[] Normal { get; } // unrolled vector coordinates (x, y , z)
}