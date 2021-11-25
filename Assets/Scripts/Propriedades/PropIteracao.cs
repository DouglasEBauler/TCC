using System.Collections.Generic;
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
    Dictionary<Property, InputField> propList;
    Dictionary<Property, Toggle> lockList;
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
                InicializaListas();

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

    void InicializaListas()
    {
        propList = new Dictionary<Property, InputField>()
        {
            { Property.IntervaloX, xIntervalo },
            { Property.IntervaloY, yIntervalo },
            { Property.IntervaloZ, zIntervalo },
            { Property.MinX, xMin },
            { Property.MinY, yMin },
            { Property.MinZ, zMin },
            { Property.MaxX, xMax },
            { Property.MaxY, yMax },
            { Property.MaxZ, zMax }
        };

        lockList = new Dictionary<Property, Toggle>()
        {
            { Property.IntervaloX, lockXIntervalo },
            { Property.IntervaloY, lockYIntervalo },
            { Property.IntervaloZ, lockZIntervalo },
            { Property.MinX, lockXMin },
            { Property.MinY, lockYMin },
            { Property.MinZ, lockZMin },
            { Property.MaxX, lockXMax },
            { Property.MaxY, lockYMax },
            { Property.MaxZ, lockZMax }
        };
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

                UpdateAllLockFields();
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

    void UpdateAllLockFields()
    {
        foreach (var lockItem in lockList)
        {
            UpdateLockFields(lockItem.Key);
        }
    }

    public void UpdateLockFields(int typeProperty)
    {
        UpdateLockFields((Property)typeProperty);
    }

    void UpdateLockFields(Property typeProperty)
    {
        if (podeAtualizar)
        {
            if (!lockList[typeProperty].isOn)
            {
                propPeca.ListPropLocks.Remove(typeProperty);
                lockList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (propPeca.ListPropLocks.ContainsKey(typeProperty))
                {
                    propPeca.ListPropLocks[typeProperty] = Util_VisEdu.ConvertField(propList[typeProperty].text).ToString();
                }
                else
                {
                    propPeca.ListPropLocks.Add(typeProperty, Util_VisEdu.ConvertField(propList[typeProperty].text).ToString());
                }

                lockList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }
        }
    }
}
