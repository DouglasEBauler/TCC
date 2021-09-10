﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;

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
    GameObject fov;
    [SerializeField]
    GameObject labelPosicao;
    [SerializeField]
    TMP_InputField nomeCamera;
    [SerializeField]
    TMP_InputField posX;
    [SerializeField]
    TMP_InputField posY;
    [SerializeField]
    TMP_InputField posZ;
    [SerializeField]
    TMP_InputField camFOV;
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
            posX.text = "0";
            posY.text = "0";
            posZ.text = "0";
            camFOV.text = "45";

            Global.propCameraGlobal.PropInicial.PosX = camObjMain.transform.position.x;
            Global.propCameraGlobal.PropInicial.PosY = camObjMain.transform.position.y;
            Global.propCameraGlobal.PropInicial.PosZ = camObjMain.transform.position.z;
            Global.propCameraGlobal.PropInicial.FOV =
                new Vector2(camObjMain.transform.localScale.x / FOV_INICIAL, camObjMain.transform.localScale.y / FOV_INICIAL);

            Global.propCameraGlobal.PosX = POS_X_INICIAL;
            Global.propCameraGlobal.PosY = POS_Y_INICIAL;
            Global.propCameraGlobal.PosZ = POS_Z_INICIAL;
            Global.propCameraGlobal.FOV = FOV_INICIAL;
        }
        else
        {
            posX.text = Global.propCameraGlobal.PosX.ToString();
            posY.text = Global.propCameraGlobal.PosY.ToString();
            posZ.text = Global.propCameraGlobal.PosZ.ToString();
            camFOV.text = Global.propCameraGlobal.FOV.ToString();
        }

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
        Global.propCameraGlobal.FOV = Util_VisEdu.ConvertField(camFOV.text);
        //Altera FoV (Field of View)
        camVisInferior.GetComponent<Camera>().fieldOfView = Util_VisEdu.ConvertField(camFOV.text);
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
            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosZ.isOn)
        {
            Global.propCameraGlobal.ListPropCamLocks.Remove("PosZ");
            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockFOV.isOn)
        {
            Global.propCameraGlobal.ListPropCamLocks.Remove("FOV");
            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (Global.propCameraGlobal.ListPropCamLocks.ContainsKey("FOV"))
            {
                Global.propCameraGlobal.ListPropCamLocks.Remove("FOV");
            }
            else
            {
                Global.propCameraGlobal.ListPropCamLocks.Add("FOV", Util_VisEdu.ConvertField(camFOV.text).ToString());
            }

            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
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
        fov.transform.localPosition =
            new Vector3(fov.transform.localPosition.x, Global.Grafico2D ? 0.110f : 0, Global.Grafico2D ? 50 : -2.999623f);
    }
}