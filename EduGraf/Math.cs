using System;

namespace EduGraf;

// Helper constant and functions.
[OmitInDocumentation]
public static class Math
{
    // If the difference of two floating point numbers is less, they are considered equal.
    public const float Precision = 1e-5f; 

    // Min-function with three arguments.
    public static float Min(float a, float b, float c) => MathF.Min(a, MathF.Min(b, c));

    // Max-function with three arguments.
    public static float Max(float a, float b, float c) => MathF.Max(a, MathF.Max(b, c));
}