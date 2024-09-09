namespace EduGraf.Tensors;

// This is the base class for all tensors.
public abstract class Tensor
{
    // of this tensor in row-major form.
    public float[] Elements { get; }


    protected Tensor(params float[] elements) => Elements = elements;

    public override string ToString() => $"({string.Join(", ", Elements)})";
}