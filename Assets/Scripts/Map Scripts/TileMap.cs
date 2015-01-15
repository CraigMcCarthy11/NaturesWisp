using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{
    public int size_x = 100;
    public int size_z = 50;
    public float tileSize = 1.0f;
    public Vector2 gridSquareSize = new Vector2(5, 5); //Whole Numbers only! Also, use same numbers in you want equal colors to line up with verts
    


    // Use this for initialization
    void Start()
    {
        BuildMesh();
    }

    void BuildTexture()
    {
        int textureWidth = (int)gridSquareSize.x;
        int textureHeight = (int)gridSquareSize.y;

        Texture2D texture = new Texture2D(textureWidth, textureHeight);

        //Sets the texture to each tile
        for (int y = 0; y < textureHeight; y++)
        {
            for( int x = 0; x < textureWidth; x++){
                //Randomly generate a color and set it
                Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                texture.SetPixel(x, y, color);
            }
        }
        //this makes it so each color stands out for testing purposes/doesnt blend.
        texture.filterMode = FilterMode.Point;
        //texture.wrapMode = WrapMode.Default;
        //Applies the texture
        texture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        //Sets it to the mesh
        mesh_renderer.sharedMaterials[0].mainTexture = texture;

        Debug.Log("Done Texture");

        CalclateGridCenterPoints();
    }

    public void BuildMesh()
    {
        int numTiles = size_x * size_z;
        int numTris = numTiles * 2;

        int vsize_x = size_x + 1;
        int vsize_z = size_z + 1;
        int numVerts = vsize_x * vsize_z;

        // Generate the mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];

        //Begins the triangle generation
        int[] triangles = new int[numTris * 3];

        int x, z;
        //Generates the vertices in the x - z direction
        for (z = 0; z < vsize_z; z++)
        {
            for (x = 0; x < vsize_x; x++)
            {
                //change y to depend on color or texture applied
                vertices[z * vsize_x + x] = new Vector3(x * tileSize, Random.Range(-.2f,.2f), z * tileSize);
                normals[z * vsize_x + x] = Vector3.up;
                uv[z * vsize_x + x] = new Vector2((float)x / size_x, (float)z / size_z);
            }
        }
        Debug.Log("Done Verts!");

        //Generates the triangles in the x-z direction, using triangle offset.
        for (z = 0; z < size_z; z++)
        {
            for (x = 0; x < size_x; x++)
            {
                int squareIndex = z * size_x + x;
                int triOffset = squareIndex * 6;
                triangles[triOffset + 0] = z * vsize_x + x + 0;
                triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
                triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;

                triangles[triOffset + 3] = z * vsize_x + x + 0;
                triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
                triangles[triOffset + 5] = z * vsize_x + x + 1;
            }
        }

        Debug.Log("Done Triangles!");

        // Create a new Mesh and populate with the data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        // Assign our mesh to our filter/renderer/collider
        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = mesh;
        //Points to an existing mesh
        mesh_collider.sharedMesh = mesh;

        //Recalculates normals for lighting purposes... I think
        mesh.RecalculateNormals();
        Debug.Log("Done Mesh");

        BuildTexture();

    }

    public void CalclateGridCenterPoints()
    {
        int gridSizeGameWorld_x, gridSizeGameWorld_y;
        gridSizeGameWorld_x = (int)tileSize * size_x;
        gridSizeGameWorld_y = (int)tileSize * size_z;
        List<Vector3> objsToSpawn = new List<Vector3>();

        float XposToSpawn = 0f;
        float YposToSpawn = gridSizeGameWorld_y / tileSize;
        for (int i = 0; i < tileSize; i++)
        {
            //Looping through each X Square
            for (int j = 0; j < tileSize; j++)
            {
                XposToSpawn = gridSizeGameWorld_x / tileSize + XposToSpawn;
                int setToMiddleX = size_x / 2;
                objsToSpawn.Add(new Vector3(XposToSpawn - setToMiddleX,1,YposToSpawn / 2));
            }
            //Completed a row, move up by one
            YposToSpawn = gridSizeGameWorld_y / tileSize + YposToSpawn;
            YposToSpawn = YposToSpawn / 2;
            YposToSpawn = gridSizeGameWorld_y / tileSize + YposToSpawn + YposToSpawn;
            XposToSpawn = 0;
        }

        //Spawn Objects
        foreach (Vector3 pos in objsToSpawn)
        {
            GameObject anchor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            anchor.transform.position = pos;
        }
    }


}