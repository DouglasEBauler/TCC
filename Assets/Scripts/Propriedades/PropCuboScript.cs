using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropCuboScript : MonoBehaviour
{
    const float POS_INIT_X = 5;
    const float POS_INIT_Y = 1;
    const float POS_INIT_Z = -17;

    [SerializeField]
    GameObject tamZCubo;
    [SerializeField]
    GameObject posZCubo;
    [SerializeField]
    InputField nomePeca;
    [SerializeField]
    InputField tamX;
    [SerializeField]
    InputField tamY;
    [SerializeField]
    InputField tamZ;
    [SerializeField]
    InputField posX;
    [SerializeField]
    InputField posY;
    [SerializeField]
    InputField posZ;
    [SerializeField]
    FlexibleColorPicker corSelecionado;
    [SerializeField]
    GameObject seletorCor;
    [SerializeField]
    GameObject texturaSelecionada;
    [SerializeField]
    GameObject baseTexturas;
    [SerializeField]
    PropCameraScript propCamera;
    [SerializeField]
    Toggle ativo;
    [SerializeField]
    Toggle lockPosX;
    [SerializeField]
    Toggle lockPosY;
    [SerializeField]
    Toggle lockPosZ;
    [SerializeField]
    Toggle lockTamX;
    [SerializeField]
    Toggle lockTamY;
    [SerializeField]
    Toggle lockTamZ;

    CuboPropriedadePeca prPeca;
    Dictionary<Property, InputField> propList;
    Dictionary<Property, Toggle> lockList;
    GameObject pecaAmbiente;
    GameObject pecaVis;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (this.prPeca != null)
        {
            EnabledZ();
            FocusCuboVis();
            UpdateColor();
        }
    }

    void FocusCuboVis()
    {
        if (pecaVis != null && Global.cameraAtiva && this.prPeca.Ativo)
        {
            propCamera.EnabledPecasVis(this.prPeca.NomePeca, false);
            pecaVis.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            pecaVis.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void Inicializa(CuboPropriedadePeca propPeca)
    {
        prPeca = propPeca;
        prPeca.Ativo = true;

        PreencheCampos();
        AtualizaListaProp();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                nomePeca.text = prPeca.Nome;
                tamX.text = Util_VisEdu.ValidaVazio(prPeca.Tam.X.ToString(), true);
                tamY.text = Util_VisEdu.ValidaVazio(prPeca.Tam.Y.ToString(), true);
                tamZ.text = Util_VisEdu.ValidaVazio(prPeca.Tam.Z.ToString(), true);
                posX.text = prPeca.Pos.X.ToString();
                posY.text = prPeca.Pos.Y.ToString();
                posZ.text = prPeca.Pos.Z.ToString();
                corSelecionado.color = prPeca.Cor;
                texturaSelecionada.GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
                foreach (Transform child in baseTexturas.transform)
                {
                    child.gameObject.GetComponent<SelecionaTextura>().NomePecaCubo = this.prPeca.NomePeca;
                }
                ativo.isOn = prPeca.Ativo;

                pecaAmbiente = GameObject.Find(prPeca.NomeCuboAmb);
                pecaVis = GameObject.Find(prPeca.NomeCuboVis);
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
            { Property.TamX, tamX },
            { Property.TamY, tamY },
            { Property.TamZ, tamZ },
            { Property.PosX, posX },
            { Property.PosY, posY },
            { Property.PosZ, posZ }
        };

        lockList = new Dictionary<Property, Toggle>()
        {
            { Property.TamX, lockTamX },
            { Property.TamY, lockTamY },
            { Property.TamZ, lockTamZ },
            { Property.PosX, lockPosX },
            { Property.PosY, lockPosY },
            { Property.PosZ, lockPosZ }
        };
    }

    public void UpdateProp()
    {
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                prPeca.Nome = nomePeca.text;
                prPeca.Tam.X = Util_VisEdu.ConvertField(tamX.text, true);
                prPeca.Tam.Y = Util_VisEdu.ConvertField(tamY.text, true);
                prPeca.Tam.Z = Util_VisEdu.ConvertField(tamZ.text, true);
                prPeca.Pos.X = Util_VisEdu.ConvertField(posX.text) * -1;
                prPeca.Pos.Y = Util_VisEdu.ConvertField(posY.text);
                prPeca.Pos.Z = Util_VisEdu.ConvertField(posZ.text);
                prPeca.Ativo = ativo.isOn;

                if (pecaAmbiente != null)
                {
                    pecaAmbiente.transform.localPosition = new Vector3(prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
                    pecaAmbiente.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
                    pecaAmbiente.GetComponent<MeshRenderer>().enabled = prPeca.Ativo;
                }

                if (pecaVis != null)
                {
                    pecaVis.transform.localPosition = new Vector3(POS_INIT_X + prPeca.Pos.X, POS_INIT_Y + prPeca.Pos.Y, POS_INIT_Z + prPeca.Pos.Z);
                    pecaVis.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
                    FocusCuboVis();
                }

                UpdateAllLockFields();
                UpdateColor();
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

    public void ControlCorPanel()
    {
        corSelecionado.gameObject.SetActive(!corSelecionado.gameObject.activeSelf);
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

    void UpdateColor()
    {
        seletorCor.GetComponent<Image>().material.color = corSelecionado.color;
        seletorCor.GetComponent<Image>().material.SetColor("_EmissionColor", corSelecionado.color);

        if (pecaAmbiente != null)
        {
            pecaAmbiente.GetComponent<MeshRenderer>().material.color = corSelecionado.color;
            pecaAmbiente.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", corSelecionado.color);
        }
        if (pecaVis != null)
        {
            pecaVis.GetComponent<MeshRenderer>().material.color = corSelecionado.color;
            pecaVis.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", corSelecionado.color);
        }
    }

    void EnabledZ()
    {
        tamZCubo.SetActive(!Global.Grafico2D);
        posZCubo.SetActive(!Global.Grafico2D);
    }
}
