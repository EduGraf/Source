namespace EduGraf.Geometries;

// This is the base interface for all geometries.
public interface IGeometry
{
    // unrolled coordinates (x, y [,z]) of the geometries vertex positions.
    float[] Position { get; }

    // of then vertices.
    int Count { get; }
}