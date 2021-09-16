using System.Globalization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropEscalarScript : MonoBehaviour
{
    [SerializeField]
    GameObject label;
    [SerializeField]
    TMP_InputField nomePeca;
    [SerializeField]
    TMP_InputField tamX;
    [SerializeField]
    TMP_InputField tamY;
    [SerializeField]
    TMP_InputField tamZ;
    [SerializeField]
    Toggle toggle;

    float x, y, z;
    PropriedadePeca prPeca;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (JaClicouEmAlgumObjeto() && Global.gameObjectName.Contains(Consts.ESCALAR))
        {
            label.SetActive(!Global.Grafico2D);
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
                toggle.isOn = prPeca.Ativo;

                tamX.text = prPeca.Tam.X.ToString();
                tamY.text = prPeca.Tam.Y.ToString();
                tamZ.text = prPeca.Tam.Z.ToString();
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
            prPeca.Tam.X = Util_VisEdu.ConvertField(tamX.text);
            prPeca.Tam.Y = Util_VisEdu.ConvertField(tamY.text);
            prPeca.Tam.Z = Util_VisEdu.ConvertField(tamZ.text);

            x = y = z = 1;

            if (prPeca.Ativo)
            {
                x = prPeca.Tam.X;
                y = prPeca.Tam.Y;
                z = prPeca.Tam.Z;
            }

            GameObject goTransformacaoAmb = GameObject.Find(prPeca.Nome + "Amb");
            GameObject goTransformacaoVis = GameObject.Find(prPeca.Nome + "Vis");

            if (goTransformacaoAmb != null)
            {
                goTransformacaoAmb.transform.localScale = new Vector3(x, y, z);
                goTransformacaoVis.transform.localScale = new Vector3(x, y, z);
            }

            string forma = Util_VisEdu.GetCuboByNomePeca(Global.gameObjectName);

            if (!string.Empty.Equals(forma) && Global.propriedadePecas.ContainsKey(forma)) //Se forma for vazio significa que não existe uma forma ainda.
            {
                PropriedadePeca prPecaCubo = Global.propriedadePecas[forma];
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
        prPeca.Ativo = toggle;
        UpdateProp();
    }

    public void AdicionaValorPropriedade()
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
}
