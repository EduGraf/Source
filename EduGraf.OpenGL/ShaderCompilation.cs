using EduGraf.Lighting;
using EduGraf.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EduGraf.OpenGL;

internal static class ShaderCompilation
{
    private const string InPosition = "Position";
    private const string InNormal = "Normal";
    private const string InTextureUv = "TextureUv";
    private const string TransferNormal = "VertexNormal";
    private const string ShaderPosition = "SurfacePosition";
    private const string ShaderNormal = "SurfaceNormal";
    private const string ShaderTextureUv = "SurfaceTextureUv";
    private const string CameraPosition = "CameraPosition";

    private static readonly Type[] VectorTypes = {
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4)
    };

    private static readonly Type[] BuiltInTypes =
        VectorTypes.Concat([
            typeof(Color3),
            typeof(Color4),
            typeof(Point2),
            typeof(Point3),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Matrix2),
            typeof(Matrix4)])
        .ToArray();

    private static string GlSlType(Type type)
    {
        if (type == typeof(bool)) return "bool";
        if (type == typeof(float)) return "float";
        if (type == typeof(Color3)) return "vec3";
        if (type == typeof(Color4)) return "vec4";
        if (type == typeof(Vector2)) return "vec2";
        if (type == typeof(Vector3)) return "vec3";
        if (type == typeof(Vector4)) return "vec4";
        if (type == typeof(Point2)) return "vec2";
        if (type == typeof(Point3)) return "vec3";
        if (type == typeof(TextureHandle)) return "sampler2D";
        if (type.IsAssignableTo(typeof(LightingBase))) return SanitizeName(type.Name);
        if (type.IsAssignableTo(typeof(LambdaExpression))) return GlSlType(type.GetGenericArguments()[0].GetGenericArguments().Last());
        throw new NotSupportedException($"undefined glsl type for dotnet type {type.Name}");
    }

    internal static GlShading GetShading(string name, GlGraphic graphic, Material material, params Light[] lights)
    {
        var lighting = lights
            .Cast<LightingBase>()
            .Concat([material])
            .ToArray();
        bool withNormals = RequireNormals(lighting);
        bool withTextures = RequireTextures(material);
        string fragShader = GetFragShader(lights, material, withNormals, withTextures);
        string vertexShader = GetVertexShader(withNormals, withTextures);
        var aspects = new List<GlShadingAspect>();
        if (withTextures)
        {
            var tm = (ColorTextureMaterial) material;
            aspects.Add(new GlNamedTextureShadingAspect($"{tm.GetType().Name}.Handle", (GlTextureHandle)tm.Handle));
        }

        return new GlShading(name, graphic, vertexShader, fragShader, lighting, aspects.ToArray());
    }

    private static string GetVertexShader(bool withNormals, bool withTextures)
    {
        var vertexShader = new StringBuilder();
        vertexShader.AppendLine("#version 410");

        vertexShader.AppendLine($"in vec3 {InPosition};");
        if (withNormals) vertexShader.AppendLine($"in vec3 {InNormal};");
        if (withTextures) vertexShader.AppendLine($"in vec2 {InTextureUv};");

        vertexShader.AppendLine("uniform mat4 Model;");
        vertexShader.AppendLine("uniform mat4 View;");
        vertexShader.AppendLine("uniform mat4 Projection;");

        vertexShader.AppendLine($"out vec3 {ShaderPosition};");
        if (withNormals) vertexShader.AppendLine($"out vec3 {TransferNormal};");
        if (withTextures) vertexShader.AppendLine($"out vec2 {ShaderTextureUv};");

        vertexShader.AppendLine("void main(void) {");
        vertexShader.AppendLine($"  vec4 worldPosition = vec4({InPosition}, 1.0) * Model;");
        vertexShader.AppendLine("  gl_Position = worldPosition * View * Projection;");
        vertexShader.AppendLine($"  {ShaderPosition} = vec3(worldPosition);");
        if (withNormals) vertexShader.AppendLine($"  {TransferNormal} = {InNormal} * mat3(Model);");
        if (withTextures) vertexShader.AppendLine($"  {ShaderTextureUv} = {InTextureUv};");
        vertexShader.AppendLine("}");

        return vertexShader.ToString();
    }

    private static string GetFragShader(Light[] lights, Material material, bool withNormals, bool withTextures)
    {
        var fragShader = new StringBuilder();
        fragShader.AppendLine("#version 410");

        var lighting = lights
            .OfType<LightingBase>()
            .Concat([ material ]);
        AddDataTypes(lighting, fragShader);

        fragShader.AppendLine($"in vec3 {ShaderPosition};");
        if (withNormals) fragShader.AppendLine($"in vec3 {TransferNormal};");
        if (withTextures) fragShader.AppendLine($"in vec2 {ShaderTextureUv};");
        fragShader.AppendLine($"uniform vec3 {CameraPosition};");

        var localDeclarations = new HashSet<string>();
        for (int i = 0; i < lights.Length; i++)
        {
            var light = lights[i];
            var lightName = light.GetType().Name;
            fragShader.AppendLine($"uniform {SanitizeName(lightName)} {SanitizeName(lightName, i)};");
            UpdateLocals(light, localDeclarations);
        }

        var materialName = material.GetType().Name;
        fragShader.AppendLine($"uniform {SanitizeName(materialName)} {SanitizeName(materialName, default)};");

        UpdateLocals(material, localDeclarations);

        fragShader.AppendLine("out vec3 fragment;");
        fragShader.AppendLine("void main() {");

        foreach (var local in localDeclarations) fragShader.AppendLine(local);

        if (withNormals) fragShader.AppendLine($"  vec3 {ShaderNormal} = normalize({TransferNormal});");
        fragShader.AppendLine("  const vec3 white3 = vec3(1, 1, 1);");
        fragShader.AppendLine("  fragment = vec3(0, 0, 0);");

        CrossCompileProperties(material, default, fragShader);

        for (int i = 0; i < lights.Length; i++)
        {
            CrossCompileProperties(lights[i], i, fragShader);
            fragShader.AppendLine($"  fragment = white3 - (white3 - fragment) * (white3 - {nameof(Light.Remission)});");
        }

        fragShader.AppendLine("}");

        return fragShader.ToString();
    }

    private static void AddDataTypes(IEnumerable<LightingBase> lighting, StringBuilder fragShader)
    {
        var types = new HashSet<Type>();
        foreach (var l in lighting)
        {
            var type = l.GetType();
            if (types.Add(type))
            {
                fragShader.AppendLine($"struct {SanitizeName(type.Name)} {{");

                foreach (var property in GetProperties<DataAttribute>(l))
                {
                    fragShader.AppendLine($"  {GlSlType(property.PropertyType)} {SanitizeName(property.Name)};");
                }

                fragShader.AppendLine("};");
            }
        }
    }

    private static bool RequireTextures(Material material)
    {
        return GetProperties<DataAttribute>(material)
            .Any(p => p.PropertyType.IsAssignableTo(typeof(TextureHandle)));
    }

    private static bool RequireNormals(IEnumerable<LightingBase> lighting)
    {
        foreach (var l in lighting)
        {
            foreach (var property in GetProperties<CalcAttribute>(l))
            {
                var expression = (LambdaExpression?)property.GetValue(l);
                if (RequiresNormals(l, expression)) return true;
            }
        }

        return false;
    }

    private static bool RequiresNormals(object @this, Expression? expression)
    {
        switch (expression)
        {
            case BinaryExpression binary:
                return
                    RequiresNormals(@this, binary.Left) ||
                    RequiresNormals(@this, binary.Right);

            case ConditionalExpression conditional:
                return
                    RequiresNormals(@this, conditional.Test) ||
                    RequiresNormals(@this, conditional.IfTrue) ||
                    RequiresNormals(@this, conditional.IfFalse);

            case ConstantExpression:
                return false;

            case MemberExpression memberExpr:
                var member = memberExpr.Member;
                string name = member.Name;
                return name is ShaderNormal
                       && memberExpr.Expression is ConstantExpression ce
                       && ce.Value == @this
                       || RequiresNormals(@this, memberExpr.Expression);

            case LambdaExpression lambda:
                return RequiresNormals(@this, lambda.Body);

            case MethodCallExpression methodCall:
                return methodCall.Arguments.Any(argument => RequiresNormals(@this, argument));

            case ParameterExpression:
                return false;

            case UnaryExpression unary:
                return RequiresNormals(@this, unary.Operand);

            case NewExpression newCall:
                return newCall.Arguments.Any(argument => RequiresNormals(@this, argument));

            case null:
                return false;

            default:
                throw new NotSupportedException(expression.ToString());
        }
    }


    private static void UpdateLocals(LightingBase lighting, HashSet<string> locals)
    {
        foreach (var property in GetProperties<CalcAttribute>(lighting))
        {
            locals.Add($"  {GlSlType(property.PropertyType)} {property.Name};");
        }
    }

    private static void CrossCompileProperties(LightingBase lighting, int? index, StringBuilder fragShader)
    {
        foreach (var property in GetProperties<CalcAttribute>(lighting))
        {
            var expression = (LambdaExpression?)property.GetValue(lighting);
            if (expression == default) throw new NotSupportedException($"calculated property {property.Name} is undefined.");

            fragShader.Append($"  {property.Name} = ");
            CrossCompile(lighting, index, expression.Body, fragShader);
            fragShader.AppendLine(";");
        }
    }

    private static void CrossCompile(object @this, int? index, Expression? expression, StringBuilder shader)
    {
        switch (expression)
        {
            case BinaryExpression binary:
                {
                    var isDotProduct = binary.NodeType == ExpressionType.Multiply && 
                            binary.Left.Type == typeof(Vector3) &&
                            binary.Right.Type == typeof(Vector3);
                    if (isDotProduct) shader.Append("dot");

                    shader.Append('(');
                    CrossCompile(@this, index, binary.Left, shader);

                    if (binary.NodeType == ExpressionType.Call)
                    {
                        var method = binary.Method!;
                        var op = BuiltInTypes.Contains(method.DeclaringType)
                            ? GetOp(method.Name)
                            : throw new NotSupportedException(expression.ToString());
                        shader.Append($" {op} ");
                    }
                    else if (isDotProduct) shader.Append($", ");
                    else shader.Append($" {GetOp(binary)} ");

                    CrossCompile(@this, index, binary.Right, shader);
                    shader.Append(')');
                    return;
                }

            case ConditionalExpression conditional:
                shader.Append('(');
                CrossCompile(@this, index, conditional.Test, shader);
                shader.Append(" ? ");
                CrossCompile(@this, index, conditional.IfTrue, shader);
                shader.Append(" : ");
                CrossCompile(@this, index, conditional.IfFalse, shader);
                shader.Append(')');
                return;

            case ConstantExpression constant:

                if (constant.Value == @this) shader.Append($"{SanitizeName(@this.GetType().Name, index)}");
                else shader.Append(constant.Value!);
                return;

            case MemberExpression memberExpr:
                {
                    var member = memberExpr.Member;
                    string name = member.Name;
                    if (name is ShaderPosition or ShaderNormal or ShaderTextureUv or CameraPosition
                        && memberExpr.Expression is ConstantExpression ce
                        && ce.Value == @this
                        || member.GetCustomAttribute<CalcAttribute>() != default)
                    {
                        shader.Append(SanitizeName(name));
                    }
                    else if (member.DeclaringType == typeof(Color4) && name == nameof(Color4.Color3))
                    {
                        CrossCompile(@this, index, memberExpr.Expression!, shader);
                        shader.Append(".xyz");
                    }
                    else if (member.DeclaringType!.IsAssignableTo(typeof(Tensor)))
                    {
                        var tensor = (Tensor)member.DeclaringType.GetField(member.Name)!.GetValue(default)!;
                        shader.Append($"{GlSlType(tensor.GetType())}({string.Join(',', tensor.Elements)})");
                    }
                    else
                    {
                        CrossCompile(@this, index, memberExpr.Expression, shader);
                        shader.Append($".{SanitizeName(name)}");
                    }
                    return;
                }

            case MethodCallExpression methodCall:
                {
                    var method = methodCall.Method;
                    var methodName = method.Name;
                    var isTexture = method.DeclaringType!.IsAssignableTo(typeof(ColorTextureMaterial)) && methodName == "Texture";
                    if (isTexture)
                    {
                        var name = @this.GetType().Name;
                        shader.Append($"texture({SanitizeName(name, index)}.Handle, ");
                    }
                    else if (method.DeclaringType == typeof(LightingBase))
                    {
                        switch (methodName)
                        {
                            case "ValueOf":
                                shader.Append('(');
                                break;

                            case "LengthOf":
                                shader.Append("length(");
                                break;

                            case "Add":
                                shader.Append("white3 - (white3 - ");
                                CrossCompile(@this, index, methodCall.Arguments[0], shader);
                                shader.Append(") * (white3 - ");
                                CrossCompile(@this, index, methodCall.Arguments[1], shader);
                                shader.Append(")");
                                return;

                            default:
                                shader.Append($"{GlSlType(method.ReturnType)}(");
                                break;
                        }
                    }
                    else if (method.DeclaringType == typeof(MathF)) shader.Append($"{GetMathName(method)}(");
                    else if (BuiltInTypes.Contains(method.DeclaringType)) shader.Append($"{methodName.ToLower()}(");
                    else throw new NotSupportedException(expression.ToString());

                    Arguments(@this, index, methodCall, shader);
                    if (isTexture) shader.Append(".rgb");
                    return;
                }

            case ParameterExpression:
                throw new NotSupportedException("lambdas with parameters are not supported");

            case UnaryExpression unary:
                {
                    switch (unary.NodeType)
                    {
                        case ExpressionType.Negate:
                            shader.Append('-');
                            CrossCompile(@this, index, unary.Operand, shader);
                            return;

                        case ExpressionType.Call:
                            var method = unary.Method!;
                            if (BuiltInTypes.Contains(method.DeclaringType))
                            {
                                if (method.Name == "op_UnaryNegation")
                                {
                                    shader.Append('-');
                                    CrossCompile(@this, index, unary.Operand, shader);
                                }
                                else if (method.Name == "op_UnaryAddition")
                                {
                                    CrossCompile(@this, index, unary.Operand, shader);
                                }
                                else throw new NotSupportedException(expression.ToString());
                            }
                            else throw new NotSupportedException(expression.ToString());
                            return;

                        case ExpressionType.Convert:
                            CrossCompile(@this, index, unary.Operand, shader);
                            return;

                        default:
                            throw new NotSupportedException(expression.ToString());
                    }
                }

            case NewExpression newCall:
                shader.Append($"{GlSlType(newCall.Type)}(");
                CrossCompile(@this, index, newCall.Arguments[0], shader);
                for (int i = 1; i < newCall.Arguments.Count; i++)
                {
                    shader.Append(", ");
                    CrossCompile(@this, index, newCall.Arguments[i], shader);
                }

                shader.Append(')');
                return;

            case null:
                shader.Append("null");
                return;

            default:
                throw new NotSupportedException(expression.ToString());
        }
    }

    private static void Arguments(object @this, int? index, MethodCallExpression methodCall, StringBuilder shader)
    {
        CrossCompile(@this, index, methodCall.Arguments[0], shader);
        for (int i = 1; i < methodCall.Arguments.Count; i++)
        {
            shader.Append(", ");
            CrossCompile(@this, index, methodCall.Arguments[i], shader);
        }

        shader.Append(')');
    }

    private static string GetMathName(MethodInfo method)
    {
        string name = method.Name;
        switch (name)
        {
            case nameof(MathF.Abs):
            case nameof(MathF.Acos):
            case nameof(MathF.Acosh):
            case nameof(MathF.Asin):
            case nameof(MathF.Asinh):
            case nameof(MathF.Atan):
            case nameof(MathF.Atanh):
            case nameof(MathF.Atan2):
            case nameof(MathF.Cos):
            case nameof(MathF.Cosh):
            case nameof(MathF.Exp):
            case nameof(MathF.Log):
            case nameof(MathF.Log10):
            case nameof(MathF.Log2):
            case nameof(MathF.Max):
            case nameof(MathF.Min):
            case nameof(MathF.Pow):
            case nameof(MathF.Sin):
            case nameof(MathF.SinCos):
            case nameof(MathF.Sinh):
            case nameof(MathF.Sqrt):
            case nameof(MathF.Tan):
            case nameof(MathF.Tanh):
                return name.ToLower();

            default:
                throw new ArgumentOutOfRangeException(nameof(method), $"not supported math function {name}");
        }
    }

    private static string GetOp(BinaryExpression binary)
    {
        return binary.NodeType switch
        {
            ExpressionType.Add => "+",
            ExpressionType.And => "&&",
            ExpressionType.Divide => "/",
            ExpressionType.Equal => "==",
            ExpressionType.GreaterThan => "<",
            ExpressionType.GreaterThanOrEqual => "<=",
            ExpressionType.LessThan => ">",
            ExpressionType.LessThanOrEqual => ">=",
            ExpressionType.Modulo => "%",
            ExpressionType.Multiply => "*",
            ExpressionType.NotEqual => "!=",
            ExpressionType.Or => "||",
            ExpressionType.Subtract => "-",
            _ => throw new NotSupportedException(binary.ToString())
        };
    }

    private static string GetOp(string methodName)
    {
        return methodName switch
        {
            "op_Add" => "+",
            "op_Subtract" => "-",
            "op_Multiply" => "*",
            "op_Divide" => "/",
            _ => throw new NotSupportedException(methodName)
        };
    }

    internal static string SanitizeName(string name) => name.Replace("_", string.Empty);

    internal static string SanitizeName(string name, int? index)
    {
        var sanitizedName = SanitizeName(name);
        return index == default
            ? $"_{sanitizedName}"
            : $"{sanitizedName}{index}";
    }

    internal static IEnumerable<PropertyInfo> GetProperties<T>(LightingBase lighting)
        where T : Attribute
    {
        return lighting
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty)
            .Where(p => p.GetCustomAttribute<T>() != default)
            .OrderBy(p => AncestorLevel(p.DeclaringType!));
    }

    private static int AncestorLevel(Type t)
    {
        if (t is not { IsClass: true }) throw new NotSupportedException();

        return t == typeof(object) 
            ? 0 
            : 1 + AncestorLevel(t.BaseType!);
    }
}