using EduGraf.Tensors;

namespace EduGraf.Lighting;

// This represents light from any direction.
public class AmbientLight(Color3 color) : Light(color);