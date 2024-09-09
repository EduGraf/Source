using EduGraf.Tensors;
using System.Collections.Generic;
using System.Linq;

namespace EduGraf;

// This represents a visual or a group of visuals that can be transformed together.
public class Visual
{
    private readonly List<Visual> _children;

    // that can be used for debugging purposes and is not used by the framework.
    public string Name { get; }

    // the visuals that have been added directly to this visual.
    public IEnumerable<Visual> Children => _children;

    // the transform-matrix applied to this visual.
    public Matrix4 Transform { get; set; }

    // this visual and recursively all that have been added to it.
    public IEnumerable<Visual> Descendants => new[] { this }
        .Concat(_children.SelectMany(child => child.Descendants));

    // Create a new visual.
    internal Visual(string name /* not used by the framework. */)
    {
        Name = name;
        _children = new List<Visual>();
        Transform = Space.Identity4;
    }

    // Add a child visual to below this.
    public void Add(Visual child) => _children.Add(child);

    // Do not call this from application programs.
    public virtual void Render() { }

    // Scale all descendants in all axis directions by the same factor.
    public virtual Visual Scale(float factor /* in world units. */)
    {
        Transform *= Space.Scale4(factor);
        foreach (var child in _children) child.Scale(factor);
        return this;
    }

    // Scale descendants by varying factors in the different axis directions.
    public virtual Visual Scale(Vector3 factor /* in world units. */)
    {
        Transform *= Space.Scale4(factor);
        foreach (var child in _children) child.Scale(factor);
        return this;
    }

    // Translate descendants by a vector.
    public virtual Visual Translate(Vector3 direction /* in world units. */)
    {
        Transform *= Space.Translation4(direction);
        foreach (var child in _children) child.Translate(direction);
        return this;
    }

    // Rotate descendants around the x axis.
    public virtual Visual RotateX(float angle /* in radians */)
    {
        Transform *= Space.RotationX4(angle);
        foreach (var child in _children) child.RotateX(angle);
        return this;
    }

    // Rotate descendants around the y axis.
    public virtual Visual RotateY(float angle /* in radians */)
    {
        Transform *= Space.RotationY4(angle);
        foreach (var child in _children) child.RotateY(angle);
        return this;
    }

    // Rotate descendants around the z axis.
    public virtual Visual RotateZ(float angle /* in radians */)
    {
        Transform *= Space.RotationZ4(angle);
        foreach (var child in _children) child.RotateZ(angle);
        return this;
    }

    public override string ToString() => Name;
}