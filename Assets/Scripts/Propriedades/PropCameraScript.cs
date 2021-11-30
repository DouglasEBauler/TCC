using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropCameraScript : MonoBehaviour
{
    const float FOV_INICIAL = 45;

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
    GameObject camVisInferior;
    [SerializeField]
    GameObject render;
    [SerializeField]
    GameObject posicaVis;
    [SerializeField]
    Toggle lockPosX;
    [SerializeField]
    Toggle lockPosY;
    [SerializeField]
    Toggle lockPosZ;
    [SerializeField]
    Toggle lockFOV;

    Dictionary<Property, InputField> propList;
    Dictionary<Property, Toggle> lockList;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            EnabledZ();
        }
    }

    public void DemosntraCamera(bool demonstra)
    {
        foreach (Transform child in camObjMain.transform)
            child.transform.gameObject.SetActive(demonstra);

        Global.cameraAtiva = demonstra;
        podeAtualizar = demonstra;
    }

    public void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(Global.propCameraGlobal.NomePeca))
        {
            podeAtualizar = false;
            try
            {
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

                camVisInferior.transform.localPosition =
                    new Vector3(camVisInferior.transform.localPosition.x + Global.propCameraGlobal.PosX,
                                camVisInferior.transform.localPosition.y + Global.propCameraGlobal.PosY,
                                camVisInferior.transform.localPosition.z + Global.propCameraGlobal.PosZ);
            }
            finally
            {
                podeAtualizar = true;
                UpdateProp();
            }
        }
    }

    void InicializaListas()
    {
        propList = new Dictionary<Property, InputField>()
        {
            { Property.PosX, posX },
            { Property.PosY, posY },
            { Property.PosZ, posZ },
            { Property.FOV, fieldFOV }
        };

        lockList = new Dictionary<Property, Toggle>()
        {
            { Property.PosX, lockPosX },
            { Property.PosY, lockPosY },
            { Property.PosZ, lockPosZ },
            { Property.FOV, lockFOV }
        };
    }

    public void UpdateProp()
    {
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(Global.propCameraGlobal.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                Global.propCameraGlobal.PosX = Util_VisEdu.ConvertField(posX.text);
                Global.propCameraGlobal.PosY = Util_VisEdu.ConvertField(posY.text);
                Global.propCameraGlobal.PosZ = Util_VisEdu.ConvertField(posZ.text);
                Global.propCameraGlobal.FOV = Util_VisEdu.ConvertField(string.Empty.Equals(fieldFOV.text) ? "45" : fieldFOV.text);

                //Atualiza posição da camera
                camObjMain.transform.position =
                        new Vector3(Global.propCameraGlobal.PropInicial.PosX + Global.propCameraGlobal.PosX,
                            Global.propCameraGlobal.PropInicial.PosY + Global.propCameraGlobal.PosY,
                            Global.propCameraGlobal.PropInicial.PosZ + Global.propCameraGlobal.PosZ);
                //Atualiza FOV da camera (Scale)
                camObjMain.transform.localScale =
                    new Vector3(Global.propCameraGlobal.PropInicial.FOV.x * Global.propCameraGlobal.FOV, Global.propCameraGlobal.PropInicial.FOV.y * Global.propCameraGlobal.FOV, camObjMain.transform.localScale.z);

                camVisInferior.transform.localPosition =
                    new Vector3(camVisInferior.transform.localPosition.x + Global.propCameraGlobal.PosX, camVisInferior.transform.localPosition.y + Global.propCameraGlobal.PosY, camVisInferior.transform.localPosition.z);

                //Altera FoV (Field of View)
                camVisInferior.GetComponent<Camera>().fieldOfView = Global.propCameraGlobal.FOV;
                camVisInferior.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");
                UpdateAllLockFields();
            }
            finally
            {
                podeAtualizar = true;
                UpdatePropList();
            }
        }
    }

    public void UpdateAllLockFields()
    {
        if (lockList == null || propList == null)
        {
            InicializaListas();
        }

        foreach (var lockItem in lockList)
        {
            UpdateLockFields(lockItem.Key);
        }
    }

    void UpdateLockFields(Property typeProperty)
    {
        if (lockList == null || propList == null)
        {
            InicializaListas();
        }

        if (!lockList[typeProperty].isOn)
        {
            Global.propCameraGlobal.ListPropLocks.Remove(typeProperty);
            lockList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (Global.propCameraGlobal.ListPropLocks.ContainsKey(typeProperty))
            {
                Global.propCameraGlobal.ListPropLocks[typeProperty] = Util_VisEdu.ConvertField(propList[typeProperty].text).ToString();
            }
            else
            {
                Global.propCameraGlobal.ListPropLocks.Add(typeProperty, Util_VisEdu.ConvertField(propList[typeProperty].text).ToString());
            }

            lockList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }

    public void InitPropCam()
    {
        Global.propCameraGlobal.PropInicial.PosX = camObjMain.transform.position.x;
        Global.propCameraGlobal.PropInicial.PosY = camObjMain.transform.position.y;
        Global.propCameraGlobal.PropInicial.PosZ = camObjMain.transform.position.z;
        RectTransform rectTransf = camObjMain.transform.GetComponent<RectTransform>();
        Global.propCameraGlobal.PropInicial.FOV = new Vector2(rectTransf.localScale.x / FOV_INICIAL, rectTransf.localScale.y / FOV_INICIAL);
    }

    void EnabledZ()
    {
        objPosZ.SetActive(!Global.Grafico2D);
        objFOV.SetActive(!Global.Grafico2D);
    }

    void UpdatePropList()
    {
        if (Global.propriedadePecas.ContainsKey(Global.propCameraGlobal.NomePeca))
        {
            Global.propriedadePecas[Global.propCameraGlobal.NomePeca] = Global.propCameraGlobal;
        }
    }
    public void EnabledPecasVis(string notEnablePeca, bool enabled)
    {
        foreach (var prop in Global.listaEncaixes)
        {
            if (!prop.Key.Equals(notEnablePeca)
                && (prop.Key.Contains(Consts.CUBO) 
                    || prop.Key.Contains(Consts.POLIGONO) 
                    || prop.Key.Contains(Consts.SPLINE)))
            {
                string numSlot = Util_VisEdu.GetNumSlot(prop.Value);

                GameObject pecaObjVis = GameObject.Find(Consts.CUBO_VIS + numSlot);
                if (pecaObjVis != null)
                {
                    pecaObjVis.GetComponent<MeshRenderer>().enabled = enabled;
                }
                else
                {
                    pecaObjVis = GameObject.Find(Consts.POLIGONO_VIS + numSlot);
                    if (pecaObjVis != null)
                    {
                        pecaObjVis.GetComponent<MeshRenderer>().enabled = enabled;
                    }
                    else
                    {
                        pecaObjVis = GameObject.Find(Consts.SPLINE_VIS + numSlot);
                        if (pecaObjVis != null)
                        {
                            pecaObjVis.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = enabled;
                        }
                    }
                }
            }
        }
    }
}
