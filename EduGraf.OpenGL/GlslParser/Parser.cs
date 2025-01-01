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
        Next(out _);
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
                Next(out _);
            }
            else
            {
                declarations.Add(Declaration());
            }
        }

        if (_errors.Count == 0) return new ProgramNode(new Location(start, EndPosition), declarations);
        
        var errorList = string.Join(Environment.NewLine, _errors);
        throw new InvalidProgramException($"GLSL program cannot be parsed because of the following errors {errorList}");
    }

    /// version = '#' "version" integer
    private void Version()
    {
        if (Is(Tag.Hash))
        {
            Next(out _);
            Check(Tag.Version);
            ReadInteger(false, out bool isAtEndOfLine);
            if (!isAtEndOfLine)
            {
                _lexer.SkipToNewLine();
                Next(out _);
            }
        }
    }

    /// declaration = struct | channel | define | procedure.
    private DeclarationNode? Declaration()
    {
        if (Is(Tag.Struct)) { Next(out _); return Struct(); }
        if (IsAny(Tag.Layout, Tag.Flat, Tag.In, Tag.Out, Tag.Uniform)) return Channel();
        if (Is(Tag.Hash)) { Next(out _); return Define(); }
        return Procedure();
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
            Next(out _);
            return Channel(Direction.Uniform);
        }

        if (Is(Tag.Flat)) Next(out _);

        if (Is(Tag.In))
        {
            Next(out _);
            return Channel(Direction.In);
        }
        if (Is(Tag.Out))
        {
            Next(out _);
            return Channel(Direction.Out);
        }

        Error("in our out parameter expected");
        return default;
    }

    /// layout = "layout" "(" "location" "=" number ")".
    private void Layout()
    {
        Next(out _);
        Check(Tag.OpenParenthesis);
        Check(Tag.Location);
        Check(Tag.Equal);
        ReadInteger(false, out _);
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
        Next(out _);
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
        Next(out _);
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

            Next(out _);
        }
    }

    private IdentifierNode Identifier() => new(_current!.Location, ReadIdentifier());

    private int StartPosition => _current == default ? -1 : _current.Location.Start;

    private int EndPosition { get; set; }

    private void Next(out bool isOnNewLine)
    {
        if (_current != default) EndPosition = _current.Location.End;
        _current = _lexer.Next(out isOnNewLine);
    }

    private bool Is(Tag tag) => _current is FixToken token && token.Tag == tag;

    private bool IsAny(params Tag[] tags) => _current is FixToken token && Array.IndexOf(tags, token.Tag) >= 0;

    private void Check(Tag tag)
    {
        if (!Is(tag))
        {
            Error($"{tag} expected");
        }
        Next(out _);
    }

    private bool IsIdentifier() => _current is IdentifierToken;

    private string ReadIdentifier()
    {
        if (!IsIdentifier())
        {
            Error("Identifier expected");
            Next(out _);
            return ErrorIdentifier;
        }
        var name = ((IdentifierToken)_current!).Name;
        Next(out _);
        return name;
    }

    private bool IsInteger() => _current is IntegerToken;

    private int ReadInteger(bool allowMinValue, out bool isAtEndOfLine)
    {
        if (!IsInteger())
        {
            Error("Integer expected");
            Next(out isAtEndOfLine);
            return default;
        }
        var token = (IntegerToken)_current!;
        if (token.Value == int.MinValue && !allowMinValue && !token.Hex)
        {
            Error("Too large integer");
        }
        Next(out isAtEndOfLine);
        return token.Value;
    }

    private void Error(string message) => _errors.Add($"{message} LOCATION {_current!.Location}");
}