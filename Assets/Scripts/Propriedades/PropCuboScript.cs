using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;

public class PropCuboScript : MonoBehaviour
{
    enum TypeTransf { Pos, Tam };

    [SerializeField]
    GameObject labelTamZCubo;
    [SerializeField]
    GameObject labelPosZCubo;
    [SerializeField]
    TMP_InputField nomePeca;
    [SerializeField]
    TMP_InputField tamX;
    [SerializeField]
    TMP_InputField tamY;
    [SerializeField]
    TMP_InputField tamZ;
    [SerializeField]
    TMP_InputField posX;
    [SerializeField]
    TMP_InputField posY;
    [SerializeField]
    TMP_InputField posZ;
    [SerializeField]
    GameObject corSelecionada;
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

    PropriedadePeca prPeca;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (Global.gameObjectName != null && Global.gameObjectName.Contains(Consts.CUBO))
        {
            EnableLabelZ();
            UpdateProp();
        }
    }

    public void Inicializa()
    {
        prPeca = Global.propriedadePecas[Global.gameObjectName];

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
                corSelecionada.GetComponent<MeshRenderer>().materials[0].color = prPeca.Cor;
                texturaSelecionada.GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
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
            prPeca.Tam.X = ConvertField(tamX.text, TypeTransf.Tam);
            prPeca.Tam.Y = ConvertField(tamY.text, TypeTransf.Tam);
            prPeca.Tam.Z = ConvertField(tamZ.text, TypeTransf.Tam);

            prPeca.Pos.X = ConvertField(posX.text, TypeTransf.Pos);
            prPeca.Pos.X = ConvertField(posY.text, TypeTransf.Pos);
            prPeca.Pos.X = ConvertField(posZ.text, TypeTransf.Pos);

            GameObject goTransformacaoAmb = GameObject.Find(prPeca.NomeCuboAmbiente);
            GameObject goTransformacaoVis = GameObject.Find(prPeca.NomeCuboVis);

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

    float ConvertField(string inputProp, TypeTransf t)
    {
        return float.Parse(ValidaVazio(inputProp, t), CultureInfo.InvariantCulture.NumberFormat);
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

    string ValidaVazio(string valor, TypeTransf tipo)
    {
        return string.Empty.Equals(valor) ? (TypeTransf.Tam.Equals(tipo) ? "1" : "0") : valor;
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
                prPeca.ListPropLocks["PosX"] = ConvertField(posX.text, TypeTransf.Pos).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("PosX", ConvertField(posX.text, TypeTransf.Pos).ToString());
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
                prPeca.ListPropLocks["PosY"] = ConvertField(posY.text, TypeTransf.Pos).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("PosY", ConvertField(posY.text, TypeTransf.Pos).ToString());
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
                prPeca.ListPropLocks["PosZ"] = ConvertField(posZ.text, TypeTransf.Pos).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("PosZ", ConvertField(posZ.text, TypeTransf.Pos).ToString());
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
                prPeca.ListPropLocks["TamX"] = ConvertField(tamX.text, TypeTransf.Tam).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("TamX", ConvertField(tamX.text, TypeTransf.Tam).ToString());
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
                prPeca.ListPropLocks["TamY"] = ConvertField(tamY.text, TypeTransf.Tam).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("TamY", ConvertField(tamY.text, TypeTransf.Tam).ToString());
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
                prPeca.ListPropLocks["TamZ"] = ConvertField(tamZ.text, TypeTransf.Tam).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("TamZ", ConvertField(tamZ.text, TypeTransf.Tam).ToString());
            }

            toggleLockTamZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }

    void EnableLabelZ()
    {
        labelTamZCubo.transform.localPosition =
            new Vector3(labelTamZCubo.transform.localPosition.x, labelTamZCubo.transform.localPosition.y, Global.Grafico2D ? 100000 : 0.000111648f);
        labelPosZCubo.transform.localPosition =
            new Vector3(labelPosZCubo.transform.localPosition.x, labelPosZCubo.transform.localPosition.y, Global.Grafico2D ? 100000 : 0.000111648f);
    }
}
