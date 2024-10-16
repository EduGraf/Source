using EduGraf.Cameras;
using System;

namespace EduGraf;

// Base interface for all shadings.
// Refer to the programming concepts documentation to understand how the different concepts work together.
public abstract class Shading(string name) : IDisposable
{
    // for debugging purposes
    public string Name { get; } = name;

    // set if the shader requires infos about it
    public Camera? Camera { get; set; }

    public abstract void Dispose();
}