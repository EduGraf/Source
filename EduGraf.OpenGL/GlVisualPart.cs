namespace EduGraf.OpenGL;

internal class GlVisualPart : VisualPart
{
    public GlVisualPart(string name, GlSurface surface)
        : base(name, surface)
    {
    }

    public override void Render() => ((GlSurface)Surface).Draw();
}