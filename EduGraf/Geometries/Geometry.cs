namespace EduGraf.Geometries;

// Each method creates some kind of geometry from the given parameters.
public static class Geometry
{
    // Create a new 2d geometry. Each vertex is defined by 3 position coordinates. Three consecutive vertices define one triangle.
    public static IGeometry Create2D(float[] positions /* unrolled (x, y) coordinates */)
    {
        return new FlatGeometry(positions);
    }

    // Create a new 2d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create2D(
        float[] positions, // unrolled (x, y) coordinates
        uint[] triangles) // unrolled list of vertex numbers
    {
        return new FlatTriangleGeometry(positions, triangles);
    }

    // Create a new 2d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create2D(
        float[] positions, // unrolled (x, y) coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new FlatTriGeometry(positions, triangles);
    }

    // Create a new 2d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv2D(
        float[] positions, // unrolled (x, y) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        uint[] triangles) // unrolled list of vertex numbers
    {
        return new FlatTexturedTriangleGeometry(positions, triangles, textureUv);
    }

    // Create a new 2d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv2D(
        float[] positions, // unrolled (x, y) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new FlatTexturedTriGeometry(positions, triangles, textureUv);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position coordinates. Three consecutive vertices define one triangle.
    public static IGeometry Create(float[] positions /* unrolled (x, y, z) coordinates */)
    {
        return new GeometryBase(positions);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive vertices define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals) // unrolled (x, y, z) coordinates
    {
        return new NormalGeometry(positions, normals);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        uint[] triangles) // unrolled list of vertex numbers
    {
        return new TriangleGeometry(positions, triangles);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new TriGeometry(positions, triangles);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        uint[] triangles)  // unrolled list of vertex numbers
    {
        return new TriangleNormalGeometry(positions, normals, triangles);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry Create(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new TriNormalGeometry(positions, normals, triangles);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] textureUv) // unrolled (u, v) texture coordinates
    {
        return new TexturedGeometry(positions, textureUv);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        float[] textureUv) // unrolled (u, v) texture coordinates
    {
        return new TexturedNormalGeometry(positions, normals, textureUv);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        uint[] triangles) // unrolled list of vertex numbers
    {
        return new TexturedTriangleGeometry(positions, triangles, textureUv);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new TexturedTriGeometry(positions, triangles, textureUv);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        uint[] triangles) // unrolled list of vertex numbers
    {
        return new TexturedTriangleNormalGeometry(positions, normals, triangles, textureUv);
    }

    // Create a new 3d geometry. Each vertex is defined by 3 position and 3 normal coordinates. Three consecutive triangle elements define one triangle.
    public static IGeometry CreateWithUv(
        float[] positions, // unrolled (x, y, z) coordinates
        float[] normals, // unrolled (x, y, z) coordinates
        float[] textureUv, // unrolled (u, v) texture coordinates
        ushort[] triangles) // unrolled list of vertex numbers
    {
        return new TexturedTriNormalGeometry(positions, normals, triangles, textureUv);
    }
}