﻿namespace EduGraf.OpenGL.GlslParser.Tokens;

internal class FixToken : Token
{
    public Tag Tag { get; }

    public FixToken(Location location, Tag tag)
        : base(location)
    {
        Tag = tag;
    }

    public override string ToString()
    {
        return $"TOKEN {Tag}";
    }
}