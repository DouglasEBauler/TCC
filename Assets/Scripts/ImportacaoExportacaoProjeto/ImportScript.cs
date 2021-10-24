using System.Collections.Generic;
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
            cameraPScript.ConfiguraPropriedadePeca(null, CreateCameraProp(project.Camera.Propriedades));
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
                    cuboScript.ConfiguraPropriedadePeca(CreateCuboPeca((CuboPropriedadePecaProject)obj.Forma.Propriedades));
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
                    iluminacaoScript.ConfiguraPropriedadePeca(CreatePeca(obj.Iluminacao.Propriedades));
                }

                foreach (var transformacao in obj.Transformacoes)
                {
                    ConfigPeca(transladarScript);
                    transladarScript.AddIluminacao();
                    transladar.GetComponent<BoxCollider>().enabled = true;
                    transladarScript.ConfiguraPropriedadePeca(CreatePeca(transformacao.Propriedades));
                }
            }
        }
    }

    PropriedadeCamera CreateCameraProp(PropriedadeCameraProject propProj)
    {
        return new PropriedadeCamera()
        {
            Nome = propProj.Nome,
            PosX = DecodeField(propProj.PosX),
            PosY = DecodeField(propProj.PosY),
            PosZ = DecodeField(propProj.PosZ),
            FOV = DecodeField(propProj.FOV),
            CameraAtiva = propProj.CameraAtiva,
            JaIniciouValores = propProj.JaIniciouValores,
            ExisteCamera = propProj.ExisteCamera,
            ListPropCamLocks = new Dictionary<string, string>()
        };
    }

    CuboPropriedadePeca CreateCuboPeca(CuboPropriedadePecaProject cuboPropPecaProj)
    {
        return new CuboPropriedadePeca()
        {
            Nome = cuboPropPecaProj.Nome,
            Tam = new Tamanho()
            {
                X = DecodeField(cuboPropPecaProj.Tam.X),
                Y = DecodeField(cuboPropPecaProj.Tam.Y),
                Z = DecodeField(cuboPropPecaProj.Tam.Z)
            },
            Pos = new Posicao()
            {
                X = DecodeField(cuboPropPecaProj.Pos.X),
                Y = DecodeField(cuboPropPecaProj.Pos.Y),
                Z = DecodeField(cuboPropPecaProj.Pos.Z)
            },
            Ativo = cuboPropPecaProj.Ativo,
            Cor = cuboPropPecaProj.Cor,
            Textura = cuboPropPecaProj.Textura,
            ListPropLocks = new Dictionary<string, string>(),
            TipoLuz = cuboPropPecaProj.TipoLuz,
            Intensidade = DecodeField(cuboPropPecaProj.Intensidade),
            ValorIluminacao = new ValorIluminacao()
            {
                X = DecodeField(cuboPropPecaProj.ValorIluminacao.X),
                Y = DecodeField(cuboPropPecaProj.ValorIluminacao.Y),
                Z = DecodeField(cuboPropPecaProj.ValorIluminacao.Z)
            },
            Distancia = DecodeField(cuboPropPecaProj.Distancia),
            Angulo = DecodeField(cuboPropPecaProj.Angulo),
            Expoente = DecodeField(cuboPropPecaProj.Expoente),
            UltimoIndexLuz = cuboPropPecaProj.UltimoIndexLuz
        };
    }

    PropriedadePeca CreatePeca(PropriedadePecaProject propPecaProj)
    {
        return new PropriedadePeca()
        {
            Nome = propPecaProj.Nome,
            Tam = new Tamanho()
            {
                X = DecodeField(propPecaProj.Tam.X),
                Y = DecodeField(propPecaProj.Tam.Y),
                Z = DecodeField(propPecaProj.Tam.Z)
            },
            Pos = new Posicao()
            {
                X = DecodeField(propPecaProj.Pos.X),
                Y = DecodeField(propPecaProj.Pos.Y),
                Z = DecodeField(propPecaProj.Pos.Z)
            },
            Ativo = propPecaProj.Ativo,
            Cor = propPecaProj.Cor,
            Textura = propPecaProj.Textura,
            ListPropLocks = new Dictionary<string, string>(),
            TipoLuz = propPecaProj.TipoLuz,
            Intensidade = DecodeField(propPecaProj.Intensidade),
            ValorIluminacao = new ValorIluminacao()
            {
                X = DecodeField(propPecaProj.ValorIluminacao.X),
                Y = DecodeField(propPecaProj.ValorIluminacao.Y),
                Z = DecodeField(propPecaProj.ValorIluminacao.Z)
            },
            Distancia = DecodeField(propPecaProj.Distancia),
            Angulo = DecodeField(propPecaProj.Angulo),
            Expoente = DecodeField(propPecaProj.Expoente),
            UltimoIndexLuz = propPecaProj.UltimoIndexLuz
        };
    }

    

    float DecodeField(string valueField)
    {
        float value;
        return float.TryParse(valueField, out value) ? Util_VisEdu.ConvertField(valueField) : Util_VisEdu.ConvertField(Util_VisEdu.Base64Decode(valueField));
    }

    public void ConfigPeca(Controller controller)
    {
        Global.AtualizaListaSlot();
        controller.GeraCopiaPeca();
        controller.ConfiguraColliderDestino();
        controller.PodeEncaixar();
    }
}
