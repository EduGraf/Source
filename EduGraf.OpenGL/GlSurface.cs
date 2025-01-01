using EduGraf.Geometries;
using EduGraf.OpenGL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EduGraf.OpenGL;

internal class GlSurface : Surface, IDisposable
{
    private readonly GlApi _api;
    private readonly GlIndexBuffer? _indexBuffer;
    private readonly int _vertexCount;

    internal uint[] VertexBuffers { get; }
    internal uint VertexArray { get; }

    public GlSurface(GlApi api, GlShading shading, IGeometry geometry)
        : base(shading, geometry)
    {
        _api = api;
        var attributes = new List<GlAttribute>();
        foreach (var property in GetProperties(geometry.GetType()))
        {
            var dimensionAttribute = property.GetCustomAttribute<DimensionAttribute>();
            if (dimensionAttribute != null)
            {
                attributes.Add(new GlAttribute(property.Name, dimensionAttribute.N, (Array)property.GetValue(geometry)!));
            }
        }

        switch (geometry)
        {
            case ITriangle32Geometry mapping32:
                _indexBuffer = new GlIndexBuffer(api, mapping32.Triangles);
                break;
            case ITriangle16Geometry mapping16:
                _indexBuffer = new GlIndexBuffer(api, mapping16.Triangles);
                break;
        }

        VertexBuffers = new uint[attributes.Count];
        var positionsAttribute = attributes.Single(a => a.Name == nameof(Geometry.Position));
        if (!positionsAttribute.Dimensionality.HasValue)
        {
            throw new NotSupportedException($"need to define the dimensionality of {positionsAttribute.Name}");
        }
        _vertexCount = positionsAttribute.Values.Length / positionsAttribute.Dimensionality.Value;

        for (int i = 0; i < attributes.Count; i++)
        {
            var attribute = attributes[i];
            if (!attribute.Dimensionality.HasValue)
            {
                throw new NotSupportedException($"need to define the dimensionality of {attribute.Name}");
            }
            if (attribute.Values.Length / attribute.Dimensionality != _vertexCount)
            {
                throw new NotSupportedException("the number of vertices does not match");
            }

            VertexBuffers[i] = GlBuffer.CreateBuffer(api, attribute.Values, GlBufferTarget.ArrayBuffer);
        }

        VertexArray = api.GenVertexArray();
        shading.SetVertexArrayAttributes(VertexArray, attributes, VertexBuffers);
    }

    internal virtual void Draw()
    {
        _api.BindVertexArray(VertexArray);

        if (_indexBuffer != null)
        {
            _api.BindBuffer(GlBufferTarget.ElementArrayBuffer, _indexBuffer.Handle);
            if (Geometry is ITriangle32Geometry)
            {
                _api.DrawIndexedTriangles<uint>(_indexBuffer.Length, nint.Zero);
            }
            else if (Geometry is ITriangle16Geometry)
            {
                _api.DrawIndexedTriangles<ushort>(_indexBuffer.Length, nint.Zero);
            }
            else
            {
                throw new InvalidOperationException("unexpected geometry type");
            }
            _api.BindBuffer(GlBufferTarget.ElementArrayBuffer, 0);
        }
        else
        {
            _api.DrawTriangles(_vertexCount);
        }

        _api.BindVertexArray(0);
    }

    private static PropertyInfo[] GetProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
            .Where(p => p.PropertyType.BaseType != typeof(LambdaExpression) && p.PropertyType.IsArray) // only array properties
            .ToArray();
    }

    public void Dispose()
    {
        _api.Invoke(() =>
        {
            _api.DeleteVertexArray(VertexArray);
            foreach (var t in VertexBuffers) _api.DeleteBuffer(t);
        });
        _indexBuffer?.Dispose();
    }
}