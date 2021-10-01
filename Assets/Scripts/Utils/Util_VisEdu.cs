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

    public static string GetCuboByNomePeca(string nome)
    {
        string cubo = string.Empty;

        if (!string.Empty.Equals(nome))
        {
            foreach (Transform child in GameObject.Find(Global.listaEncaixes[nome]).transform.parent)
            {
                if ("FormasSlot".Equals(child.name))
                {
                    foreach (var pair in Global.listaEncaixes)
                    {
                        if (pair.Value.Equals(child.name)) return pair.Key;
                    }
                }
            }
        }

        return cubo;
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
        return (string.Empty.Equals(valor)) ? (!isTamInput ? "0" : "1") : valor;
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

public static class Extension
{
    public static IEnumerable<Transform> GetChildren(this Transform tr)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in tr)
        {
            children.Add(child);
        }

        // You can make the return type an array or a list or else.
        return children;
    }
}
