using System;

namespace EduGraf.OpenGL.Enums;

[OmitInDocumentation]
[Flags]
public enum GlBufferBit
{
    Depth = 0x00000100,
    Stencil = 0x00000400,
    Color = 0x00004000,
}