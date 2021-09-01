using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using UnityEngine.UI;

public class PropIluminacaoScript : PropIluminacaoPadrao {

    public TMP_InputField mainInputField;
    public Toggle toggleField;

    public void Start()
    {    
        if (mainInputField != null)
            mainInputField.onValueChanged.AddListener(delegate { AdicionaValorPropriedadeInput(); });

        if (toggleField != null)
            toggleField.onValueChanged.AddListener(delegate { AdicionaValorPropriedadeToggle(); });

        if (clicouPeca)
        {
            propIluminacao = GameObject.Find("PropIluminacao");
            preencheCampos();
        }
            
    }

    void LateUpdate()
    {       
        if (jaClicouEmAlgumObjeto() && Global.gameObjectName.Contains(Consts.Iluminacao))
        {
            EnableLabelZ();

            if (Global.propriedadePecas.ContainsKey(Global.gameObjectName))
            {
                if (!Global.propriedadePecas[Global.gameObjectName].JaIniciouValores)
                {
                    propIluminacao = GameObject.Find("PropIluminacao");
                    preencheCampos();
                }
                    
            }
        }
    }

    public void AdicionaValorPropriedadeInput()
    {
        if (gameObject.GetComponent<TMP_InputField>().text != string.Empty)
            updatePosition();
    }

    public void AdicionaValorPropriedadeToggle()
    {
        updatePosition();
    }

    public void Start(bool clicouPeca)
    {
        this.clicouPeca = clicouPeca;
        Start();
    }

    private void EnableLabelZ()
    {
        GameObject GOLuzAmbiente = GameObject.Find("LuzAmbiente");
        GameObject GOLuzDirectional = GameObject.Find("LuzDirectional");
        GameObject GOLuzPoint = GameObject.Find("LuzPoint");
        GameObject GOLuzSpot = GameObject.Find("LuzSpot");

        float posZ = 0.000111648f;

        if (Global.Grafico2D)
        {
            if (GOLuzAmbiente != null)
                GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x,
                    GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, 100000);

            if (GOLuzDirectional != null)
            {
                GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x,
                    GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, 100000);

                GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition.x,
                    GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition.y, 100000);
            }                

            if (GOLuzPoint != null)
                GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x,
                    GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, 100000);

            if (GOLuzSpot != null)
            {
                GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x,
                    GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, 100000);

                GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition.x,
                    GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition.y, 100000);
            }
                
        }
        else
        {
            if (GOLuzAmbiente != null)
                GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x,
                    GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, posZ);

            if (GOLuzDirectional != null)
            {
                GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x,
                    GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, posZ);

                GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition.x,
                    GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition.y, posZ);
            }                

            if (GOLuzPoint != null)
                GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x,
                    GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, posZ);

            if (GOLuzSpot != null)
            {
                GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x,
                    GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, posZ);

                GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition.x,
                    GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition.y, posZ);
            }
                
        }
    }
}
