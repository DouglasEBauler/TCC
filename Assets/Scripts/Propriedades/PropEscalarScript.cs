using UnityEngine;
using UnityEngine.UI;

public class PropEscalarScript : MonoBehaviour
{
    const float SCALE_X = 1000f;
    const float SCALE_Y = 1000f;
    const float SCALE_Z = 1000f;

    [SerializeField]
    GameObject label;
    [SerializeField]
    InputField nomePeca;
    [SerializeField]
    InputField tamX;
    [SerializeField]
    InputField tamY;
    [SerializeField]
    InputField tamZ;
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
            if (Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
            {
                nomePeca.text = prPeca.Nome;
                tamX.text = Util_VisEdu.ValidaVazio(prPeca.Pos.X.ToString(), true);
                tamY.text = Util_VisEdu.ValidaVazio(prPeca.Pos.Y.ToString(), true);
                tamZ.text = Util_VisEdu.ValidaVazio(prPeca.Pos.Z.ToString(), true);
                ativo.isOn = prPeca.Ativo;
            }
        }
        finally
        {
            podeAtualizar = true;
            UpdateProp();
        }
    }

    public void UpdateProp(bool isIteration = false)
    {
        if (isIteration || (podeAtualizar && Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca)))
        {
            podeAtualizar = false;
            try
            {
                if (!isIteration)
                {
                    prPeca.Pos.X = Util_VisEdu.ConvertField(tamX.text);
                    prPeca.Pos.Y = Util_VisEdu.ConvertField(tamY.text);
                    prPeca.Pos.Z = Util_VisEdu.ConvertField(tamZ.text);
                    prPeca.Ativo = ativo.isOn;
                }

                if (prPeca.Ativo)
                {
                    GameObject goTransformacaoAmb = GameObject.Find(prPeca.NomePecaAmb);
                    if (goTransformacaoAmb != null)
                    {
                        goTransformacaoAmb.transform.localScale = new Vector3(prPeca.Pos.X + SCALE_X, prPeca.Pos.Y + SCALE_Y, prPeca.Pos.Z + SCALE_Z);
                    }

                    GameObject goTransformacaoVis = GameObject.Find(prPeca.NomePecaVis);
                    if (goTransformacaoVis != null)
                    {
                        goTransformacaoVis.transform.localScale = new Vector3(prPeca.Pos.X + SCALE_X, prPeca.Pos.Y + SCALE_Y, prPeca.Pos.Z + SCALE_Z);
                    }
                }

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
                prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(tamX.text).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(tamX.text).ToString());
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
                prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(tamY.text).ToString());
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
                prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(tamZ.text).ToString());
            }

            lockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }
}
