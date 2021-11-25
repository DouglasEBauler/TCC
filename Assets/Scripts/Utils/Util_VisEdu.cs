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

   public static string GetNumSlot(string slotName, bool isTransfSlot = false)
    {
        string slot = slotName;

        slot = slot.Substring(slot.IndexOf("Slot") + 4);

        if (!isTransfSlot && slot.Contains("_"))
        {
            slot = slot.Remove(slot.IndexOf("_"));
        }

        return slot;
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
        return float.Parse(ValidaVazio(input, isTamInput).Trim(), CultureInfo.InvariantCulture.NumberFormat);
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
        else if (isTamInput && "0".Equals(valor))
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