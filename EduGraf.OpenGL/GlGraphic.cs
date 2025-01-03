﻿using EduGraf.Cameras;
using EduGraf.Geometries;
using EduGraf.Lighting;
using EduGraf.OpenGL.Enums;
using EduGraf.Tensors;
using EduGraf.UI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;

namespace EduGraf.OpenGL;

// This is the generic OpenGL implementation of the Graphic abstraction.
public abstract class GlGraphic(GlApi api) : Graphic
{
    internal GlApi Api { get; } = api;

    // Refer to the documentation of the class Graphic.
    public override Shading CreateShading(string name, Material material, params Light[] lights)
    {
        return ShaderCompilation.GetShading(name, this, material, lights);
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
    public override GlTextureHandle CreateTexture(Image<Rgba32> image)
    {
        return new GlTextureHandle(Api, GlTextures.CreateMipmapTexture(Api, image));
    }

    // Refer to the documentation of the class Graphic.
    public override void Render(Window window, Rendering rendering, Camera camera)
    {
        Api.ExecutePending();
        var allParts = GetPartScene(rendering.Scene);
        GlRenderer.Render(Api, camera, allParts, window.Width, window.Height, rendering.Background);
    }

    // Create a new rgb-texture with pixel-type unsigned byte or float.
    public GlTextureHandle CreateRgbTexture<T>(int width, int height)
        where T : struct
    {
        return new GlTextureHandle(Api, GlTextures.CreateTexture(Api, width, height, GlPixelFormat.Rgb, GetPixelType<T>()));
    }

    // Create a new rgba-texture with pixel-type unsigned byte or float.
    public GlTextureHandle CreateRgbaTexture<T>(int width, int height)
        where T : struct
    {
        return new GlTextureHandle(Api, GlTextures.CreateTexture(Api, width, height, GlPixelFormat.Rgba, GetPixelType<T>()));
    }

    // Create a new texture having a single color channel with pixel-type unsigned byte or float.
    public GlTextureHandle CreateRedTexture<T>(int width, int height)
        where T : struct
    {
        return new GlTextureHandle(Api, GlTextures.CreateTexture(Api, width, height, GlPixelFormat.Red, GetPixelType<T>()));
    }

    private static GlPixelType GetPixelType<T>() where T : struct
    {
        return typeof(T) == typeof(byte) ? GlPixelType.UnsignedByte
            : typeof(T) == typeof(float) ? GlPixelType.Float
            : throw new ArgumentOutOfRangeException(nameof(T));
    }

    // Create a new cube-texture.
    public GlTextureHandle CreateCubeTexture<TPixel>(
        Image<TPixel> left,
        Image<TPixel> right,
        Image<TPixel> bottom,
        Image<TPixel> top,
        Image<TPixel> front,
        Image<TPixel> back) 
        where TPixel : unmanaged, IPixel<TPixel>
    {
        return new GlTextureHandle(Api, GlTextures.CreateCubeTexture(Api, left, right, bottom, top, front, back));
    }

    // Take a color-picture of the scene and store it in the provided texture.
    public void TakePicture(
        Visual scene,
        Camera camera,
        Color3 background,
        int width, int height,
        params GlTextureHandle[] textures)
    {
        var partScene = GetPartScene(scene);
        GlRenderer.TakePicture(Api, camera, partScene, width, height, background, textures);
    }

    [OmitInDocumentation]
    public static void Dispose(Visual scene)
    {
        foreach (var (part, _) in GetPartScene(scene))
        {
            var surface = (GlSurface)part.Surface;
            surface.Shading.Dispose();
            surface.Dispose();
        }
    }

    private static List<(GlVisualPart part, Matrix4 transform)> GetPartScene(Visual scene)
    {
        var partScene = new List<(GlVisualPart part, Matrix4 transform)>();
        CollectPartScene(scene, Matrix4.Identity, partScene);
        return partScene;
    }

    private static void CollectPartScene(Visual group, Matrix4 overallTransform, List<(GlVisualPart part, Matrix4 transform)> partScene)
    {
        var transform = group.Transform * overallTransform;
        if (group is GlVisualPart part) partScene.Add((part, transform));

        foreach (var visual in group.Children) CollectPartScene(visual, transform, partScene);
    }
}