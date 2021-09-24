using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
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
    Toggle toggleField;
    [SerializeField]
    Toggle toggleLockPosX;
    [SerializeField]
    Toggle toggleLockPosY;
    [SerializeField]
    Toggle toggleLockPosZ;
    
    float x, y, z;
    PropriedadePeca prPeca;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (JaClicouEmAlgumObjeto() && Global.gameObjectName.Contains(Consts.ROTACIONAR))
        {
            labelZ.SetActive(!Global.Grafico2D);

            UpdateProp();
        }
    }

    public void Inicializa()
    {
        prPeca = Global.propriedadePecas[Global.gameObjectName];

        if (prPeca == null)
        {
            InstanciaTransformacao();
            prPeca.Ativo = true;
            podeAtualizar = true;

            AtualizaListaProp();
        }

        PreencheCampos();
    }

    void PreencheCampos()
    {
        podeAtualizar = false;
        try
        {
            nomePeca.text = prPeca.Nome;

            if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
            {
                InstanciaTransformacao();

                toggleField.isOn = prPeca.Ativo;

                posX.text = prPeca.Pos.X.ToString();
                posY.text = prPeca.Pos.Y.ToString();
                posZ.text = prPeca.Pos.Z.ToString();
            }
        }
        finally
        {
            podeAtualizar = true;
            UpdateProp();
        }
    }

    void UpdateProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome) && podeAtualizar)
        {
            prPeca.Pos.X = Util_VisEdu.ConvertField(posX.text);
            prPeca.Pos.Y = Util_VisEdu.ConvertField(posY.text);
            prPeca.Pos.Z = Util_VisEdu.ConvertField(posZ.text);

            x = y = z = 0;

            if (prPeca.Ativo)
            {
                x = prPeca.Pos.X;
                y = prPeca.Pos.Y;
                z = prPeca.Pos.Z;
            }

            GameObject goTransformacaoAmb = GameObject.Find(prPeca.Nome + "Amb");
            GameObject goTransformacaoVis = GameObject.Find(prPeca.Nome + "Vis");

            if (goTransformacaoAmb != null)
            {
                goTransformacaoAmb.transform.localRotation = Quaternion.Euler(x, y * -1, z * -1);
                goTransformacaoVis.transform.localRotation = Quaternion.Euler(x, y, z);
            }

            string forma = Util_VisEdu.GetCuboByNomePeca(Global.gameObjectName);

            if (!string.Empty.Equals(forma) && Global.propriedadePecas.ContainsKey(forma)) //Se forma for vazio significa que não existe uma forma ainda.
            {
                CuboPropriedadePeca prPecaCubo = Global.propriedadePecas[forma] as CuboPropriedadePeca;
                goTransformacaoAmb = GameObject.Find(prPecaCubo.NomeCuboAmbiente);
                goTransformacaoVis = GameObject.Find(prPecaCubo.NomeCuboVis);

                if (goTransformacaoAmb != null && goTransformacaoVis != null)
                {
                    goTransformacaoAmb.transform.localPosition = new Vector3(prPecaCubo.Pos.X * -1, prPecaCubo.Pos.Y, prPecaCubo.Pos.Z);
                    goTransformacaoVis.transform.localPosition = new Vector3(prPecaCubo.Pos.X * -1, prPecaCubo.Pos.Y, prPecaCubo.Pos.Z);
                    goTransformacaoAmb.transform.localScale = new Vector3(prPecaCubo.Tam.X, prPecaCubo.Tam.Y, prPecaCubo.Tam.Z);
                    goTransformacaoVis.transform.localScale = new Vector3(prPecaCubo.Tam.X, prPecaCubo.Tam.Y, prPecaCubo.Tam.Z);
                }
            }
        }

        AtualizaListaProp();
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            Global.propriedadePecas.Remove(prPeca.Nome);
            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
        }
    }

    void ToggleChanged()
    {
        prPeca.Ativo = toggleField.isOn;
        UpdateProp();
    }

    public void AdicionarValorPropriedade()
    {
        ToggleChanged();
    }

    bool JaClicouEmAlgumObjeto()
    {
        return Global.gameObjectName != null;
    }

    void InstanciaTransformacao()
    {
        if (prPeca != null)
        {
            if (prPeca.Pos == null)
                prPeca.Pos = new Posicao();

            prPeca.Pos.X = 0;
            prPeca.Pos.Y = 0;
            prPeca.Pos.Z = 0;

            if (prPeca.Tam == null)
                prPeca.Tam = new Tamanho();

            prPeca.Tam.X = 1;
            prPeca.Tam.Y = 1;
            prPeca.Tam.Z = 1;
        }
    }

    public void UpdateLockFields()
    {
        if (!toggleLockPosX.isOn)
        {
            prPeca.ListPropLocks.Remove("PosX");
            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (prPeca.ListPropLocks.ContainsKey("PosX"))
            {
                prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(posX.text).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(posX.text).ToString());
            }

            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosY.isOn)
        {
            prPeca.ListPropLocks.Remove("PosY");
            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (prPeca.ListPropLocks.ContainsKey("PosY"))
            {
                prPeca.ListPropLocks.Remove("PosY");
            }
            else
            {
                prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(posY.text).ToString());
            }

            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosZ.isOn)
        {
            prPeca.ListPropLocks.Remove("PosZ");
            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (prPeca.ListPropLocks.ContainsKey("PosZ"))
            {
                prPeca.ListPropLocks.Remove("PosZ");
            }
            else
            {
                prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(posZ.text).ToString());
            }

            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }
}
