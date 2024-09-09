namespace EduGraf.OpenGL.GlslParser.Tree;

internal class ChannelNode : DeclarationNode
{
    public Direction Direction { get; }
    public VariableNode Variable { get; }

    public ChannelNode(Direction direction, VariableNode variable)
        : base(variable.Location)
    {
        Direction = direction;
        Variable = variable;
    }

    public override string Name => Variable.Name;

    public override string ToString() => $"{Direction} {Variable}";
}