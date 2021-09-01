using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropIluminacaoPadrao : MonoBehaviour {

    protected enum InputSelected { InputPosX, InputPosY, InputPosZ, ToggleAtivo, InputIntensidade, InputValorX, InputValorY, InputValorZ, InputDistancia, InputAngulo, InputExpoente, InputEmpty };
    public enum TipoIluminacao { Ambiente, Directional, Point, Spot };

    protected InputSelected inputSelected;
    protected GameObject propIluminacao;
    protected bool clicouPeca;

    private const float VALOR_INICIAL_ROTACAO_X = 71.819f;
    private const float VALOR_INICIAL_ROTACAO_Y = 90f;

    protected void preencheCampos()
    {
        Global.propriedadePecas[Global.gameObjectName].JaIniciouValores = true;
        propIluminacao.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.gameObjectName;

        if (!Global.propriedadePecas[Global.gameObjectName].JaInstanciou)
            instanciaTransformacao();

        if (Global.propriedadePecas.ContainsKey(Global.gameObjectName))
            preencheCamposIluminacao((TipoIluminacao)Global.propriedadePecas[Global.gameObjectName].TipoLuz);  
    }

    protected void updatePosition()
    {
        if (Global.propriedadePecas[Global.gameObjectName].JaInstanciou)
        {
            string inputValue = string.Empty;

            if (!gameObject.name.Contains("Toggle"))
                inputValue = gameObject.GetComponent<TMP_InputField>().text;

            inputSelected = GetFieldSelected(gameObject.name); 

            if (inputSelected == InputSelected.InputPosX)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Pos.X = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.InputPosY)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Pos.Y = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.InputPosZ)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Pos.Z = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.ToggleAtivo)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Ativo = gameObject.GetComponent<Toggle>().isOn;
            else if (inputSelected == InputSelected.InputIntensidade)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Intensidade = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.InputValorX)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.InputValorY)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.InputValorZ)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.InputDistancia)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Distancia = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.InputAngulo)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Angulo = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            else if (inputSelected == InputSelected.InputExpoente)
                Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Expoente = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);

            GameObject lightObject = null;
            bool podeAtualizarValor = false;

            if (Global.propriedadePecas[Global.gameObjectName].TipoLuz == 0)
            {
                lightObject = GameObject.Find("Ambiente" + Global.gameObjectName);

                Color cor = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Cor;
                bool ativo = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Ativo;

                for (int i = 0; i < lightObject.transform.childCount; i++)
                {
                    lightObject.transform.GetChild(i).GetComponent<Light>().color = cor;
                    lightObject.transform.GetChild(i).GetComponent<Light>().enabled = ativo;
                }

                if (inputSelected == InputSelected.ToggleAtivo)
                {
                    AtivaCamera(ativo);
                    Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Ativo = ativo;
                    Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Ativo = ativo;
                    Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Ativo = ativo;
                    Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Ativo = ativo;
                }

            }                
            else if (Global.propriedadePecas[Global.gameObjectName].TipoLuz == 1)
            {
                lightObject = GameObject.Find("Directional" + Global.gameObjectName);

                if (inputSelected == InputSelected.InputValorX)
                {
                    podeAtualizarValor = true;                    

                    //Se o Valor X estiver sendo alterado, atualiza Y e Z com o mesmo valor
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X;
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X;

                    //Altera no painel
                    GameObject propGO = GameObject.Find("PropIluminacao");

                    propGO.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X.ToString();
                    propGO.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X.ToString();
                }
                else if (inputSelected == InputSelected.InputValorY)
                {
                    podeAtualizarValor = true;

                    //Se o Valor Y estiver sendo alterado, atualiza X e Z com o mesmo valor
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y;
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y;

                    //Altera no painel
                    GameObject propGO = GameObject.Find("PropIluminacao");

                    propGO.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y.ToString();
                    propGO.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y.ToString();
                }
                else if (inputSelected == InputSelected.InputValorZ)
                {
                    podeAtualizarValor = true;

                    //Se o Valor Z estiver sendo alterado, atualiza X e Y com o mesmo valor
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z;
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z;

                    //Altera no painel
                    GameObject propGO = GameObject.Find("PropIluminacao");

                    propGO.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z.ToString();
                    propGO.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z.ToString();
                }

                if (podeAtualizarValor)
                {
                    if (Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X == 0 &&
                        Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y == 0 &&
                        Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z == 0)
                        lightObject.GetComponent<CameraObjectScript>().enabled = true;
                    else
                        lightObject.GetComponent<CameraObjectScript>().enabled = false;

                    lightObject.transform.localRotation = Quaternion.Euler(Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X + VALOR_INICIAL_ROTACAO_X, VALOR_INICIAL_ROTACAO_Y, 0);
                }   
            }                
            else if (Global.propriedadePecas[Global.gameObjectName].TipoLuz == 2)
            {
                lightObject = GameObject.Find("Point" + Global.gameObjectName);
                lightObject.GetComponent<Light>().range = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Distancia;
            }                
            else if (Global.propriedadePecas[Global.gameObjectName].TipoLuz == 3)
            {
                lightObject = GameObject.Find("Spot" + Global.gameObjectName);
                lightObject.GetComponent<Light>().range = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Distancia;

                // Altera escala de acordo com o range.
                GameObject ObjSpot = GameObject.Find("ObjSpot" + Global.gameObjectName);

                float scaleObjSpot = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Distancia / 1000;

                ObjSpot.transform.localScale = new Vector3(scaleObjSpot, scaleObjSpot, scaleObjSpot);
                ObjSpot.transform.localPosition = Vector3.zero;                

                lightObject.GetComponent<Light>().spotAngle = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Angulo;

                // Altera o ângulo de acordo com o SpotAngle.
                GameObject SpotIlum = GameObject.Find("Spot" + Global.gameObjectName);

                float scaleMeshSpot = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Angulo * 0.033f;

                SpotIlum.transform.localScale = new Vector3(scaleMeshSpot, scaleMeshSpot, SpotIlum.transform.localScale.z);

                if (inputSelected == InputSelected.InputValorX)
                {
                    podeAtualizarValor = true;

                    //Se o Valor X estiver sendo alterado, atualiza Y e Z com o mesmo valor
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X;
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X;

                    //Altera no painel
                    GameObject propGO = GameObject.Find("PropIluminacao");

                    propGO.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X.ToString();
                    propGO.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X.ToString();
                }
                else if (inputSelected == InputSelected.InputValorY)
                {
                    podeAtualizarValor = true;

                    //Se o Valor Y estiver sendo alterado, atualiza X e Z com o mesmo valor
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y;
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y;

                    //Altera no painel
                    GameObject propGO = GameObject.Find("PropIluminacao");

                    propGO.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y.ToString();
                    propGO.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y.ToString();
                }
                else if (inputSelected == InputSelected.InputValorZ)
                {
                    podeAtualizarValor = true;

                    //Se o Valor Z estiver sendo alterado, atualiza X e Y com o mesmo valor
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z;
                    Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z;

                    //Altera no painel
                    GameObject propGO = GameObject.Find("PropIluminacao");

                    propGO.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z.ToString();
                    propGO.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z.ToString();
                }

                if (podeAtualizarValor)
                {
                    if (Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X == 0 &&
                         Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Y == 0 &&
                         Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.Z == 0)
                        lightObject.GetComponent<CameraObjectScript>().enabled = true;
                    else
                        lightObject.GetComponent<CameraObjectScript>().enabled = false;

                    lightObject.transform.localRotation = Quaternion.Euler(Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].ValorIluminacao.X + VALOR_INICIAL_ROTACAO_X, VALOR_INICIAL_ROTACAO_Y, 0);
                }

            }

            // Propriedades comuns entre todas iluminações, exceto iluminação "Ambiente".
            if (Global.propriedadePecas[Global.gameObjectName].TipoLuz != 0)
            {
                lightObject.transform.localPosition = new Vector3(-Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Pos.X, Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Pos.Y, Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Pos.Z);
                lightObject.GetComponent<Light>().color = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Cor;
                lightObject.GetComponent<Light>().intensity = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Intensidade;
                AtivaIluminacao(GetTipoLuzPorExtenso(Global.propriedadePecas[Global.gameObjectName].TipoLuz) + Global.gameObjectName, Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Ativo);

                if (inputSelected == InputSelected.ToggleAtivo)
                {
                    bool ativo = Global.propriedadeIluminacao[Global.gameObjectName][Global.propriedadePecas[Global.gameObjectName].TipoLuz].Ativo;

                    AtivaCamera(ativo);
                    Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Ativo = ativo;
                    Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Ativo = ativo;
                    Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Ativo = ativo;
                    Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Ativo = ativo;
                }
            }

            AtualizaCamera();
        }        
    }

    protected bool jaClicouEmAlgumObjeto()
    {
        return Global.gameObjectName != null;
    }

    private string validaVazio(string valor)
    {
        if (Equals(valor, System.String.Empty))
        {
            return "0";
        }
        return valor;
    }

    protected InputSelected GetFieldSelected(string fieldName)
    { 
        if (fieldName.Contains("PosicaoX"))
            return InputSelected.InputPosX;
        else if (fieldName.Contains("PosicaoY"))
            return InputSelected.InputPosY;
        else if (fieldName.Contains("PosicaoZ"))
            return InputSelected.InputPosZ;
        else if (fieldName.Contains("Toggle"))
            return InputSelected.ToggleAtivo;
        else if (fieldName.Contains("Intensidade"))
            return InputSelected.InputIntensidade;
        else if (fieldName.Contains("XValores"))
            return InputSelected.InputValorX;
        else if (fieldName.Contains("YValores"))
            return InputSelected.InputValorY;
        else if (fieldName.Contains("ZValores"))
            return InputSelected.InputValorZ;
        else if (fieldName.Contains("Distancia"))
            return InputSelected.InputDistancia;
        else if (fieldName.Contains("Angulo"))
            return InputSelected.InputAngulo;
        else if (fieldName.Contains("Expoente"))
            return InputSelected.InputExpoente;

        return InputSelected.InputEmpty;
    }

    public void AjustaCameraEmX()
    {
        GameObject go = GameObject.Find("CameraObjetoMain");
        go.transform.GetComponent<RectTransform>().transform.position =
            new Vector3(go.transform.GetComponent<RectTransform>().transform.position.x + 1,
                        go.transform.GetComponent<RectTransform>().transform.position.y,
                        go.transform.GetComponent<RectTransform>().transform.position.z);
    }

    public void preencheCamposIluminacao(TipoIluminacao iluminacao)
    {
        if (propIluminacao == null)
            propIluminacao = GameObject.Find("PropIluminacao");

        GameObject lightObject = null;

        switch ((int)iluminacao)
        {
            case (int)TipoIluminacao.Ambiente:
                propIluminacao.transform.GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>().value = 0;
                propIluminacao.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Pos.X.ToString();
                propIluminacao.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Pos.Y.ToString();
                propIluminacao.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Pos.Z.ToString();
                GameObject.Find("CorSelecionadaAmbiente").GetComponent<MeshRenderer>().materials[0].color = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Cor;
                propIluminacao.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(4).GetComponent<Toggle>().isOn = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Ativo;

                //AtivaCamera(Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Ativo);
                break;
            case (int)TipoIluminacao.Directional:
                propIluminacao.transform.GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>().value = 1;
                propIluminacao.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Pos.X.ToString();
                propIluminacao.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Pos.Y.ToString();
                propIluminacao.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Pos.Z.ToString();
                GameObject.Find("CorSelecionadaDirectional").GetComponent<MeshRenderer>().materials[0].color = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Cor;
                propIluminacao.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(4).GetComponent<Toggle>().isOn = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Ativo;
                propIluminacao.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Intensidade.ToString();
                propIluminacao.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].ValorIluminacao.X.ToString();
                propIluminacao.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].ValorIluminacao.Y.ToString();
                propIluminacao.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].ValorIluminacao.Z.ToString();

                // Alterações do Objeto e Iluminação "Directional".
                lightObject = GameObject.Find("Directional" + Global.gameObjectName);
                lightObject.transform.localPosition = new Vector3(-Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Pos.X, Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Pos.Y, Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Pos.Z);
                lightObject.GetComponent<Light>().color = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Cor;
                lightObject.GetComponent<Light>().intensity = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Intensidade;
                AtivaIluminacao(lightObject.name, Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Ativo);

                //AtivaCamera(Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Ativo);

                break;
            case (int)TipoIluminacao.Point:
                propIluminacao.transform.GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>().value = 2;
                propIluminacao.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Pos.X.ToString();
                propIluminacao.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Pos.Y.ToString();
                propIluminacao.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Pos.Z.ToString();
                GameObject.Find("CorSelecionadaPoint").GetComponent<MeshRenderer>().materials[0].color = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Cor;
                propIluminacao.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(4).GetComponent<Toggle>().isOn = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Ativo;
                propIluminacao.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Intensidade.ToString();
                propIluminacao.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Distancia.ToString();

                // Alterações do Objeto e Iluminação "Point".
                lightObject = GameObject.Find("Point" + Global.gameObjectName);
                lightObject.transform.localPosition = new Vector3(-Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Pos.X, Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Pos.Y, Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Pos.Z);
                lightObject.GetComponent<Light>().color = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Cor;
                lightObject.GetComponent<Light>().intensity = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Intensidade;
                lightObject.GetComponent<Light>().range = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Distancia;

                //AtivaCamera(Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Ativo);

                break;
            case (int)TipoIluminacao.Spot:
                propIluminacao.transform.GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>().value = 3;
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Pos.X.ToString();
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Pos.Y.ToString();
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Pos.Z.ToString();
                GameObject.Find("CorSelecionadaSpot").GetComponent<MeshRenderer>().materials[0].color = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Cor;
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(4).GetComponent<Toggle>().isOn = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Ativo;
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Intensidade.ToString();
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Distancia.ToString();
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Angulo.ToString();
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(3).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Expoente.ToString();
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].ValorIluminacao.X.ToString();
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].ValorIluminacao.Y.ToString();
                propIluminacao.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].ValorIluminacao.Z.ToString();

                // Alterações do Objeto e Iluminação "Spot".
                lightObject = GameObject.Find("Spot" + Global.gameObjectName);
                lightObject.transform.localPosition = new Vector3(-Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Pos.X, Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Pos.Y, Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Pos.Z);
                lightObject.GetComponent<Light>().color = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Cor;
                lightObject.GetComponent<Light>().intensity = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Intensidade;
                lightObject.GetComponent<Light>().range = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Distancia;

               // AtivaCamera(Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Ativo);

                //// Altera escala de acordo com o range.
                //float scaleObjSpot = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Distancia / 1000;
                //Transform objSpot = lightObject.transform.GetChild(0).transform;

                //objSpot.localScale = new Vector3(scaleObjSpot, scaleObjSpot, scaleObjSpot);
                //objSpot.localPosition = Vector3.zero;
                break;
        }   
    }

    public void preencheCamposIluminacao(int iluminacao)
    {
        switch (iluminacao)
        {
            case 0:
                preencheCamposIluminacao(TipoIluminacao.Ambiente);
                break;
            case 1:
                preencheCamposIluminacao(TipoIluminacao.Directional);
                break;
            case 2:
                preencheCamposIluminacao(TipoIluminacao.Point);
                break;
            case 3:
                preencheCamposIluminacao(TipoIluminacao.Spot);
                break;
        }        
    }

    public void instanciaTransformacao()
    {
        if (!Global.propriedadeIluminacao.ContainsKey(Global.gameObjectName))
        {
            PropriedadePeca[] pecas = new PropriedadePeca[4];

            for (int i = 0; i < pecas.Length; i++)
            {
                PropriedadePeca peca = new PropriedadePeca();

                peca.Pos = new Posicao();
                peca.ValorIluminacao = new ValorIluminacao();

                peca.Pos.X = 100;
                peca.Pos.Y = 300;
                peca.Pos.Z = 0;
                peca.Cor = Color.white;
                peca.Ativo = true;
                peca.Intensidade = 1.5f;
                peca.ValorIluminacao.X = 0;
                peca.ValorIluminacao.Y = 0;
                peca.ValorIluminacao.Z = 0;
                peca.Distancia = 1000;
                peca.Angulo = 30f;
                peca.Expoente = 10f;

                pecas[i] = peca;
            }

            Global.propriedadeIluminacao.Add(Global.gameObjectName, pecas);
            Global.propriedadePecas[Global.gameObjectName].JaInstanciou = true;
        }        
    }

    public bool existeIluminacao()
    {
        foreach (KeyValuePair<string, string> peca in Global.listaEncaixes)
        {
            if (peca.Key.Contains("Iluminacao"))
                return true;
        }

        return false;
    }

    public string GetTipoLuzPorExtenso(int tipoLuz)
    {
        string tipoLuzExtenso = string.Empty;

        switch (tipoLuz)
        {
            case 0: tipoLuzExtenso = "Ambiente";
                break;
            case 1: tipoLuzExtenso = "Directional";
                break;
            case 2: tipoLuzExtenso = "Point";
                break;
            case 3: tipoLuzExtenso = "Spot";
                break;
        }

        return tipoLuzExtenso;
    }

    public void AtivaIluminacao(string nomeIluminacao, bool ativo)
    {
        if (!nomeIluminacao.Contains("Ambiente"))
        {
            GameObject.Find(nomeIluminacao).GetComponent<Light>().enabled = ativo;

            GameObject GO = GameObject.Find(nomeIluminacao).transform.GetChild(0).gameObject;

            if (nomeIluminacao.Contains("Spot"))
            {
                for (int i = 0; i < GO.transform.GetChild(0).GetChild(0).childCount; i++)
                {
                    GO.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = ativo;
                    GO.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = ativo;
                }
            }
            else
            {
                for (int i = 0; i < GO.transform.childCount; i++)
                    GO.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = ativo;
            }
        }
        else
        {
            for (int i = 0; i < GameObject.Find(nomeIluminacao).transform.childCount; i++)
            {
                GameObject.Find(nomeIluminacao).transform.GetChild(i).GetComponent<Light>().enabled = ativo;
            }
        }      
    }

    public void AtivaCamera(bool status)
    {
        if (status && Global.cameraAtiva)
            GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");        
        else
            GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Nothing");
    }

    public void AtualizaCamera()
    {
        bool existeCameraAtiva = false;

        foreach (KeyValuePair<string, PropriedadePeca[]> dic in Global.propriedadeIluminacao)
        {    
            for (int i = 0; i < dic.Value.Length; i++)
            {
                if (Global.propriedadeIluminacao[dic.Key][i].Ativo)
                {
                    AtivaCamera(true);
                    existeCameraAtiva = true;
                    break;
                }                   
            }

            if (existeCameraAtiva)
                break;
        }

        if (!existeCameraAtiva)
            AtivaCamera(false);
    }

}
