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

    public PoligonoPropriedadePeca PropPeca;

    Mesh mesh;
    LineRenderer lineRender;

    void Start()
    {
        mesh = new Mesh();
        lineRender = lines.GetComponent<LineRenderer>();

        DefineMesh();
    }

    void DefineMesh()
    {
        var vertices = new Vector2[]
        {
            new Vector2(-width, -height)
            , new Vector2(-width, height)
            , new Vector2(width, height)
            //, new Vector2(width, -height)
            //, new Vector2(-width*2.5f, height)
        };

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
                case TipoPrimitiva.Cheio: DefinePrimitivaCheio(); break;
            }
        }
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
        lines.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        lineRender.enabled = true;
        lineRender.positionCount = mesh.triangles.Length;

        for (int i = 0; i < mesh.triangles.Length; i++)
        {
            lineRender.SetPosition(i, new Vector3(mesh.vertices[mesh.triangles[i]].x, mesh.vertices[mesh.triangles[i]].y, mesh.vertices[mesh.triangles[i]].z + round_z));
        }

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

        for (int i = 0; i < mesh.triangles.Length; i++)
        {
            if (GameObject.Find("Vertex" + mesh.triangles[i]) == null)
            {
                GameObject vertexCircle = Instantiate(circle, circle.transform.position, circle.transform.rotation, circle.transform.parent);
                vertexCircle.name = "Vertex" + mesh.triangles[i];
                vertexCircle.transform.localPosition = new Vector3(mesh.vertices[mesh.triangles[i]].x, mesh.vertices[mesh.triangles[i]].y, mesh.vertices[mesh.triangles[i]].z + round_z);
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
