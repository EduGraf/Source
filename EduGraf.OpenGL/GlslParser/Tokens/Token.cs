namespace EduGraf.OpenGL.GlslParser.Tokens;

internal abstract class Token
{
    public Location Location { get; }

    public Token(Location location)
    {
        Location = location;
    }

    public override abstract string ToString();
}