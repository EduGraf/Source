namespace EduGraf.OpenGL.GlslParser.Tree;

internal class ProcedureNode : DeclarationNode
{
    public IdentifierNode ReturnType { get; }
    public IdentifierNode Identifier { get; }

    public ProcedureNode(Location location, IdentifierNode returnType, IdentifierNode identifier)
        : base(location)
    {
        ReturnType = returnType;
        Identifier = identifier;
    }

    public override string Name => Identifier.Name;

    public override string ToString() => $"{ReturnType} {Name}() {{}}";
}