namespace EduGraf.Geometries;

internal class GeometryBase : IGeometry
{
    [Dimension(3)]
    public float[] Position { get; }

    public int Count => Position.Length / 3;

    public GeometryBase(float[] position)
    {
        Position = position;
    }
}