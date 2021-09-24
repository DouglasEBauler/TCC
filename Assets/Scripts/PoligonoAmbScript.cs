using UnityEditor;
using UnityEngine;

public class PoligonoAmbScript : MonoBehaviour
{
    [SerializeField]
    Material mat;

    Mesh mesh;

    float width = 1;
    float height = 1;

    void Start()
    {
        var vertices = new Vector2[]
        {
            new Vector2(-width, -height),
            new Vector2(-width, height),
            new Vector2(width, height),
            new Vector2(width, -height),
            new Vector2(-width*3, height)
        };

        Triangulator triangulator = new Triangulator(vertices);

        mesh = new Mesh()
        {
            name = "Polygon",
            vertices = System.Array.ConvertAll<Vector2, Vector3>(vertices, p => new Vector3(p.x, p.y)),
            triangles = triangulator.Triangulate()
        };

        GetComponent<MeshRenderer>().material = mat;
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void FixedUpdate()
    {
        //var filter = gameObject.GetComponent<MeshFilter>();
        //filter.mesh.vertices = vertices;
        //filter.mesh.triangles = triangles;
        //filter.mesh.SetIndices(filter.mesh.GetIndices(0), MeshTopology.Lines, 0);
    }
}
