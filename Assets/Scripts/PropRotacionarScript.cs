using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PropRotacionarScript : PropriedadePecaPadrao {

    public GameObject label;

    void Update()
    {
        if (jaClicouEmAlgumObjeto() && Global.gameObjectName.Contains(Consts.Rotacionar))
        {
            label.SetActive(!Global.Grafico2D);

            tipoTransformacao = typeTransformacao.Rotacionar;

            if (!inicializou() || pieceChanged())
                mainMethod();

            if (Input.GetKey(KeyCode.Return))
                updatePosition();

            toggleChanged();
        }        
    }   
    
}
