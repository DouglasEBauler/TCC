using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using SFB;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.Text;
using UnityEngine.UI;

public class ExportScript : MonoBehaviour
{
    [SerializeField]
    GameObject contentRender;

#if UNITY_WEBGL && !UNITY_EDITOR
    // WebGL
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);
#endif

    void OnMouseDown()
    {
        Util_VisEdu.EnableColliderFabricaPecas(false, false);
        Util_VisEdu.EnableGOAmbienteGrafico(false);
        Util_VisEdu.EnableGOVisualizador(false);
        try
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        var bytes = Encoding.UTF8.GetBytes(GenerateProject());
        DownloadFile(gameObject.name, string.Empty, Consts.INITIAL_FILE_PROJECT, bytes, bytes.Length);
#else
            FileBrowser.AllFilesFilterText = "*.json";
            FileBrowser.ShowSaveDialog(
                onSuccess: (path) => { GenerateProject(); },
                onCancel: null,
                pickMode: FileBrowser.PickMode.Files,
                allowMultiSelection: false,
                initialPath: null,
                initialFilename: Consts.INITIAL_FILE_PROJECT,
                title: Consts.TITLE_SELECT_EXPORT_DIRECTORY,
                saveButtonText: Consts.BTN_SELECT_PROJECT
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
    string GenerateProject()
#else
    void GenerateProject()
#endif
    {
        var project = new ProjectVisEduClass();

        foreach (Transform child in contentRender.transform)
        {
            if (child.name.Contains(Consts.SLOT_CAM))
            {
                project.Camera.Propriedades = CreatePropCamera(project);
            }
            else if (child.name.Contains(Consts.SLOT_OBJ_GRAFICO))
            {
                ObjetoGraficoProject objGrafPeca = CreateObjGraf(project, child);

                if (objGrafPeca != null)
                {
                    project.ObjetosGraficos.Add(objGrafPeca);
                }
            }
        }
#if UNITY_WEBGL && !UNITY_EDITOR
    return JsonUtility.ToJson(project, true);
#else
        using (var streamWriter = new StreamWriter(new FileStream(FileBrowser.Result[0].ToString(), FileMode.OpenOrCreate)))
        {
            streamWriter.Write(JsonUtility.ToJson(project, true));
        }
#endif
    }

    ObjetoGraficoProject CreateObjGraf(ProjectVisEduClass project, Transform obgGrafSlot)
    {
        Transform objSlot = null;

        foreach (Transform child in obgGrafSlot.transform)
        {
            if (child.name.Contains(Consts.OBJ_GRAFICO_SLOT))
            {
                objSlot = child;
                break;
            }
        }

        if (objSlot != null)
        {
            string key = GetPeca(objSlot.name);
            if (!string.Empty.Equals(key) && Global.propriedadePecas.ContainsKey(key))
            {
                ObjetoGraficoProject objGrafPeca = new ObjetoGraficoProject();
                objGrafPeca.Propriedades = Global.propriedadePecas[key];

                objGrafPeca.Cubo = CreateCuboProject(objSlot);
                objGrafPeca.Poligono = CreatePoligonoProject(objSlot);
                objGrafPeca.Spline = CreateSplineProject(objSlot);

                return objGrafPeca;
            }
        }

        return null;
    }

    List<TransformacaoProject> CreateTransformacoes(Transform obgGrafSlot)
    {
        string numSlot = Util_VisEdu.GetNumSlot(obgGrafSlot.name);
        List<TransformacaoProject> listTransf = new List<TransformacaoProject>();

        foreach (Transform child in contentRender.transform)
        {
            if (child.name.Contains(Consts.SLOT_TRANSF + numSlot))
            {
                Transform transfSlot = null;

                foreach (Transform _child in child)
                {
                    if (_child.name.Contains(Consts.TRANSF_SLOT))
                    {
                        transfSlot = _child;
                        break;
                    }
                }

                if (transfSlot != null)
                {
                    string numTransfSlot = Util_VisEdu.GetNumSlot(transfSlot.name, true);

                    Transform transformacao = transfSlot.transform.Find(Consts.ESCALAR + numTransfSlot);
                    if (transformacao != null && Global.propriedadePecas.ContainsKey(transformacao.name))
                    {
                        listTransf.Add(CreateTransfProject(transformacao, Global.propriedadePecas[transformacao.name] as TransformacaoPropriedadePeca));
                    }
                    else
                    {
                        transformacao = transfSlot.transform.Find(Consts.ROTACIONAR + numTransfSlot);
                        if (transformacao != null && Global.propriedadePecas.ContainsKey(transformacao.name))
                        {
                            listTransf.Add(CreateTransfProject(transformacao, Global.propriedadePecas[transformacao.name] as TransformacaoPropriedadePeca));
                        }
                        else
                        {
                            transformacao = transfSlot.transform.Find(Consts.TRANSLADAR + numTransfSlot);
                            if (transformacao != null && Global.propriedadePecas.ContainsKey(transformacao.name))
                            {
                                listTransf.Add(CreateTransfProject(transformacao, Global.propriedadePecas[transformacao.name] as TransformacaoPropriedadePeca));
                            }
                        }
                    }
                }
            }
        }

        return listTransf.Count > 0 ? listTransf : null;
    }

    CuboProject CreateCuboProject(Transform obgGrafSlot)
    {
        string numSlot = Util_VisEdu.GetNumSlot(obgGrafSlot.name);

        GameObject slotObjFormaExport = GameObject.Find(Consts.SLOT_FORMA + numSlot);
        if (slotObjFormaExport != null)
        {
            Transform formaSlot = slotObjFormaExport.transform.Find(Consts.FORMA_SLOT + numSlot);

            if (formaSlot != null)
            {
                Transform forma = formaSlot.transform.Find(Consts.CUBO + numSlot);
                if (forma != null && Global.propriedadePecas.ContainsKey(forma.name))
                {
                    CuboProject formaProj = CreateFormCubo(Global.propriedadePecas[forma.name] as CuboPropriedadePeca);

                    if (formaProj != null)
                    {
                        formaProj.Transformacoes = CreateTransformacoes(obgGrafSlot);
                        if (Global.propriedadePecas.ContainsKey(Consts.ILUMINACAO + numSlot))
                        {
                            formaProj.Iluminacao = CreateIluminacao(Global.propriedadePecas[Consts.ILUMINACAO + numSlot] as IluminacaoPropriedadePeca);
                        }
                        return formaProj;
                    }
                }
            }
        }

        return null;
    }

    PoligonoProject CreatePoligonoProject(Transform obgGrafSlot)
    {
        string numSlot = Util_VisEdu.GetNumSlot(obgGrafSlot.name);

        GameObject slotObjFormaExport = GameObject.Find(Consts.SLOT_FORMA + numSlot);
        if (slotObjFormaExport != null)
        {
            Transform formaSlot = slotObjFormaExport.transform.Find(Consts.FORMA_SLOT + numSlot);

            if (formaSlot != null)
            {
                Transform forma = formaSlot.transform.Find(Consts.POLIGONO + numSlot);
                if (forma != null && Global.propriedadePecas.ContainsKey(forma.name))
                {
                    PoligonoProject formaProj = CreateFormPoligono(Global.propriedadePecas[forma.name] as PoligonoPropriedadePeca);

                    if (formaProj != null)
                    {
                        formaProj.Transformacoes = CreateTransformacoes(obgGrafSlot);
                        formaProj.Iluminacao = CreateIluminacao(Global.propriedadePecas[Consts.ILUMINACAO + numSlot] as IluminacaoPropriedadePeca);
                        return formaProj;
                    }
                }
            }
        }

        return null;
    }

    SplineProject CreateSplineProject(Transform obgGrafSlot)
    {
        string numSlot = Util_VisEdu.GetNumSlot(obgGrafSlot.name);

        GameObject slotObjFormaExport = GameObject.Find(Consts.SLOT_FORMA + numSlot);
        if (slotObjFormaExport != null)
        {
            Transform formaSlot = slotObjFormaExport.transform.Find(Consts.FORMA_SLOT + numSlot);

            if (formaSlot != null)
            {
                Transform forma = formaSlot.transform.Find(Consts.SPLINE + numSlot);
                if (forma != null && Global.propriedadePecas.ContainsKey(forma.name))
                {
                    SplineProject formaProj = CreateFormSpline(Global.propriedadePecas[forma.name] as SplinePropriedadePeca);

                    if (formaProj != null)
                    {
                        formaProj.Transformacoes = CreateTransformacoes(obgGrafSlot);
                        formaProj.Iluminacao = CreateIluminacao(Global.propriedadePecas[Consts.ILUMINACAO + numSlot] as IluminacaoPropriedadePeca);
                        return formaProj;
                    }
                }
            }
        }

        return null;
    }

    CuboProject CreateFormCubo(CuboPropriedadePeca propPeca)
    {
        return new CuboProject()
        {
            Propriedades = new CuboPropriedadePecaProject()
            {
                Nome = propPeca.Nome,
                Tam = new TamanhoProject()
                {
                    X = EncryptCuboPropPeca(propPeca, Property.TamX),
                    Y = EncryptCuboPropPeca(propPeca, Property.TamY),
                    Z = EncryptCuboPropPeca(propPeca, Property.TamZ)
                },
                Pos = new PosicaoProject()
                {
                    X = EncryptCuboPropPeca(propPeca, Property.PosX),
                    Y = EncryptCuboPropPeca(propPeca, Property.PosY),
                    Z = EncryptCuboPropPeca(propPeca, Property.PosZ)
                },
                NomeCuboAmbiente = propPeca.NomeCuboAmb,
                NomeCuboVis = propPeca.NomeCuboVis,
                Cor = propPeca.Cor,
                Textura = propPeca.Textura,
                Ativo = propPeca.Ativo
            }
        };
    }

    PoligonoProject CreateFormPoligono(PoligonoPropriedadePeca propPeca)
    {
        return new PoligonoProject()
        {
            Propriedades = new PoligonoPropriedadePecaProject()
            {
                Pos = new PosicaoProject()
                {
                    X = EncryptPoligonoPropPeca(propPeca, Property.PosX),
                    Y = EncryptPoligonoPropPeca(propPeca, Property.PosY),
                    Z = EncryptPoligonoPropPeca(propPeca, Property.PosZ)
                },
                Pontos = EncryptPoligonoPropPeca(propPeca, Property.Pontos),
                Primitiva = propPeca.Primitiva,
                PoligonoAmb = propPeca.PoligonoAmb,
                PoligonoVis = propPeca.PoligonoAmb,
                Ativo = propPeca.Ativo
            }
        };
    }

    SplineProject CreateFormSpline(SplinePropriedadePeca propPeca)
    {
        return new SplineProject()
        {
            Propriedades = new SplinePropriedadePecaProject()
            {
                P1 = new PosicaoProject()
                {
                    X = EncryptSplinePropPeca(propPeca, Property.P1PosX),
                    Y = EncryptSplinePropPeca(propPeca, Property.P1PosY),
                    Z = EncryptSplinePropPeca(propPeca, Property.P1PosZ),
                },
                P2 = new PosicaoProject()
                {
                    X = EncryptSplinePropPeca(propPeca, Property.P2PosX),
                    Y = EncryptSplinePropPeca(propPeca, Property.P2PosY),
                    Z = EncryptSplinePropPeca(propPeca, Property.P2PosZ),
                },
                P3 = new PosicaoProject()
                {
                    X = EncryptSplinePropPeca(propPeca, Property.P3PosX),
                    Y = EncryptSplinePropPeca(propPeca, Property.P3PosY),
                    Z = EncryptSplinePropPeca(propPeca, Property.P3PosZ),
                },
                P4 = new PosicaoProject()
                {
                    X = EncryptSplinePropPeca(propPeca, Property.P4PosX),
                    Y = EncryptSplinePropPeca(propPeca, Property.P4PosY),
                    Z = EncryptSplinePropPeca(propPeca, Property.P4PosZ),
                },
                P5 = new PosicaoProject()
                {
                    X = EncryptSplinePropPeca(propPeca, Property.P5PosX),
                    Y = EncryptSplinePropPeca(propPeca, Property.P5PosY),
                    Z = EncryptSplinePropPeca(propPeca, Property.P5PosZ),
                },
                QuantidadePontos = EncryptSplinePropPeca(propPeca, Property.QuantidadePontos),
                SplineAmb = propPeca.SplineAmb,
                SplineVis = propPeca.SplineVis
            }
        };
    }

    IluminacaoProject CreateIluminacao(IluminacaoPropriedadePeca propPeca)
    {
        return new IluminacaoProject()
        {
            Propriedades = new PropriedadeIluminacaoPecaProject()
            {
                Nome = propPeca.Nome,
                Cor = propPeca.Cor,
                Pos = new PosicaoProject()
                {
                    X = EncryptIluminacaoPropPeca(propPeca, Property.PosX),
                    Y = EncryptIluminacaoPropPeca(propPeca, Property.PosX),
                    Z = EncryptIluminacaoPropPeca(propPeca, Property.PosX)
                },
                ValorIluminacao = new ValorIluminacaoProject()
                {
                    X = EncryptIluminacaoPropPeca(propPeca, Property.ValorX),
                    Y = EncryptIluminacaoPropPeca(propPeca, Property.ValorY),
                    Z = EncryptIluminacaoPropPeca(propPeca, Property.ValorZ)
                },
                Intensidade = EncryptIluminacaoPropPeca(propPeca, Property.Intensidade),
                Distancia = EncryptIluminacaoPropPeca(propPeca, Property.Distancia),
                Angulo = EncryptIluminacaoPropPeca(propPeca, Property.Angulo),
                Expoente = EncryptIluminacaoPropPeca(propPeca, Property.Expoente),
                TipoLuz = propPeca.TipoLuz,
                Ativo = propPeca.Ativo
            }
        };
    }

    PropriedadeCameraProject CreatePropCamera(ProjectVisEduClass project)
    {
        return new PropriedadeCameraProject()
        {
            Nome = Global.propCameraGlobal.Nome,
            PosX = EncryptPropCam(Property.PosX),
            PosY = EncryptPropCam(Property.PosY),
            PosZ = EncryptPropCam(Property.PosZ),
            FOV = EncryptPropCam(Property.FOV),
            CameraAtiva = Global.propCameraGlobal.CameraAtiva,
            JaIniciouValores = Global.propCameraGlobal.JaIniciouValores,
            ExisteCamera = Global.propCameraGlobal.ExisteCamera,
        };
    }

    TransformacaoProject CreateTransfProject(Transform transformacao, TransformacaoPropriedadePeca propPeca)
    {
        TransformacaoProject transfProj = null;

        if (transformacao.name.Contains(Consts.ESCALAR)) transfProj = new TransformacaoProject(TypeTransform.Escalar);
        else if (transformacao.name.Contains(Consts.ROTACIONAR)) transfProj = new TransformacaoProject(TypeTransform.Rotacionar);
        else if (transformacao.name.Contains(Consts.TRANSLADAR)) transfProj = new TransformacaoProject(TypeTransform.Transladar);

        if (transfProj != null)
        {
            transfProj.Propriedades = CreateTransformacaoPropPeca(propPeca);

            GameObject iteracao = GameObject.Find(Consts.ITERACAO + Util_VisEdu.GetNumSlot(transformacao.transform.parent.name, true));

            if (iteracao != null)
            {
                transfProj.Iteracao = CreateIteracaoProject(transfProj, Global.propriedadePecas[iteracao.name] as IteracaoPropriedadePeca);
            }

            return transfProj;
        }

        return null;
    }

    IteracaoProject CreateIteracaoProject(TransformacaoProject transformacaoProj, IteracaoPropriedadePeca propPeca)
    {
        return new IteracaoProject()
        {
            NomeTransformacao = transformacaoProj.Propriedades.Nome,
            Propriedades = CreateIteracaoPropPeca(propPeca)
        };
    }

    IteracaoPropriedadePecaProject CreateIteracaoPropPeca(IteracaoPropriedadePeca propPeca)
    {
        return new IteracaoPropriedadePecaProject()
        {
            Intervalo = new PosicaoProject()
            {
                X = EncryptIteracaoPropPeca(propPeca, Property.IntervaloX),
                Y = EncryptIteracaoPropPeca(propPeca, Property.IntervaloY),
                Z = EncryptIteracaoPropPeca(propPeca, Property.IntervaloZ)
            },
            Min = new PosicaoProject()
            {
                X = EncryptIteracaoPropPeca(propPeca, Property.MinX),
                Y = EncryptIteracaoPropPeca(propPeca, Property.MinY),
                Z = EncryptIteracaoPropPeca(propPeca, Property.MinZ)
            },
            Max = new PosicaoProject()
            {
                X = EncryptIteracaoPropPeca(propPeca, Property.MaxX),
                Y = EncryptIteracaoPropPeca(propPeca, Property.MaxY),
                Z = EncryptIteracaoPropPeca(propPeca, Property.MaxZ)
            }
        };
    }

    string EncryptIteracaoPropPeca(IteracaoPropriedadePeca propPeca, Property propType)
    {
        switch (propType)
        {
            case Property.IntervaloX: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.X.ToString();
            case Property.IntervaloY: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.Y.ToString();
            case Property.IntervaloZ: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.Z.ToString();

            case Property.MinX: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.X.ToString();
            case Property.MinY: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.Y.ToString();
            case Property.MinZ: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.Z.ToString();

            case Property.MaxX: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.X.ToString();
            case Property.MaxY: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.Y.ToString();
            case Property.MaxZ: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intervalo.Z.ToString();
            default: return string.Empty;
        }
    }

    string EncryptIluminacaoPropPeca(IluminacaoPropriedadePeca propPeca, Property propType)
    {
        switch (propType)
        {
            case Property.PosX: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Pos.X.ToString();
            case Property.PosY: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Pos.Y.ToString();
            case Property.PosZ: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Pos.Z.ToString();

            case Property.ValorX: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.ValorIluminacao.X.ToString();
            case Property.ValorY: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.ValorIluminacao.Y.ToString();
            case Property.ValorZ: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.ValorIluminacao.Z.ToString();

            case Property.Intensidade: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Intensidade.ToString();
            case Property.Distancia: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Distancia.ToString();
            case Property.Angulo: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Angulo.ToString();
            case Property.Expoente: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Expoente.ToString();
            default: return string.Empty;
        }
    }

    TransformacaoPropriedadePecaProject CreateTransformacaoPropPeca(TransformacaoPropriedadePeca propPeca)
    {
        return new TransformacaoPropriedadePecaProject()
        {
            Nome = propPeca.Nome,
            Pos = new PosicaoProject()
            {
                X = EncryptTransformacaoPropPeca(propPeca, Property.PosX),
                Y = EncryptTransformacaoPropPeca(propPeca, Property.PosY),
                Z = EncryptTransformacaoPropPeca(propPeca, Property.PosZ)
            },
            Ativo = propPeca.Ativo
        };
    }

    string EncryptTransformacaoPropPeca(TransformacaoPropriedadePeca propPeca, Property propType)
    {
        switch (propType)
        {
            case Property.PosX: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Pos.X.ToString();
            case Property.PosY: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Pos.Y.ToString();
            case Property.PosZ: return (propPeca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks[propType]) : propPeca.Pos.Z.ToString();
            default: return string.Empty;
        }
    }

    string EncryptPropCam(Property propType)
    {
        PropriedadeCamera propCam = Global.propCameraGlobal;

        switch (propType)
        {
            case Property.PosX: return (propCam.ListPropCamLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propCam.ListPropCamLocks[propType]) : propCam.PosX.ToString();
            case Property.PosY: return (propCam.ListPropCamLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propCam.ListPropCamLocks[propType]) : propCam.PosY.ToString();
            case Property.PosZ: return (propCam.ListPropCamLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propCam.ListPropCamLocks[propType]) : propCam.PosZ.ToString();
            case Property.FOV: return (propCam.ListPropCamLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(propCam.ListPropCamLocks[propType]) : propCam.FOV.ToString();
            default: return string.Empty;
        }
    }

    string EncryptCuboPropPeca(CuboPropriedadePeca peca, Property propType)
    {
        switch (propType)
        {
            case Property.PosX: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Pos.X.ToString();
            case Property.PosY: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Pos.Y.ToString();
            case Property.PosZ: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Pos.Z.ToString();
            case Property.TamX: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Tam.X.ToString();
            case Property.TamY: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Tam.Y.ToString();
            case Property.TamZ: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Tam.Z.ToString();
            default: return string.Empty;
        }
    }
    string EncryptPoligonoPropPeca(PoligonoPropriedadePeca peca, Property propType)
    {
        switch (propType)
        {
            case Property.Pontos: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Pontos.ToString();
            case Property.PosX: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Pos.X.ToString();
            case Property.PosY: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Pos.Y.ToString();
            case Property.PosZ: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.Pos.Z.ToString();
            default: return string.Empty;
        }
    }

    string EncryptSplinePropPeca(SplinePropriedadePeca peca, Property propType)
    {
        switch (propType)
        {
            case Property.P1PosX: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P1.X.ToString();
            case Property.P1PosY: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P1.Y.ToString();
            case Property.P1PosZ: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P1.Z.ToString();

            case Property.P2PosX: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P2.X.ToString();
            case Property.P2PosY: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P2.Y.ToString();
            case Property.P2PosZ: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P2.Z.ToString();

            case Property.P3PosX: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P3.X.ToString();
            case Property.P3PosY: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P3.Y.ToString();
            case Property.P3PosZ: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P3.Z.ToString();

            case Property.P4PosX: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P4.X.ToString();
            case Property.P4PosY: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P4.Y.ToString();
            case Property.P4PosZ: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P4.Z.ToString();

            case Property.P5PosX: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P5.X.ToString();
            case Property.P5PosY: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P5.Y.ToString();
            case Property.P5PosZ: return (peca.ListPropLocks.ContainsKey(propType)) ? Util_VisEdu.Base64Encode(peca.ListPropLocks[propType]) : peca.P5.Z.ToString();

            default: return string.Empty;
        }
    }

    string GetPeca(string slot)
    {
        foreach (var peca in Global.listaEncaixes)
        {
            if (peca.Value.Equals(slot)) return peca.Key;
        }

        return string.Empty;
    }
}