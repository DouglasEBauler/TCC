using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public enum Passo { 
        PulouTutorial, PrimeiroPasso, SegundoPasso, TerceiroPasso, QuartoPasso, QuintoPasso, SextoPasso, SetimoPasso, UltimoPasso, FistTime = 99 
    };

    const float SPEED_DESLOC = 8f;

    public Passo PassoTutorial = Passo.FistTime;
    public Passo PassoExecutando = Passo.PrimeiroPasso;
    public bool EstaExecutandoTutorial;
    public string Nivel;
    public int AnswerMsg;
    public bool AbriuMessageBox;

    [SerializeField]
    GameObject CursorMouse;
    [SerializeField]
    GameObject cameraP;
    [SerializeField]
    GameObject objGraficoP;
    [SerializeField]
    GameObject cubo;
    [SerializeField]
    GameObject spline;
    [SerializeField]
    GameObject poligono;
    [SerializeField]
    GameObject transladar;
    [SerializeField]
    GameObject rotacionar;
    [SerializeField]
    GameObject escalar;
    [SerializeField]
    GameObject iluminacao;
    [SerializeField]
    GameObject propCamera;
    [SerializeField]
    GameObject propriedadePecas;
    [SerializeField]
    GameObject objGraficoSlot;
    [SerializeField]
    GameObject formasSlot;
    [SerializeField]
    GameObject propCubo;
    [SerializeField]
    GameObject propRotacionar;
    [SerializeField]
    GameObject inputPosicaoXCubo;
    [SerializeField]
    GameObject cuboAmbiente;
    [SerializeField]
    GameObject btnFabPecas;
    [SerializeField]
    GameObject btnPropPecas;
    [SerializeField]
    GameObject cameraSlot;
    [SerializeField]
    GameObject transformacoesSlot;
    [SerializeField]
    GameObject inputYRotacionar;
    [SerializeField]
    GameObject iluminacaoSlot;
    [SerializeField]
    GameObject go_fabricaPecas;
    [SerializeField]
    GameObject fabricaPecas;

    GameObject GOTutorial;

    void Update()
    {
        if (!PassoTutorial.Equals(Passo.PulouTutorial))
        {
            if (PassoTutorial.Equals(Passo.FistTime))
            {
                MensagemTutorial();
            }
            else if (PassoTutorial.Equals(Passo.PrimeiroPasso) && PassoExecutando.Equals(Passo.PrimeiroPasso))
            {
                if (AnswerMsg != 0)
                {
                    float distance = GetDistance();

                    SetMessageImage(true);

                    if (distance > 0)
                    {
                        GOTutorial.transform.position = Vector3.Lerp(
                            GOTutorial.transform.position,
                            new Vector3(objGraficoSlot.transform.position.x + 4.55f, objGraficoSlot.transform.position.y + 0.2f, objGraficoSlot.transform.position.z - 2),
                            Time.deltaTime * SPEED_DESLOC / distance
                        );

                        CursorMouse.transform.position = Vector3.Lerp(
                            CursorMouse.transform.position,
                            new Vector3(objGraficoSlot.transform.position.x + 6f, objGraficoSlot.transform.position.y + 0.2f, objGraficoSlot.transform.position.z - 2),
                            Time.deltaTime * SPEED_DESLOC / distance
                        );
                    }
                    else
                    {
                        OnMouseUp();
                        SetMessageImage(false);

                        PassoExecutando = Passo.SegundoPasso;
                        PassoTutorial = Passo.SegundoPasso;
                        MensagemTutorial();
                    }
                }
            }
            else if (PassoTutorial.Equals(Passo.SegundoPasso) && PassoExecutando.Equals(Passo.SegundoPasso))
            {
                if (AnswerMsg != 0)
                {
                    float distance = GetDistance();

                    SetMessageImage(true);

                    if (distance > 0)
                    {
                        GOTutorial.transform.position = Vector3.Lerp(
                            GOTutorial.transform.position,
                            new Vector3(formasSlot.transform.position.x + 3.1f, formasSlot.transform.position.y + 0.2f, formasSlot.transform.position.z - 3),
                            Time.deltaTime * SPEED_DESLOC / distance
                         );

                        CursorMouse.transform.position = Vector3.Lerp(
                            CursorMouse.transform.position,
                            new Vector3(formasSlot.transform.position.x + 3.1f, formasSlot.transform.position.y + 0.2f, formasSlot.transform.position.z - 3),
                            Time.deltaTime * SPEED_DESLOC / distance
                        );
                    }
                    else
                    {
                        OnMouseUp();
                        SetMessageImage(false);

                        PassoExecutando = Passo.TerceiroPasso;
                        PassoTutorial = Passo.TerceiroPasso;

                        MensagemTutorial();
                    }
                }
            }
            else if (PassoTutorial.Equals(Passo.TerceiroPasso) && PassoExecutando.Equals(Passo.TerceiroPasso))
            {
                if (AnswerMsg != 0)
                {
                    SetMessageImage(true);

                    btnPropPecas.GetComponent<BotaoPropPecas>().ActivePanelProp(propCubo.name);

                    propCubo.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "Cubo";
                    propCubo.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "1";
                    propCubo.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = "1";
                    propCubo.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = "1";
                    propCubo.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "0";
                    propCubo.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = "0";
                    propCubo.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = "0";

                    float distance = GetDistance();
                    if (distance > 0)
                    {
                        CursorMouse.transform.position = Vector3.Lerp(
                            CursorMouse.transform.position,
                            new Vector3(inputPosicaoXCubo.transform.position.x, inputPosicaoXCubo.transform.position.y, inputPosicaoXCubo.transform.position.z),
                            Time.deltaTime * SPEED_DESLOC / distance);
                    }
                    else
                    {
                        propCubo.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "2";
                        cuboAmbiente.transform.localScale = new Vector3(2, 1, 1);

                        PassoExecutando = Passo.QuartoPasso;
                        PassoTutorial = Passo.QuartoPasso;

                        SetMessageImage(false);
                        MensagemTutorial();
                    }
                }
            }
            else if (PassoTutorial.Equals(Passo.QuartoPasso) && PassoExecutando.Equals(Passo.QuartoPasso))
            {
                if (AnswerMsg != 0)
                {
                    SetMessageImage(true);

                    float distance = GetDistance("4.1");
                    if (distance > 0 && "4.1".Equals(Nivel))
                    {
                        CursorMouse.transform.position = Vector3.Lerp(
                            CursorMouse.transform.position,
                            new Vector3(btnFabPecas.transform.position.x, btnFabPecas.transform.position.y, btnFabPecas.transform.position.z),
                            Time.deltaTime * SPEED_DESLOC / distance);
                    }
                    else
                    {
                        if ("4.1".Equals(Nivel))
                        {
                            btnFabPecas.SendMessage("OnMouseDown");
                        }

                        float distance1 = GetDistance("4.2");
                        if (distance1 > 0 && "4.2".Equals(Nivel))
                        {
                            CursorMouse.transform.position = Vector3.Lerp(
                                CursorMouse.transform.position,
                                new Vector3(cameraP.transform.position.x, cameraP.transform.position.y, cameraP.transform.position.z - 1),
                                Time.deltaTime * SPEED_DESLOC / distance1
                            );
                        }
                        else
                        {
                            Nivel = "4.3";

                            float distance2 = GetDistance("4.3");
                            if (distance2 > 0)
                            {
                                CursorMouse.transform.position = Vector3.Lerp(
                                    CursorMouse.transform.position,
                                    new Vector3(cameraSlot.transform.position.x + 3, cameraSlot.transform.position.y, cameraSlot.transform.position.z - 1),
                                    Time.deltaTime * SPEED_DESLOC / distance2
                                );

                                GOTutorial.transform.position = Vector3.Lerp(
                                    GOTutorial.transform.position,
                                    new Vector3(cameraSlot.transform.position.x + 3.4f, cameraSlot.transform.position.y + 0.1f, cameraSlot.transform.position.z - 1f),
                                    Time.deltaTime * SPEED_DESLOC / distance2
                                );
                            }
                            else
                            {
                                OnMouseUp();
                                SetMessageImage(false);

                                PassoExecutando = Passo.QuintoPasso;
                                PassoTutorial = Passo.QuintoPasso;

                                MensagemTutorial();
                            }
                        }
                    }
                }
            }
            else if (PassoTutorial.Equals(Passo.QuintoPasso) && PassoExecutando.Equals(Passo.QuintoPasso))
            {
                if (AnswerMsg != 0)
                {
                    SetMessageImage(true);
                    float distance = GetDistance("4.1");

                    if (distance > 0 && "4.1".Equals(Nivel))
                    {
                        CursorMouse.transform.position = Vector3.Lerp(
                            CursorMouse.transform.position,
                            new Vector3(rotacionar.transform.position.x, rotacionar.transform.position.y, rotacionar.transform.position.z),
                            Time.deltaTime * SPEED_DESLOC / distance);
                    }
                    else
                    {
                        float distance1 = GetDistance("4.2");
                        if ((distance1 > 0) && ("4.1".Equals(Nivel) || "4.2".Equals(Nivel)))
                        {
                            CursorMouse.transform.position = Vector3.Lerp(
                                CursorMouse.transform.position,
                                new Vector3(transformacoesSlot.transform.position.x + 3.8f, transformacoesSlot.transform.position.y, transformacoesSlot.transform.position.z - 2),
                                Time.deltaTime * SPEED_DESLOC / distance1);

                            GOTutorial.transform.position = Vector3.Lerp(
                                GOTutorial.transform.position,
                                new Vector3(transformacoesSlot.transform.position.x + 3.8f, transformacoesSlot.transform.position.y, transformacoesSlot.transform.position.z),
                                Time.deltaTime * SPEED_DESLOC / distance1);

                            Nivel = "4.2";
                        }
                        else
                        {
                            if ("4.2".Equals(Nivel))
                            {
                                Nivel = "4.3";
                                rotacionar.GetComponent<Controller>().SendMessage("OnMouseUp");

                                cuboAmbiente.transform.localScale = new Vector3(2, 1, 1);

                                btnPropPecas.GetComponent<BotaoPropPecas>().ActivePanelProp(propRotacionar.name);

                                propRotacionar.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "Rotacionar";
                                propRotacionar.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "0";
                                propRotacionar.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = "0";
                                propRotacionar.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = "0";
                            }

                            float distance2 = GetDistance("4.3");
                            if (distance2 > 0)
                            {
                                CursorMouse.transform.position = Vector3.Lerp(
                                    CursorMouse.transform.position,
                                    new Vector3(inputYRotacionar.transform.position.x, inputYRotacionar.transform.position.y, inputYRotacionar.transform.position.z),
                                    Time.deltaTime * SPEED_DESLOC / distance2);
                            }
                            else
                            {
                                propRotacionar.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = "45";

                                cuboAmbiente.transform.localScale = new Vector3(2, 1, 1);
                                cuboAmbiente.transform.localRotation = Quaternion.Euler(cuboAmbiente.transform.localRotation.x, 45, cuboAmbiente.transform.localRotation.z);

                                ++DropPeca.countTransformacoes;
                                PassoExecutando = Passo.SextoPasso;
                                PassoTutorial = Passo.SextoPasso;

                                SetMessageImage(false);
                                MensagemTutorial();
                            }
                        }
                    }
                }
            }
            else if (PassoTutorial.Equals(Passo.SextoPasso) && PassoExecutando.Equals(Passo.SextoPasso))
            {
                if (AnswerMsg != 0)
                {
                    SetMessageImage(true);

                    float distance = GetDistance("4.1");
                    if (distance > 0 && "4.1".Equals(Nivel))
                    {
                        CursorMouse.transform.position = Vector3.Lerp(
                            CursorMouse.transform.position,
                            new Vector3(btnFabPecas.transform.position.x, btnFabPecas.transform.position.y, btnFabPecas.transform.position.z),
                            Time.deltaTime * SPEED_DESLOC / distance);
                    }
                    else
                    {
                        if ("4.1".Equals(Nivel))
                        {
                            btnFabPecas.SendMessage("OnMouseDown");
                            CursorMouse.transform.position = new Vector3(iluminacao.transform.position.x, iluminacao.transform.position.y, iluminacao.transform.position.z);
                        }

                        float distance1 = GetDistance("4.2");
                        if (distance1 > 0 && "4.2".Equals(Nivel))
                        {
                            GOTutorial.transform.position = Vector3.Lerp(
                                GOTutorial.transform.position,
                                new Vector3(iluminacaoSlot.transform.position.x + 5.1f, iluminacaoSlot.transform.position.y + 0.9f, iluminacaoSlot.transform.position.z - 4),
                                Time.deltaTime * SPEED_DESLOC / distance1);

                            CursorMouse.transform.position = Vector3.Lerp(
                                CursorMouse.transform.position,
                                new Vector3(iluminacaoSlot.transform.position.x + 3.1f, iluminacaoSlot.transform.position.y + 0.2f, iluminacaoSlot.transform.position.z - 5),
                                Time.deltaTime * SPEED_DESLOC / distance1);
                        }
                        else
                        {
                            Nivel = "4.3";

                            OnMouseUp();
                            SetMessageImage(false);

                            PassoExecutando = Passo.UltimoPasso;
                            PassoTutorial = Passo.UltimoPasso;

                            MensagemTutorial();
                        }
                    }
                }
            }
            else if (PassoTutorial.Equals(Passo.UltimoPasso))
            {
                MensagemTutorial();
            }
        }
    }

    void OnMouseUp()
    {
        if (EstaExecutandoTutorial)
        {
            switch (PassoTutorial)
            {
                case Passo.PrimeiroPasso: objGraficoP.GetComponent<Controller>().AddObjGrafico(true); break;
                case Passo.SegundoPasso: cubo.GetComponent<Controller>().AddCuboTransform(); break;
                case Passo.QuartoPasso: cameraP.GetComponent<Controller>().AddCamera(); break;
                case Passo.QuintoPasso: rotacionar.GetComponent<Controller>().AddTransformacoesSlot(true); break;
                case Passo.SextoPasso: iluminacao.GetComponent<Controller>().AddIluminacao(true); break;
            }
        }
    }

    public void MensagemTutorial(bool proximo = false)
    {
        EstaExecutandoTutorial = true;

        switch (PassoTutorial)
        {
            case Passo.PrimeiroPasso:
                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg01", true);

                if (AnswerMsg == 2)
                {
                    PulouTutorial();
                    PassoTutorial = Passo.PulouTutorial;
                }

                AnswerMsg = 0;
                break;

            case Passo.SegundoPasso:
                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg02", true);

                AnswerMsg = 0;

                if (AnswerMsg == 2)
                {
                    PulouTutorial();
                    PassoTutorial = Passo.PulouTutorial;
                }
                else
                {
                    CursorMouse.transform.position = new Vector3(cubo.transform.position.x + 2.5f, cubo.transform.position.y - 1, cubo.transform.position.z - 6);

                    GOTutorial = Instantiate(cubo, cubo.transform.position, cubo.transform.rotation, cubo.transform.parent);
                    GOTutorial.name = "CuboTutorial";
                    GOTutorial.transform.position = new Vector3(cubo.transform.position.x, cubo.transform.position.y, cubo.transform.position.z);
                }
                break;

            case Passo.TerceiroPasso:
                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg03", true);

                AnswerMsg = 0;
                break;

            case Passo.QuartoPasso:
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg04", true);

                AnswerMsg = 0;

                GOTutorial = Instantiate(cameraP, cameraP.transform.position, cameraP.transform.rotation, cameraP.transform.parent);
                GOTutorial.name = "CameraTutorial";
                GOTutorial.transform.position = new Vector3(cameraP.transform.position.x, cameraP.transform.position.y, cameraP.transform.position.z);
                break;

            case Passo.QuintoPasso:
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg05", true);

                AnswerMsg = 0;

                GOTutorial = Instantiate(rotacionar, rotacionar.transform.position, rotacionar.transform.rotation, rotacionar.transform.parent);
                GOTutorial.name = "RotacionarTutorial";
                GOTutorial.transform.position = new Vector3(rotacionar.transform.position.x, rotacionar.transform.position.y, rotacionar.transform.position.z);
                break;

            case Passo.SextoPasso:
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg06", true);

                AnswerMsg = 0;

                GOTutorial = Instantiate(iluminacao, iluminacao.transform.position, iluminacao.transform.rotation, iluminacao.transform.parent);
                GOTutorial.name = "IluminacaoTutorial";
                GOTutorial.transform.position = new Vector3(iluminacao.transform.position.x, iluminacao.transform.position.y, iluminacao.transform.position.z);
                break;

            case Passo.SetimoPasso:
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg07", true);

                AnswerMsg = 0;

                GOTutorial = GameObject.Find("ObjetoGraficoPTutorial");
                break;

            case Passo.UltimoPasso:
                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg07", true);

                AnswerMsg = 0;
                break;

            case Passo.FistTime:
                if (GameObject.Find("PainelConfirmacao") != null)
                {
                    if (!AbriuMessageBox)
                        MessageBoxVisEdu("PainelConfirmacao", true);

                    CursorMouse = GameObject.Find("CursorMouse");

                    if (AnswerMsg != 0)
                    {
                        if (AnswerMsg == 1)
                        {
                            CursorMouse.GetComponent<RawImage>().enabled = true;

                            GOTutorial = Instantiate(objGraficoP, objGraficoP.transform.position, objGraficoP.transform.rotation, objGraficoP.transform.parent);
                            GOTutorial.name = "ObjetoGraficoPTutorial";
                            GOTutorial.transform.position = new Vector3(objGraficoP.transform.position.x, objGraficoP.transform.position.y, objGraficoP.transform.position.z);

                            PassoTutorial = Passo.PrimeiroPasso;

                            MessageBoxVisEdu("PainelConfirmacao", false);
                        }
                        else
                        {
                            PassoTutorial = Passo.PulouTutorial;
                            EstaExecutandoTutorial = false;
                            MessageBoxVisEdu("PainelConfirmacao", false);
                            AnswerMsg = 0;
                        }
                    }
                }
                break;
        }
    }

    public void PulouTutorial()
    {
        EstaExecutandoTutorial = false;
        CursorMouse.GetComponent<RawImage>().enabled = false;
        PassoTutorial = Passo.PulouTutorial;

        Destroy(GameObject.Find("ObjetoGraficoPTutorial"));
        Destroy(GameObject.Find("CuboTutorial"));
        Destroy(GameObject.Find("CameraTutorial"));
        Destroy(GameObject.Find("RotacionarTutorial"));
        Destroy(GameObject.Find("IluminacaoTutorial"));
        Destroy(GameObject.Find("ObjGraficoSlotTutorial"));
        Destroy(GameObject.Find("CuboVisObjectMain(Clone)"));
        Destroy(GameObject.Find("CuboVisObjectMainTutorial"));

        GameObject gameObj = GameObject.Find("FormasSlotTutorial");
        if (gameObj != null)
        {
            gameObj.name = "FormasSlot";
            gameObj.SetActive(false);
        }

        gameObj = GameObject.Find("TransformacoesSlotTutorial");
        if (gameObj != null)
        {
            gameObj.name = "TransformacoesSlot";
            gameObj.SetActive(false);
        }

        gameObj = GameObject.Find("IluminacaoSlotTutorial");
        if (gameObj != null)
        {
            gameObj.name = "IluminacaoSlot";
            gameObj.SetActive(false);
        }

        gameObj = GameObject.Find("BaseRenderLateralGOTutorial");
        if (gameObj != null)
        {
            gameObj.name = "BaseRenderLateralGO";
        }

        gameObj = GameObject.Find("BaseRenderLateralTutorial");
        if (gameObj != null)
        {
            gameObj.name = "BaseRenderLateral";
            gameObj.SetActive(false);
        }

        gameObj = GameObject.Find("BaseObjetoGraficoGOTutorial");
        if (gameObj != null)
        {
            gameObj.name = "BaseObjetoGraficoGO";
        }

        gameObj = GameObject.Find("BaseObjetoGraficoTutorial");
        if (gameObj != null)
        {
            gameObj.name = "BaseObjetoGrafico";
            gameObj.SetActive(false);
        }

        GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Nothing");
        propCamera.GetComponent<PropCameraScript>().DemosntraCamera(false);

        GameObject cuboAmb = GameObject.Find("CuboAmbiente");
        if (cuboAmb != null)
        {
            cuboAmb.transform.localScale = new Vector3(1, 1, 1);
            cuboAmb.transform.localRotation = Quaternion.Euler(0, 0, 0);
            cuboAmb.GetComponent<MeshRenderer>().enabled = false;
        }

        var btnFabPecas = GameObject.Find("BtnFabPecas");
        btnFabPecas.SendMessage("OnMouseDown");

        GameObject.Find("PainelBase").SetActive(false);
    }

    float GetDistance(string nivel = "")
    {
        switch (PassoTutorial)
        {
            case Passo.PrimeiroPasso:
                return Vector3.Distance(GOTutorial.transform.position, new Vector3(objGraficoSlot.transform.position.x + 4.55f, objGraficoSlot.transform.position.y + 0.2f, objGraficoSlot.transform.position.z - 2));

            case Passo.SegundoPasso:
                return Vector3.Distance(GOTutorial.transform.position, new Vector3(formasSlot.transform.position.x + 3.1f, formasSlot.transform.position.y + 0.2f, formasSlot.transform.position.z - 3));

            case Passo.TerceiroPasso:
                return Vector3.Distance(CursorMouse.transform.position, inputPosicaoXCubo.transform.position);

            case Passo.QuartoPasso:
                switch (nivel)
                {
                    case "4.1": return Vector3.Distance(CursorMouse.transform.position, btnFabPecas.transform.position);
                    case "4.2": return Vector3.Distance(CursorMouse.transform.position, new Vector3(cameraP.GetComponent<Controller>().transform.position.x, cameraP.transform.position.y, cameraP.transform.position.z - 1));
                    case "4.3": return Vector3.Distance(CursorMouse.transform.position, new Vector3(cameraSlot.transform.position.x + 3, cameraSlot.transform.position.y, cameraSlot.transform.position.z - 1));
                    default: return 0;
                }

            case Passo.QuintoPasso:
                switch (nivel)
                {
                    case "4.1": return Vector3.Distance(CursorMouse.transform.position, rotacionar.transform.position);
                    case "4.2": return Vector3.Distance(CursorMouse.transform.position, new Vector3(transformacoesSlot.transform.position.x + 3.8f, transformacoesSlot.transform.position.y, transformacoesSlot.transform.position.z - 2));
                    case "4.3": return Vector3.Distance(CursorMouse.transform.position, new Vector3(inputYRotacionar.transform.position.x, inputYRotacionar.transform.position.y, inputYRotacionar.transform.position.z));
                    default: return 0;
                }

            case Passo.SextoPasso:
                switch (nivel)
                {
                    case "4.1": return Vector3.Distance(CursorMouse.transform.position, btnFabPecas.transform.position);
                    case "4.2": return Vector3.Distance(GOTutorial.transform.position, new Vector3(iluminacaoSlot.transform.position.x + 5.1f, iluminacaoSlot.transform.position.y + 0.9f, iluminacaoSlot.transform.position.z - 4));
                    default: return 0;
                }

            default: return 0;
        }
    }

    void SetMessageImage(bool status)
    {
        string msg = string.Empty;

        switch (PassoTutorial)
        {
            case Passo.PrimeiroPasso: msg = "Msg01"; break;
            case Passo.SegundoPasso: msg = "Msg02"; break;
            case Passo.TerceiroPasso: msg = "Msg03"; break;
            case Passo.QuartoPasso: msg = "Msg04"; break;
            case Passo.QuintoPasso: msg = "Msg05"; break;
            case Passo.SextoPasso: msg = "Msg06"; break;
            case Passo.SetimoPasso: msg = "Msg07"; break;
        }

        if (msg != string.Empty)
            GameObject.Find(msg).GetComponent<RawImage>().enabled = status;
    }

    public void MessageBoxVisEdu(string painel, bool active)
    {
        GameObject painelActive = GameObject.Find(painel);
        if (painelActive != null)
        {
            RawImage imgPnl = painelActive.GetComponent<RawImage>();

            if (imgPnl != null)
            {
                imgPnl.enabled = active;
                painelActive.transform.GetChild(0).gameObject.SetActive(active);

                if (painelActive.transform.childCount > 1)
                    painelActive.transform.GetChild(1).gameObject.SetActive(active);

                painelActive.transform.parent.GetComponent<BoxCollider>().enabled = active;

                AbriuMessageBox = active;
            }
        }
    }
}

