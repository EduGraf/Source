namespace EduGraf.OpenGL.GlslParser.Tree;

internal class ArrayVariableNode : VariableNode
{
    public ArrayVariableNode(Location location, VariableNode variable)
        : base(location, variable.Type, variable.Identifier)
    {
    }

    public override string ToString() => $"{base.ToString()}[]";
}