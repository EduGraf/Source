using EduGraf.Tensors;
using EduGraf.UI;

namespace EduGraf;

// This is the abstraction for visualizations of 2D and 3D scenes.
public abstract class Rendering(Graphic graphic, Color3 background)
{
    protected internal Graphic Graphic { get; } = graphic; // where this is rendered to

    public Color3 Background { get; } = background; // color used if no visual is hit

    public Visual Scene { get; } = Graphic.CreateGroup("scene"); // containing all rendered visuals

    // The framework calls this method when the rendering is displayed for the first time.
    public virtual void OnLoad(Window window) { }

    // The framework calls this method before each frame update.
    protected internal virtual void OnUpdateFrame(Window window) { }
}