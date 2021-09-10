using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropRenderer : MonoBehaviour 
{
    [SerializeField]
    TMP_Dropdown mainDropdown;
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
        if (0.Equals(mainDropdown.GetComponent<TMP_Dropdown>().value)) // 3D
        {
            cam.orthographic = false;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, 125, cam.transform.localPosition.z);
            cam.transform.localRotation = Quaternion.Euler(33.75f, -180, 0);
            cam.GetComponent<MoveAmbiente>().enabled = true;

            camVis.orthographic = false;

            camVisPai.transform.localPosition = new Vector3(1400f, 4000f, -4800f);
            camVisPai.transform.localRotation = Quaternion.Euler(37.28f, -15.229f, 0);

            camObjeto.transform.localPosition = new Vector3(966f, 0, -19962.67f);
            camObjeto.transform.localRotation = Quaternion.Euler(38.701f, -195.786f, -100.024f);

            Global.Grafico2D = false;
        }
        else // 2D
        {
            cam.orthographic = true;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, -200, cam.transform.localPosition.z);
            cam.transform.localRotation = Quaternion.Euler(0, -180, 0);
            cam.orthographicSize = 300f;
            cam.GetComponent<MoveAmbiente>().enabled = false;
            
            camVis.orthographic = true;
            camVis.orthographicSize = 150f;

            camVisPai.transform.localPosition = new Vector3(31, 49, -4256);
            camVisPai.transform.localRotation = Quaternion.Euler(0.215f, 0, 0);

            camObjeto.transform.localPosition = new Vector3(1057f, -296, -19920);
            camObjeto.transform.localRotation = Quaternion.Euler(0.475f, -181.515f, -90.013f);

            Global.Grafico2D = true;
        }
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
