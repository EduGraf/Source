using System;

namespace EduGraf.OpenGL.GlslParser.Tree;

internal class IdentifierNode : Node, IEquatable<IdentifierNode>
{
    public string Name { get; }

    public IdentifierNode(Location location, string name)
        : base(location)
    {
        Name = name;
    }

    public override string ToString() => Name;

    public bool Equals(IdentifierNode? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj) => Equals(obj as IdentifierNode);

    public override int GetHashCode() => Name.GetHashCode();
}