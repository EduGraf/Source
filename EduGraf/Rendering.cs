using EduGraf.Tensors;
using EduGraf.UI;
using System.Collections.Generic;

namespace EduGraf;

// This is the abstraction for visualizations of 2D and 3D scenes.
public abstract class Rendering(Graphic graphic, Color3 background)
{
    // where this is rendered to.
    protected internal Graphic Graphic { get; } = graphic;

    // color used if no visual is hit.
    public Color3 Background { get; } = background;

    // containing all rendered visuals.
    public List<Visual> Scene { get; } = new();

    // Called by the framework when the rendering is displayed for the first time.
    public virtual void OnLoad(Window window) { }

    // Called by the framework before each frame update.
    protected internal virtual void OnUpdateFrame(Window window) { }
}