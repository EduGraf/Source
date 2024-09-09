namespace EduGraf.Geometries;

internal class FlatGeometry : IGeometry
{
    [Dimension(2)]
    public float[] Position { get; }
    public int Count => Position.Length / 2;

    public FlatGeometry(float[] position)
    {
        Position = position;
    }
}