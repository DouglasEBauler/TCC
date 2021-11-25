using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropTransladarScript : MonoBehaviour
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
            posX.text = prPeca.Pos.X.ToString();
            posY.text = prPeca.Pos.Y.ToString();
            posZ.text = prPeca.Pos.Z.ToString();
            InicializaListas();
            ativo.isOn = prPeca.Ativo;
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

    public void UpdateProp(bool isIteration = false)
    {
        if (isIteration || (podeAtualizar && Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca)))
        {
            podeAtualizar = false;
            try
            {
                if (!isIteration)
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
                        goTransformacaoAmb.transform.localPosition = new Vector3(prPeca.Pos.X * -1, prPeca.Pos.Y, prPeca.Pos.Z);
                    }

                    GameObject goTransformacaoVis = GameObject.Find(prPeca.NomePecaVis);
                    if (goTransformacaoVis != null)
                    {
                        goTransformacaoVis.transform.localPosition = new Vector3(prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
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
            //Property property = (Property)typeProperty;

            if (!lockList[typeProperty].isOn)
            {
                prPeca.ListPropLocks.Remove(typeProperty);
                propList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

                propList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }
        }
    }
}
