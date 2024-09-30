namespace EduGraf.OpenGL;

internal class GlVisualPart(string name, GlSurface surface) : VisualPart(name, surface)
{
    public override void Render() => ((GlSurface)Surface).Draw();
}