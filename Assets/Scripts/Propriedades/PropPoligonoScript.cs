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
    Button corSeletor;
    [SerializeField]
    FlexibleColorPicker corSelecionada;
    [SerializeField]
    Toggle toggleQtdPontos;
    [SerializeField]
    Toggle toggleLockPosX;
    [SerializeField]
    Toggle toggleLockPosY;
    [SerializeField]
    Toggle toggleLockPosZ;
    [SerializeField]
    Toggle ativo;

    PoligonoPropriedadePeca propPeca;
    GameObject goPoligonoAmb;
    bool podeAtualizar;

    public void Inicializa()
    {
        propPeca = Global.propriedadePecas[Global.gameObjectName] as PoligonoPropriedadePeca;

        if (propPeca == null)
        {
            propPeca.Ativo = true;
            AtualizaListaProp();
        }
        goPoligonoAmb = GameObject.Find(propPeca.PoligonoAmbiente);

        PreencheCampos();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(propPeca.Nome))
        {
            InstanceTransform();

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

    void InstanceTransform()
    {
        if (propPeca != null)
        {
            propPeca.Pos = propPeca.Pos ?? new Posicao();
            propPeca.Pos.X = 0;
            propPeca.Pos.Y = 0;
            propPeca.Pos.Z = 0;
            propPeca.Pontos = 3;
            propPeca.Primitiva = TipoPrimitiva.Cheio;
            propPeca.Cor = Color.white;
            propPeca.Ativo = true;
        }
    }

    public void UpdateLockFields()
    {
        if (!toggleQtdPontos.isOn)
        {
            propPeca.ListPropLocks.Remove("Pontos");
            toggleQtdPontos.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleQtdPontos.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosX.isOn)
        {
            propPeca.ListPropLocks.Remove("PosX");
            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosY.isOn)
        {
            propPeca.ListPropLocks.Remove("PosY");
            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosZ.isOn)
        {
            propPeca.ListPropLocks.Remove("PosZ");
            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
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

            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }
}
