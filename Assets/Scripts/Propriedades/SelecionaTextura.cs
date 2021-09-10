using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecionaTextura : MonoBehaviour {

    public Texture texturaObjeto;

    private void OnMouseDown()
    {
        //Pega a texutra da matriz de texturas e demonstra na variável
        texturaObjeto = gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture;

        //Inclui a textura no objeto
        if (Global.propriedadePecas.ContainsKey(Global.gameObjectName))
            Global.propriedadePecas[Global.gameObjectName].Textura = texturaObjeto;

        //Texturiza os cubos
        GameObject.Find(Global.propriedadePecas[Global.gameObjectName].NomeCuboAmbiente).GetComponent<MeshRenderer>().materials[0].mainTexture = texturaObjeto;
        GameObject.Find(Global.propriedadePecas[Global.gameObjectName].NomeCuboVis).GetComponent<MeshRenderer>().materials[0].mainTexture = texturaObjeto;

        //Muda seletor de textura para a textura selecionada
        GameObject.Find("TexturaSelecionada").GetComponent<MeshRenderer>().materials[0].mainTexture = texturaObjeto;
        
        gameObject.transform.parent.transform.parent.gameObject.SetActive(false); //Texturas
    }
}
