using EduGraf.Tensors;
using System.Collections.Generic;

namespace EduGraf;

// This represents a visual or a group of visuals that can be transformed together.
public class Visual
{
    private readonly List<Visual> _children;

    // for debugging purposes (not used by the framework)
    public string Name { get; }

    // added directly to this
    public IEnumerable<Visual> Children => _children;

    // applied to this; overall transform = product of this and all ancestors
    public Matrix4 Transform { get; set; }

    // Create a new visual.
    internal Visual(string name /* not used by the framework. */)
    {
        Name = name;
        _children = [];
        Transform = Matrix4.Identity;
    }

    // Add a child visual to below this.
    public void Add(Visual child) => _children.Add(child);

    // Remove all children.
    public void ClearChildren() => _children.Clear();

    // Remove a specific child.
    public bool Remove(Visual child) => _children.Remove(child);

    // Do not call this from application programs.
    public virtual void Render() { }

    // Scale all descendants in all axis directions by the same factor.
    public virtual Visual Scale(float factor /* in world units. */)
    {
        Transform *= Matrix4.Scale(factor);
        return this;
    }

    // Scale descendants by varying factors in the different axis directions.
    public virtual Visual Scale(Vector3 factor /* in world units. */)
    {
        Transform *= Matrix4.Scale(factor);
        return this;
    }

    // Translate descendants by a vector.
    public virtual Visual Translate(Vector3 direction /* in world units. */)
    {
        Transform *= Matrix4.Translation(direction);
        return this;
    }

    // Rotate descendants around the x-axis.
    public virtual Visual RotateX(float angle /* in radians */)
    {
        Transform *= Matrix4.RotationX(angle);
        return this;
    }

    // Rotate descendants around the y-axis.
    public virtual Visual RotateY(float angle /* in radians */)
    {
        Transform *= Matrix4.RotationY(angle);
        return this;
    }

    // Rotate descendants around the z-axis.
    public virtual Visual RotateZ(float angle /* in radians */)
    {
        Transform *= Matrix4.RotationZ(angle);
        return this;
    }

    public override string ToString() => Name;
}