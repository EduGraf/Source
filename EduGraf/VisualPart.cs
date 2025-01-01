namespace EduGraf;

// This represents a single visual part that is placed at a particular position, rotation and size in the virtual world.
public abstract class VisualPart : Visual
{
    public Surface Surface { get; } // parts must have a surface

    // If true, the part is not rendered.
    public bool Invisible { get; set; }

    // Create a new visual part.
    protected internal VisualPart(string name  /* not used by the framework. */, Surface surface)
        : base(name)
    {
        Surface = surface;
    }
}