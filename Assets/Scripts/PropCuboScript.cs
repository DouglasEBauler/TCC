using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;

enum Transforms { PosX, PosY, PosZ };
enum typeTransf { Pos, Tam };

public class PropCuboScript : MonoBehaviour
{
    [SerializeField]
    GameObject labelTamZCubo;
    [SerializeField]
    GameObject labelPosZCubo;
    [SerializeField]
    TMP_InputField nomePeca;
    [SerializeField]
    TMP_InputField TamX;
    [SerializeField]
    TMP_InputField TamY;
    [SerializeField]
    TMP_InputField TamZ;
    [SerializeField]
    TMP_InputField PosX;
    [SerializeField]
    TMP_InputField PosY;
    [SerializeField]
    TMP_InputField PosZ;
    [SerializeField]
    GameObject corSelecionada;
    [SerializeField]
    GameObject texturaSelecionada;
    [SerializeField]
    Toggle toggleCubo;

    PropriedadePeca prPeca;

    void FixedUpdate()
    {
        if (Global.gameObjectName != null && Global.gameObjectName.Contains(Consts.Cubo))
        {
            EnableLabelZ();            
            UpdateProp();
        }
    }

    public void Inicializar()
    {
        prPeca = Global.propriedadePecas[Global.gameObjectName];

        if (prPeca == null)
        {
            if (!prPeca.JaIniciouValores)
            {
                prPeca.Ativo = true;
                prPeca.PodeAtualizar = true;
                prPeca.JaIniciouValores = true;

                AtualizaListaProp();
            }
        }

        PreencheCampos();
    }

    public void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            nomePeca.text = prPeca.Nome;
            toggleCubo.isOn = prPeca.Ativo;

            instanciaTransformacao();

            prPeca.PodeAtualizar = false;
            try
            {
                TamX.text = prPeca.Tam.X.ToString();
                TamY.text = prPeca.Tam.Y.ToString();
                TamZ.text = prPeca.Tam.Z.ToString();
                PosX.text = prPeca.Pos.X.ToString();
                PosY.text = prPeca.Pos.Y.ToString();
                PosZ.text = prPeca.Pos.Z.ToString();
                corSelecionada.GetComponent<MeshRenderer>().materials[0].color = prPeca.Cor;
                texturaSelecionada.GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
            }
            finally
            {
                prPeca.PodeAtualizar = true;
                UpdateProp();
            }
        }
    }

    void UpdateProp()
    {
        prPeca.Nome = prPeca.Nome ?? "Cubo";

        if (Global.propriedadePecas.ContainsKey(prPeca.Nome) && prPeca.PodeAtualizar)
        {
            prPeca.Tam.X = ConvertField(TamX.text, typeTransf.Tam);
            prPeca.Tam.Y = ConvertField(TamY.text, typeTransf.Tam);
            prPeca.Tam.Z = ConvertField(TamZ.text, typeTransf.Tam);

            prPeca.Pos.X = ConvertField(PosX.text, typeTransf.Pos);
            prPeca.Pos.X = ConvertField(PosY.text, typeTransf.Pos);
            prPeca.Pos.X = ConvertField(PosZ.text, typeTransf.Pos);

            GameObject goTransformacaoAmb = GameObject.Find(prPeca.NomeCuboAmbiente);
            GameObject goTransformacaoVis = GameObject.Find(prPeca.NomeCuboVis);

            if (goTransformacaoAmb != null && goTransformacaoVis != null)
            {
                float x = prPeca.Pos.X * -1;

                goTransformacaoAmb.transform.localPosition = new Vector3(x, prPeca.Pos.Y, prPeca.Pos.Z);
                goTransformacaoVis.transform.localPosition = new Vector3(x, prPeca.Pos.Y, prPeca.Pos.Z);
                goTransformacaoAmb.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
                goTransformacaoVis.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
            }
        }
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            Global.propriedadePecas.Remove(prPeca.Nome);
            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
        }
    }

    void toggleChanged()
    {
        if (Global.gameObjectName != null)
        {
            prPeca.Ativo = toggleCubo.isOn;
            UpdateProp();

            string goObjGraficoSlotPai = GameObject.Find(GameObject.Find(Global.listaEncaixes[Global.gameObjectName]).transform.parent.name).name;

            string pecaObjGrafico = string.Empty;

            // Pega o nome da peça conectada ao "goObjGraficoSlotPai"
            foreach (var peca in Global.listaEncaixes)
            {
                if (peca.Value.Equals(goObjGraficoSlotPai))
                {
                    pecaObjGrafico = peca.Key;
                    break;
                }
            }

            bool existeProp = false;

            foreach (var pec in Global.propriedadePecas)
            {
                if (pec.Key.Equals(pecaObjGrafico))
                {
                    existeProp = true;
                    break;
                }
            }

            MeshRenderer meshRendererCubo = GameObject.Find(prPeca.NomeCuboAmbiente).GetComponent<MeshRenderer>();
            meshRendererCubo.enabled = (existeProp && Global.propriedadePecas[pecaObjGrafico].Ativo) || !existeProp ? prPeca.Ativo : false;
        }
    }

    float ConvertField(string inputProp, typeTransf t)
    {
        return float.Parse(validaVazio(inputProp, t), CultureInfo.InvariantCulture.NumberFormat);
    }

    void instanciaTransformacao()
    {
        if (!prPeca.JaInstanciou)
        {
            prPeca.Pos = prPeca.Pos ?? new Posicao();
            prPeca.Pos.X = 0;
            prPeca.Pos.Y = 0;
            prPeca.Pos.Z = 0;

            prPeca.Tam = prPeca.Tam ?? new Tamanho();
            prPeca.Tam.X = 1;
            prPeca.Tam.Y = 1;
            prPeca.Tam.Z = 1;

            prPeca.Cor = Color.white;
            prPeca.Ativo = true;
            prPeca.JaInstanciou = true;
        }
    }

    string validaVazio(string valor, typeTransf tipo)
    {
        return string.Empty.Equals(valor) ? (typeTransf.Tam.Equals(tipo) ? "1" : "0") : valor;
    }

    public void AdicionaValorPropriedade()
    {
        toggleChanged();
    }

    void EnableLabelZ()
    {
        labelTamZCubo.transform.localPosition =
            new Vector3(labelTamZCubo.transform.localPosition.x, labelTamZCubo.transform.localPosition.y, Global.Grafico2D ? 100000 : 0.000111648f);
        labelPosZCubo.transform.localPosition =
            new Vector3(labelPosZCubo.transform.localPosition.x, labelPosZCubo.transform.localPosition.y, Global.Grafico2D ? 100000 : 0.000111648f);
    }
}
