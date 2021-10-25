using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Dictionary<string, PropriedadePeca> propriedadePecas = new Dictionary<string, PropriedadePeca>();
    public static Dictionary<string, string> listaEncaixes = new Dictionary<string, string>(); // Peça / Slot
    public static Dictionary<string, PropriedadePeca[]> propriedadeIluminacao = new Dictionary<string, PropriedadePeca[]>();
    public static PropriedadeCamera propCameraGlobal = new PropriedadeCamera();
    public static ArrayList lightObjectList = new ArrayList();

    public static GameObject lastPressedButton;
    public static string gameObjectName;
    public static bool podeAtualizar;
    public static string lastObjetoGraficoName;
    public static string slotName;
    public static GameObject cuboVis;
    public static GameObject posAmb;
    public static bool cameraAtiva;
    public static bool Grafico2D;

    public static string GetSlot(string pecaName)
    {
        string nomeObj = string.Empty;

        if (pecaName.Contains("Camera"))
            nomeObj = "CameraSlot";
        else if (pecaName.Contains("ObjetoGraficoP"))
            nomeObj = "ObjGraficoSlot";
        else if (pecaName.Contains("Cubo") || pecaName.Contains("Poligono") || pecaName.Contains("Spline"))
            nomeObj = "FormasSlot";
        else if (pecaName.Contains("Transladar") || pecaName.Contains("Rotacionar") || pecaName.Contains("Escalar"))
            nomeObj = "TransformacoesSlot";
        else if (pecaName.Contains("Iluminacao"))
            nomeObj = "IluminacaoSlot";

        return nomeObj;
    }
}
