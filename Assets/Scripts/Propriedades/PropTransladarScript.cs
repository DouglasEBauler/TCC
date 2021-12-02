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
            InicializaListas();

            nomePeca.text = prPeca.Nome;
            posX.text = prPeca.Pos.X.ToString();
            posY.text = prPeca.Pos.Y.ToString();
            posZ.text = prPeca.Pos.Z.ToString();
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

                float x, y, z;
                if (prPeca.Ativo)
                {
                    x = -prPeca.Pos.X;
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
                    goTransformacaoAmb.transform.localPosition = new Vector3(x, y, z);
                }

                GameObject goTransformacaoVis = GameObject.Find(prPeca.NomePecaVis);
                if (goTransformacaoVis != null)
                {
                    goTransformacaoVis.transform.localPosition = new Vector3(x, y, z);
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
        if (lockList == null || propList == null)
        {
            InicializaListas();
        }

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
        if (lockList == null || propList == null)
        {
            InicializaListas();
        }

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
