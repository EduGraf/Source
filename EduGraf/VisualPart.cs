namespace EduGraf;

// This represents a single visual part that is placed at a particular position, rotation and size in the virtual world.
public abstract class VisualPart : Visual
{
    // The part's surface.
    public Surface Surface { get; }

    // Create a new visual part.
    protected internal VisualPart(string name  /* not used by the framework. */, Surface surface)
        : base(name)
    {
        Surface = surface;
    }
}