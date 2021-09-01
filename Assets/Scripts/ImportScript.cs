using System.IO;
using UnityEditor;
using UnityEngine;

public class ImportScript : MonoBehaviour
{
    [SerializeField]
    GameObject cameraP;
    [SerializeField]
    GameObject objetoGraficoP;
    [SerializeField]
    GameObject cubo;
    [SerializeField]
    GameObject poligono;
    [SerializeField]
    GameObject spline;
    [SerializeField]
    GameObject transladar;
    [SerializeField]
    GameObject iluminacao;

    Controller cameraPScript;
    Controller objetoGraficoPScript;
    Controller cuboScript;
    Controller poligonoScript;
    Controller splineScript;
    Controller transladarScript;
    Controller iluminacaoScript;

    void Start()
    {
        cameraPScript = cameraP.GetComponent<Controller>();
        objetoGraficoPScript = objetoGraficoP.GetComponent<Controller>();
        cuboScript = cubo.GetComponent<Controller>();
        poligonoScript = poligono.GetComponent<Controller>();
        splineScript = spline.GetComponent<Controller>();
        transladarScript = transladar.GetComponent<Controller>();
        iluminacaoScript = iluminacao.GetComponent<Controller>();
    }

    void OnMouseDown()
    {
        string path = EditorUtility.OpenFilePanel("Selecione o projeto a ser importado", "", "json");

        if (path.Length != 0)
        {
            string projectJson = string.Empty;

            using (var streamReader = new StreamReader(path))
            {
                projectJson = streamReader.ReadToEnd();
            }

            if (!string.Empty.Equals(projectJson))
            {
                ProjectVisEduClass projectVisEdu = JsonUtility.FromJson<ProjectVisEduClass>(projectJson);

                ImportProject(projectVisEdu);
            }
        }
    }

    void ImportProject(ProjectVisEduClass project)
    {
        if (project.Camera != null)
        {
            ConfigPeca(cameraPScript);
            cameraPScript.AddCamera();
            cameraP.GetComponent<BoxCollider>().enabled = true;
            cameraPScript.ConfiguraPropriedadePeca(null, project.Camera.Propriedades);
        }

        if (project.ObjetosGraficos?.Count > 0)
        {
            foreach (var obj in project.ObjetosGraficos)
            {
                ConfigPeca(objetoGraficoPScript);
                objetoGraficoPScript.AddObjGrafico();
                objetoGraficoP.GetComponent<BoxCollider>().enabled = true;
                objetoGraficoPScript.ConfiguraPropriedadePeca(obj.Propriedades);

                if (obj.Forma.Propriedades.Nome.Contains("Cubo"))
                {
                    ConfigPeca(cuboScript);
                    cuboScript.AddCuboTransform();
                    cubo.GetComponent<BoxCollider>().enabled = true;
                    cuboScript.ConfiguraPropriedadePeca(obj.Forma.Propriedades);
                }
                else if (obj.Forma.Propriedades.Nome.Contains("Poligono"))
                {
                    // Add futuramente peca Poligono
                }
                else if (obj.Forma.Propriedades.Nome.Contains("Spline"))
                {
                    // Add futuramente peca Spline
                }

                if (obj.Iluminacao.Propriedades.Nome.Contains("Iluminacao"))
                {
                    ConfigPeca(iluminacaoScript);
                    iluminacaoScript.AddIluminacao();
                    iluminacao.GetComponent<BoxCollider>().enabled = true;
                    iluminacaoScript.ConfiguraPropriedadePeca(obj.Iluminacao.Propriedades);
                }

                foreach (var transformacao in obj.Transformacoes)
                {
                    ConfigPeca(transladarScript);
                    transladarScript.AddIluminacao();
                    transladar.GetComponent<BoxCollider>().enabled = true;
                    transladarScript.ConfiguraPropriedadePeca(transformacao.Propriedades);
                }
            }
        }
    }

    public void ConfigPeca(Controller controller)
    {
        Global.atualizaListaSlot();
        controller.GeraCopiaPeca();
        controller.ConfiguraColliderDestino();
        controller.PodeEncaixar();
    }
}
