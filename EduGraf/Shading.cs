using EduGraf.Cameras;
using System;

namespace EduGraf;

// Base interface for all shadings.
// Refer to the programming concepts documentation to understand how the different concepts work together.
public abstract class Shading(Camera? camera) : IDisposable
{
    // set if the shader requires infos about it
    public Camera? Camera { get; } = camera;

    public abstract void Dispose();
}