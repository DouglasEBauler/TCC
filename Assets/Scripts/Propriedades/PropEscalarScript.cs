using System.Globalization;
using System.Collections.Generic;
using TMPro;
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
    Toggle toggle;
    [SerializeField]
    Toggle toggleLockPosX;
    [SerializeField]
    Toggle toggleLockPosY;
    [SerializeField]
    Toggle toggleLockPosZ;

    PropriedadeTransformacao prPeca;
    bool podeAtualizar;

    public void Inicializa()
    {
        prPeca = Global.propriedadePecas[Util_VisEdu.GetPecaByName(Global.gameObjectName) + Global.gameObjectName] as PropriedadeTransformacao;
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

            if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
            {
                toggle.isOn = prPeca.Ativo;

                tamX.text = prPeca.Pos.X.ToString();
                tamY.text = prPeca.Pos.Y.ToString();
                tamZ.text = prPeca.Pos.Z.ToString();
            }
        }
        finally
        {
            podeAtualizar = true;
            UpdateProp();
        }
    }

    public void UpdateProp()
    {
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            prPeca.Pos.X = Util_VisEdu.ConvertField(tamX.text);
            prPeca.Pos.Y = Util_VisEdu.ConvertField(tamY.text);
            prPeca.Pos.Z = Util_VisEdu.ConvertField(tamZ.text);
            prPeca.Ativo = toggle.isOn;

            if (prPeca.Ativo)
            {
                GameObject goTransformacaoAmb = GameObject.Find(prPeca.Nome + "Amb");
                if (goTransformacaoAmb != null)
                {
                    goTransformacaoAmb.transform.localScale = new Vector3(prPeca.Pos.X+SCALE_X, prPeca.Pos.Y + SCALE_Y, prPeca.Pos.Z + SCALE_Z);
                }

                GameObject goTransformacaoVis = GameObject.Find(prPeca.Nome + "Vis");
                if (goTransformacaoVis != null)
                {
                    goTransformacaoVis.transform.localScale = new Vector3(prPeca.Pos.X + SCALE_X, prPeca.Pos.Y + SCALE_Y, prPeca.Pos.Z + SCALE_Z);
                }
            }

            //string forma = Util_VisEdu.GetPecaByName(Global.gameObjectName);

            //if (!string.Empty.Equals(forma) && Global.propriedadePecas.ContainsKey(forma)) //Se forma for vazio significa que não existe uma forma ainda.
            //{
            //    CuboPropriedadePeca prPecaCubo = Global.propriedadePecas[forma] as CuboPropriedadePeca;
            //    goTransformacaoAmb = GameObject.Find(prPecaCubo.NomeCuboAmbiente);
            //    goTransformacaoVis = GameObject.Find(prPecaCubo.NomeCuboVis);

            //    if (goTransformacaoAmb != null && goTransformacaoVis != null)
            //    {
            //        goTransformacaoAmb.transform.localPosition = new Vector3(prPecaCubo.Pos.X * -1, prPecaCubo.Pos.Y, prPecaCubo.Pos.Z);
            //        goTransformacaoVis.transform.localPosition = new Vector3(prPecaCubo.Pos.X * -1, prPecaCubo.Pos.Y, prPecaCubo.Pos.Z);
            //        goTransformacaoAmb.transform.localScale = new Vector3(prPecaCubo.Tam.X, prPecaCubo.Tam.Y, prPecaCubo.Tam.Z);
            //        goTransformacaoVis.transform.localScale = new Vector3(prPecaCubo.Tam.X, prPecaCubo.Tam.Y, prPecaCubo.Tam.Z);
            //    }
            //}
        }

        AtualizaListaProp();
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            Global.propriedadePecas[prPeca.Nome] = prPeca;
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
                prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(tamX.text).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(tamX.text).ToString());
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
                prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(tamY.text).ToString());
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
                prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(tamZ.text).ToString());
            }

            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }
}
