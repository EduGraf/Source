using EduGraf.OpenGL.Enums;
using EduGraf.OpenGL.GlslParser;
using EduGraf.OpenGL.GlslParser.Tree;
using EduGraf.Tensors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace EduGraf.OpenGL;

// This is the generic shading implementation for OpenGL.
public class GlShading : Shading
{
    private readonly Dictionary<string, Direction> _inputs = [];
    private readonly Dictionary<string, ChannelNode> _netChannels = [];
    private readonly uint _handle;
    private readonly List<DeclarationNode> _declarations;
    private bool _disposed;

    [OmitInDocumentation]
    protected internal GlApi Api { get; }

    public GlShadingAspect[] Aspects { get; } // setting up shading context

    // Create a new shading.
    protected internal GlShading(
        string name,
        GlGraphic graphic,
        string vertexShader,
        string fragShader,
        params GlShadingAspect[] aspects)
        : base(name)
    {
        Api = graphic.Api;
        Aspects = aspects;
        foreach (var aspect in aspects) aspect.Shading = this;

        var errors = new List<string>();

        _declarations = GetDeclarations(vertexShader, errors)
            .Cast<DeclarationNode>()
            .ToList();
        var fragmentDeclarations = GetDeclarations(fragShader, errors)
            .Cast<DeclarationNode>()
            .ToList();
        CheckShader(_declarations);
        CheckShader(fragmentDeclarations);
        _declarations.AddRange(fragmentDeclarations.Where(fd => _declarations.All(vd => vd.Name != fd?.Name)));

        var fragOutChannels = fragmentDeclarations
            .OfType<ChannelNode>()
            .Where(c => c.Direction == Direction.Out)
            .ToList();
        if (fragOutChannels.Count < 1)
        {
            throw new ArgumentException("the fragment shader must have at least one output defining the color.");
        }

        _handle = Api.Compile(vertexShader, fragShader, fragOutChannels[0].Name, errors);

        Report(errors);
    }

    // Execute the action in the context of this shader.
    protected internal void DoInContext(Action action)
    {
        try
        {
            Api.UseProgram(_handle);
            action();
        }
        finally
        {
            Api.UseProgram(0);
        }
    }

    internal void SetVertexArrayAttributes(uint vertexArray, List<GlAttribute> attributes, uint[] vertexBuffers)
    {
        DoInContext(() =>
        {
            Api.BindVertexArray(vertexArray);

            for (int i = 0; i < attributes.Count; i++)
            {
                var attribute = attributes[i];
                SetVertexAttribute(attribute.Name, attribute.Dimensionality!.Value, vertexBuffers[i]);
            }

            Api.BindVertexArray(0);
        });
    }

    private void SetVertexAttribute(string name, int rank, uint vertexBuffer)
    {
        _inputs[name] = Direction.In;

        Api.BindBuffer(GlBufferTarget.ArrayBuffer, vertexBuffer);

        uint location = Api.GetAttributeLocation(_handle, name);
        Api.EnableVertexAttribArray(location);
        Api.FloatVertexAttribPointer(location, rank, rank, 0);

        Api.BindBuffer(GlBufferTarget.ArrayBuffer, 0);
    }

    private static List<DeclarationNode?> GetDeclarations(string shader, List<string> errors)
    {
        var reader = new StringReader(shader);
        var parser = new Parser(new Lexer(reader, errors), errors);
        var program = parser.ParseProgram();
        var declarations = program.Declarations;
        return declarations;
    }

    private void CheckShader(List<DeclarationNode> declarations)
    {
        var channels = declarations
            .OfType<ChannelNode>()
            .ToList();
        foreach (var channel in channels)
        {
            string channelName = channel.Name;
            var direction = channel.Direction;
            if (direction == Direction.In && _netChannels.TryGetValue(channelName, out var prev))
            {
                if (prev.Direction == Direction.Out) _netChannels.Remove(channelName);
                else throw new ArgumentException("matching shader channels require 'out' in vertex and 'in' in fragment shader", channelName);
                if (!Equals(prev.Variable, channel.Variable)) throw new ArgumentException("matching shader channels must be of the same type", channelName);
            }
            else
            {
                var declaration = declarations.SingleOrDefault(d => d.Name == channel.Variable.Type.Name);
                if (declaration is StructNode @struct)
                {
                    foreach (var member in @struct.Members)
                    {
                        _netChannels.Add($"{channelName}.{member.Name}", new(direction, member));
                    }
                }
                else
                {
                    _netChannels[channelName] = channel;
                }
            }
        }
    }

    internal static void Set(GlShading shading, string name, bool transpose, Matrix4 matrix)
    {
        shading.DoInContext(() => shading.Set(name, transpose, matrix));
    }

    private void SetUniform(string channel, string glslType, bool @checked, bool isArray = false)
    {
        if (@checked)
        {
            if (!_netChannels.TryGetValue(channel, out var netChannel)) Throw(channel, "not found");

            if (netChannel?.Direction != Direction.Uniform) Throw(channel, "is not uniform");

            if (netChannel!.Variable.Type.Name != glslType) Throw(channel, $"should be of type {glslType}");
            if (isArray != netChannel.Variable is ArrayVariableNode) Throw(channel, $"should be an array of {glslType}");
        }

        _inputs[channel] = Direction.Uniform;
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, int value, bool @checked = true)
    {
        SetUniform(name, "int", @checked);
        Api.Uniform1(Api.GetUniformLocation(_handle, name), value);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, uint value, string glslType, bool @checked = true)
    {
        SetUniform(name, glslType, @checked);
        Api.Uniform1(Api.GetUniformLocation(_handle, name), value);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, bool value, bool @checked = true)
    {
        SetUniform(name, "bool", @checked);
        Api.Uniform1(Api.GetUniformLocation(_handle, name), value ? 1 : 0);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, float value, bool @checked = true)
    {
        SetUniform(name, "float", @checked);
        Api.Uniform1(Api.GetUniformLocation(_handle, name), value);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, Color3 value, bool @checked = true)
    {
        SetUniform(name, "vec3", @checked);
        Api.Uniform3(Api.GetUniformLocation(_handle, name), value.R, value.G, value.B);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, Color4 value, bool @checked = true)
    {
        SetUniform(name, "vec4", @checked);
        Api.Uniform4(Api.GetUniformLocation(_handle, name), value.R, value.G, value.B, value.A);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, Point3 value, bool @checked = true)
    {
        SetUniform(name, "vec3", @checked);
        Api.Uniform3(Api.GetUniformLocation(_handle, name), value.X, value.Y, value.Z);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, Vector3 value, bool @checked = true)
    {
        SetUniform(name, "vec3", @checked);
        Api.Uniform3(Api.GetUniformLocation(_handle, name), value.X, value.Y, value.Z);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, Vector4 value, bool @checked = true)
    {
        SetUniform(name, "vec4", @checked);
        Api.Uniform4(Api.GetUniformLocation(_handle, name), value.X, value.Y, value.Z, value.W);
    }

    // Pass value for name to the shader.
    protected internal void SetMatrix2(string name, bool transpose, float[] elements, bool @checked = true)
    {
        SetUniform(name, "mat2", @checked);
        Api.UniformMatrix2(Api.GetUniformLocation(_handle, name), transpose, elements);
    }

    // Pass value for name to the shader.
    protected internal void SetMatrix3(string name, bool transpose, float[] elements, bool @checked = true)
    {
        SetUniform(name, "mat3", @checked);
        Api.UniformMatrix3(Api.GetUniformLocation(_handle, name), transpose, elements);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, bool transpose, Matrix4 value, bool @checked = true)
    {
        SetUniform(name, "mat4", @checked);
        Api.UniformMatrix4(Api.GetUniformLocation(_handle, name), transpose, value);
    }

    // Pass value for name to the shader.
    protected internal void Set(string name, int dimensions, float[] values, bool @checked = true)
    {
        var glslType = dimensions == 0 ? "float" : $"vec{dimensions}";
        SetUniform(name, glslType, @checked, true);
        uint location = Api.GetUniformLocation(_handle, name);
        switch (dimensions)
        {
            case 0: Api.Uniform1(location, values); break;
            case 1: Api.Uniform1(location, values); break;
            case 2: Api.Uniform2(location, values); break;
            case 3: Api.Uniform3(location, values); break;
            case 4: Api.Uniform4(location, values); break;
            default: throw new ArgumentOutOfRangeException(nameof(dimensions));
        }
    }

    // Reset value for name in the shader.
    protected internal void Reset(string input, bool assertExists = true)
    {
        if (assertExists && !_netChannels.ContainsKey(input)) throw new ArgumentException("shader channel not found", nameof(input));
        _inputs.Remove(input);
    }

    internal void CheckInputs()
    {
        foreach (var (name, channel) in _netChannels)
        {
            if (_inputs.TryGetValue(name, out var kind))
            {
                if (kind != channel.Direction) Throw(name, $"{kind} should be {channel}");
            }
            else
            {
                if (channel.Direction != Direction.Out) Throw(name, $"{kind} has no input");
            }
        }
    }

    private static void Report(List<string> errors)
    {
        foreach (var error in errors)
        {
            if (!string.IsNullOrEmpty(error)) Console.WriteLine(error);
        }
    }

    private void Throw(string channel, string message)
    {
        var shader = _declarations.Any(d => d.Name == channel)
            ? "vertex"
            : "fragment";
        throw new ArgumentException($"{shader} shader channel {channel} {message}");
    }

    public override void Dispose()
    {
        if (!Interlocked.Exchange(ref _disposed, true))
        {
            GC.SuppressFinalize(this);

            Api.Invoke(() => Api.DeleteProgram(_handle));
            foreach (var aspect in Aspects) aspect.Dispose();
        }
    }
}