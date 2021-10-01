using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropCameraScript : MonoBehaviour
{
    const float FOV_INICIAL = 45;

    //enum TipoLock
    //{
    //    LockX,
    //    LockY,
    //    LockZ,
    //    LockFOV
    //}

    //Dictionary<TipoLock, string> fieldsLocks;
    //Dictionary<TipoLock, Toggle> fieldsToggleLocks;

    [SerializeField]
    Transform cuboAmb;
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject objFOV;
    [SerializeField]
    GameObject objPosZ;
    [SerializeField]
    InputField nomeCamera;
    [SerializeField]
    InputField posX;
    [SerializeField]
    InputField posY;
    [SerializeField]
    InputField posZ;
    [SerializeField]
    InputField fieldFOV;
    [SerializeField]
    GameObject camObjMain;
    [SerializeField]
    GameObject camObjPos;
    [SerializeField]
    GameObject camVisInferior;
    [SerializeField]
    Toggle toggleLockPosX;
    [SerializeField]
    Toggle toggleLockPosY;
    [SerializeField]
    Toggle toggleLockPosZ;
    [SerializeField]
    Toggle toggleLockFOV;

    float time;
    bool podeAtualizarCamera;

    void Start()
    {
        PreencheCampos();
    }

    void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            EnabledZ();
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

        InitPropCam();
        nomeCamera.text = string.Empty.Equals(Global.propCameraGlobal.Nome) ? "Camera" : Global.propCameraGlobal.Nome;
        posX.text = Global.propCameraGlobal.PosX.Equals(0) ? "0" : Global.propCameraGlobal.PosX.ToString();
        posY.text = Global.propCameraGlobal.PosY.Equals(0) ? "0" : Global.propCameraGlobal.PosY.ToString();
        posZ.text = Global.propCameraGlobal.PosZ.Equals(0) ? "0" : Global.propCameraGlobal.PosZ.ToString();
        fieldFOV.text = Global.propCameraGlobal.FOV.Equals(0) ? "45" : Global.propCameraGlobal.FOV.ToString();

        camObjMain.transform.position =
            new Vector3(Global.propCameraGlobal.PropInicial.PosX + Global.propCameraGlobal.PosX,
                        Global.propCameraGlobal.PropInicial.PosY + Global.propCameraGlobal.PosY,
                        Global.propCameraGlobal.PropInicial.PosZ + Global.propCameraGlobal.PosZ);

        camObjPos.transform.localPosition =
            new Vector3(-Global.propCameraGlobal.PosX * 14, Global.propCameraGlobal.PosY * 13.333333f, -Global.propCameraGlobal.PosZ * 16);
    }

    public void UpdatePosition()
    {
        Global.propCameraGlobal.PosX = -Util_VisEdu.ConvertField(posX.text);
        Global.propCameraGlobal.PosY = Util_VisEdu.ConvertField(posY.text);
        Global.propCameraGlobal.PosZ = Util_VisEdu.ConvertField(posZ.text);
        Global.propCameraGlobal.FOV = Util_VisEdu.ConvertField(fieldFOV.text);
        //Altera FoV (Field of View)
        camVisInferior.GetComponent<Camera>().fieldOfView = Util_VisEdu.ConvertField(fieldFOV.text);
        podeAtualizarCamera = true;

        //Atualiza posição da camera
        if (Global.propCameraGlobal.JaIniciouValores)
        {
            camObjMain.transform.position =
                    new Vector3(Global.propCameraGlobal.PropInicial.PosX + Global.propCameraGlobal.PosX,
                        Global.propCameraGlobal.PropInicial.PosY + Global.propCameraGlobal.PosY,
                        Global.propCameraGlobal.PropInicial.PosZ + Global.propCameraGlobal.PosZ);

            camObjPos.transform.localPosition =
                new Vector3(-Global.propCameraGlobal.PosX * 14, Global.propCameraGlobal.PosY * 13.333333f, -Global.propCameraGlobal.PosZ * 16);

            //Atualiza FOV da camera (Scale)
            camObjMain.transform.localScale =
                new Vector3(Global.propCameraGlobal.PropInicial.FOV.x * Global.propCameraGlobal.FOV, Global.propCameraGlobal.PropInicial.FOV.y * Global.propCameraGlobal.FOV, camObjMain.transform.localScale.z);
        }
        else
        {
            PreencheCampos();
        }

        UpdateLockFields();
    }

    public void UpdateLockFields()
    {
        if (!toggleLockPosX.isOn)
        {
            Global.propCameraGlobal.ListPropCamLocks.Remove("PosX");
            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (Global.propCameraGlobal.ListPropCamLocks.ContainsKey("PosX"))
            {
                Global.propCameraGlobal.ListPropCamLocks["PosX"] = Util_VisEdu.ConvertField(posX.text).ToString();
            }
            else
            {
                Global.propCameraGlobal.ListPropCamLocks.Add("PosX", Util_VisEdu.ConvertField(posX.text).ToString());
            }

            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosY.isOn)
        {
            Global.propCameraGlobal.ListPropCamLocks.Remove("PosY");
            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (Global.propCameraGlobal.ListPropCamLocks.ContainsKey("PosY"))
            {
                Global.propCameraGlobal.ListPropCamLocks.Remove("PosY");
            }
            else
            {
                Global.propCameraGlobal.ListPropCamLocks.Add("PosY", Util_VisEdu.ConvertField(posY.text).ToString());
            }

            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosZ.isOn)
        {
            Global.propCameraGlobal.ListPropCamLocks.Remove("PosZ");
            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (Global.propCameraGlobal.ListPropCamLocks.ContainsKey("PosZ"))
            {
                Global.propCameraGlobal.ListPropCamLocks.Remove("PosZ");
            }
            else
            {
                Global.propCameraGlobal.ListPropCamLocks.Add("PosZ", Util_VisEdu.ConvertField(posZ.text).ToString());
            }

            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockFOV.isOn)
        {
            Global.propCameraGlobal.ListPropCamLocks.Remove("FOV");
            toggleLockFOV.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (Global.propCameraGlobal.ListPropCamLocks.ContainsKey("FOV"))
            {
                Global.propCameraGlobal.ListPropCamLocks.Remove("FOV");
            }
            else
            {
                Global.propCameraGlobal.ListPropCamLocks.Add("FOV", Util_VisEdu.ConvertField(fieldFOV.text).ToString());
            }

            toggleLockFOV.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }

    public void InitPropCam()
    {
        Global.propCameraGlobal.PropInicial.PosX = camObjMain.transform.position.x;
        Global.propCameraGlobal.PropInicial.PosY = camObjMain.transform.position.y;
        Global.propCameraGlobal.PropInicial.PosZ = camObjMain.transform.position.z;
        Global.propCameraGlobal.PropInicial.FOV = new Vector2(camObjMain.transform.localScale.x / FOV_INICIAL, camObjMain.transform.localScale.y / FOV_INICIAL);
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

    void EnabledZ()
    {
        objPosZ.SetActive(!Global.Grafico2D);
        objFOV.SetActive(!Global.Grafico2D);
    }
}
