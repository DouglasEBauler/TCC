using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public enum Passo { 
        PulouTutorial, PrimeiroPasso, SegundoPasso, TerceiroPasso, QuartoPasso, QuintoPasso, SextoPasso, SetimoPasso, UltimoPasso, FistTime = 99 
    };

    const float SPEED_DESLOC = 8f;

    [HideInInspector]
    public Passo PassoTutorial = Passo.FistTime;
    [HideInInspector]
    public Passo PassoExecutando = Passo.PrimeiroPasso;
    [HideInInspector]
    public bool EstaExecutandoTutorial;
    [HideInInspector]
    public string Nivel;
    [HideInInspector]
    public int AnswerMsg;
    [HideInInspector]
    public bool AbriuMessageBox;

    [SerializeField]
    GameObject cursorMouse;
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
    InputField inputTamanhoXCubo;
    [SerializeField]
    GameObject cuboAmbiente;
    [SerializeField]
    GameObject btnFabPecas;
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    GameObject cameraSlot;
    [SerializeField]
    GameObject transformacoesSlot;
    [SerializeField]
    InputField inputYRotacionar;
    [SerializeField]
    GameObject iluminacaoSlot;
    [SerializeField]
    GameObject go_fabricaPecas;
    [SerializeField]
    GameObject fabricaPecas;
    [SerializeField]
    LixeiraScript lixeiraScript;

    GameObject GOTutorial;

    void Update()
    {
        if (!PassoTutorial.Equals(Passo.PulouTutorial))
        {
            switch (PassoTutorial)
            {
                case Passo.PrimeiroPasso:
                    if (AnswerMsg != 0 && PassoExecutando.Equals(Passo.PrimeiroPasso))
                    {
                        float distance = GetDistance();

                        SetMessageImage(true);

                        if (distance > 0)
                        {
                            GOTutorial.transform.position = 
                                Vector3.Lerp(GOTutorial.transform.position, objGraficoSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance);

                            cursorMouse.transform.position = 
                                Vector3.Lerp(cursorMouse.transform.position, objGraficoSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance);
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
                    break;

                case Passo.SegundoPasso:
                    if (AnswerMsg != 0 && PassoExecutando.Equals(Passo.SegundoPasso))
                    {
                        float distance = GetDistance();

                        MessageBoxVisEdu("PainelMsg02", false);
                        SetMessageImage(true);

                        if (distance > 0)
                        {
                            GOTutorial.transform.position = 
                                Vector3.Lerp(GOTutorial.transform.position, formasSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance);

                            cursorMouse.transform.position = 
                                Vector3.Lerp(cursorMouse.transform.position, formasSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance);
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
                    break;

                case Passo.TerceiroPasso:
                    if (AnswerMsg != 0 && PassoExecutando.Equals(Passo.TerceiroPasso))
                    {
                        MessageBoxVisEdu("PainelMsg03", false);
                        SetMessageImage(true);

                        cubo.GetComponent<CuboScript>().ConfiguraPropriedadePeca();

                        float distance = GetDistance();
                        if (distance > 0)
                        {
                            cursorMouse.transform.position = 
                                Vector3.Lerp(cursorMouse.transform.position, inputPosicaoXCubo.transform.position, Time.deltaTime * SPEED_DESLOC / distance);
                        }
                        else
                        {
                            inputTamanhoXCubo.text = "2";
                            cuboAmbiente.transform.localScale = new Vector3(2, 1, 1);
                            SetMessageImage(false);

                            PassoExecutando = Passo.QuartoPasso;
                            PassoTutorial = Passo.QuartoPasso;

                            MensagemTutorial();
                        }
                    }
                    break;

                case Passo.QuartoPasso:
                    if (AnswerMsg != 0 && PassoExecutando.Equals(Passo.QuartoPasso))
                    {
                        MessageBoxVisEdu("PainelMsg04", false);
                        SetMessageImage(true);

                        float distance = GetDistance("4.1");
                        if (distance > 0 && "4.1".Equals(Nivel))
                        {
                            cursorMouse.transform.position = 
                                Vector3.Lerp(cursorMouse.transform.position, btnFabPecas.transform.position, Time.deltaTime * SPEED_DESLOC / distance);
                        }
                        else
                        {
                            if ("4.1".Equals(Nivel))
                            {
                                menuControl.GetComponent<MenuScript>().EnablePanelFabPecas();
                            }

                            float distance1 = GetDistance("4.2");
                            if (distance1 > 0 && "4.2".Equals(Nivel))
                            {
                                cursorMouse.transform.position = 
                                    Vector3.Lerp(cursorMouse.transform.position, cameraP.transform.position, Time.deltaTime * SPEED_DESLOC / distance1);
                            }
                            else
                            {
                                Nivel = "4.3";

                                float distance2 = GetDistance("4.3");
                                if (distance2 > 0)
                                {
                                    cursorMouse.transform.position = 
                                        Vector3.Lerp(cursorMouse.transform.position, cameraSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance2);

                                    GOTutorial.transform.position = 
                                        Vector3.Lerp(GOTutorial.transform.position, cameraSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance2);
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
                    break;

                case Passo.QuintoPasso:
                    if (AnswerMsg != 0 && PassoExecutando.Equals(Passo.QuintoPasso))
                    {
                        MessageBoxVisEdu("PainelMsg05", false);
                        SetMessageImage(true);
                        float distance = GetDistance("4.1");

                        if (distance > 0 && "4.1".Equals(Nivel))
                        {
                            cursorMouse.transform.position = 
                                Vector3.Lerp(cursorMouse.transform.position, rotacionar.transform.position, Time.deltaTime * SPEED_DESLOC / distance);
                        }
                        else
                        {
                            float distance1 = GetDistance("4.2");
                            if ((distance1 > 0) && ("4.1".Equals(Nivel) || "4.2".Equals(Nivel)))
                            {
                                cursorMouse.transform.position = 
                                    Vector3.Lerp(cursorMouse.transform.position, transformacoesSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance1);

                                GOTutorial.transform.position = 
                                    Vector3.Lerp(GOTutorial.transform.position, transformacoesSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance1);

                                Nivel = "4.2";
                            }
                            else
                            {
                                if ("4.2".Equals(Nivel))
                                {
                                    Nivel = "4.3";
                                    rotacionar.GetComponent<TransformacaoScript>().SendMessage("OnMouseUp");

                                    cuboAmbiente.transform.localScale = new Vector3(2, 1, 1);

                                    rotacionar.GetComponent<TransformacaoScript>().ConfiguraPropriedadePeca();
                                }

                                float distance2 = GetDistance("4.3");
                                if (distance2 > 0)
                                {
                                    cursorMouse.transform.position = 
                                        Vector3.Lerp(cursorMouse.transform.position, inputYRotacionar.transform.position, Time.deltaTime * SPEED_DESLOC / distance2);
                                }
                                else
                                {
                                    inputYRotacionar.text = "45";

                                    cuboAmbiente.transform.localScale = new Vector3(2, 1, 1);
                                    cuboAmbiente.transform.localRotation = Quaternion.Euler(cuboAmbiente.transform.localRotation.x, 45, cuboAmbiente.transform.localRotation.z);

                                    SetMessageImage(false);

                                    PassoExecutando = Passo.SextoPasso;
                                    PassoTutorial = Passo.SextoPasso;

                                    MensagemTutorial();
                                }
                            }
                        }
                    }
                    break;

                case Passo.SextoPasso:
                    if (AnswerMsg != 0 && PassoExecutando.Equals(Passo.SextoPasso))
                    {
                        MessageBoxVisEdu("PainelMsg06", false);
                        SetMessageImage(true);

                        float distance = GetDistance("4.1");
                        if (distance > 0 && "4.1".Equals(Nivel))
                        {
                            cursorMouse.transform.position = 
                                Vector3.Lerp(cursorMouse.transform.position, btnFabPecas.transform.position, Time.deltaTime * SPEED_DESLOC / distance);
                        }
                        else
                        {
                            if ("4.1".Equals(Nivel))
                            {
                                menuControl.GetComponent<MenuScript>().EnablePanelFabPecas();
                                cursorMouse.transform.position = new Vector3(iluminacao.transform.position.x, iluminacao.transform.position.y, iluminacao.transform.position.z);
                            }

                            float distance1 = GetDistance("4.2");
                            if (distance1 > 0 && "4.2".Equals(Nivel))
                            {
                                GOTutorial.transform.position = 
                                    Vector3.Lerp(GOTutorial.transform.position, iluminacaoSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance1);

                                cursorMouse.transform.position = 
                                    Vector3.Lerp(cursorMouse.transform.position, iluminacaoSlot.transform.position, Time.deltaTime * SPEED_DESLOC / distance1);
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
                    break;

                case Passo.FistTime:
                case Passo.UltimoPasso:
                    MensagemTutorial();
                    break;
            }
        }
    }

    void OnMouseUp()
    {
        if (EstaExecutandoTutorial)
        {
            switch (PassoTutorial)
            {
                case Passo.PrimeiroPasso: objGraficoP.GetComponent<ObjetoGraficoScript>().AddObjGrafico(); break;
                case Passo.SegundoPasso: cubo.GetComponent<CuboScript>().AddCubo(); break;
                case Passo.QuartoPasso: cameraP.GetComponent<CameraPScript>().AddCamera(); break;
                case Passo.QuintoPasso: rotacionar.GetComponent<TransformacaoScript>().AddTransformacao(); break;
                case Passo.SextoPasso: iluminacao.GetComponent<IluminacaoScript>().AddIluminacao(); break;
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
                {
                    MessageBoxVisEdu("PainelMsg02", true);
                }

                AnswerMsg = 0;

                if (AnswerMsg == 2)
                {
                    PulouTutorial();
                }
                else
                {
                    cursorMouse.transform.position = new Vector3(cubo.transform.position.x, cubo.transform.position.y, cubo.transform.position.z);
                    cubo.GetComponent<CuboScript>().CopiaPeca();
                    GOTutorial = cubo;
                }
                break;

            case Passo.TerceiroPasso:
                if (!AbriuMessageBox)
                {
                    MessageBoxVisEdu("PainelMsg03", true);
                }

                AnswerMsg = 0;
                break;

            case Passo.QuartoPasso:
                Nivel = "4.1";

                if (!AbriuMessageBox)
                {
                    MessageBoxVisEdu("PainelMsg04", true);
                }

                AnswerMsg = 0;

                cameraP.GetComponent<CameraPScript>().CopiaPeca();
                GOTutorial = cameraP;
                break;

            case Passo.QuintoPasso:
                Nivel = "4.1";

                if (!AbriuMessageBox)
                {
                    MessageBoxVisEdu("PainelMsg05", true);
                }

                AnswerMsg = 0;

                rotacionar.GetComponent<TransformacaoScript>().CopiaPeca();
                GOTutorial = rotacionar;

                break;

            case Passo.SextoPasso:
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg06", true);

                AnswerMsg = 0;

                iluminacao.GetComponent<IluminacaoScript>().CopiaPeca();
                GOTutorial = iluminacao;
                break;

            case Passo.SetimoPasso:
                Nivel = "4.1";

                if (!AbriuMessageBox)
                {
                    MessageBoxVisEdu("PainelMsg07", true);
                }

                AnswerMsg = 0;

                GOTutorial = GameObject.Find(Consts.OBJETOGRAFICO+"1");
                break;

            case Passo.UltimoPasso:
                if (!AbriuMessageBox)
                {
                    MessageBoxVisEdu("PainelMsg07", true);
                }

                AnswerMsg = 0;
                break;

            case Passo.FistTime:
                if (GameObject.Find("PainelConfirmacao") != null)
                {
                    if (!AbriuMessageBox)
                    {
                        MessageBoxVisEdu("PainelConfirmacao", true);
                    }

                    if (AnswerMsg != 0)
                    {
                        if (AnswerMsg == 1)
                        {
                            cursorMouse.GetComponent<RawImage>().enabled = true;
                            objGraficoP.GetComponent<ObjetoGraficoScript>().CopiaPeca();
                            GOTutorial = objGraficoP;
                            PassoTutorial = Passo.PrimeiroPasso;
                            MessageBoxVisEdu("PainelConfirmacao", false);
                        }
                        else
                        {
                            PulouTutorial();
                        }
                    }
                }
                break;
        }
    }

    public void PulouTutorial()
    {
        EstaExecutandoTutorial = false;
        cursorMouse.GetComponent<RawImage>().enabled = false;
        PassoTutorial = Passo.PulouTutorial;

        var objDrop = GameObject.Find(Consts.OBJETOGRAFICO + "1");
        if (objDrop != null)
        {
            lixeiraScript.objDrop = objDrop;
            lixeiraScript.RemovePeca();
            objDrop = GameObject.Find(Consts.CAMERA + "1");
            if (objDrop != null)
            {
                lixeiraScript.objDrop = objDrop;
                lixeiraScript.RemovePeca();
            }
        }

        GameObject.Find("PainelBase").SetActive(false);
    }

    float GetDistance(string nivel = "")
    {
        switch (PassoTutorial)
        {
            case Passo.PrimeiroPasso:
                return Vector3.Distance(GOTutorial.transform.position, objGraficoSlot.transform.position);

            case Passo.SegundoPasso:
                return Vector3.Distance(GOTutorial.transform.position, formasSlot.transform.position);

            case Passo.TerceiroPasso:
                return Vector3.Distance(cursorMouse.transform.position, inputPosicaoXCubo.transform.position);

            case Passo.QuartoPasso:
                switch (nivel)
                {
                    case "4.1": return Vector3.Distance(cursorMouse.transform.position, btnFabPecas.transform.position);
                    case "4.2": return Vector3.Distance(cursorMouse.transform.position, cameraP.transform.position);
                    case "4.3": return Vector3.Distance(cursorMouse.transform.position, cameraSlot.transform.position);
                    default: return 0;
                }

            case Passo.QuintoPasso:
                switch (nivel)
                {
                    case "4.1": return Vector3.Distance(cursorMouse.transform.position, rotacionar.transform.position);
                    case "4.2": return Vector3.Distance(cursorMouse.transform.position, transformacoesSlot.transform.position);
                    case "4.3": return Vector3.Distance(cursorMouse.transform.position, inputYRotacionar.transform.position);
                    default: return 0;
                }

            case Passo.SextoPasso:
                switch (nivel)
                {
                    case "4.1": return Vector3.Distance(cursorMouse.transform.position, btnFabPecas.transform.position);
                    case "4.2": return Vector3.Distance(GOTutorial.transform.position, iluminacaoSlot.transform.position);
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
        {
            GameObject.Find(msg).GetComponent<RawImage>().enabled = status;
        }
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
                {
                    painelActive.transform.GetChild(1).gameObject.SetActive(active);
                }

                AbriuMessageBox = active;
            }
        }
    }

    public void SetAnswerMsg(GameObject button)
    {
        if ("Pular".Equals(button.name))
        {
            PassoTutorial = Passo.PulouTutorial;
            PulouTutorial();
        }
        else
        {
            AnswerMsg = 1;
        }

        MessageBoxVisEdu("PainelConfirmacao", false);
    }
}

