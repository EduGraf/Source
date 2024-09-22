using System;

namespace EduGraf.Shapes;

// Geometric representation of a rectangular surface patch as a function of (x, z) by triangles indexing distinct vertices.
public static class Patch
{
    // Return the surface.
    public static float[] GetPositions(
        int nx, // number of sub-patches in x-axis direction.
        int nz, // number of sub-patches in y-axis direction. 
        Func<float, float, float> y) // height in y-axis-direction for any given point on the patch.
    {
        var positions = new float[(nx + 1) * (nz + 1) * 3];
        float d = 2f / (nx + nz);
        float cz = -nz * d / 2f;
        float cx0 = -nx * d / 2f;
        int i = 0;
        for (int z = 0; z <= nz; z++)
        {
            float cx = cx0;

            for (int x = 0; x <= nx; x++)
            {
                positions[i++] = cx;
                positions[i++] = y(cx, cz);
                positions[i++] = cz;
                cx += d;
            }

            cz += d;
        }

        return positions;
    }

    // Return the surface.
    public static ushort[] GetTriangles(
        int nx, // number of sub-patches in x-axis direction.
        int nz) // number of sub-patches in y-axis direction. )
    {
        var triangles = new ushort[nx * nz * 2 * 3];
        int t = 0;

        for (int z = 0; z < nz; z++)
        {
            for (int x = 0; x < nx; x++)
            {
                int i = z * (nx + 1) + x;
                triangles[t++] = (ushort)i;
                triangles[t++] = (ushort)(i + nx + 1);
                triangles[t++] = (ushort)(i + 1);

                triangles[t++] = (ushort)(i + nx + 2);
                triangles[t++] = (ushort)(i + 1);
                triangles[t++] = (ushort)(i + nx + 1);
            }
        }

        return triangles;
    }

    // Return (u, v) for all vertices.
    public static float[] GetTextureUvs(
    int nx, // number of sub-patches in x-axis direction.
    int nz) // number of sub-patches in y-axis direction. 
    {
        var vertices = new float[(nx + 1) * (nz + 1) * 2];
        float d = 2f / (nx + nz);
        float cz = 0;
        int i = 0;
        for (int z = 0; z <= nz; z++)
        {
            float cx = 0;
            for (int x = 0; x <= nx; x++)
            {
                vertices[i++] = cx;
                vertices[i++] = cz;
                cx += d;
            }
            cz += d;
        }

        return vertices;
    }
}