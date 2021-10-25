using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Util_VisEdu : MonoBehaviour
{
    static GameObject _fabricaPecas;
    static GameObject _go_visualizador;
    static GameObject _go_ambienteGrafico;

    public static GameObject CuboVisObjectMain;
    public static GameObject PosicaoAmb;

    static GameObject fabricaPecas
    {
        get
        {
            if (_fabricaPecas == null)
            {
                _fabricaPecas = GameObject.Find("FabricaPecas");
            }

            return _fabricaPecas;
        }
    }
    static GameObject go_visualizador
    {
        get
        {
            if (_go_visualizador == null)
            {
                _go_visualizador = GameObject.Find("GO_Visualizador");
            }

            return _go_visualizador;
        }
    }
    static GameObject go_ambienteGrafico
    {
        get
        {
            if (_go_ambienteGrafico == null)
            {
                _go_ambienteGrafico = GameObject.Find("GO_AmbienteGrafico");
            }

            return _go_ambienteGrafico;
        }
    }

    public static void CriaFormasVazias()
    {
        GameObject cuboVisObjectMain = GameObject.Find("CuboVisObjectMain");
        CuboVisObjectMain = Instantiate(cuboVisObjectMain, cuboVisObjectMain.transform.position, cuboVisObjectMain.transform.rotation, cuboVisObjectMain.transform.parent);

        GameObject posicaoAmb = GameObject.Find("PosicaoAmb");
        PosicaoAmb = Instantiate(posicaoAmb, posicaoAmb.transform.position, posicaoAmb.transform.rotation, posicaoAmb.transform.parent);

        Global.cuboVis = CuboVisObjectMain;
        Global.posAmb = PosicaoAmb;
    }

    public static string GetPecaByName(string nome)
    {
        Transform parentTransf = GameObject.Find(Global.listaEncaixes[nome]).transform.parent;

        foreach (Transform child in parentTransf)
        {
            if (child.name.Contains("FormasSlot"))
            {
                foreach (var encaixe in Global.listaEncaixes)
                {
                    if (encaixe.Value.Equals(child.name)) return encaixe.Key;
                }
            }
        }

        return string.Empty;
    }

    public static string GetForma(string gameObjName)
    {
        string forma = GetPecaByName(gameObjName);

        if (forma.Contains(Consts.CUBO))
        {
            return Consts.CUBO;
        }
        else if (forma.Contains(Consts.POLIGONO))
        {
            return Consts.POLIGONO;
        }
        else if (forma.Contains(Consts.SPLINE))
        {
            return Consts.SPLINE;
        }

        return string.Empty;
    }

    public static string GetSlot(string gameObjName)
    {
        string formaSlot = Global.listaEncaixes[GetPecaByName(gameObjName)];

        return formaSlot.Substring(formaSlot.IndexOf("Slot") + 4, 1);
    }

    public static string GetNumeroSlotObjetoGrafico(GameObject gameObj)
    {
        Transform parentTransf = GameObject.Find(Global.listaEncaixes[gameObj.name]).transform.parent;

        if (parentTransf.name.Contains("ObjGraficoSlot") && parentTransf.name.Length > "ObjGraficoSlot".Length)
        {
            return parentTransf.name.Substring(parentTransf.name.IndexOf("Slot") + 4, 1);
        }

        return GetNumeroSlotObjetoGrafico(parentTransf.gameObject);
    }

    public static PropriedadePeca GetObjetoGraficoSlot(GameObject gameObject)
    {
        return Global.propriedadePecas["ObjetoGraficoP" + GetNumeroSlotObjetoGrafico(gameObject)];
    }

    public static void EnableColliderFabricaPecas(bool enable, bool enablePecas = true)
    {
        foreach (Transform child in fabricaPecas.transform)
        {
            child.GetComponent<BoxCollider>().enabled = Global.listaEncaixes.ContainsKey(child.name) ? enablePecas : enable;
        }
    }

    public static void EnableGOVisualizador(bool enable)
    {
        go_visualizador.SetActive(enable);
    }

    public static void EnableGOAmbienteGrafico(bool enable)
    {
        go_ambienteGrafico.SetActive(enable);
    }

    public static float ConvertField(string input, bool isTamInput = false)
    {
        return float.Parse(ValidaVazio(input, isTamInput), CultureInfo.InvariantCulture.NumberFormat);
    }

    public static int ConvertField(string input)
    {
        return int.Parse(ValidaVazio(input), CultureInfo.InvariantCulture.NumberFormat);
    }

    public static string ValidaVazio(string valor, bool isTamInput = false)
    {
        if (string.Empty.Equals(valor))
        {
            if (isTamInput)
            {
                return "1";
            }

            return "0";
        }
        else if (isTamInput)
        {
            return "1";
        }

        return valor;
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}