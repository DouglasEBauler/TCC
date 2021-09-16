using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturaSelecionada : MonoBehaviour {

    private void OnMouseDown()
    {
        GameObject.Find("MatrizTextura").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("MatrizCor").transform.GetChild(0).gameObject.SetActive(false);
    }
}
