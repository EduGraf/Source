using System;

namespace EduGraf.OpenGL;

internal struct GlAttribute
{
    public string Name { get; set; }
    public int? Dimensionality { get; set; }
    public Array Values { get; set; }

    public GlAttribute(string name, int? dimensionality, Array values)
    {
        Name = name;
        Dimensionality = dimensionality;
        Values = values;
    }
}
