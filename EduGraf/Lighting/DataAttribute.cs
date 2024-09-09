using System;

namespace EduGraf.Lighting;

// This marks properties who's value is transferred to the GPU.
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class DataAttribute : Attribute
{
}