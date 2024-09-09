namespace EduGraf.Geometries;

// This interface declares a geometry to include texture-mapping information.
public interface IUvMapping : IGeometry
{
    // unrolled (u, v) texture coordinates.
    float[] TextureUv { get; }
}