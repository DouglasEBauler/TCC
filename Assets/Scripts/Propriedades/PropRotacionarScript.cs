using UnityEngine;
using UnityEngine.UI;

public class PropRotacionarScript : MonoBehaviour
{
    [SerializeField]
    GameObject labelZ;
    [SerializeField]
    InputField nomePeca;
    [SerializeField]
    InputField posX;
    [SerializeField]
    InputField posY;
    [SerializeField]
    InputField posZ;
    [SerializeField]
    Toggle ativo;
    [SerializeField]
    Toggle lockPosX;
    [SerializeField]
    Toggle lockPosY;
    [SerializeField]
    Toggle lockPosZ;

    TransformacaoPropriedadePeca prPeca;
    bool podeAtualizar;

    public void Inicializa(TransformacaoPropriedadePeca propTransformacao)
    {
        prPeca = propTransformacao;
        prPeca.Ativo = true;
        podeAtualizar = true;

        PreencheCampos();
    }

    void PreencheCampos()
    {
        podeAtualizar = false;
        try
        {
            nomePeca.text = prPeca.Nome;

            if (Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
            {
                posX.text = Util_VisEdu.ValidaVazio(prPeca.Pos.X.ToString());
                posY.text = prPeca.Pos.Y.ToString();
                posZ.text = prPeca.Pos.Z.ToString();
                ativo.isOn = prPeca.Ativo;
            }
        }
        finally
        {
            podeAtualizar = true;
            UpdateProp();
        }
    }

    public void UpdateProp(bool isIteracao = false)
    {
        if (isIteracao || (podeAtualizar && Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca)))
        {
            podeAtualizar = false;
            try
            {
                labelZ.SetActive(!Global.Grafico2D);
                if (!isIteracao)
                {
                    prPeca.Pos.X = Util_VisEdu.ConvertField(posX.text);
                    prPeca.Pos.Y = Util_VisEdu.ConvertField(posY.text);
                    prPeca.Pos.Z = Util_VisEdu.ConvertField(posZ.text);
                    prPeca.Ativo = ativo.isOn;
                }

                if (prPeca.Ativo)
                {
                    GameObject goTransformacaoAmb = GameObject.Find(prPeca.NomePecaAmb);
                    if (goTransformacaoAmb != null)
                    {
                        goTransformacaoAmb.transform.localEulerAngles = new Vector3(prPeca.Pos.X, prPeca.Pos.Y + (prPeca.NomePeca.Contains(Consts.POLIGONO) ? 180 : 0), prPeca.Pos.Z);
                    }

                    GameObject goTransformacaoVis = GameObject.Find(prPeca.NomePecaVis);
                    if (goTransformacaoVis != null)
                    {
                        goTransformacaoVis.transform.localEulerAngles = new Vector3(prPeca.Pos.X, prPeca.Pos.Y + (prPeca.NomePeca.Contains(Consts.POLIGONO) ? 180 : 0), prPeca.Pos.Z);
                    }
                }

                UpdateLockFields();
                AtualizaListaProp();
            }
            finally
            {
                podeAtualizar = true;
            }
        }
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
        {
            Global.propriedadePecas[this.prPeca.NomePeca] = prPeca;
        }
    }
    public void UpdateLockFields()
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
                prPeca.ListPropLocks.Remove("PosY");
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
                prPeca.ListPropLocks.Remove("PosZ");
            }
            else
            {
                prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(posZ.text).ToString());
            }

            lockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        AtualizaListaProp();
    }
}
