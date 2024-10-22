﻿using System;
using System.Collections.Generic;

namespace EduGraf.Shapes;

// Geometric representation of a sphere by triangles indexing distinct vertices.
public static class Sphere
{
    // Return a triangulation that corresponds to the usual sub-division of a globe.
    public static float[] GetPositions(int longitudeCount, int latitudeCount)
    {
        var positions = new float[(longitudeCount + 1) * (latitudeCount + 1) * 3];

        float lngStep = 2 * MathF.PI / longitudeCount;
        float latStep = MathF.PI / latitudeCount;

        int i = 0;
        for (int lat = 0; lat <= latitudeCount; lat++)
        {
            float latitude = lat * latStep - MathF.PI / 2;
            float xz = MathF.Cos(latitude);
            float y = MathF.Sin(latitude); // vertex position

            // add (sectorCount+1) vertices per stack
            // the first and last vertices have same position and normal, but different tex coords
            float longitude = 0;
            for (int lng = 0; lng <= longitudeCount; lng++)
            {
                positions[i++] = xz * MathF.Cos(longitude); // x
                positions[i++] = y;
                positions[i++] = xz * MathF.Sin(longitude); // z
                longitude += lngStep;
            }
        }

        return positions;
    }

    // Return (u, v) for all vertices.
    public static float[] GetTextureUvs(int longitudeCount, int latitudeCount)
    {
        var vertices = new float[(longitudeCount + 1) * (latitudeCount + 1) * 2];

        float lngStep = 1f / longitudeCount;
        float latStep = 1f / latitudeCount;

        int i = 0;
        float latitude = 0;
        for (int lat = 0; lat <= latitudeCount; lat++)
        {
            // add (sectorCount+1) vertices per stack
            // the first and last vertices have same position and normal, but different tex coords
            float longitude = 0;
            for (int lng = 0; lng <= longitudeCount; lng++)
            {
                vertices[i++] = 1 - longitude;
                vertices[i++] = latitude;
                longitude += lngStep;
            }

            latitude += latStep;
        }

        return vertices;
    }

    // Return a triangulation that corresponds to the usual sub-division of a globe.
    public static ushort[] GetTriangles(int longitudeCount, int latitudeCount)
    {
        ushort[] triangles = new ushort[longitudeCount * latitudeCount * 2 * 3];
        int upperCapOffset = (latitudeCount - 1) * (longitudeCount + 1);

        int i = 0;
        for (ushort longitude = 0; longitude < longitudeCount; longitude++)
        {
            int k1 = longitude;
            int k2 = k1 + longitudeCount + 1;
            triangles[i++] = (ushort)k1;
            triangles[i++] = (ushort)k2;
            triangles[i++] = (ushort)(k2 + 1);

            for (int latitude = 1; latitude < latitudeCount; latitude++)
            {
                k1 = longitude + latitude * (longitudeCount + 1);
                k2 = k1 + longitudeCount + 1;

                // 2 triangles per patch
                triangles[i++] = (ushort)k1;
                triangles[i++] = (ushort)(k2 + 1);
                triangles[i++] = (ushort)(k1 + 1);

                triangles[i++] = (ushort)(k2 + 1);
                triangles[i++] = (ushort)k1;
                triangles[i++] = (ushort)k2;
            }

            k1 = upperCapOffset + longitude;
            triangles[i++] = (ushort)k1;
            triangles[i++] = (ushort)(k1 + longitudeCount);
            triangles[i++] = (ushort)(k1 + 1);
        }

        return triangles;
    }

    // Return equilateral triangles.
    public static float[] GetIsoPositions(int rings)
    {
        int length = 3 * FullIsoVerticesCount(rings);
        var positions = new float[length];
        SetHemiIsoVertices(rings, 1, positions, 0);
        SetHemiIsoVertices(rings, -1, positions, 3 * HemiIsoVerticesCount(rings));
        return positions;
    }

    private static void SetHemiIsoVertices(int rings, int sign, float[] positions, int i)
    {
        positions[i++] = 0;
        positions[i++] = sign;
        positions[i++] = 0;

        float latiAngle = 0.5f * MathF.PI;
        float deltaLatiAngle = latiAngle / rings;
        int sectors = 6;
        for (int ring = 1; ring <= rings - (1 - sign) / 2; ring++)
        {
            latiAngle -= deltaLatiAngle;
            float sinLatiAngle = MathF.Sin(latiAngle);
            float cosLatiAngle = MathF.Cos(latiAngle);
            float longAngle = 0;
            var deltaLongAngle = 2 * MathF.PI / sectors;
            for (int sector = 0; sector < sectors; sector++)
            {
                positions[i++] = cosLatiAngle * MathF.Cos(longAngle);
                positions[i++] = sinLatiAngle * sign;
                positions[i++] = cosLatiAngle * MathF.Sin(longAngle);
                longAngle += deltaLongAngle;
            }

            sectors += 6;
        }
    }

    // Return a triangulation consisting of equilateral triangles.
    public static ushort[] GetIsoTriangles(int rings)
    {
        var triangles = new List<ushort>();
        Circle.AddIsoTriangles(rings, triangles);

        int fullHemiIndicesCount = triangles.Count;
        int hemiIsoVerticesCount = HemiIsoVerticesCount(rings);
        int equatorVertexIndex = hemiIsoVerticesCount - EquatorIsoVerticesCount(rings);

        for (int i = 0; i < fullHemiIndicesCount; i++)
        {
            int index = triangles[i];
            if (triangles[i] < equatorVertexIndex)
            {
                index = hemiIsoVerticesCount + triangles[i];
            }
            triangles.Add((ushort)index);
        }

        for (int i = 0; i < fullHemiIndicesCount; i += 3)
        {
            (triangles[i], triangles[i + 1]) = (triangles[i + 1], triangles[i]);
        }

        return triangles.ToArray();
    }

    private static int FullIsoVerticesCount(int rings)
    {
        int hemiCount = HemiIsoVerticesCount(rings);
        int equatorCount = EquatorIsoVerticesCount(rings);
        return 2 * hemiCount - equatorCount;
    }

    private static int EquatorIsoVerticesCount(int rings)
    {
        return rings * 6;
    }

    private static int HemiIsoVerticesCount(int rings)
    {
        return rings * (rings + 1) / 2 * 6 + 1;
    }
}