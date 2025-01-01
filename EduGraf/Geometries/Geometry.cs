namespace EduGraf.Geometries;

// Each method creates some kind of geometry from the given parameters.
public static class Geometry
{
    // Create a new geometry. Each vertex is defined by 3 position coordinates. Three consecutive vertices define one triangle.
    public static IGeometry Create(float[] positions /* unrolled (x, y, z) coordinates */)
    {
        return new NonIndexedGeometry(positions);
    }

    // Create a new geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive vertices define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals) // unrolled (x, y, z) coordinates
    {
        return new NormalNonIndexedGeometry(positions, normals);
    }

    // Create a new geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        uint[] triangles) // unrolled list of vertex numbers
    {
        return new Triangle32Geometry(positions, triangles);
    }

    // Create a new geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new Triangle16Geometry(positions, triangles);
    }

    // Create a new geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        uint[] triangles)  // unrolled list of vertex numbers
    {
        return new Triangle32NormalGeometry(positions, normals, triangles);
    }

    // Create a new geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new Triangle16NormalGeometry(positions, normals, triangles);
    }

    // Create a new geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] textureUv) // unrolled (u, v) texture coordinates
    {
        return new TexturedNonIndexedGeometry(positions, textureUv);
    }

    // Create a new geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        float[] textureUv) // unrolled (u, v) texture coordinates
    {
        return new TexturedNormalNonIndexedGeometry(positions, normals, textureUv);
    }

    // Create a new geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        uint[] triangles) // unrolled list of vertex numbers
    {
        return new TexturedTriangle32Geometry(positions, triangles, textureUv);
    }

    // Create a new geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new TexturedTriangle16Geometry(positions, triangles, textureUv);
    }

    // Create a new geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        uint[] triangles) // unrolled list of vertex numbers
    {
        return new TexturedTriangle32NormalGeometry(positions, normals, triangles, textureUv);
    }

    // Create a new geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new TexturedTriangle16NormalGeometry(positions, normals, triangles, textureUv);
    }
}