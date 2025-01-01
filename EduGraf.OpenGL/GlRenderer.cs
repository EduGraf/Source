using System;
using EduGraf.Cameras;
using EduGraf.OpenGL.Enums;
using EduGraf.Tensors;
using System.Collections.Generic;
using System.Linq;

namespace EduGraf.OpenGL;

internal static class GlRenderer
{
    public static void TakePicture(
        GlApi api,
        Camera camera,
        List<(GlVisualPart part, Matrix4 transform)> partScene,
        int width, int height,
        Color3 background,
        GlTextureHandle[] textures)
    {
        const GlFramebufferTarget bufferTarget = GlFramebufferTarget.Framebuffer;
        const GlFramebufferAttachment attachment = GlFramebufferAttachment.ColorAttachment0;
        const GlTextureTarget textureTarget = GlTextureTarget.Texture2D;
        int count = textures.Length;
        var attachments = new GlFramebufferAttachment[count];

        using (new GlFrameRenderBuffer(api, width, height, GlRenderbufferStorage.Depth24Stencil8, GlFramebufferAttachment.DepthStencilAttachment))
        {
            for (int i = 0; i < count; i++)
            {
                attachments[i] = attachment + i;
                api.FramebufferTexture2D(bufferTarget, attachment + i, textureTarget, textures[i].Handle, 0);
            }

            api.DrawBuffers(attachments);

            Render(api, camera, partScene, width, height, background);

            for (int i = 0; i < count; i++)
            {
                api.FramebufferTexture2D(bufferTarget, attachment + i, textureTarget, 0, 0);
            }
        }
    }

    // TODO: properly sort transparent objects among each other
    public static void Render(
        GlApi api,
        Camera camera,
        List<(GlVisualPart part, Matrix4 transform)> partScene,
        int width, int height,
        Color3 background)
    {
        api.Enable(GlCap.DepthTest);
        api.Viewport(0, 0, width, height);
        api.ClearColor(background.R, background.G, background.B, 1);
        api.Clear(GlBufferBit.Color | GlBufferBit.Depth);

        var opaques = new List<(GlVisualPart visual, Matrix4 transform)>();
        var transps = new List<(GlVisualPart visual, Matrix4 transform)>();
        foreach (var (part, transform) in partScene)
        {
            if (part.Invisible) continue;

            var shading = (GlShading) part.Surface.Shading;
            var hasTransparency = Array.Exists(shading.Aspects, a => a is GlTransparentShadingAspect);
            if (hasTransparency) transps.Add((part, transform));
            else opaques.Add((part, transform));
        }

        var viewMatrix = camera.View.GetMatrix();
        var cameraPosition = camera.View.Position;
        var projection = camera.Projection.GetMatrix((float)width / height);

        foreach (var (part, transform) in opaques.Concat(transps))
        {
            var shading = (GlShading) part.Surface.Shading;

            shading.DoInContext(() =>
            {
                shading.Set("View", true, viewMatrix);
                shading.Set("Projection", true, projection);
                shading.Set("CameraPosition", cameraPosition, false);
                if (shading is GlLightedShading lightedShading) lightedShading.SetParameters();

                foreach (var aspect in shading.Aspects) aspect.Apply();

                shading.Set("Model", true, transform, false);
                shading.CheckInputs();

                part.Render();

                shading.Set("Model", false, Matrix4.Identity, false);
                shading.Reset("Model", false);

                foreach (var property in shading.Aspects) property.UnApply();
            });
        }

        api.CheckAllTextureUnitReleased();
    }
}
