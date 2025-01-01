namespace EduGraf.Tensors;

// This is the base class for all tensors.
public interface ITensor
{
    public float[] Elements { get; } // all in row-major form
}