using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecionaCor : MonoBehaviour {

    public Color corObjeto;
    private string corSelecionada = "CorSelecionada";

    private void OnMouseDown()
    {   
        // Pega a cor da matriz de cores e demonstra na variável.
        corObjeto = gameObject.GetComponent<MeshRenderer>().materials[0].color;

        // Inclui a cor no objeto.
        if (Global.propriedadePecas.ContainsKey(Global.gameObjectName))
            Global.propriedadePecas[Global.gameObjectName].Cor = corObjeto;

        //Pinta os cubos

        if (Global.gameObjectName.Contains("Iluminacao"))
        {
            GameObject light = GameObject.Find(new PropIluminacaoPadrao().GetTipoLuzPorExtenso(Global.propriedadePecas[Global.gameObjectName].TipoLuz) + Global.gameObjectName);

            if (light.name.Contains("Ambiente"))
            {
                // Muda a cor de todos objetos da luz ambiente. 
                for (int i = 0; i < light.transform.childCount; i++)
                    light.transform.GetChild(i).GetComponent<Light>().color = corObjeto;
            }
            else
            {
                // Muda cor da luz.
                light.GetComponent<Light>().color = corObjeto;
                // Muda cor do objeto da luz.
                ChageObjectLightColor("Obj" + new PropIluminacaoPadrao().GetTipoLuzPorExtenso(Global.propriedadePecas[Global.gameObjectName].TipoLuz) + Global.gameObjectName, corObjeto);
            }
            
        }
        else
        {
            GameObject.Find(Global.propriedadePecas[Global.gameObjectName].NomeCuboAmbiente).GetComponent<MeshRenderer>().materials[0].color = corObjeto;
            GameObject.Find(Global.propriedadePecas[Global.gameObjectName].NomeCuboVis).GetComponent<MeshRenderer>().materials[0].color = corObjeto;
        }
        
        corSelecionada = corSelecionadaIluminacao();        

        // Muda seletor de cor para a cor selecionada.
        GameObject.Find(corSelecionada).GetComponent<MeshRenderer>().materials[0].color = corObjeto;

        gameObject.transform.parent.gameObject.SetActive(false);
    }

    private string corSelecionadaIluminacao()
    {
        if (Global.gameObjectName.Contains("Iluminacao"))
        {
            string tipoIluminacao = gameObject.transform.parent.parent.name.Substring("MatrizCor".Length, gameObject.transform.parent.parent.name.Length - "MatrizCor".Length);
            int idxIluminacao = 0;

            if (Equals(tipoIluminacao, "Ambiente"))
                idxIluminacao = 0;
            if (Equals(tipoIluminacao, "Directional"))
                idxIluminacao = 1;
            if (Equals(tipoIluminacao, "Point"))
                idxIluminacao = 2;
            if (Equals(tipoIluminacao, "Spot"))
                idxIluminacao = 3;

            Global.propriedadeIluminacao[Global.gameObjectName][idxIluminacao].Cor = corObjeto;
            return corSelecionada.Substring(0, "CorSelecionada".Length) + tipoIluminacao;
        }            

        return corSelecionada;
    }

    private void ChageObjectLightColor(string nome, Color cor)
    {
        GameObject go = GameObject.Find(nome);

        for (int i = 0; i < go.transform.childCount; i++)
        {
            // Se for Spot tem mais hierarquia...
            if (nome.Contains("Spot"))
            {
                go.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().materials[0].color = cor;

                for (int j = 0; j < go.transform.GetChild(0).GetChild(i).childCount; j++)
                    go.transform.GetChild(0).GetChild(i).GetChild(j).GetComponent<MeshRenderer>().materials[0].color = cor;
            }else
                go.transform.GetChild(i).GetComponent<MeshRenderer>().materials[0].color = cor;
        }
    }
}
