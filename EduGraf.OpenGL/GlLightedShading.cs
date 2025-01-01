using System;
using System.Reflection;
using EduGraf.Lighting;
using EduGraf.Tensors;

namespace EduGraf.OpenGL;

// This is the base class for shading using materials and lights.
public class GlLightedShading : GlShading
{
    private readonly LightingBase[] _lighting;

    // Create a new shading.
    protected internal GlLightedShading(string name, GlGraphic graphic, string vertexShader, string fragShader, LightingBase[] lighting, params GlShadingAspect[] aspects) 
        : base(name, graphic, vertexShader, fragShader, aspects)
    {
        _lighting = lighting;
    }

    // Called by the framework in the shading context.
    internal void SetParameters()
    {
        int lightsCount = 0;
        foreach (var lighting in _lighting)
        {
            SetParameters(
                lighting,
                lighting switch
                {
                    Light => lightsCount++,
                    Material => null,
                    _ => throw new InvalidOperationException()
                });
        }
    }

    private void SetParameters(LightingBase lighting, int? i)
    {
        var name = ShaderCompilation.SanitizeName(lighting.GetType().Name, i);

        foreach (var property in ShaderCompilation.GetProperties<DataAttribute>(lighting))
        {
            var (fieldObj, fieldType) = GetValueType(lighting, property);
            string fieldName = $"{name}.{ShaderCompilation.SanitizeName(property.Name)}";
            if (fieldType.IsAssignableTo(typeof(bool))) Set(fieldName, (bool)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(int))) Set(fieldName, (int)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(float))) Set(fieldName, (float)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(Vector3))) Set(fieldName, (Vector3)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(Vector4))) Set(fieldName, (Vector4)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(Color3))) Set(fieldName, (Color3)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(Color4))) Set(fieldName, (Color4)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(Point3))) Set(fieldName, (Point3)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(Matrix4))) Set(fieldName, false, (Matrix4)fieldObj);
            else if (fieldType.IsAssignableTo(typeof(TextureHandle))) Set(fieldName, ((GlTextureHandle)fieldObj).Handle, "sampler2D");
            else throw new NotSupportedException($"type {fieldType} of {fieldObj} not supported");
        }
    }

    private static (object, Type) GetValueType(object obj, PropertyInfo property)
    {
        var value = property.GetValue(obj)!;
        return (value, value.GetType());
    }
}