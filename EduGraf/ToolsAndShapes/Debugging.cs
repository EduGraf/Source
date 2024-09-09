using EduGraf.Tensors;

namespace EduGraf.ToolsAndShapes
{
    // Help to debug incorrectly defined normals.
    public static class Debugging
    {
        // Creates a tetrahedron for each given (point, direction)-pair.
        public static (float[] positions, uint[] triangles) CreateTetrahedronsFromPointVectors(float[] points, float[] directions, float scale)
        {
            var arrow = Tetrahedron.GetPositions(0.05f);
            var pointCount = points.Length / 3;
            var positions = new float[pointCount * arrow.Length];
            var triangles = new uint[pointCount * Tetrahedron.Triangles.Length];
            int v = 0;
            int i = 0;
            uint indexOffset = 0;
            for (int p = 0; p < points.Length; p += 3)
            {
                for (int a = 0; a < arrow.Length; a += 3)
                {
                    float x = directions[p];
                    float y = directions[p + 1];
                    float z = directions[p + 2];
                    float x2 = x * x;
                    float y2 = y * y;
                    float z2 = z * z;
                    float xy = x * y;
                    float xz = x * z;
                    float yz = y * z;
                    float x2z2 = x2 + z2;
                    var rotation = new Matrix4(
                        -y - z, x + z, x - y, 0,
                        x, y, z, 0,
                        xy - y2 - xz - z2, xy - yz - x2z2, x2z2 + xz + y2, 0,
                        0, 0, 0, 1
                    );

                    var translation = new Vector4(points[p], points[p + 1], points[p + 2], 1);

                    var position = new Vector4(arrow[a], arrow[a + 1], arrow[a + 2], 1);

                    position *= scale;
                    position *= rotation;
                    position += translation;

                    positions[v++] = position.X;
                    positions[v++] = position.Y;
                    positions[v++] = position.Z;
                }

                foreach (var index in Tetrahedron.Triangles)
                {
                    triangles[i++] = index + indexOffset;
                }

                indexOffset += 4;
            }

            return (positions, triangles);
        }
    }
}
