using System;

namespace EduGraf;

// Base interface for all shadings.
// Refer to the programming concepts documentation to understand how the different concepts work together.
public abstract class Shading(string name) : IDisposable
{
    // for debugging purposes
    public string Name { get; } = name;

    public abstract void Dispose();
}