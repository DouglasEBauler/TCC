using UnityEngine;

public class SelecionaCor : MonoBehaviour 
{
    public Color corObjeto;
    string corSelecionada = "CorSelecionada";

    void OnMouseDown()
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
                foreach (Transform child in light.transform)
                    child.GetComponent<Light>().color = corObjeto;
            }
            else
            {
                // Muda cor da luz.
                light.GetComponent<Light>().color = corObjeto;
                // Muda cor do objeto da luz.
                ChageObjectLightColor("Obj" + new PropIluminacaoPadrao().GetTipoLuzPorExtenso(Global.propriedadePecas[Global.gameObjectName].TipoLuz) + Global.gameObjectName, corObjeto);
            }
            
        }
        else if (Global.gameObjectName.Contains("Cubo"))
        {
            GameObject.Find((Global.propriedadePecas[Global.gameObjectName] as CuboPropriedadePeca).NomeCuboAmbiente).GetComponent<MeshRenderer>().materials[0].color = corObjeto;
            GameObject.Find((Global.propriedadePecas[Global.gameObjectName] as CuboPropriedadePeca).NomeCuboVis).GetComponent<MeshRenderer>().materials[0].color = corObjeto;
        }
        else if (Global.gameObjectName.Contains("Poligono"))
        {
            GameObject.Find((Global.propriedadePecas[Global.gameObjectName] as PoligonoPropriedadePeca).PoligonoAmbiente).GetComponent<MeshRenderer>().materials[0].color = corObjeto;
        }
        else // PropRenderer
        {
            PropRenderer propRenderer = GameObject.Find("PropRenderer")?.GetComponent<PropRenderer>();

            if (propRenderer != null)
            {
                propRenderer.Visualizador.GetComponent<MeshRenderer>().materials[0].color = corObjeto;
                propRenderer.VisualizadorGraf.GetComponent<MeshRenderer>().materials[0].color = corObjeto;
            }
        }
        
        corSelecionada = CorSelecionadaIluminacao();        

        // Muda seletor de cor para a cor selecionada.
        GameObject.Find(corSelecionada).GetComponent<MeshRenderer>().materials[0].color = corObjeto;

        gameObject.transform.parent.gameObject.SetActive(false);
    }

    string CorSelecionadaIluminacao()
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

    void ChageObjectLightColor(string nome, Color cor)
    {
        GameObject go = GameObject.Find(nome);

        for (int i = 0; i < go.transform.childCount; i++)
        {
            // Se for Spot tem mais hierarquia...
            if (nome.Contains("Spot"))
            {
                go.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().materials[0].color = cor;

                foreach (Transform child in go.transform.GetChild(0).GetChild(i))
                    child.GetComponent<MeshRenderer>().materials[0].color = cor;
            }
            else
                go.transform.GetChild(i).GetComponent<MeshRenderer>().materials[0].color = cor;
        }
    }
}
