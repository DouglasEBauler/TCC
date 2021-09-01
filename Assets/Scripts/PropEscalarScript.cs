using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PropEscalarScript : PropriedadePecaPadrao
{
    public GameObject label;

    void Update()
    {       
        if (jaClicouEmAlgumObjeto() && Global.gameObjectName.Contains(Consts.Escalar))
        {
            label.SetActive(!Global.Grafico2D);

            tipoTransformacao = typeTransformacao.Escalar;

            if (!inicializou() || pieceChanged())
                mainMethod();

            if (Input.GetKey(KeyCode.Return))
                updatePosition();

            toggleChanged();
        }            
    }
}
