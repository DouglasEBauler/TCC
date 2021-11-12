using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Dictionary<string, PropriedadePeca> propriedadePecas = new Dictionary<string, PropriedadePeca>(); // Nome da peça -> Propriedades
    public static Dictionary<string, string> listaEncaixes = new Dictionary<string, string>(); // Nome da peça -> slot
    public static PropriedadeCamera propCameraGlobal = new PropriedadeCamera();

    public static bool cameraAtiva;
    public static bool Grafico2D;
    public static int countTransformacoes;
    public static int countObjetosGraficos;
}
