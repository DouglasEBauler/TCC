using UnityEngine;
using UnityEngine.UI;

public class PropIteracao : MonoBehaviour
{
    [SerializeField]
    InputField nome;
    [SerializeField]
    InputField xIntervalo;
    [SerializeField]
    InputField yIntervalo;
    [SerializeField]
    InputField zIntervalo;
    [SerializeField]
    InputField xMin;
    [SerializeField]
    InputField yMin;
    [SerializeField]
    InputField zMin;
    [SerializeField]
    InputField xMax;
    [SerializeField]
    InputField yMax;
    [SerializeField]
    InputField zMax;
    [SerializeField]
    Toggle lockXIntervalo;
    [SerializeField]
    Toggle lockYIntervalo;
    [SerializeField]
    Toggle lockZIntervalo;
    [SerializeField]
    Toggle lockXMin;
    [SerializeField]
    Toggle lockYMin;
    [SerializeField]
    Toggle lockZMin;
    [SerializeField]
    Toggle lockXMax;
    [SerializeField]
    Toggle lockYMax;
    [SerializeField]
    Toggle lockZMax;
    [SerializeField]
    Toggle ativo;
    [SerializeField]
    Toggle ativoX;
    [SerializeField]
    Toggle ativoY;
    [SerializeField]
    Toggle ativoZ;

    IteracaoPropriedadePeca propPeca;
    bool podeAtualizar;

    public void Inicializa(IteracaoPropriedadePeca propIteracao)
    {
        propPeca = propIteracao;
        propPeca.Ativo = true;

        PreencheCampos();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(this.propPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                nome.text = propPeca.Nome;
                xIntervalo.text = propPeca.Intervalo.X.ToString();
                yIntervalo.text = propPeca.Intervalo.Y.ToString();
                zIntervalo.text = propPeca.Intervalo.Z.ToString();
                xMin.text = propPeca.Min.X.ToString();
                yMin.text = propPeca.Min.Y.ToString();
                zMin.text = propPeca.Min.Z.ToString();
                xMax.text = propPeca.Max.X.ToString();
                yMax.text = propPeca.Max.Y.ToString();
                zMax.text = propPeca.Max.Z.ToString();
                ativo.isOn = propPeca.Ativo;
                ativoX.isOn = propPeca.AtivoX;
                ativoY.isOn = propPeca.AtivoY;
                ativoZ.isOn = propPeca.AtivoZ;
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
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(this.propPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                propPeca.Nome = nome.text;
                propPeca.Intervalo.X = Util_VisEdu.ConvertField(xIntervalo.text);
                propPeca.Intervalo.Y = Util_VisEdu.ConvertField(yIntervalo.text);
                propPeca.Intervalo.Z = Util_VisEdu.ConvertField(zIntervalo.text);
                propPeca.Min.X = Util_VisEdu.ConvertField(xMin.text);
                propPeca.Min.Y = Util_VisEdu.ConvertField(yMin.text);
                propPeca.Min.Z = Util_VisEdu.ConvertField(zMin.text);
                propPeca.Max.X = Util_VisEdu.ConvertField(xMax.text);
                propPeca.Max.Y = Util_VisEdu.ConvertField(yMax.text);
                propPeca.Max.Z = Util_VisEdu.ConvertField(zMax.text);
                propPeca.Ativo = ativo.isOn;
                propPeca.AtivoX = !propPeca.Ativo ? false : ativoX.isOn;
                propPeca.AtivoY = !propPeca.Ativo ? false : ativoY.isOn;
                propPeca.AtivoZ = !propPeca.Ativo ? false : ativoZ.isOn;

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
        if (Global.propriedadePecas.ContainsKey(this.propPeca.NomePeca))
        {
            Global.propriedadePecas[this.propPeca.NomePeca] = propPeca;
        }
    }

    public void UpdateLockFields()
    {
        if (!lockXIntervalo.isOn)
        {
            propPeca.ListPropLocks.Remove("IntervaloX");
            lockXIntervalo.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("IntervaloX"))
            {
                propPeca.ListPropLocks["IntervaloX"] = Util_VisEdu.ConvertField(xIntervalo.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("IntervaloX", Util_VisEdu.ConvertField(xIntervalo.text).ToString());
            }

            lockXIntervalo.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
        
        if (!lockYIntervalo.isOn)
        {
            propPeca.ListPropLocks.Remove("IntervaloY");
            lockYIntervalo.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("IntervaloY"))
            {
                propPeca.ListPropLocks["IntervaloY"] = Util_VisEdu.ConvertField(yIntervalo.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("IntervaloY", Util_VisEdu.ConvertField(yIntervalo.text).ToString());
            }

            lockYIntervalo.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockZIntervalo.isOn)
        {
            propPeca.ListPropLocks.Remove("IntervaloZ");
            lockZIntervalo.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("IntervaloZ"))
            {
                propPeca.ListPropLocks["IntervaloZ"] = Util_VisEdu.ConvertField(zIntervalo.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("IntervaloZ", Util_VisEdu.ConvertField(zIntervalo.text).ToString());
            }

            lockZIntervalo.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockXMin.isOn)
        {
            propPeca.ListPropLocks.Remove("MinX");
            lockXMin.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("MinX"))
            {
                propPeca.ListPropLocks["MinX"] = Util_VisEdu.ConvertField(xMin.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("MinX", Util_VisEdu.ConvertField(xMin.text).ToString());
            }

            lockXMin.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockYMin.isOn)
        {
            propPeca.ListPropLocks.Remove("MinY");
            lockYMin.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("MinY"))
            {
                propPeca.ListPropLocks["MinY"] = Util_VisEdu.ConvertField(yMin.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("MinY", Util_VisEdu.ConvertField(yMin.text).ToString());
            }

            lockYMin.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockZMin.isOn)
        {
            propPeca.ListPropLocks.Remove("MinZ");
            lockZMin.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("MinZ"))
            {
                propPeca.ListPropLocks["MinZ"] = Util_VisEdu.ConvertField(zMin.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("MinZ", Util_VisEdu.ConvertField(zMin.text).ToString());
            }

            lockZMin.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockXMax.isOn)
        {
            propPeca.ListPropLocks.Remove("MaxX");
            lockXMax.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("MaxX"))
            {
                propPeca.ListPropLocks["MaxX"] = Util_VisEdu.ConvertField(xMax.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("MaxX", Util_VisEdu.ConvertField(xMax.text).ToString());
            }

            lockXMax.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockYMax.isOn)
        {
            propPeca.ListPropLocks.Remove("MaxY");
            lockYMax.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("MaxY"))
            {
                propPeca.ListPropLocks["MaxY"] = Util_VisEdu.ConvertField(yMax.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("MaxY", Util_VisEdu.ConvertField(yMax.text).ToString());
            }

            lockYMax.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!lockZMax.isOn)
        {
            propPeca.ListPropLocks.Remove("MaxZ");
            lockZMax.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (propPeca.ListPropLocks.ContainsKey("MaxZ"))
            {
                propPeca.ListPropLocks["MaxZ"] = Util_VisEdu.ConvertField(zMax.text).ToString();
            }
            else
            {
                propPeca.ListPropLocks.Add("MaxZ", Util_VisEdu.ConvertField(zMax.text).ToString());
            }

            lockZMax.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }
}
