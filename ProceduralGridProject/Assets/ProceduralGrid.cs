using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    [SerializeField] private int xSize, ySize;

    private MeshFilter meshFilter;

    private Vector3[] vertices;
    private int[] tris;
    private Vector2[] uv;
    private Vector4[] tangents;

    private Mesh mesh;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter> ();

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        uv = new Vector2[vertices.Length];
        tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for(int i = 0, y = 0; y <= ySize; y++)
        {
            for(int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y, 0);
                uv[i] = new Vector2(x / (float)xSize, y / (float)ySize);
                tangents[i] = tangent;
            }
        }

        tris = new int[xSize * ySize * 6];
        for(int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for(int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                tris[ti] = vi;
                tris[ti + 3] = tris[ti + 2] = vi + 1;
                tris[ti + 4] = tris[ti + 1] = vi + xSize + 1;
                tris[ti + 5] = vi + xSize + 2;
            }
        }

        mesh = new Mesh ();
        mesh.name = "Procedural Grid";
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        mesh.triangles = tris;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if(vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        float colorStep = 1.0f / vertices.Length;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color += new Color (colorStep, colorStep, colorStep);
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
