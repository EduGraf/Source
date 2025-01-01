using System;

namespace EduGraf;

// This is the base class for all shadings.
// Refer to the programming concepts documentation to understand how the different concepts work together.
public abstract class Shading(string name) : IDisposable
{
    public string Name { get; } = name; // for debugging purposes

    public abstract void Dispose();
}