using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorSelecionada : MonoBehaviour 
{
    const string AMBIENTE = "Ambiente";
    const string DIRECTIONAL = "Directional";
    const string POINT = "Point";
    const string SPOT = "Spot";

    void OnMouseDown()
    {
        string iluminacao = string.Empty;

        if (gameObject.name.Contains(AMBIENTE))
            iluminacao = AMBIENTE;
        if (gameObject.name.Contains(DIRECTIONAL))
            iluminacao = DIRECTIONAL;
        if (gameObject.name.Contains(POINT))
            iluminacao = POINT;
        if (gameObject.name.Contains(SPOT))
            iluminacao = SPOT;

        GameObject.Find("MatrizCor" + iluminacao).transform.GetChild(0).gameObject.SetActive(true);

        if (string.Empty.Equals(iluminacao))
            GameObject.Find("MatrizTextura").transform.GetChild(0).gameObject.SetActive(false);
    }
}
