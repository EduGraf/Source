namespace EduGraf.OpenGL.GlslParser.Tree;

internal class DefineNode : DeclarationNode
{
    public DefineNode(Location location)
        : base(location)
    {
    }

    public override string Name => string.Empty;

    public override string ToString() => "#define";
}