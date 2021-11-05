using System;
using UnityEngine;
using UnityEngine.UI;

public class PropRenderer : MonoBehaviour
{
    [SerializeField]
    GameObject corSeletorLimpeza;
    [SerializeField]
    FlexibleColorPicker corSelecionadaLimpeza;
    [SerializeField]
    GameObject corSeletorFundo;
    [SerializeField]
    FlexibleColorPicker corSelecionadaFundo;
    [SerializeField]
    Toggle toggleEixo;
    [SerializeField]
    Toggle toggleGrade;
    [SerializeField]
    Dropdown graficos;

    [SerializeField]
    GameObject visualizadorGraf;
    [SerializeField]
    GameObject visualizador;
    [SerializeField]
    GameObject eixoX;
    [SerializeField]
    GameObject eixoY;
    [SerializeField]
    GameObject eixoZ;

    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject camObjeto;
    [SerializeField]
    Camera camVis;
    [SerializeField]
    GameObject camVisPai;
    [SerializeField]
    GameObject gizmo;

    void Start()
    {
        corSelecionadaLimpeza.color = Color.black;
        visualizador.GetComponent<MeshRenderer>().materials[0].color = corSelecionadaLimpeza.color;
        corSelecionadaFundo.color = Color.black;
        visualizadorGraf.GetComponent<MeshRenderer>().materials[0].color = corSelecionadaFundo.color;
        toggleEixo.isOn = true;
        toggleGrade.isOn = true;
        Global.Grafico2D = false;
    }

    void FixedUpdate()
    {
        UpdateProp();
    }

    public void ControlPanelColorLimpeza()
    {
        corSelecionadaLimpeza.gameObject.SetActive(!corSelecionadaLimpeza.gameObject.activeSelf);
    }
    public void ControlPanelColorFundo()
    {
        corSelecionadaFundo.gameObject.SetActive(!corSelecionadaFundo.gameObject.activeSelf);
    }

    public void UpdateProp()
    {
        DefineDimensao();
        DefineCores();
        EnabledEixo();
        EnabledGrid();
    }

    void EnabledEixo()
    {
        eixoX.GetComponent<MeshRenderer>().enabled = toggleEixo.isOn;
        eixoY.GetComponent<MeshRenderer>().enabled = toggleEixo.isOn;
        eixoZ.GetComponent<MeshRenderer>().enabled = toggleEixo.isOn;
    }

    void EnabledGrid()
    {
        // Gambiarra a ser resolvida
        int count = 0;
        foreach (Transform child in gizmo.transform)
        {
            if (count > 2)
            {
                child.GetComponent<MeshRenderer>().enabled = toggleGrade.isOn;
            }
            count++;
        }
    }

    void DefineCores()
    {
        corSeletorLimpeza.GetComponent<Image>().material.color = corSelecionadaLimpeza.color;
        visualizador.GetComponent<MeshRenderer>().materials[0].color = corSelecionadaLimpeza.color;

        corSeletorFundo.GetComponent<Image>().material.color = corSelecionadaLimpeza.color;
        visualizadorGraf.GetComponent<MeshRenderer>().materials[0].color = corSelecionadaFundo.color;
    }

    void DefineDimensao()
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
}
