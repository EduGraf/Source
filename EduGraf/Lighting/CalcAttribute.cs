using System;

namespace EduGraf.Lighting
{
    // This marks properties that are computed on the GPU.
    [AttributeUsage(AttributeTargets.Property)]
    public class CalcAttribute : Attribute;
}
