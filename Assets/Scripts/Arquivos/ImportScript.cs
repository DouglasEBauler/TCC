using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

public class ImportScript : MonoBehaviour
{
    [SerializeField]
    CameraPScript cameraPScript;
    [SerializeField]
    ObjetoGraficoScript objetoGraficoPScript;
    [SerializeField]
    CuboScript cuboScript;
    [SerializeField]
    PoligonoScript poligonoScript;
    [SerializeField]
    SplineScript splineScript;
    [SerializeField]
    TransformacaoScript rotacionarScript;
    [SerializeField]
    TransformacaoScript transladarScript;
    [SerializeField]
    TransformacaoScript escalarScript;
    [SerializeField]
    IteracaoScript iteracaoScript;
    [SerializeField]
    IluminacaoScript iluminacaoScript;

#if UNITY_WEBGL && !UNITY_EDITOR
    // WebGL
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);
#endif

    void OnMouseDown()
    {
        Util_VisEdu.EnableColliderFabricaPecas(false, false);
        Util_VisEdu.EnableGOAmbienteGrafico(false);
        Util_VisEdu.EnableGOVisualizador(false);
        try
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            UploadFile(gameObject.name, "OnFileUpload", ".json", false);
#else
            FileBrowser.AllFilesFilterText = "*.json";
            FileBrowser.ShowLoadDialog(
                onSuccess: (path) => { ImportProject(path[0].ToString()); },
                onCancel: null,
                pickMode: FileBrowser.PickMode.Files,
                allowMultiSelection: false,
                initialPath: null,
                title: Consts.TITLE_SELECT_IMPORT_DIRECTORY,
                loadButtonText: Consts.BTN_SELECT_PROJECT
            );
#endif
        }
        finally
        {
            Util_VisEdu.EnableColliderFabricaPecas(true);
            Util_VisEdu.EnableGOAmbienteGrafico(true);
            Util_VisEdu.EnableGOVisualizador(true);
        }
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    public void OnFileUpload(string url)
    {
        StartCoroutine(OutputRoutine(url));
    }
#endif

    void ImportProject(string path)
    {
        string projectJson = string.Empty;

#if UNITY_WEBGL && !UNITY_EDITOR
        projectJson = path;
#else
        using (var streamReader = new StreamReader(path))
        {
            projectJson = streamReader.ReadToEnd();
        }
#endif
        if (!string.Empty.Equals(projectJson))
        {
            ProjectVisEduClass project = JsonUtility.FromJson<ProjectVisEduClass>(projectJson);

            if (project.Camera != null)
            {
                cameraPScript.gameObject.GetComponent<BoxCollider>().enabled = false;
                try
                {
                    cameraPScript.CopiaPeca();
                    cameraPScript.slot = GameObject.Find(Consts.CAMERA_SLOT);
                    cameraPScript.transform.position =
                        new Vector3(cameraPScript.slot.transform.position.x, cameraPScript.slot.transform.position.y, cameraPScript.slot.transform.position.z);
                    cameraPScript.PodeEncaixar();
                    cameraPScript.AddCamera(CreateCameraProp(project.Camera.Propriedades));
                }
                finally
                {
                    cameraPScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }

            if (project.ObjetosGraficos?.Count > 0)
            {
                foreach (var obj in project.ObjetosGraficos)
                {
                    objetoGraficoPScript.GetComponent<BoxCollider>().enabled = false;
                    try
                    {
                        objetoGraficoPScript.CopiaPeca();
                        objetoGraficoPScript.slot = GameObject.Find(Consts.OBJ_GRAFICO_SLOT);
                        objetoGraficoPScript.transform.position =
                            new Vector3(objetoGraficoPScript.slot.transform.position.x, objetoGraficoPScript.slot.transform.position.y, objetoGraficoPScript.slot.transform.position.z);
                        objetoGraficoPScript.PodeEncaixar();
                        objetoGraficoPScript.AddObjGrafico(obj.Propriedades);

                        if (obj.Cubo != null && !obj.Cubo.Propriedades.Nome.Equals(string.Empty))
                        {
                            cuboScript.gameObject.GetComponent<BoxCollider>().enabled = false;
                            try
                            {
                                cuboScript.CopiaPeca();
                                cuboScript.slot = GameObject.Find(Consts.FORMA_SLOT + Util_VisEdu.GetNumSlot(objetoGraficoPScript.slot.name));
                                cuboScript.transform.position =
                                    new Vector3(cuboScript.slot.transform.position.x, cuboScript.slot.transform.position.y, cuboScript.slot.transform.position.z);
                                cuboScript.PodeEncaixar();
                                cuboScript.AddCubo(CreateCubo(obj.Cubo.Propriedades));

                                if (obj.Cubo.Transformacoes != null)
                                {
                                    ImportTransformacoes(obj.Cubo.Transformacoes);
                                }

                                if (obj.Cubo.Iluminacao != null && obj.Cubo.Iluminacao.Propriedades.Nome.Contains(Consts.ILUMINACAO))
                                {
                                    iluminacaoScript.AddIluminacao(true);
                                    iluminacaoScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                                    iluminacaoScript.ConfiguraPropriedadePeca(CreatePecaIluminacao(obj.Cubo.Iluminacao.Propriedades));
                                }
                            }
                            finally
                            {
                                cuboScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                            }
                        }
                        else if (obj.Poligono != null && !obj.Poligono.Propriedades.Nome.Equals(string.Empty))
                        {
                            poligonoScript.gameObject.GetComponent<BoxCollider>().enabled = false;
                            try
                            {
                                poligonoScript.CopiaPeca();
                                poligonoScript.slot = GameObject.Find(Consts.FORMA_SLOT + Util_VisEdu.GetNumSlot(objetoGraficoPScript.slot.name));
                                poligonoScript.transform.position =
                                    new Vector3(poligonoScript.slot.transform.position.x, poligonoScript.slot.transform.position.y, poligonoScript.slot.transform.position.z);
                                poligonoScript.PodeEncaixar();
                                poligonoScript.AddPoligono(CreatePoligono(obj.Poligono.Propriedades));

                                if (obj.Poligono.Transformacoes != null)
                                {
                                    ImportTransformacoes(obj.Poligono.Transformacoes);
                                }

                                if (obj.Poligono.Iluminacao != null && obj.Poligono.Iluminacao.Propriedades.Nome.Contains(Consts.ILUMINACAO))
                                {
                                    iluminacaoScript.AddIluminacao();
                                    iluminacaoScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                                    iluminacaoScript.ConfiguraPropriedadePeca(CreatePecaIluminacao(obj.Poligono.Iluminacao.Propriedades));
                                }
                            }
                            finally
                            {
                                poligonoScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                            }
                        }
                        else if (obj.Spline != null && !obj.Spline.Propriedades.Nome.Equals(string.Empty))
                        {
                            splineScript.gameObject.GetComponent<BoxCollider>().enabled = false;
                            try
                            {
                                splineScript.CopiaPeca();
                                splineScript.slot = GameObject.Find(Consts.FORMA_SLOT + Util_VisEdu.GetNumSlot(objetoGraficoPScript.slot.name));
                                splineScript.transform.position =
                                    new Vector3(splineScript.slot.transform.position.x, splineScript.slot.transform.position.y, splineScript.slot.transform.position.z);
                                splineScript.PodeEncaixar();
                                splineScript.AddSpline(CreateSpline(obj.Spline.Propriedades));

                                if (obj.Spline.Transformacoes != null)
                                {
                                    ImportTransformacoes(obj.Spline.Transformacoes);
                                }

                                if (obj.Spline.Iluminacao != null && obj.Spline.Iluminacao.Propriedades.Nome.Contains(Consts.ILUMINACAO))
                                {
                                    iluminacaoScript.AddIluminacao();
                                    iluminacaoScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                                    iluminacaoScript.ConfiguraPropriedadePeca(CreatePecaIluminacao(obj.Spline.Iluminacao.Propriedades));
                                }
                            }
                            finally
                            {
                                splineScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                            }
                        }

                    }
                    finally
                    {
                        objetoGraficoPScript.GetComponent<BoxCollider>().enabled = true;
                    }
                }
            }
        }
    }

    void ImportTransformacoes(List<TransformacaoProject> transformacoes)
    {
        foreach (TransformacaoProject transformacao in transformacoes)
        {
            TransformacaoScript transformacaoScript = null;

            if (transformacao.Propriedades.Nome.Contains(Consts.ROTACIONAR))
            {
                transformacaoScript = rotacionarScript;
                transformacaoScript.gameObject.name = Consts.ROTACIONAR;
            }
            else if (transformacao.Propriedades.Nome.Contains(Consts.ESCALAR))
            {
                transformacaoScript = escalarScript;
                transformacaoScript.gameObject.name = Consts.ESCALAR;
            }
            else if (transformacao.Propriedades.Nome.Contains(Consts.TRANSLADAR))
            {
                transformacaoScript = transladarScript;
                transformacaoScript.gameObject.name = Consts.TRANSLADAR;
            }

            if (transformacaoScript != null)
            {
                transformacaoScript.gameObject.GetComponent<BoxCollider>().enabled = false;
                try
                {
                    transformacaoScript.CopiaPeca();
                    transformacaoScript.slot = GameObject.Find(Consts.TRANSF_SLOT + Util_VisEdu.GetNumSlot(objetoGraficoPScript.slot.name));
                    transformacaoScript.transform.position =
                        new Vector3(transformacaoScript.slot.transform.position.x, transformacaoScript.slot.transform.position.y, transformacaoScript.slot.transform.position.z);
                    transformacaoScript.PodeEncaixar();
                    transformacaoScript.AddTransformacao(CreatePropTransformacao(transformacao.Propriedades));

                    if (transformacao.Iteracao != null)
                    {
                        iteracaoScript.gameObject.GetComponent<BoxCollider>().enabled = false;
                        try
                        {
                            iteracaoScript.CopiaPeca();
                            iteracaoScript.slot = GameObject.Find(Consts.ITERACAO_SLOT + Util_VisEdu.GetNumSlot(transformacaoScript.slot.name, true));
                            iteracaoScript.transform.position =
                                new Vector3(iteracaoScript.slot.transform.position.x, iteracaoScript.slot.transform.position.y, iteracaoScript.slot.transform.position.z);
                            iteracaoScript.PodeEncaixar();
                            iteracaoScript.AddIteracao(CreatePropIteracao(transformacao.Iteracao.Propriedades));
                        }
                        finally
                        {
                            iteracaoScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                finally
                {
                    transformacaoScript.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
    }

    IteracaoPropriedadePeca CreatePropIteracao(IteracaoPropriedadePecaProject propriedades)
    {
        return new IteracaoPropriedadePeca()
        {
            Intervalo = new Posicao()
            {
                X = DecodeField(propriedades.Intervalo.X),
                Y = DecodeField(propriedades.Intervalo.Y),
                Z = DecodeField(propriedades.Intervalo.Z),
            },
            Min = new Posicao()
            {
                X = DecodeField(propriedades.Min.X),
                Y = DecodeField(propriedades.Min.Y),
                Z = DecodeField(propriedades.Min.Z),
            },
            Max = new Posicao()
            {
                X = DecodeField(propriedades.Max.X),
                Y = DecodeField(propriedades.Max.Y),
                Z = DecodeField(propriedades.Max.Z),
            }
        };
    }

    TransformacaoPropriedadePeca CreatePropTransformacao(TransformacaoPropriedadePecaProject propriedades)
    {
        return new TransformacaoPropriedadePeca()
        {
            Nome = propriedades.Nome,
            Pos = new Posicao()
            {
                X = DecodeField(propriedades.Pos.X),
                Y = DecodeField(propriedades.Pos.Y),
                Z = DecodeField(propriedades.Pos.Z)
            },
            Ativo = propriedades.Ativo
        };
    }

    CuboPropriedadePeca CreateCubo(CuboPropriedadePecaProject propriedades)
    {
        return new CuboPropriedadePeca()
        {
            Nome = propriedades.Nome,
            Tam = new Tamanho()
            {
                X = DecodeField(propriedades.Tam.X),
                Y = DecodeField(propriedades.Tam.Y),
                Z = DecodeField(propriedades.Tam.Z)
            },
            Pos = new Posicao()
            {
                X = DecodeField(propriedades.Pos.X),
                Y = DecodeField(propriedades.Pos.Y),
                Z = DecodeField(propriedades.Pos.Z)
            },
            Ativo = propriedades.Ativo,
            Cor = propriedades.Cor,
            Textura = propriedades.Textura
        };
    }

    PoligonoPropriedadePeca CreatePoligono(PoligonoPropriedadePecaProject propriedades)
    {
        return new PoligonoPropriedadePeca()
        {
            Pontos = Convert.ToInt32(DecodeField(propriedades.Pontos)),
            Primitiva = propriedades.Primitiva
        };
    }
    SplinePropriedadePeca CreateSpline(SplinePropriedadePecaProject propriedades)
    {
        return new SplinePropriedadePeca()
        {
            P1 = new Posicao()
            {
                X = DecodeField(propriedades.P1.X),
                Y = DecodeField(propriedades.P1.Y),
                Z = DecodeField(propriedades.P1.Z)
            },
            P2 = new Posicao()
            {
                X = DecodeField(propriedades.P2.X),
                Y = DecodeField(propriedades.P2.Y),
                Z = DecodeField(propriedades.P2.Z)
            },
            P3 = new Posicao()
            {
                X = DecodeField(propriedades.P3.X),
                Y = DecodeField(propriedades.P3.Y),
                Z = DecodeField(propriedades.P3.Z)
            },
            P4 = new Posicao()
            {
                X = DecodeField(propriedades.P4.X),
                Y = DecodeField(propriedades.P4.Y),
                Z = DecodeField(propriedades.P4.Z)
            },
            P5 = new Posicao()
            {
                X = DecodeField(propriedades.P5.X),
                Y = DecodeField(propriedades.P5.Y),
                Z = DecodeField(propriedades.P5.Z)
            },
            QuantidadePontos = DecodeFieldInt(propriedades.QuantidadePontos)
        };
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
            ExisteCamera = propProj.ExisteCamera
        };
    }

    IluminacaoPropriedadePeca CreatePecaIluminacao(PropriedadeIluminacaoPecaProject propIluminacaoPecaProj)
    {
        return new IluminacaoPropriedadePeca()
        {
            Nome = propIluminacaoPecaProj.Nome,
            Cor = propIluminacaoPecaProj.Cor,
            Pos = new Posicao()
            {
                X = DecodeField(propIluminacaoPecaProj.Pos.X),
                Y = DecodeField(propIluminacaoPecaProj.Pos.Y),
                Z = DecodeField(propIluminacaoPecaProj.Pos.Z)
            },
            ValorIluminacao = new ValorIluminacao()
            {
                X = DecodeField(propIluminacaoPecaProj.ValorIluminacao.X),
                Y = DecodeField(propIluminacaoPecaProj.ValorIluminacao.Y),
                Z = DecodeField(propIluminacaoPecaProj.ValorIluminacao.Z)
            },
            Intensidade = DecodeField(propIluminacaoPecaProj.Intensidade),
            Distancia = DecodeField(propIluminacaoPecaProj.Distancia),
            Angulo = DecodeField(propIluminacaoPecaProj.Angulo),
            Expoente = DecodeField(propIluminacaoPecaProj.Expoente),
            TipoLuz = propIluminacaoPecaProj.TipoLuz,
            Ativo = propIluminacaoPecaProj.Ativo
        };
    }

    float DecodeField(string valueField)
    {
        float value;
        return float.TryParse(valueField, out value) ? Util_VisEdu.ConvertField(valueField) : Util_VisEdu.ConvertField(Util_VisEdu.Base64Decode(valueField));
    }
    
    int DecodeFieldInt(string valueField)
    {
        int value;
        return int.TryParse(valueField, out value) ? Util_VisEdu.ConvertField(valueField) : Util_VisEdu.ConvertField(Util_VisEdu.Base64Decode(valueField));
    }


#if UNITY_WEBGL && !UNITY_EDITOR
    private IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);
        yield return loader;

        ImportProject(loader.text);
    }
#endif
}
