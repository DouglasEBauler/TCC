using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;

public class ExportScript : MonoBehaviour
{
    [SerializeField]
    GameObject render;

    void OnMouseDown()
    {
        Util_VisEdu.EnableColliderFabricaPecas(false, false);
        Util_VisEdu.EnableGOAmbienteGrafico(false);
        Util_VisEdu.EnableGOVisualizador(false);
        try
        {
            FileBrowser.AllFilesFilterText = "*.json";
            FileBrowser.ShowLoadDialog(
                onSuccess: (path) => { GenerateProject(); },
                onCancel: null,
                pickMode: FileBrowser.PickMode.Files,
                allowMultiSelection: false,
                initialPath: null,
                initialFilename: Consts.INITIAL_FILE_PROJECT,
                title: Consts.TITLE_SELECT_EXPORT_DIRECTORY,
                loadButtonText: Consts.BTN_SELECT_PROJECT
            );
        }
        finally
        {
            Util_VisEdu.EnableColliderFabricaPecas(true);
            Util_VisEdu.EnableGOAmbienteGrafico(true);
            Util_VisEdu.EnableGOVisualizador(true);
        }
    }

    void GenerateProject()
    {
        var project = new ProjectVisEduClass();

        foreach (Transform child in render.transform)
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

        using (var streamWriter = new StreamWriter(FileBrowser.Result[0].ToString()))
        {
            streamWriter.Write(JsonUtility.ToJson(project, true));
        }
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

        foreach (Transform child in render.transform)
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
                        listTransf.Add(CreateTransfProject(transformacao, Global.propriedadePecas[transformacao.name] as PropriedadeTransformacao));
                    }
                    else
                    {
                        transformacao = transfSlot.transform.Find(Consts.ROTACIONAR + numTransfSlot);
                        if (transformacao != null && Global.propriedadePecas.ContainsKey(transformacao.name))
                        {
                            listTransf.Add(CreateTransfProject(transformacao, Global.propriedadePecas[transformacao.name] as PropriedadeTransformacao));
                        }
                        else
                        {
                            transformacao = transfSlot.transform.Find(Consts.TRANSLADAR + numTransfSlot);
                            if (transformacao != null && Global.propriedadePecas.ContainsKey(transformacao.name))
                            {
                                listTransf.Add(CreateTransfProject(transformacao, Global.propriedadePecas[transformacao.name] as PropriedadeTransformacao));
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
                        formaProj.Iluminacao = CreateIluminacao(obgGrafSlot);
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
                    PoligonoProject formaProj = CreateFormPoligono(forma.name, Global.propriedadePecas[forma.name] as PoligonoPropriedadePeca);

                    if (formaProj != null)
                    {
                        formaProj.Transformacoes = CreateTransformacoes(obgGrafSlot);
                        formaProj.Iluminacao = CreateIluminacao(obgGrafSlot);
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
                    SplineProject formaProj = CreateFormSpline(forma.name, Global.propriedadePecas[forma.name] as SplinePropriedadePeca);

                    if (formaProj != null)
                    {
                        formaProj.Transformacoes = CreateTransformacoes(obgGrafSlot);
                        formaProj.Iluminacao = CreateIluminacao(obgGrafSlot);
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
                NomeCuboAmbiente = propPeca.NomeCuboAmbiente,
                NomeCuboVis = propPeca.NomeCuboVis,
                Cor = propPeca.Cor,
                Textura = propPeca.Textura,
                Ativo = propPeca.Ativo
            }
        };
    }

    PoligonoProject CreateFormPoligono(string formName, PoligonoPropriedadePeca propPeca)
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
                PoligonoAmbiente = propPeca.PoligonoAmbiente,
                Ativo = propPeca.Ativo
            }
        };
    }

    SplineProject CreateFormSpline(string formName, SplinePropriedadePeca propPeca)
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
                SplineAmbiente = propPeca.SplineAmbiente
            }
        };
    }

    IluminacaoProject CreateIluminacao(Transform obgGrafSlot)
    {
        IluminacaoProject IluminacaoProj = new IluminacaoProject();
        //IluminacaoProj.Propriedades = CreatePropPeca(propPeca);

        return IluminacaoProj;
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

    TransformacaoProject CreateTransfProject(Transform transformacao, PropriedadeTransformacao propPeca)
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
            case Property.IntervaloX: return (propPeca.ListPropLocks.ContainsKey("IntervaloX")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["IntervaloX"]) : propPeca.Intervalo.X.ToString();
            case Property.IntervaloY: return (propPeca.ListPropLocks.ContainsKey("IntervaloY")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["IntervaloY"]) : propPeca.Intervalo.Y.ToString();
            case Property.IntervaloZ: return (propPeca.ListPropLocks.ContainsKey("IntervaloZ")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["IntervaloZ"]) : propPeca.Intervalo.Z.ToString();

            case Property.MinX: return (propPeca.ListPropLocks.ContainsKey("MinX")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["MinX"]) : propPeca.Intervalo.X.ToString();
            case Property.MinY: return (propPeca.ListPropLocks.ContainsKey("MinY")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["MinY"]) : propPeca.Intervalo.Y.ToString();
            case Property.MinZ: return (propPeca.ListPropLocks.ContainsKey("MinZ")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["MinZ"]) : propPeca.Intervalo.Z.ToString();

            case Property.MaxX: return (propPeca.ListPropLocks.ContainsKey("MaxX")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["MaxX"]) : propPeca.Intervalo.X.ToString();
            case Property.MaxY: return (propPeca.ListPropLocks.ContainsKey("MaxY")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["MaxY"]) : propPeca.Intervalo.Y.ToString();
            case Property.MaxZ: return (propPeca.ListPropLocks.ContainsKey("MaxZ")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["MaxZ"]) : propPeca.Intervalo.Z.ToString();
            default: return string.Empty;
        }
    }

    TransformacaoPropriedadePecaProject CreateTransformacaoPropPeca(PropriedadeTransformacao propPeca)
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
            NomePeca = propPeca.NomePeca,
            Ativo = propPeca.Ativo
        };
    }

    string EncryptTransformacaoPropPeca(PropriedadeTransformacao propPeca, Property propType)
    {
        switch (propType)
        {
            case Property.PosX: return (propPeca.ListPropLocks.ContainsKey("PosX")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["PosX"]) : propPeca.Pos.X.ToString();
            case Property.PosY: return (propPeca.ListPropLocks.ContainsKey("PosY")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["PosY"]) : propPeca.Pos.Y.ToString();
            case Property.PosZ: return (propPeca.ListPropLocks.ContainsKey("PosZ")) ? Util_VisEdu.Base64Encode(propPeca.ListPropLocks["PosZ"]) : propPeca.Pos.Z.ToString();
            default: return string.Empty;
        }
    }

    string EncryptPropCam(Property prop)
    {
        PropriedadeCamera propCam = Global.propCameraGlobal;

        switch (prop)
        {
            case Property.PosX: return (propCam.ListPropCamLocks.ContainsKey("PosX")) ? Util_VisEdu.Base64Encode(propCam.ListPropCamLocks["PosX"]) : propCam.PosX.ToString();
            case Property.PosY: return (propCam.ListPropCamLocks.ContainsKey("PosY")) ? Util_VisEdu.Base64Encode(propCam.ListPropCamLocks["PosY"]) : propCam.PosY.ToString();
            case Property.PosZ: return (propCam.ListPropCamLocks.ContainsKey("PosZ")) ? Util_VisEdu.Base64Encode(propCam.ListPropCamLocks["PosZ"]) : propCam.PosZ.ToString();
            case Property.FOV: return (propCam.ListPropCamLocks.ContainsKey("FOV")) ? Util_VisEdu.Base64Encode(propCam.ListPropCamLocks["FOV"]) : propCam.FOV.ToString();
            default: return string.Empty;
        }
    }

    string EncryptCuboPropPeca(CuboPropriedadePeca peca, Property propType)
    {
        switch (propType)
        {
            case Property.PosX: return (peca.ListPropLocks.ContainsKey("PosX")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["PosX"]) : peca.Pos.X.ToString();
            case Property.PosY: return (peca.ListPropLocks.ContainsKey("PosY")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["PosY"]) : peca.Pos.Y.ToString();
            case Property.PosZ: return (peca.ListPropLocks.ContainsKey("PosZ")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["PosZ"]) : peca.Pos.Z.ToString();
            case Property.TamX: return (peca.ListPropLocks.ContainsKey("TamX")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["TamX"]) : peca.Tam.X.ToString();
            case Property.TamY: return (peca.ListPropLocks.ContainsKey("TamY")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["TamY"]) : peca.Tam.Y.ToString();
            case Property.TamZ: return (peca.ListPropLocks.ContainsKey("TamZ")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["TamZ"]) : peca.Tam.Z.ToString();
            default: return string.Empty;
        }
    }
    string EncryptPoligonoPropPeca(PoligonoPropriedadePeca peca, Property propType)
    {
        switch (propType)
        {
            case Property.Pontos: return (peca.ListPropLocks.ContainsKey("Pontos")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["Pontos"]) : peca.Pontos.ToString();
            case Property.PosX: return (peca.ListPropLocks.ContainsKey("PosX")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["PosX"]) : peca.Pos.X.ToString();
            case Property.PosY: return (peca.ListPropLocks.ContainsKey("PosY")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["PosY"]) : peca.Pos.Y.ToString();
            case Property.PosZ: return (peca.ListPropLocks.ContainsKey("PosZ")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["PosZ"]) : peca.Pos.Z.ToString();
            default: return string.Empty;
        }
    }

    string EncryptSplinePropPeca(SplinePropriedadePeca peca, Property propType)
    {
        switch (propType)
        {
            case Property.P1PosX: return (peca.ListPropLocks.ContainsKey("P1PosX")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P1PosX"]) : peca.P1.X.ToString();
            case Property.P1PosY: return (peca.ListPropLocks.ContainsKey("P1PosY")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P1PosY"]) : peca.P1.Y.ToString();
            case Property.P1PosZ: return (peca.ListPropLocks.ContainsKey("P1PosZ")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P1PosZ"]) : peca.P1.Z.ToString();

            case Property.P2PosX: return (peca.ListPropLocks.ContainsKey("P2PosX")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P2PosX"]) : peca.P2.X.ToString();
            case Property.P2PosY: return (peca.ListPropLocks.ContainsKey("P2PosY")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P2PosY"]) : peca.P2.Y.ToString();
            case Property.P2PosZ: return (peca.ListPropLocks.ContainsKey("P2PosZ")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P2PosZ"]) : peca.P2.Z.ToString();

            case Property.P3PosX: return (peca.ListPropLocks.ContainsKey("P3PosX")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P3PosX"]) : peca.P3.X.ToString();
            case Property.P3PosY: return (peca.ListPropLocks.ContainsKey("P3PosY")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P3PosY"]) : peca.P3.Y.ToString();
            case Property.P3PosZ: return (peca.ListPropLocks.ContainsKey("P3PosZ")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P3PosZ"]) : peca.P3.Z.ToString();

            case Property.P4PosX: return (peca.ListPropLocks.ContainsKey("P4PosX")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P4PosX"]) : peca.P4.X.ToString();
            case Property.P4PosY: return (peca.ListPropLocks.ContainsKey("P4PosY")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P4PosY"]) : peca.P4.Y.ToString();
            case Property.P4PosZ: return (peca.ListPropLocks.ContainsKey("P4PosZ")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P4PosZ"]) : peca.P4.Z.ToString();

            case Property.P5PosX: return (peca.ListPropLocks.ContainsKey("P5PosX")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P5PosX"]) : peca.P5.X.ToString();
            case Property.P5PosY: return (peca.ListPropLocks.ContainsKey("P5PosY")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P5PosY"]) : peca.P5.Y.ToString();
            case Property.P5PosZ: return (peca.ListPropLocks.ContainsKey("P5PosZ")) ? Util_VisEdu.Base64Encode(peca.ListPropLocks["P5PosZ"]) : peca.P5.Z.ToString();

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