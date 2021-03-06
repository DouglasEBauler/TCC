using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropPoligonoScript : MonoBehaviour
{
    const float POS_INIT_X = 5;
    const float POS_INIT_Y = 1;
    const float POS_INIT_Z = -17;

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
    PropCameraScript propCamera;
    [SerializeField]
    Toggle lockQtdPontos;
    [SerializeField]
    Toggle lockPosX;
    [SerializeField]
    Toggle lockPosY;
    [SerializeField]
    Toggle lockPosZ;
    [SerializeField]
    Toggle ativo;

    PoligonoPropriedadePeca propPeca;
    Dictionary<Property, InputField> propList;
    Dictionary<Property, Toggle> lockList;
    GameObject pecaAmb;
    GameObject pecaVis;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (this.propPeca != null)
        {
            FocusPoligonoVis();
            UpdateColor();
        }

        if (pecaAmb != null)
        {
            pecaAmb.GetComponent<MeshRenderer>().enabled = propPeca != null ? propPeca.Ativo : true;
        }
    }

    void FocusPoligonoVis()
    {
        if (pecaVis != null && Global.cameraAtiva && this.propPeca.Ativo)
        {
            propCamera.EnabledPecasVis(this.propPeca.NomePeca, false);
            pecaVis.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            pecaVis.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void Inicializa(PoligonoPropriedadePeca prPeca)
    {
        propPeca = prPeca;
        propPeca.Ativo = true;

        PreencheCampos();
        AtualizaListaProp();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(this.propPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                nome.text = propPeca.Nome;
                posX.text = propPeca.Pos.X.ToString();
                posY.text = propPeca.Pos.Y.ToString();
                posZ.text = propPeca.Pos.Z.ToString();
                pontos.text = propPeca.Pontos.ToString();
                primitiva.value = (int)TipoPrimitiva.Preenchido;
                ativo.isOn = propPeca.Ativo;

                corSeletor.GetComponent<Image>().material.color = propPeca.Cor;

                pecaAmb = GameObject.Find(propPeca.PoligonoAmb);
                pecaVis = GameObject.Find(propPeca.PoligonoVis);
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
            { Property.Pontos, pontos },
            { Property.PosX, posX },
            { Property.PosY, posY },
            { Property.PosZ, posZ }
        };

        lockList = new Dictionary<Property, Toggle>()
        {
            { Property.Pontos, lockQtdPontos },
            { Property.PosX, lockPosX },
            { Property.PosY, lockPosY },
            { Property.PosZ, lockPosZ }
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
                propPeca.Pontos = Util_VisEdu.ConvertField(pontos.text);
                propPeca.Pos.X = Util_VisEdu.ConvertField(posX.text) * -1;
                propPeca.Pos.Y = Util_VisEdu.ConvertField(posY.text);
                propPeca.Pos.Z = Util_VisEdu.ConvertField(posZ.text);
                propPeca.Cor = corSelecionada.color;
                propPeca.Primitiva = (TipoPrimitiva)primitiva.value;
                propPeca.Ativo = ativo.isOn;

                if (pecaAmb != null)
                {
                    pecaAmb.GetComponent<PoligonoAmbScript>().PropPeca = propPeca;
                    pecaAmb.GetComponent<MeshRenderer>().enabled = propPeca.Ativo;
                }

                if (pecaVis != null)
                {
                    pecaVis.GetComponent<PoligonoAmbScript>().PropPeca = propPeca;
                    FocusPoligonoVis();
                }

                UpdateAllLockFields();
                UpdatePoligonoAmbiente();
                AtualizaListaProp();
            }
            finally
            {
                podeAtualizar = true;
            }
        }
    }

    void UpdatePoligonoAmbiente()
    {
        if (pecaAmb != null && pecaVis != null)
        {
            UpdateColor();

            pecaAmb.GetComponent<MeshRenderer>().enabled = propPeca.Ativo;
            pecaAmb.GetComponent<PoligonoAmbScript>().ConfiguratePoints(propPeca.Pontos);
            pecaAmb.GetComponent<PoligonoAmbScript>().ConfiguratePoligono();
            pecaAmb.transform.localPosition = new Vector3(propPeca.Pos.X, propPeca.Pos.Y, propPeca.Pos.Z);
            foreach (Transform child in pecaAmb.transform)
            {
                LineRenderer mesh = child.GetComponent<LineRenderer>();

                if (mesh != null)
                {
                    mesh.enabled = propPeca.Ativo && !propPeca.Primitiva.Equals(TipoPrimitiva.Vertices);
                }
            }

            pecaVis.GetComponent<MeshRenderer>().enabled = propPeca.Ativo && Global.cameraAtiva;
            pecaVis.GetComponent<PoligonoAmbScript>().ConfiguratePoints(propPeca.Pontos);
            pecaVis.GetComponent<PoligonoAmbScript>().ConfiguratePoligono();
            pecaVis.transform.localPosition = new Vector3(POS_INIT_X + propPeca.Pos.X, POS_INIT_Y + propPeca.Pos.Y, POS_INIT_Z + propPeca.Pos.Z);
            foreach (Transform child in pecaVis.transform)
            {
                LineRenderer mesh = child.GetComponent<LineRenderer>();

                if (mesh != null)
                {
                    mesh.enabled = propPeca.Ativo && Global.cameraAtiva && !propPeca.Primitiva.Equals(TipoPrimitiva.Vertices);
                }
            }
        }
    }

    void UpdateColor()
    {
        this.propPeca.Cor = corSelecionada.color;
        corSeletor.GetComponent<Image>().material.color = this.propPeca.Cor;
        corSeletor.GetComponent<Image>().material.SetColor("_EmissionColor", this.propPeca.Cor);

        if (pecaAmb != null)
        {
            pecaAmb.GetComponent<MeshRenderer>().material.color = this.propPeca.Cor;
            pecaAmb.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", this.propPeca.Cor);
        }
        if (pecaVis != null)
        {
            pecaVis.GetComponent<MeshRenderer>().material.color = this.propPeca.Cor;
            pecaVis.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", this.propPeca.Cor);
        }
    }

    public void ControlPanelColor()
    {
        corSelecionada.gameObject.SetActive(!corSelecionada.gameObject.activeSelf);
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
