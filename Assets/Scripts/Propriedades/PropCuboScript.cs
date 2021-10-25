using UnityEngine;
using UnityEngine.UI;

public class PropCuboScript : MonoBehaviour
{
    [SerializeField]
    GameObject tamZCubo;
    [SerializeField]
    GameObject posZCubo;
    [SerializeField]
    InputField nomePeca;
    [SerializeField]
    InputField tamX;
    [SerializeField]
    InputField tamY;
    [SerializeField]
    InputField tamZ;
    [SerializeField]
    InputField posX;
    [SerializeField]
    InputField posY;
    [SerializeField]
    InputField posZ;
    [SerializeField]
    FlexibleColorPicker corSelecionado;
    [SerializeField]
    GameObject seletorCor;
    [SerializeField]
    GameObject texturaSelecionada;
    [SerializeField]
    Toggle ativo;
    [SerializeField]
    Toggle lockPosX;
    [SerializeField]
    Toggle lockPosY;
    [SerializeField]
    Toggle lockPosZ;
    [SerializeField]
    Toggle lockTamX;
    [SerializeField]
    Toggle lockTamY;
    [SerializeField]
    Toggle lockTamZ;

    CuboPropriedadePeca prPeca;
    GameObject goTransformacaoAmb;
    GameObject goTransformacaoVis;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (Global.gameObjectName != null && Global.gameObjectName.Contains(Consts.CUBO))
        {
            EnabledZ();
            UpdateColor();
        }
    }

    public void Inicializa()
    {
        prPeca = Global.propriedadePecas[Global.gameObjectName] as CuboPropriedadePeca;
        prPeca.Ativo = true;

        AtualizaListaProp();
        PreencheCampos();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(Global.gameObjectName))
        {
            podeAtualizar = false;
            try
            {
                nomePeca.text = prPeca.Nome;
                tamX.text = prPeca.Tam.X.ToString();
                tamY.text = prPeca.Tam.Y.ToString();
                tamZ.text = prPeca.Tam.Z.ToString();
                posX.text = prPeca.Pos.X.ToString();
                posY.text = prPeca.Pos.Y.ToString();
                posZ.text = prPeca.Pos.Z.ToString();
                corSelecionado.color = prPeca.Cor;
                texturaSelecionada.GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
                ativo.isOn = prPeca.Ativo;

                goTransformacaoAmb = GameObject.Find(prPeca.NomeCuboAmbiente);
                goTransformacaoVis = GameObject.Find(prPeca.NomeCuboVis);
            }
            finally
            {
                podeAtualizar = true;
                UpdateProp();
            }
        }
    }

    public void UpdateProp()
    {
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(Global.gameObjectName))
        {
            podeAtualizar = false;
            try
            {
                prPeca.Nome = nomePeca.text;
                prPeca.Tam.X = Util_VisEdu.ConvertField(tamX.text, true);
                prPeca.Tam.Y = Util_VisEdu.ConvertField(tamY.text, true);
                prPeca.Tam.Z = Util_VisEdu.ConvertField(tamZ.text, true);

                prPeca.Pos.X = Util_VisEdu.ConvertField(posX.text);
                prPeca.Pos.X = Util_VisEdu.ConvertField(posY.text);
                prPeca.Pos.X = Util_VisEdu.ConvertField(posZ.text);

                if (goTransformacaoAmb != null)
                {
                    goTransformacaoAmb.transform.localPosition = new Vector3(prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
                    goTransformacaoAmb.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
                }

                if (goTransformacaoVis != null)
                {
                    goTransformacaoVis.transform.localPosition = new Vector3(prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
                    goTransformacaoVis.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
                }

                ConfigCuboAmb();

                UpdateLockFields();
                UpdateColor();
            }
            finally
            {
                podeAtualizar = true;
            }
        }
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(Global.gameObjectName))
        {
            Global.propriedadePecas[Global.gameObjectName] = prPeca;
        }
    }

    void ConfigCuboAmb()
    {
        PropriedadePeca propObjGraf = Global.propriedadePecas["ObjetoGraficoP" + Util_VisEdu.GetSlot(prPeca.Nome)];
        MeshRenderer meshRendererCubo = GameObject.Find(prPeca.NomeCuboAmbiente).GetComponent<MeshRenderer>();
        meshRendererCubo.enabled = prPeca.Ativo && propObjGraf.Ativo;
    }

    public void ControlCorPanel()
    {
        corSelecionado.gameObject.SetActive(!corSelecionado.gameObject.activeSelf);
    }

    public void UpdateLockFields()
    {
        if (podeAtualizar)
        {
            if (!lockPosX.isOn)
            {
                prPeca.ListPropLocks.Remove("PosX");
                lockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosX"))
                {
                    prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(posX.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(posX.text).ToString());
                }

                lockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockPosY.isOn)
            {
                prPeca.ListPropLocks.Remove("PosY");
                lockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosY"))
                {
                    prPeca.ListPropLocks["PosY"] = Util_VisEdu.ConvertField(posY.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(posY.text).ToString());
                }

                lockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockPosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("PosZ");
                lockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosZ"))
                {
                    prPeca.ListPropLocks["PosZ"] = Util_VisEdu.ConvertField(posZ.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(posZ.text).ToString());
                }

                lockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockTamX.isOn)
            {
                prPeca.ListPropLocks.Remove("TamX");
                lockTamX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("TamX"))
                {
                    prPeca.ListPropLocks["TamX"] = Util_VisEdu.ConvertField(tamX.text, true).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("TamX", Util_VisEdu.ConvertField(tamX.text, true).ToString());
                }

                lockTamX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockTamY.isOn)
            {
                prPeca.ListPropLocks.Remove("TamY");
                lockTamY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("TamY"))
                {
                    prPeca.ListPropLocks["TamY"] = Util_VisEdu.ConvertField(tamY.text, true).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("TamY", Util_VisEdu.ConvertField(tamY.text, true).ToString());
                }

                lockTamY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockTamZ.isOn)
            {
                prPeca.ListPropLocks.Remove("TamZ");
                lockTamZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("TamZ"))
                {
                    prPeca.ListPropLocks["TamZ"] = Util_VisEdu.ConvertField(tamZ.text, true).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("TamZ", Util_VisEdu.ConvertField(tamZ.text, true).ToString());
                }

                lockTamZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }
        }
    }

    void UpdateColor()
    {
        seletorCor.GetComponent<Image>().material.color = corSelecionado.color;

        if (goTransformacaoAmb != null)
        {
            goTransformacaoAmb.GetComponent<MeshRenderer>().materials[0].color = corSelecionado.color;
        }
        if (goTransformacaoVis != null)
        {
            goTransformacaoVis.GetComponent<MeshRenderer>().materials[0].color = corSelecionado.color;
        }
    }

    void EnabledZ()
    {
        tamZCubo.SetActive(!Global.Grafico2D);
        posZCubo.SetActive(!Global.Grafico2D);
    }
}
