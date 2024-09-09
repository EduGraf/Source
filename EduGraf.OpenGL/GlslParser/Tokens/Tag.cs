namespace EduGraf.OpenGL.GlslParser.Tokens;

internal enum Tag
{
    // keywords
    Version,
    Layout,
    Location,
    Flat,
    In,
    Out,
    Uniform,
    Struct,
    Define,

    // interpunction
    OpenBrace, CloseBrace,
    OpenBracket, CloseBracket,
    OpenParenthesis, CloseParenthesis,
    Equal,
    Hash,
    Semicolon,
    LineComment,
    End,
}