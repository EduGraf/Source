using System;

namespace EduGraf.Lighting;

// This marks properties whose value is transferred to the GPU.
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class DataAttribute : Attribute;
