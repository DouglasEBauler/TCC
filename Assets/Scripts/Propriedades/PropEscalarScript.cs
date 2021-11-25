using System.Collections.Generic;
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
            if (Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
            {
                nomePeca.text = prPeca.Nome;
                tamX.text = Util_VisEdu.ValidaVazio(prPeca.Pos.X.ToString(), true);
                tamY.text = Util_VisEdu.ValidaVazio(prPeca.Pos.Y.ToString(), true);
                tamZ.text = Util_VisEdu.ValidaVazio(prPeca.Pos.Z.ToString(), true);
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
            { Property.TamX, tamX },
            { Property.TamY, tamY },
            { Property.TamZ, tamZ }
        };

        lockList = new Dictionary<Property, Toggle>()
        {
            { Property.TamX, lockPosX },
            { Property.TamY, lockPosY },
            { Property.TamZ, lockPosZ }
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
                UpdateAllLockFields();
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
