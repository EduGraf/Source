using System;

namespace EduGraf.OpenGL;

internal struct GlAttribute(string name, int? dimensionality, Array values)
{
    public string Name { get; set; } = name;
    public int? Dimensionality { get; set; } = dimensionality;
    public Array Values { get; set; } = values;
}
