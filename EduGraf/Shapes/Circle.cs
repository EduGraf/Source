using System;
using System.Collections.Generic;

namespace EduGraf.Shapes;

// Geometric representation of a circle by triangles indexing distinct vertices.
public static class Circle
{
    // Return a triangulation with a single center vertex and all other vertices on the circle boundary.
    public static float[] GetCakePositions(int sectors)
    {
        var positions = new float[3 * (sectors + 1)];
        int i = 3;

        float angle = 0;
        var deltaAngle = 2 * MathF.PI / sectors;
        for (int sector = 0; sector < sectors; sector++)
        {
            positions[i++] = MathF.Cos(angle);
            positions[i++] = MathF.Sin(angle);
            positions[i++] = 0;
            angle += deltaAngle;
        }

        return positions;
    }

    // Return a triangulation with a single center vertex and all other vertices on the circle boundary.
    public static ushort[] GetCakeTriangles(int sectors)
    {
        var triangles = new ushort[3 * sectors];
        int i = 0;
        for (int sector = 0; sector < sectors; sector++)
        {
            triangles[i++] = 0;
            triangles[i++] = (ushort)(1 + sector);
            triangles[i++] = (ushort)(1 + (sector + 1) % sectors);
        }

        return triangles;
    }

    // Return a triangulation splitting up the circle in a number of rings each having the form of a stripe of equilateral triangles.
    public static float[] GetIsoPositions(int rings)
    {
        var positions = new float[rings * (rings + 1) / 2 * 6 * 3 + 3];
        int i = 0;
        positions[i++] = 0;
        positions[i++] = 0;
        positions[i++] = 0;

        float r = 0;
        float deltaR = 0.5f / rings;
        int sectors = 6;
        for (int ring = 1; ring <= rings; ring++)
        {
            r += deltaR;
            float angle = 0;
            var deltaAngle = 2 * MathF.PI / sectors;
            for (int sector = 0; sector < sectors; sector++)
            {
                positions[i++] = MathF.Cos(angle) * r;
                positions[i++] = 0;
                positions[i++] = MathF.Sin(angle) * r;
                angle += deltaAngle;
            }

            sectors += 6;
        }

        return positions;
    }

    // Return a triangulation splitting up the circle in a number of rings each having the form of a stripe of equilateral triangles.
    public static ushort[] GetIsoTriangles(int rings)
    {
        var triangles = new List<ushort>();
        AddIsoTriangles(rings, triangles);
        return triangles.ToArray();
    }

    internal static void AddIsoTriangles(int rings, List<ushort> triangles)
    {
        int sectors = 6;
        for (int sector = 0; sector < sectors; sector++)
        {
            triangles.Add(0);
            triangles.Add((ushort)(1 + sector));
            triangles.Add((ushort)(1 + (sector + 1) % sectors));
        }

        int innerStart = 1;
        for (int ring = 2; ring <= rings; ring++)
        {
            int outerStart = innerStart + sectors;
            int outerSector = 0;
            int moduloCorders = sectors / 6;
            for (int innerSector = 0; innerSector < sectors; innerSector++)
            {
                int innerNext = (innerSector + 1) % sectors;
                int outerNext = (outerSector + 1) % (sectors + 6);

                triangles.Add((ushort)(innerStart + innerSector));
                triangles.Add((ushort)(outerStart + outerSector));
                triangles.Add((ushort)(outerStart + outerNext));

                triangles.Add((ushort)(innerStart + innerSector));
                triangles.Add((ushort)(outerStart + outerNext));
                triangles.Add((ushort)(innerStart + innerNext));

                outerSector = outerNext;
                if (innerSector % moduloCorders == moduloCorders - 1)
                {
                    triangles.Add((ushort)(innerStart + innerNext));
                    triangles.Add((ushort)(outerStart + outerSector));
                    outerSector = (outerSector + 1) % (sectors + 6);
                    triangles.Add((ushort)(outerStart + outerSector));
                }
            }

            sectors += 6;
            innerStart = outerStart;
        }
    }
}