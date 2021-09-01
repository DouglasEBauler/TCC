using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;
using System;

public abstract class PropriedadePecaPadrao : MonoBehaviour {

    protected enum typeTransformacao { Transladar, Rotacionar, Escalar};

    protected TMP_InputField PosX, TamX;
    protected TMP_InputField PosY, TamY;
    protected TMP_InputField PosZ, TamZ;
    protected float x, y, z;
    protected bool ativo = true;
    protected PropriedadePeca prPeca = new PropriedadePeca();
    protected typeTransformacao tipoTransformacao;
    protected string nomePeca;
    private bool dadosIniciais = false;

    protected void mainMethod()
    {
        if (prPeca.PodeAtualizar)
        {        
            preencheCampos();
            prPeca.PodeAtualizar = false;
        }
    }  
    
    protected bool inicializou()
    {
        bool inicializou = false;

        if (!Global.propriedadePecas.ContainsKey(Global.gameObjectName))
            inicializou = true;
        else
        {
            prPeca = Global.propriedadePecas[Global.gameObjectName];

            if (prPeca.JaIniciouValores)
            {
                dadosIniciais = false;
                return true;
            }
            else
            {
                prPeca.Ativo = gameObject.transform.GetChild(3).GetComponent<Toggle>().isOn;
                prPeca.PodeAtualizar = true;
                prPeca.JaIniciouValores = true;
                nomePeca = prPeca.Nome;

                inicializou = false;
                dadosIniciais = true;

                atualizaListaProp();
            }
        }        

        return inicializou;
    }

    protected void preencheCampos()
    {        
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Nome;

        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {     
            gameObject.transform.GetChild(3).GetComponent<Toggle>().isOn = prPeca.Ativo;

            instanciaTransformacao();

            if (tipoTransformacao == typeTransformacao.Escalar)
            {
                TamX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                TamY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                TamZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                TamX.text = prPeca.Tam.X.ToString();
                TamY.text = prPeca.Tam.Y.ToString();
                TamZ.text = prPeca.Tam.Z.ToString();
            }
            else
            {
                PosX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                PosY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                PosZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                PosX.text = prPeca.Pos.X.ToString();
                PosY.text = prPeca.Pos.Y.ToString();
                PosZ.text = prPeca.Pos.Z.ToString();
            }
        }        
    }

    protected void updatePosition()
    {
        
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            if (tipoTransformacao == typeTransformacao.Escalar)
            {
                TamX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                TamY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                TamZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                prPeca.Tam.X = float.Parse(validaVazio(TamX.text), CultureInfo.InvariantCulture.NumberFormat);
                prPeca.Tam.Y = float.Parse(validaVazio(TamY.text), CultureInfo.InvariantCulture.NumberFormat);
                prPeca.Tam.Z = float.Parse(validaVazio(TamZ.text), CultureInfo.InvariantCulture.NumberFormat);

                x = y = z = 1;

                if (prPeca.Ativo)
                {
                    x = prPeca.Tam.X;
                    y = prPeca.Tam.Y;
                    z = prPeca.Tam.Z;
                }
            }
            else
            {
                PosX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                PosY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                PosZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                prPeca.Pos.X = float.Parse(validaVazio(PosX.text), CultureInfo.InvariantCulture.NumberFormat);
                prPeca.Pos.Y = float.Parse(validaVazio(PosY.text), CultureInfo.InvariantCulture.NumberFormat);
                prPeca.Pos.Z = float.Parse(validaVazio(PosZ.text), CultureInfo.InvariantCulture.NumberFormat);

                x = y = z = 0;

                if (prPeca.Ativo)
                {
                    x = prPeca.Pos.X;
                    y = prPeca.Pos.Y;
                    z = prPeca.Pos.Z;
                }
            }              

            GameObject goTransformacaoAmb = GameObject.Find(prPeca.Nome + "Amb");
            GameObject goTransformacaoVis = GameObject.Find(prPeca.Nome + "Vis");

            if (goTransformacaoAmb != null /*&& goTransformacaoVis != null*/)
            {
                if (tipoTransformacao == typeTransformacao.Transladar)
                {
                    goTransformacaoAmb.transform.localPosition = new Vector3(x * -1, y, z);
                    goTransformacaoVis.transform.localPosition = new Vector3(x, y, z);
                }
                else if (tipoTransformacao == typeTransformacao.Rotacionar)
                {
                    goTransformacaoAmb.transform.localRotation = Quaternion.Euler(x, y*-1, z*-1);
                    goTransformacaoVis.transform.localRotation = Quaternion.Euler(x, y, z);
                }
                else
                {
                    goTransformacaoAmb.transform.localScale = new Vector3(x, y, z);
                    goTransformacaoVis.transform.localScale = new Vector3(x, y, z);
                }
            }

            string forma = Util_VisEdu.GetCuboByNomePeca(Global.gameObjectName);

            if (forma != System.String.Empty && Global.propriedadePecas.ContainsKey(forma)) //Se forma for vazio significa que não existe uma forma ainda.
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

        atualizaListaProp();
    }

    private void atualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            Global.propriedadePecas.Remove(prPeca.Nome);
            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
        }
    }  
    
    protected void toggleChanged()
    {
        prPeca.Ativo = gameObject.transform.GetChild(3).GetComponent<Toggle>().isOn;
        updatePosition();
    }

    protected bool pieceChanged()
    {
        bool result = nomePeca != prPeca.Nome;
        nomePeca = prPeca.Nome;
        prPeca.PodeAtualizar = result;
        return result;
    }

    protected bool jaClicouEmAlgumObjeto()
    {
        return Global.gameObjectName != null;
    }

    private void instanciaTransformacao()
    {
        if (!prPeca.JaInstanciou)
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

            prPeca.JaInstanciou = true;            
        }        
    }

    private string validaVazio(string valor)
    {
        if (Equals(valor, String.Empty))
        {
            if (tipoTransformacao == typeTransformacao.Escalar)
                return "1";
            return "0";
        }
        return valor;
    }

}
