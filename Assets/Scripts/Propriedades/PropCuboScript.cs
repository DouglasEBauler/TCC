using UnityEngine;
using TMPro;
using System.Globalization;
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
    Toggle toggleCubo;
    [SerializeField]
    Toggle toggleLockPosX;
    [SerializeField]
    Toggle toggleLockPosY;
    [SerializeField]
    Toggle toggleLockPosZ;
    [SerializeField]
    Toggle toggleLockTamX;
    [SerializeField]
    Toggle toggleLockTamY;
    [SerializeField]
    Toggle toggleLockTamZ;

    CuboPropriedadePeca prPeca;
    GameObject goTransformacaoAmb;
    GameObject goTransformacaoVis;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (Global.gameObjectName != null && Global.gameObjectName.Contains(Consts.CUBO))
        {
            EnabledZ();
            UpdateProp();
            UpdateColor();
        }
    }

    public void Inicializa()
    {
        prPeca = Global.propriedadePecas[Global.gameObjectName] as CuboPropriedadePeca;

        if (prPeca == null)
        {
            prPeca.Ativo = true;
            AtualizaListaProp();
        }

        PreencheCampos();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            nomePeca.text = prPeca.Nome;
            toggleCubo.isOn = prPeca.Ativo;

            InstanceTransform();

            podeAtualizar = false;
            try
            {
                tamX.text = prPeca.Tam.X.ToString();
                tamY.text = prPeca.Tam.Y.ToString();
                tamZ.text = prPeca.Tam.Z.ToString();
                posX.text = prPeca.Pos.X.ToString();
                posY.text = prPeca.Pos.Y.ToString();
                posZ.text = prPeca.Pos.Z.ToString();
                corSelecionado.color = prPeca.Cor;
                texturaSelecionada.GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;

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

    void UpdateProp()
    {
        prPeca.Nome = prPeca.Nome ?? "Cubo";

        if (Global.propriedadePecas.ContainsKey(prPeca.Nome) && podeAtualizar)
        {
            prPeca.Tam.X = Util_VisEdu.ConvertField(tamX.text, true);
            prPeca.Tam.Y = Util_VisEdu.ConvertField(tamY.text, true);
            prPeca.Tam.Z = Util_VisEdu.ConvertField(tamZ.text, true);

            prPeca.Pos.X = Util_VisEdu.ConvertField(posX.text);
            prPeca.Pos.X = Util_VisEdu.ConvertField(posY.text);
            prPeca.Pos.X = Util_VisEdu.ConvertField(posZ.text);

            if (goTransformacaoAmb != null && goTransformacaoVis != null)
            {
                float x = prPeca.Pos.X * -1;

                goTransformacaoAmb.transform.localPosition = new Vector3(x, prPeca.Pos.Y, prPeca.Pos.Z);
                goTransformacaoVis.transform.localPosition = new Vector3(x, prPeca.Pos.Y, prPeca.Pos.Z);
                goTransformacaoAmb.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
                goTransformacaoVis.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
            }
        }

        UpdateLockFields();
        UpdateColor();
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            Global.propriedadePecas.Remove(prPeca.Nome);
            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
        }
    }

    void ToggleChanged()
    {
        if (Global.gameObjectName != null)
        {
            prPeca.Ativo = toggleCubo.isOn;
            UpdateProp();

            string goObjGraficoSlotPai = GameObject.Find(GameObject.Find(Global.listaEncaixes[Global.gameObjectName]).transform.parent.name).name;

            string pecaObjGrafico = string.Empty;

            // Pega o nome da peça conectada ao "goObjGraficoSlotPai"
            foreach (var peca in Global.listaEncaixes)
            {
                if (peca.Value.Equals(goObjGraficoSlotPai))
                {
                    pecaObjGrafico = peca.Key;
                    break;
                }
            }

            bool existeProp = false;

            foreach (var pec in Global.propriedadePecas)
            {
                if (pec.Key.Equals(pecaObjGrafico))
                {
                    existeProp = true;
                    break;
                }
            }

            MeshRenderer meshRendererCubo = GameObject.Find(prPeca.NomeCuboAmbiente).GetComponent<MeshRenderer>();
            meshRendererCubo.enabled = (existeProp && Global.propriedadePecas[pecaObjGrafico].Ativo) || !existeProp ? prPeca.Ativo : false;
        }
    }

    void InstanceTransform()
    {
        if (prPeca != null)
        {
            prPeca.Pos = prPeca.Pos ?? new Posicao();
            prPeca.Pos.X = 0;
            prPeca.Pos.Y = 0;
            prPeca.Pos.Z = 0;

            prPeca.Tam = prPeca.Tam ?? new Tamanho();
            prPeca.Tam.X = 1;
            prPeca.Tam.Y = 1;
            prPeca.Tam.Z = 1;

            prPeca.Cor = Color.white;
            prPeca.Ativo = true;
        }
    }

    public void ControlCorPanel()
    {
        corSelecionado.gameObject.SetActive(!corSelecionado.gameObject.activeSelf);
    }

    public void AdicionaValorPropriedade()
    {
        ToggleChanged();
    }

    public void UpdateLockFields()
    {
        if (!toggleLockPosX.isOn)
        {
            prPeca.ListPropLocks.Remove("PosX");
            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosY.isOn)
        {
            prPeca.ListPropLocks.Remove("PosY");
            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosZ.isOn)
        {
            prPeca.ListPropLocks.Remove("PosZ");
            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockTamX.isOn)
        {
            prPeca.ListPropLocks.Remove("TamX");
            toggleLockTamX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockTamX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockTamY.isOn)
        {
            prPeca.ListPropLocks.Remove("TamY");
            toggleLockTamY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockTamY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockTamZ.isOn)
        {
            prPeca.ListPropLocks.Remove("TamZ");
            toggleLockTamZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockTamZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
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
