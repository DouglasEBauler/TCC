using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropRenderer : MonoBehaviour
{
    public GameObject VisualizadorGraf;
    public GameObject Visualizador;

    [SerializeField]
    Dropdown graficos;
    [SerializeField]
    Toggle toggleField;
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject camObjeto;
    [SerializeField]
    Camera camVis;
    [SerializeField]
    GameObject camVisPai;
    [SerializeField]
    GameObject gizmoPivot;

    public void AdicionaValorPropriedade()
    {
        Global.Grafico2D = !0.Equals(graficos.value);

        cam.orthographic = Global.Grafico2D;
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, !Global.Grafico2D ? 125 : -200, cam.transform.localPosition.z);
        cam.transform.localRotation = Quaternion.Euler(!Global.Grafico2D ? 33.75f : 0, -180, 0);
        cam.GetComponent<MoveAmbiente>().enabled = !Global.Grafico2D;
        if (Global.Grafico2D)
        {
            camVis.orthographicSize = 150f;
        }

        camVisPai.transform.localPosition = !Global.Grafico2D ? new Vector3(1400f, 4000f, -4800f) : new Vector3(31, 49, -4256);
        camVisPai.transform.localRotation = !Global.Grafico2D ? Quaternion.Euler(37.28f, -15.229f, 0) : Quaternion.Euler(0.215f, 0, 0);

        camObjeto.transform.localPosition = !Global.Grafico2D ? new Vector3(966f, 0, -19962.67f) : new Vector3(1057f, -296, -19920);
        camObjeto.transform.localRotation = !Global.Grafico2D ? Quaternion.Euler(38.701f, -195.786f, -100.024f) : Quaternion.Euler(0.475f, -181.515f, -90.013f);


    }

    public void AdicionaValorPropriedadeToggle()
    {
        EnableGizmo(toggleField.GetComponent<Toggle>().isOn);
    }

    void EnableGizmo(bool status)
    {
        foreach (Transform child in gizmoPivot.transform.GetChild(0))
            child.GetComponent<MeshRenderer>().enabled = status;
    }
}
