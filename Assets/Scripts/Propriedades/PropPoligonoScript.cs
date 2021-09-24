using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    GameObject corSelecionada;
    [SerializeField]
    Toggle ativo;

    PoligonoPropriedadePeca propPeca;
    bool podeAtualizar;
    Color cor;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetPoligonoSelecionado())
        {
            UpdateProp();
        }
    }

    bool GetPoligonoSelecionado()
    {
        return Global.gameObjectName != null && Global.gameObjectName.Contains(Consts.POLIGONO);
    }

    public void AdicionaValorPropridedades()
    {
        UpdateProp();
    }

    public void Inicializa()
    {
        propPeca = Global.propriedadePecas[Global.gameObjectName] as PoligonoPropriedadePeca;

        if (propPeca == null)
        {
            propPeca.Ativo = true;
            AtualizaListaProp();
        }

        PreencheCampos();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(propPeca.Nome))
        {
            nome.text = propPeca.Nome;
            ativo.isOn = propPeca.Ativo;

            InstanceTransform();

            podeAtualizar = false;
            try
            {
                posX.text = propPeca.Pos.X.ToString();
                posY.text = propPeca.Pos.Y.ToString();
                posZ.text = propPeca.Pos.Z.ToString();
                pontos.text = propPeca.Pontos.ToString();
                primitiva.value = (int)propPeca.Primitiva;

                corSelecionada.GetComponent<MeshRenderer>().materials[0].color = propPeca.Cor;
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
        propPeca.Nome = propPeca.Nome ?? "Poligono";

        if (Global.propriedadePecas.ContainsKey(propPeca.Nome) && podeAtualizar)
        {
            propPeca.Nome = nome.text;
            propPeca.Pontos = int.Parse(pontos.text);
            propPeca.Pos.X = Util_VisEdu.ConvertField(posX.text);
            propPeca.Pos.Y = Util_VisEdu.ConvertField(posY.text);
            propPeca.Pos.Z = Util_VisEdu.ConvertField(posZ.text);
            propPeca.Primitiva = (TipoPrimitiva)primitiva.value;
            propPeca.Ativo = ativo.isOn;

            GameObject goTransformacaoAmb = GameObject.Find(propPeca.PoligonoAmbiente);

            if (goTransformacaoAmb != null)
            {
                float x = propPeca.Pos.X * -1;

                goTransformacaoAmb.transform.localPosition = new Vector3(x, propPeca.Pos.Y, propPeca.Pos.Z);
            }
        }

        //UpdateLockFields();
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(propPeca.Nome))
        {
            Global.propriedadePecas.Remove(propPeca.Nome);
            Global.propriedadePecas.Add(propPeca.Nome, propPeca);
        }
    }

    void ToggleChanged()
    {
        if (Global.gameObjectName != null)
        {
            propPeca.Ativo = ativo.isOn;
            UpdateProp();

            MeshRenderer meshRendererPoligono = GameObject.Find(propPeca.PoligonoAmbiente).GetComponent<MeshRenderer>();
            meshRendererPoligono.enabled = propPeca.Ativo;
        }
    }

    void InstanceTransform()
    {
        if (propPeca != null)
        {
            propPeca.Pos = propPeca.Pos ?? new Posicao();
            propPeca.Pos.X = 0;
            propPeca.Pos.Y = 0;
            propPeca.Pos.Z = 0;
            propPeca.Primitiva = TipoPrimitiva.Cheio;
            propPeca.Cor = Color.white;
            propPeca.Ativo = true;
        }
    }

    public void AdicionaValorPropriedade()
    {
        ToggleChanged();
    }

    //public void UpdateLockFields()
    //{
    //    if (!toggleLockPosX.isOn)
    //    {
    //        prPeca.ListPropLocks.Remove("PosX");
    //        toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
    //    }
    //    else
    //    {
    //        if (prPeca.ListPropLocks.ContainsKey("PosX"))
    //        {
    //            prPeca.ListPropLocks["PosX"] = ConvertField(posX.text, TypeTransf.Pos).ToString();
    //        }
    //        else
    //        {
    //            prPeca.ListPropLocks.Add("PosX", ConvertField(posX.text, TypeTransf.Pos).ToString());
    //        }

    //        toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
    //    }

    //    if (!toggleLockPosY.isOn)
    //    {
    //        prPeca.ListPropLocks.Remove("PosY");
    //        toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
    //    }
    //    else
    //    {
    //        if (prPeca.ListPropLocks.ContainsKey("PosY"))
    //        {
    //            prPeca.ListPropLocks["PosY"] = ConvertField(posY.text, TypeTransf.Pos).ToString();
    //        }
    //        else
    //        {
    //            prPeca.ListPropLocks.Add("PosY", ConvertField(posY.text, TypeTransf.Pos).ToString());
    //        }

    //        toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
    //    }

    //    if (!toggleLockPosZ.isOn)
    //    {
    //        prPeca.ListPropLocks.Remove("PosZ");
    //        toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
    //    }
    //    else
    //    {
    //        if (prPeca.ListPropLocks.ContainsKey("PosZ"))
    //        {
    //            prPeca.ListPropLocks["PosZ"] = ConvertField(posZ.text, TypeTransf.Pos).ToString();
    //        }
    //        else
    //        {
    //            prPeca.ListPropLocks.Add("PosZ", ConvertField(posZ.text, TypeTransf.Pos).ToString());
    //        }

    //        toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
    //    }
    //}
}
