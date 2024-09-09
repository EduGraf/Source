namespace EduGraf.Geometries;

internal class TexturedNormalGeometry : NormalGeometry, IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; }

    public TexturedNormalGeometry(float[] position, float[] normals, float[] textureUvs)
        : base(position, normals)
    {
        TextureUv = textureUvs;
    }
}