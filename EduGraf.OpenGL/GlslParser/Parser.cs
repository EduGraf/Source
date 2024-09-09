using EduGraf.OpenGL.GlslParser.Tokens;
using EduGraf.OpenGL.GlslParser.Tree;
using System;
using System.Collections.Generic;

namespace EduGraf.OpenGL.GlslParser;

/// <summary>
/// EBNF:
/// program = version { ( "//" ... | declaration ) }.
/// version = '#' "version" integer "core".
/// declaration = struct | channel | define | procedure.
/// struct = "struct" identifier '{' { variable } ';'.
/// channel = [ layout ] ("in" | "uniform" | "out") variable.
/// layout = "layout" "(" "location" "=" number ")".
/// variable = identifier identifier [ '[' ... ']' ] ';'.
/// define = '#' define = ...
/// procedure = identifier identifier '(' ... .
/// number = integer | real.
/// </summary>
internal sealed class Parser
{
    private const string ErrorIdentifier = "$ERROR$";

    private readonly Lexer _lexer;
    private readonly List<string> _errors;
    private Token? _current;

    public Parser(Lexer lexer, List<string> errors)
    {
        _lexer = lexer;
        _errors = errors;
        Next();
    }

    /// program = version { ( "//" ... | declaration ) }.
    public ProgramNode ParseProgram()
    {
        var start = StartPosition;
        Version();
        var declarations = new List<DeclarationNode?>();
        while (!Is(Tag.End))
        {
            if (Is(Tag.LineComment))
            {
                _lexer.SkipToNewLine();
                Next();
            }
            else
            {
                declarations.Add(Declaration());
            }
        }
        return new ProgramNode(new Location(start, EndPosition), declarations);
    }

    /// version = '#' "version" integer
    private void Version()
    {
        if (Is(Tag.Hash))
        {
            Next();
            Check(Tag.Version);
            ReadInteger(false);
        }
    }

    /// declaration = struct | channel | define | procedure.
    private DeclarationNode? Declaration()
    {
        if (Is(Tag.Struct)) { Next(); return Struct(); }
        if (IsAny(Tag.Layout, Tag.Flat, Tag.In, Tag.Out, Tag.Uniform)) return Channel();
        if (Is(Tag.Hash)) { Next(); return Define(); }
        else return Procedure();
    }


    /// struct = "struct" identifier '{' { variable } '}' ';'.
    private StructNode Struct()
    {
        int startPosition = StartPosition;
        var identifier = Identifier();
        var members = new List<VariableNode>();
        Check(Tag.OpenBrace);
        while (IsIdentifier()) members.Add(Variable());
        Check(Tag.CloseBrace);
        Check(Tag.Semicolon);
        return new StructNode(new Location(startPosition, EndPosition), identifier, members);
    }

    /// channel = "uniform" variable | "flat" ( "in" | "out") variable.
    private ChannelNode? Channel()
    {
        if (Is(Tag.Layout)) Layout();

        if (Is(Tag.Uniform))
        {
            Next();
            return Channel(Direction.Uniform);
        }

        if (Is(Tag.Flat)) Next();

        if (Is(Tag.In))
        {
            Next();
            return Channel(Direction.In);
        }
        if (Is(Tag.Out))
        {
            Next();
            return Channel(Direction.Out);
        }

        Error("in our out parameter expected");
        return default;
    }

    /// layout = "layout" "(" "location" "=" number ")".
    private void Layout()
    {
        Next();
        Check(Tag.OpenParenthesis);
        Check(Tag.Location);
        Check(Tag.Equal);
        ReadInteger(false);
        Check(Tag.CloseParenthesis);
    }

    private ChannelNode Channel(Direction kind) => new(kind, Variable());

    /// variable = identifier identifier [ '[' ... ']' ] ';'.
    private VariableNode Variable()
    {
        var variable = new VariableNode(_current!.Location, Identifier(), Identifier());
        if (Is(Tag.OpenBracket))
        {
            SkipClause(Tag.OpenBracket, Tag.CloseBracket, false);
            Check(Tag.Semicolon);
            return new ArrayVariableNode(_current.Location, variable);
        }
        else
        {
            Check(Tag.Semicolon);
            return variable;
        }
    }

    /// define = '#' define = ...
    private DefineNode Define()
    {
        int startPosition = StartPosition;
        if (!Is(Tag.Define))
        {
            Error($"{Tag.Define} expected");
        }
        _lexer.SkipToNewLine();
        Next();
        return new DefineNode(new Location(startPosition, EndPosition));
    }

    /// procedure = identifier identifier '(' ... .
    private ProcedureNode Procedure()
    {
        int startPosition = StartPosition;
        var returnType = Identifier();
        var name = Identifier();
        SkipClause(Tag.OpenParenthesis, Tag.CloseParenthesis, false);
        SkipClause(Tag.OpenBrace, Tag.CloseBrace, true);
        return new ProcedureNode(new Location(startPosition, EndPosition), returnType, name);
    }

    private void SkipClause(Tag open, Tag close, bool nested)
    {
        if (!Is(open)) return;
        Next();
        int count = 1;
        while (count > 0)
        {
            if (Is(Tag.End))
            {
                Error("unexpected end of file");
                break;
            }
            else if (Is(open))
            {
                if (!nested)
                {
                    Error("clause cannot be nested");
                    break;
                }
                count++;
            }
            else if (Is(close))
            {
                count--;
            }

            Next();
        }
    }

    private IdentifierNode Identifier()
    {
        return new IdentifierNode(_current!.Location, ReadIdentifier());
    }

    private int StartPosition => _current == default ? -1 : _current.Location.Start;

    private int EndPosition { get; set; }

    private void Next()
    {
        if (_current != default)
        {
            EndPosition = _current.Location.End;
        }
        _current = _lexer.Next();
    }

    private bool Is(Tag tag)
    {
        return _current is FixToken token && token.Tag == tag;
    }

    private bool IsAny(params Tag[] tags)
    {
        return _current is FixToken token && Array.IndexOf(tags, token.Tag) >= 0;
    }

    private void Check(Tag tag)
    {
        if (!Is(tag))
        {
            Error($"{tag} expected");
        }
        Next();
    }

    private bool IsIdentifier()
    {
        return _current is IdentifierToken;
    }

    private string ReadIdentifier()
    {
        if (!IsIdentifier())
        {
            Error("Identifier expected");
            Next();
            return ErrorIdentifier;
        }
        var name = ((IdentifierToken)_current!).Name;
        Next();
        return name;
    }

    private bool IsInteger()
    {
        return _current is IntegerToken;
    }

    private int ReadInteger(bool allowMinValue)
    {
        if (!IsInteger())
        {
            Error("Integer expected");
            Next();
            return default;
        }
        var token = (IntegerToken)_current!;
        if (token.Value == int.MinValue && !allowMinValue && !token.Hex)
        {
            Error("Too large integer");
        }
        Next();
        return token.Value;
    }

    private void Error(string message)
    {
        _errors.Add($"{message} LOCATION {_current!.Location}");
    }
}