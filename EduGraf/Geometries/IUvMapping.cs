namespace EduGraf.Geometries;

// This interface declares a geometry to include texture-mapping information.
public interface IUvMapping : IGeometry
{
    public float[] TextureUv { get; } // unrolled (u, v) texture coordinates
}