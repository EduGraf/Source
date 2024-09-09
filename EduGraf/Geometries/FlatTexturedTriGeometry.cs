namespace EduGraf.Geometries;

internal class FlatTexturedTriGeometry : FlatTriGeometry, IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; }

    public FlatTexturedTriGeometry(float[] position, ushort[] triangles, float[] textureUv)
        : base(position, triangles)
    {
        TextureUv = textureUv;
    }
}