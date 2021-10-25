using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizadorGrafScript : MonoBehaviour {

    public static bool entrouVisualizadorGrafico;

    private void OnMouseEnter()
    {
        entrouVisualizadorGrafico = true;
    }

    private void OnMouseExit()
    {
        entrouVisualizadorGrafico = false;
    }


}
