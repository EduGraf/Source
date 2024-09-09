using EduGraf.Cameras;
using EduGraf.Geometries;
using EduGraf.Lighting;
using EduGraf.OpenGL.Enums;
using EduGraf.Tensors;
using EduGraf.UI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduGraf.OpenGL;

// This is the generic OpenGL implementation of the Graphic abstraction.
public abstract class GlGraphic : Graphic
{
    internal GlApi Api { get; }

    protected GlGraphic(GlApi api)
    {
        Api = api;
    }

    // Refer to the documentation of the class Graphic.
    public override Shading CreateShading(Light[] lights, Material[] materials, Camera? camera = default)
    {
        return ShaderCompilation.GetShading(this, camera, lights, materials);
    }

    // Refer to the documentation of the class Graphic.
    public override Surface CreateSurface(Shading shading, IGeometry geometry)
    {
        return new GlSurface(Api, (GlShading)shading, geometry);
    }

    // Refer to the documentation of the class Graphic.
    public override VisualPart CreateVisual(string name, Surface surface)
    {
        return new GlVisualPart(name, (GlSurface)surface);
    }

    // Refer to the documentation of the class Graphic.
    public override TextureHandle CreateTexture(Image<Rgba32> image)
    {
        return new GlTextureHandle(Api, GlTextures.CreateMipmapTexture(Api, image));
    }

    // Refer to the documentation of the class Graphic.
    public override void Render(Window window, Rendering rendering)
    {
        Api.ExecutePending();
        GlRenderer.Render(Api, default, GetAllVisualsByShading(rendering.Scene), window.Width, window.Height, rendering.Background);
    }

    // Create a newn rgb-texture with pixel-type unsigned byte.
    public TextureHandle CreateRgbTexture<T>(int width, int height)
        where T : struct
    {
        var pixelType =
            typeof(T) == typeof(byte) ? GlPixelType.UnsignedByte
            : typeof(T) == typeof(float) ? GlPixelType.Float
            : throw new ArgumentOutOfRangeException(nameof(T));

        return new GlTextureHandle(Api, GlTextures.CreateTexture(Api, width, height, GlPixelFormat.Rgb, pixelType));
    }

    // Create a new texture with a single color channel.
    public TextureHandle CreateRedTexture<T>(int width, int height)
        where T : struct
    {
        var pixelType =
            typeof(T) == typeof(byte) ? GlPixelType.UnsignedByte
            : typeof(T) == typeof(float) ? GlPixelType.Float
            : throw new ArgumentOutOfRangeException(nameof(T));

        return new GlTextureHandle(Api, GlTextures.CreateTexture(Api, width, height, GlPixelFormat.Red, pixelType));
    }

    // Take a color-picture of the scene and store it in the provided texture.
    public void TakePicture(
        IEnumerable<Visual> scene,
        Camera camera,
        Color3 background,
        int width, int height,
        params TextureHandle[] textures)
    {
        GlRenderer.TakePicture(Api, camera, GetAllVisualsByShading(scene), width, height, background, textures);
    }

    [OmitInDocumentation]
    public static void Dispose(IEnumerable<Visual> scene)
    {
        foreach (var group in GetAllVisualsByShading(scene))
        {
            var shading = group.Key;
            foreach (var part in group) ((GlSurface)part.Surface).Dispose();
            shading.Dispose();
        }
    }

    private static List<IGrouping<GlShading, GlVisualPart>> GetAllVisualsByShading(IEnumerable<Visual> scene)
    {
        return scene
            .SelectMany(visual => visual.Descendants)
            .OfType<GlVisualPart>()
            .GroupBy(part => (GlShading)part.Surface.Shading)
            .ToList();
    }
}