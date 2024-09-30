namespace EduGraf.Tensors;

// This is the base class for all tensors.
public abstract class Tensor(params float[] elements)
{
    // of this tensor in row-major form.
    public float[] Elements { get; } = elements;

    public override string ToString() => $"({string.Join(", ", Elements)})";
}