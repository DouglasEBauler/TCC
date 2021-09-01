using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropTransladarScript : PropriedadePecaPadrao
{
    public GameObject label;

    void Update()
    {     
        if (jaClicouEmAlgumObjeto() && Global.gameObjectName.Contains(Consts.Transladar))
        {
            label.SetActive(!Global.Grafico2D);

            tipoTransformacao = typeTransformacao.Transladar;

            if (!inicializou() || pieceChanged())
                mainMethod();

            if (Input.GetKey(KeyCode.Return))
                updatePosition();

            toggleChanged();            
        }            
    }
}
