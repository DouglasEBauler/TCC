using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static List<GameObject> listaObjetos;
    public static List<string> listaSequenciaSlots = new List<string>();
    public static Dictionary<string, PropriedadePeca> propriedadePecas = new Dictionary<string, PropriedadePeca>();
    public static Dictionary<string, string> listaEncaixes = new Dictionary<string, string>(); // Peça / Slot
    public static Dictionary<string, float> listaPosicaoSlot = new Dictionary<string, float>(); // Slot / Posição de y
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

    public static void AddObject(GameObject go)
    {
        if (listaObjetos == null)
            listaObjetos = new List<GameObject>();

        if (go != null && !listaObjetos.Contains(go))
            listaObjetos.Add(go);
    }

    public static void RemoveObject(GameObject go)
    {
        if (go != null && listaObjetos.Contains(go))
            listaObjetos.Remove(go);
    }

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

    public static void AtualizaListaSlot()
    {
        listaPosicaoSlot.Clear();

        GameObject render = GameObject.Find("Render");

        foreach (Transform child in render.transform)
        {
            if (child.name.Contains("Slot"))
            {
                listaPosicaoSlot.Add(child.name, child.position.y);

                if (child.name.Contains("ObjGraficoSlot"))
                {
                    GameObject objGrafico = GameObject.Find(child.name);

                    foreach (Transform _child in objGrafico.transform)
                    {
                        if (_child.name.Contains("Slot"))
                        {
                            listaPosicaoSlot.Add(_child.name, _child.position.y);
                        }
                    }
                }
            }
        }
    }

    public static void IniciaListaSequenciaSlots(int numObjeto)
    {
        string numObjStr = numObjeto.ToString();

        if (0.Equals(numObjeto))
        {
            listaSequenciaSlots.Add("CameraSlot");
            numObjStr = string.Empty;
        }

        listaSequenciaSlots.Add("ObjGraficoSlot" + numObjStr);
        listaSequenciaSlots.Add("FormasSlot" + numObjStr);
        listaSequenciaSlots.Add("TransformacoesSlot" + numObjStr);
        listaSequenciaSlots.Add("IluminacaoSlot" + numObjStr);
    }
}
