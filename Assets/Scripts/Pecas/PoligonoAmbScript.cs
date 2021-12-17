using System.Collections.Generic;
using UnityEngine;

public class PoligonoAmbScript : MonoBehaviour
{
    const float round_z = 0.01f;
    const float width = 1f;
    const float height = 1f;

    [SerializeField]
    Material mat;
    [SerializeField]
    GameObject circle;
    [SerializeField]
    GameObject lines;

    [HideInInspector]
    public PoligonoPropriedadePeca PropPeca;

    Mesh mesh;
    LineRenderer lineRender;
    Vector2[] vertices;

    void Start()
    {
        mesh = new Mesh();

        DefineMesh();
    }

    void DefineMesh()
    {
        lineRender = lines.GetComponent<LineRenderer>();

        if (vertices == null)
        {
            vertices  = new Vector2[]
            {
                new Vector2(-width, -height)
                , new Vector2(-width, height)
                , new Vector2(width, height)
            };
        }

        Triangulator triangulator = new Triangulator(vertices);

        mesh.name = "Polygon";
        mesh.vertices = System.Array.ConvertAll<Vector2, Vector3>(vertices, p => new Vector3(p.x, p.y));
        mesh.triangles = triangulator.Triangulate();

        GetComponent<MeshRenderer>().material = mat;
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void ConfiguratePoligono()
    {
        if (PropPeca != null)
        {
            DefineMesh();

            switch (PropPeca.Primitiva)
            {
                case TipoPrimitiva.Vertices: DefinePrimitivaVertices(); break;
                case TipoPrimitiva.Aberto: DefinePrimitivaAberto(); break;
                case TipoPrimitiva.Fechado: DefinePrimitivaFechado(); break;
                case TipoPrimitiva.Preenchido: DefinePrimitivaCheio(); break;
            }
        }
    }

    public void ConfiguratePoints(int qtdPontos)
    {
        if (qtdPontos > 3)
        {
            List<Vector2> verticesNew = new List<Vector2>()
            {
                new Vector2(-width, -height)
                , new Vector2(-width, height)
                , new Vector2(width, height)
            };

            int i = 3;
            while (i < qtdPontos)
            {
                verticesNew.Add(new Vector2(width/2, -height/2));
                i++;
            }

            vertices = verticesNew.ToArray();
        }
        DefineMesh();
    }

    void DefinePrimitivaCheio()
    {
        lineRender.enabled = false;
        EnabledVertices(false);
        GetComponent<MeshRenderer>().enabled = true;
        mesh.SetIndices(mesh.triangles, MeshTopology.Triangles, 0);
    }

    void DefinePrimitivaFechado()
    {
        EnabledVertices(false);
        lines.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        lineRender.enabled = true;
        lineRender.positionCount = mesh.triangles.Length + 1;

        for (int i = 0; i < mesh.triangles.Length; i++)
        {
            lineRender.SetPosition(i, new Vector3(mesh.vertices[mesh.triangles[i]].x, mesh.vertices[mesh.triangles[i]].y, mesh.vertices[mesh.triangles[i]].z + round_z));
        }
        lineRender.SetPosition(mesh.triangles.Length, new Vector3(mesh.vertices[mesh.triangles[0]].x, mesh.vertices[mesh.triangles[0]].y, mesh.vertices[mesh.triangles[0]].z + round_z));

        mesh.SetIndices(mesh.triangles, MeshTopology.Points, 0);
    }

    void DefinePrimitivaAberto()
    {
        EnabledVertices(false);
        DefinePrimitivaFechado();
        lineRender.positionCount--;
        GetComponent<MeshRenderer>().enabled = false;
    }

    void DefinePrimitivaVertices()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.name.Contains("Vertex"))
            {
                Destroy(child.gameObject);
            }
        }

        foreach (int triangle in mesh.triangles)
        {
            if (GameObject.Find("Vertex" + triangle.ToString()) == null)
            {
                GameObject vertexCircle = Instantiate(circle, circle.transform.position, circle.transform.rotation, circle.transform.parent);
                vertexCircle.name = "Vertex" + triangle.ToString();
                vertexCircle.transform.localPosition = new Vector3(mesh.vertices[triangle].x, mesh.vertices[triangle].y, mesh.vertices[triangle].z + round_z);
                vertexCircle.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        lineRender.enabled = false;
        mesh.SetIndices(mesh.triangles, MeshTopology.Points, 0);
    }

    void EnabledVertices(bool enabled)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.name.Contains("Vertex"))
            {
                child.GetComponent<SpriteRenderer>().enabled = enabled;
            }
        }
    }
}
