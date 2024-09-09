using EduGraf.Cameras;
using EduGraf.Geometries;
using EduGraf.Lighting;
using EduGraf.UI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace EduGraf;

// This is the central abstraction enabling platform independent graphic programming.
// Refer to the programming concepts documentation to understand how the different creation methods work together.
public abstract class Graphic
{
    // Create a new group of objects that is transformed together.
    public static Visual Group(string name  /* of the group (not used by the framework). */) => new(name);

    // Create a new texture from an image.
    public abstract TextureHandle CreateTexture(Image<Rgba32> image);

    // Create a new shading, i.e. a particular look, from light- and material-properties.
    public abstract Shading CreateShading(Light[] lights, Material[] materials, Camera? camera = default);

    // Create a new surface with a particular shading and geometry.
    public abstract Surface CreateSurface(Shading shading, IGeometry geometry);

    // Create a newn object with the given surface that can be placed into the virtual world.
    public abstract VisualPart CreateVisual(string name, Surface surface);

    // Display the rendering in the given window.
    public abstract void Render(Window window, Rendering rendering);
}
