using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class PropCameraScript : MonoBehaviour
{
    const float POS_X_INICIAL = 0;
    const float POS_Y_INICIAL = 0;
    const float POS_Z_INICIAL = 0;
    const float FOV_INICIAL = 45;

    [SerializeField]
    Transform cuboAmb;
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject FOV;
    [SerializeField]
    GameObject labelPosicao;
    [SerializeField]
    TMP_InputField nomeCamera;
    [SerializeField]
    TMP_InputField PosX;
    [SerializeField]
    TMP_InputField PosY;
    [SerializeField]
    TMP_InputField PosZ;
    [SerializeField]
    TMP_InputField camFOV;
    [SerializeField]
    GameObject camObjMain;
    [SerializeField]
    GameObject camObjPos;
    [SerializeField]
    GameObject camVisInferior;

    float time;
    bool podeAtualizarCamera;

    void Start()
    {
        PreencheCampos();
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time >= 3 && gameObject.GetComponent<MeshRenderer>().enabled)
        {
            AtualizaPainel();
            time = 0;
        }

        if ((Global.gameObjectName != null)
            && Global.propCameraGlobal.ExisteCamera
            && (!Global.propCameraGlobal.JaIniciouValores))
        {
            PreencheCampos();
        }

        if (podeAtualizarCamera)
        {
            AjustaCameraEmX();
            podeAtualizarCamera = false;
        }
    }

    public void DemosntraCamera(bool demonstra)
    {
        foreach (Transform child in camObjMain.transform)
            child.transform.gameObject.SetActive(demonstra);

        Global.cameraAtiva = demonstra;
    }

    public void PreencheCampos()
    {
        Global.propCameraGlobal.JaIniciouValores = true;

        nomeCamera.text = "Camera";
        if (Global.propCameraGlobal.PropInicial == null)
        {
            Global.propCameraGlobal.PropInicial = new PropriedadeCameraInicial();
            PosX.text = "0";
            PosY.text = "0";
            PosZ.text = "0";
            camFOV.text = "45";

            Global.propCameraGlobal.PropInicial.PosX = camObjMain.transform.position.x;
            Global.propCameraGlobal.PropInicial.PosY = camObjMain.transform.position.y;
            Global.propCameraGlobal.PropInicial.PosZ = camObjMain.transform.position.z;
            Global.propCameraGlobal.PropInicial.FOV =
                new Vector2(camObjMain.transform.localScale.x / FOV_INICIAL, camObjMain.transform.localScale.y / FOV_INICIAL);
        }
        else
        {
            PosX.text = Global.propCameraGlobal.PosX.ToString();
            PosY.text = Global.propCameraGlobal.PosY.ToString();
            PosZ.text = Global.propCameraGlobal.PosZ.ToString();
            camFOV.text = (FOV_INICIAL + Global.propCameraGlobal.PropInicial.FOV.sqrMagnitude).ToString();
        }

        Global.propCameraGlobal.PosX = POS_X_INICIAL;
        Global.propCameraGlobal.PosY = POS_Y_INICIAL;
        Global.propCameraGlobal.PosZ = POS_Z_INICIAL;
        Global.propCameraGlobal.FOV = new Vector2(FOV_INICIAL, FOV_INICIAL);

        camObjMain.transform.position =
            new Vector3(Global.propCameraGlobal.PropInicial.PosX + Global.propCameraGlobal.PosX,
                        Global.propCameraGlobal.PropInicial.PosY + Global.propCameraGlobal.PosY,
                        Global.propCameraGlobal.PropInicial.PosZ + Global.propCameraGlobal.PosZ);

        camObjPos.transform.localPosition =
            new Vector3(-Global.propCameraGlobal.PosX * 14, Global.propCameraGlobal.PosY * 13.333333f, -Global.propCameraGlobal.PosZ * 16);
    }

    public void UpdatePosition()
    {
        Global.propCameraGlobal.PosX = -ConvertFloat(PosX.text);
        Global.propCameraGlobal.PosY = ConvertFloat(PosY.text);
        Global.propCameraGlobal.PosZ = ConvertFloat(PosZ.text);
        Global.propCameraGlobal.FOV = new Vector2(ConvertFloat(camFOV.text), ConvertFloat(camFOV.text));
        //Altera FoV (Field of View)
        camVisInferior.GetComponent<Camera>().fieldOfView = ConvertFloat(camFOV.text);
        podeAtualizarCamera = true;

        //Atualiza posição da camera
        if (Global.propCameraGlobal.PropInicial != null)
        {
            camObjMain.transform.position =
                new Vector3(Global.propCameraGlobal.PropInicial.PosX + Global.propCameraGlobal.PosX, 
                    Global.propCameraGlobal.PropInicial.PosY + Global.propCameraGlobal.PosY, 
                    Global.propCameraGlobal.PropInicial.PosZ + Global.propCameraGlobal.PosZ);

            camObjPos.transform.localPosition =
                new Vector3(-Global.propCameraGlobal.PosX * 14, Global.propCameraGlobal.PosY * 13.333333f, -Global.propCameraGlobal.PosZ * 16);

            //Atualiza FOV da camera (Scale)
            camObjMain.transform.localScale =
                new Vector3(Global.propCameraGlobal.PropInicial.FOV.x * Global.propCameraGlobal.FOV.x, Global.propCameraGlobal.PropInicial.FOV.y * Global.propCameraGlobal.FOV.y, camObjMain.transform.localScale.z);
        }
        else
        {
            PreencheCampos();
        }
    }

    float ConvertFloat(string input)
    {
        return float.Parse(string.Empty.Equals(input) ? "0" : input, CultureInfo.InvariantCulture.NumberFormat);
    }

    public void AjustaCameraEmX()
    {
        RectTransform rectTransf = camObjMain.transform.GetComponent<RectTransform>();
        rectTransf.transform.position = new Vector3(rectTransf.transform.position.x + 1, rectTransf.transform.position.y, rectTransf.transform.position.z);
    }

    public void AdicionaValorPropriedade()
    {
        UpdatePosition();
    }

    void AtualizaPainel()
    {
        labelPosicao.transform.localPosition =
            new Vector3(labelPosicao.transform.localPosition.x, labelPosicao.transform.localPosition.y, Global.Grafico2D ? 50 : -2.999623f);
        FOV.transform.localPosition =
            new Vector3(FOV.transform.localPosition.x, Global.Grafico2D ? 0.110f : 0, Global.Grafico2D ? 50 : -2.999623f);
    }
}
