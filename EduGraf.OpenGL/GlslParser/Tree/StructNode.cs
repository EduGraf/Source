using System;
using System.Collections.Generic;
using System.Linq;

namespace EduGraf.OpenGL.GlslParser.Tree;

internal class StructNode : DeclarationNode
{
    public IdentifierNode Identifier { get; }
    public List<VariableNode> Members { get; }

    public StructNode(Location location, IdentifierNode identifier, List<VariableNode> members)
        : base(location)
    {
        Identifier = identifier;
        Members = members;
    }

    public override string Name => Identifier.Name;

    public override string ToString()
    {
        return $"struct {Name} {{ {Members.Aggregate(string.Empty, (a, b) => a + Environment.NewLine + b)} }}";
    }
}