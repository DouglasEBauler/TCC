using UnityEngine;
using UnityEngine.UI;

public class PropPoligonoScript : MonoBehaviour
{
    [SerializeField]
    InputField nome;
    [SerializeField]
    InputField pontos;
    [SerializeField]
    InputField posX;
    [SerializeField]
    InputField posY;
    [SerializeField]
    InputField posZ;
    [SerializeField]
    Dropdown primitiva;
    [SerializeField]
    Button corSeletor;
    [SerializeField]
    FlexibleColorPicker corSelecionada;
    [SerializeField]
    Toggle lockQtdPontos;
    [SerializeField]
    Toggle lockPosX;
    [SerializeField]
    Toggle lockPosY;
    [SerializeField]
    Toggle lockPosZ;
    [SerializeField]
    Toggle ativo;

    PoligonoPropriedadePeca propPeca;
    GameObject goPoligonoAmb;
    bool podeAtualizar;

    public void Inicializa()
    {
        propPeca = Global.propriedadePecas[Global.gameObjectName] as PoligonoPropriedadePeca;
        propPeca.Ativo = true;

        PreencheCampos();
        AtualizaListaProp();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(propPeca.Nome))
        {
            podeAtualizar = false;
            try
            {
                nome.text = propPeca.Nome;
                posX.text = propPeca.Pos.X.ToString();
                posY.text = propPeca.Pos.Y.ToString();
                posZ.text = propPeca.Pos.Z.ToString();
                pontos.text = propPeca.Pontos.ToString();
                primitiva.value = (int)TipoPrimitiva.Cheio;
                ativo.isOn = propPeca.Ativo;

                corSeletor.GetComponent<Image>().material.color = propPeca.Cor;
                goPoligonoAmb = GameObject.Find(propPeca.PoligonoAmbiente);
                goPoligonoAmb.GetComponent<PoligonoAmbScript>().PropPeca = propPeca;
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
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(propPeca.Nome))
        {
            propPeca.Nome = nome.text;
            propPeca.Pontos = Util_VisEdu.ConvertField(pontos.text);
            propPeca.Pos.X = Util_VisEdu.ConvertField(posX.text);
            propPeca.Pos.Y = Util_VisEdu.ConvertField(posY.text);
            propPeca.Pos.Z = Util_VisEdu.ConvertField(posZ.text);
            propPeca.Cor = corSelecionada.color;
            propPeca.Primitiva = (TipoPrimitiva)primitiva.value;
            propPeca.Ativo = ativo.isOn;

            corSeletor.GetComponent<Image>().material.color = propPeca.Cor;

            UpdateLockFields();
            UpdatePoligonoAmbiente();
        }
    }

    void UpdatePoligonoAmbiente()
    {
        if (goPoligonoAmb != null)
        {
            goPoligonoAmb.transform.localPosition = new Vector3(propPeca.Pos.X, propPeca.Pos.Y, propPeca.Pos.Z);
            goPoligonoAmb.GetComponent<MeshRenderer>().materials[0].color = propPeca.Cor;
            goPoligonoAmb.GetComponent<MeshRenderer>().enabled = propPeca.Ativo;
            if (propPeca.Ativo)
            {
                goPoligonoAmb.GetComponent<PoligonoAmbScript>().ConfiguratePoligono();
            }
        }
    }

    public void ControlPanelColor()
    {
        corSelecionada.gameObject.SetActive(!corSelecionada.gameObject.activeSelf);
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(propPeca.Nome))
        {
            Global.propriedadePecas[propPeca.Nome] = propPeca;
        }
    }

    public void UpdateLockFields()
    {
        if (!lockQtdPontos.isOn)
        {
            propPeca.ListPropLocks.Remove("Pontos");
            lockQtdPontos.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("Pontos"))
            {
                propPeca.ListPropLocks["Pontos"] = Util_VisEdu.ConvertField(posX.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("Pontos", Util_VisEdu.ConvertField(posX.text).ToString());
            }

            lockQtdPontos.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockPosX.isOn)
        {
            propPeca.ListPropLocks.Remove("PosX");
            lockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("PosX"))
            {
                propPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(posX.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(posX.text).ToString());
            }

            lockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockPosY.isOn)
        {
            propPeca.ListPropLocks.Remove("PosY");
            lockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("PosY"))
            {
                propPeca.ListPropLocks["PosY"] = Util_VisEdu.ConvertField(posY.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(posY.text).ToString());
            }

            lockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockPosZ.isOn)
        {
            propPeca.ListPropLocks.Remove("PosZ");
            lockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("PosZ"))
            {
                propPeca.ListPropLocks["PosZ"] = Util_VisEdu.ConvertField(posZ.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(posZ.text).ToString());
            }

            lockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }
}
