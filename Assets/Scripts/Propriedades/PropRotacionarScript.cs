using System.Collections.Generic;
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
    Dictionary<Property, InputField> propList;
    Dictionary<Property, Toggle> lockList;
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
                InicializaListas();
                ativo.isOn = prPeca.Ativo;
            }
        }
        finally
        {
            podeAtualizar = true;
            UpdateProp();
        }
    }

    void InicializaListas()
    {
        propList = new Dictionary<Property, InputField>()
        {
            { Property.PosX, posX },
            { Property.PosY, posY },
            { Property.PosZ, posZ }
        };

        lockList = new Dictionary<Property, Toggle>()
        {
            { Property.PosX, lockPosX },
            { Property.PosY, lockPosY },
            { Property.PosZ, lockPosZ }
        };
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


                float x, y, z;
                if (prPeca.Ativo)
                {
                    x = prPeca.Pos.X;
                    y = prPeca.Pos.Y;
                    z = prPeca.Pos.Z;
                }
                else
                {
                    x = y = z = 0;
                }

                GameObject goTransformacaoAmb = GameObject.Find(prPeca.NomePecaAmb);
                if (goTransformacaoAmb != null)
                {
                    goTransformacaoAmb.transform.localEulerAngles = new Vector3(x, y + (prPeca.NomePeca.Contains(Consts.POLIGONO) ? 180 : 0), z);
                }

                GameObject goTransformacaoVis = GameObject.Find(prPeca.NomePecaVis);
                if (goTransformacaoVis != null)
                {
                    goTransformacaoVis.transform.localEulerAngles = new Vector3(x, y + (prPeca.NomePeca.Contains(Consts.POLIGONO) ? 180 : 0), z);
                }

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
        if (Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
        {
            Global.propriedadePecas[this.prPeca.NomePeca] = prPeca;
        }
    }
    void UpdateAllLockFields()
    {
        foreach (var lockItem in lockList)
        {
            UpdateLockFields((lockItem.Key));
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
                prPeca.ListPropLocks.Remove(typeProperty);
                lockList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey(typeProperty))
                {
                    prPeca.ListPropLocks[typeProperty] = Util_VisEdu.ConvertField(propList[typeProperty].text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add(typeProperty, Util_VisEdu.ConvertField(propList[typeProperty].text).ToString());
                }

                lockList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }
        }
    }
}
