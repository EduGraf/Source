namespace EduGraf.OpenGL.GlslParser.Tree;

internal abstract class DeclarationNode(Location location) : Node(location)
{
    public abstract string Name { get; }
}