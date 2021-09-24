using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PropObjetoGraficoScript : MonoBehaviour 
{
    [SerializeField]
    Toggle toggleField;
    [SerializeField]
    TMP_InputField nomePeca;

    void Start()
    {
        if (Global.gameObjectName != null)
        {
            if (Global.propriedadePecas[Global.gameObjectName] != null)
            {
                Global.propriedadePecas[Global.gameObjectName].Ativo = true;
            }

            // Nome.
            nomePeca.text = "ObjetoGraficoP" + (Global.gameObjectName.Length > "ObjetoGraficoP".Length ? Global.gameObjectName.Substring("ObjetoGraficoP".Length, 1) : string.Empty);

            // Toggle.
            toggleField.isOn = Global.propriedadePecas[Global.gameObjectName].Ativo;
        }        
    }  

    public void OnValueChanged()
    {
        Global.propriedadePecas[Global.gameObjectName].Ativo = toggleField.isOn;
        AtualizaCubo(toggleField.isOn);
    }

    void AtualizaCubo(bool isOn)
    {
        MeshRenderer cuboMeshRenderer = GameObject.Find(((CuboPropriedadePeca)Global.propriedadePecas[Global.gameObjectName]).NomeCuboAmbiente).GetComponent<MeshRenderer>();

        if (!isOn)
            cuboMeshRenderer.enabled = false;
        else
        {
            GameObject goObjGraficoSlot = GameObject.Find(Global.listaEncaixes[Global.gameObjectName]);
            string formasSlot = string.Empty;
            string peca = string.Empty;

            // Descobre nome do FormasSlot correto.
            foreach (Transform child in goObjGraficoSlot.transform)
            {
                if (child.name.Contains("FormasSlot"))
                {
                    formasSlot = child.name;
                    break;
                }
            }

            // Descobre nome da peça para acessar suas propriedades.
            foreach (var enc in Global.listaEncaixes)
            {
                if (enc.Value.Equals(formasSlot)) peca = enc.Key;
            }

            // Se o cubo estiver ativo então demonstra a peça, senão continua desabilitada.
            if (!string.Empty.Equals(peca))
            {
                bool existePropriedade = false;

                // Verifica se a peça ja foi iniciada.
                foreach (var prop in Global.propriedadePecas)
                {
                    if (prop.Key.Equals(peca))
                    {
                        existePropriedade = true;
                        break;
                    }                        
                }

                cuboMeshRenderer.enabled = existePropriedade ? Global.propriedadePecas[peca].Ativo : true;
            }
        }
    }
}
