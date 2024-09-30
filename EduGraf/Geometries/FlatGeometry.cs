namespace EduGraf.Geometries;

internal class FlatGeometry(float[] position) : IGeometry
{
    [Dimension(2)]
    public float[] Position { get; } = position;

    public int Count => Position.Length / 2;
}