using EduGraf.Cameras;
using EduGraf.OpenGL.Enums;
using EduGraf.Tensors;
using System.Collections.Generic;
using System.Linq;

namespace EduGraf.OpenGL;

internal static class GlRenderer
{
    public static void Render(
        GlApi api,
        Camera? omniCamera,
        IEnumerable<IGrouping<GlShading, Visual>> scene,
        int width, int height,
        Color3 background)
    {
        api.Enable(GlCap.DepthTest);
        api.Viewport(0, 0, width, height);
        api.ClearColor(background.R, background.G, background.B, 1);
        api.Clear(GlBufferBit.Color | GlBufferBit.Depth);

        foreach (var group in scene)
        {
            var shading = group.Key;
            var camera = omniCamera ?? shading.Camera;
            if (camera is not null)
            {
                var projection = camera.Projection.GetMatrix((float)width / height);
                shading.DoInContext(() =>
                {
                    shading.Set("View", true, camera.View.GetViewMatrix());
                    shading.Set("cameraPosition", camera.View.Position, false);
                    shading.Set("Projection", true, projection);
                });
            }
            shading.SetParameters();
            shading.DoInContext(() =>
            {
                foreach (var aspect in shading.Aspects) aspect.Apply();

                foreach (var visual in group)
                {
                    shading.Set("Model", true, visual.Transform, false);
                    shading.CheckInputs();

                    visual.Render();

                    shading.Set("Model", true, Space.Identity4, false);
                    shading.Reset("Model", false);
                }

                foreach (var property in shading.Aspects) property.UnApply();
            });
        }

        api.CheckAllTextureUnitReleased();
    }

    public static void TakePicture(
        GlApi api,
        Camera camera,
        IEnumerable<IGrouping<GlShading, Visual>> scene,
        int width, int height,
        Color3 background,
        TextureHandle[] textures)
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
                api.FramebufferTexture2D(bufferTarget, attachment + i, textureTarget, ((GlTextureHandle)textures[i]).Handle, 0);
            }

            api.DrawBuffers(attachments);

            Render(api, camera, scene, width, height, background);

            for (int i = 0; i < count; i++)
            {
                api.FramebufferTexture2D(bufferTarget, attachment + i, textureTarget, 0, 0);
            }
        }
    }
}