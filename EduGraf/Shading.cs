using EduGraf.Cameras;
using System;

namespace EduGraf;

// Base interface for all shadings.
// Refer to the programming concepts documentation to understand how the different concepts work together.
public abstract class Shading : IDisposable
{
    // set if the shader requires infos about it
    public Camera? Camera { get; }

    // Create a new shading.
    protected Shading(Camera? camera) => Camera = camera;

    public abstract void Dispose();
}