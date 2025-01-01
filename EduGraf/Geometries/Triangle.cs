namespace EduGraf.Geometries;

// Represents a triangle with corners referring to vertices.
public readonly struct Triangle(uint c1, uint c2, uint c3)
{
    public uint C1 { get; } = c1; // index of first corner
    public uint C2 { get; } = c2; // index of second corner
    public uint C3 { get; } = c3; // index of third corner
}