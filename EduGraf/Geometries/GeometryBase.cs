namespace EduGraf.Geometries;

internal class GeometryBase(float[] position) : IGeometry
{
    [Dimension(3)]
    public float[] Position { get; } = position;

    public int Count => Position.Length / 3;
}