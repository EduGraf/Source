using System;

namespace EduGraf.OpenGL.GlslParser.Tree;

internal class VariableNode : DeclarationNode, IEquatable<VariableNode>
{
    public IdentifierNode Type { get; }
    public IdentifierNode Identifier { get; }

    public VariableNode(Location location, IdentifierNode type, IdentifierNode identifier) :
        base(location)
    {
        Type = type;
        Identifier = identifier;
    }

    public override string Name => Identifier.Name;

    public override string ToString() => $"{Type} {Identifier};";

    public bool Equals(VariableNode? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Type.Equals(other.Type) && Identifier.Equals(other.Identifier);
    }

    public override bool Equals(object? obj) => Equals(obj as VariableNode);

    public override int GetHashCode() => HashCode.Combine(Type, Identifier);
}