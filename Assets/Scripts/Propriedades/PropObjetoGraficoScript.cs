using UnityEngine;
using UnityEngine.UI;

public class PropObjetoGraficoScript : MonoBehaviour
{
    [SerializeField]
    InputField nomePeca;
    [SerializeField]
    Toggle ativo;

    PropriedadePeca propPeca;
    bool podeAtualizar;

    public void Inicializa()
    {
        propPeca = Global.propriedadePecas[Global.gameObjectName];
        propPeca.Ativo = true;

        AtualizaListaProp();
        PreencheCampos();
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(Global.gameObjectName))
        {
            Global.propriedadePecas[Global.gameObjectName] = propPeca;
        }    
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(Global.gameObjectName))
        {
            podeAtualizar = false;
            try
            {
                nomePeca.text = propPeca.Nome;
                ativo.isOn = propPeca.Ativo;
            }
            finally
            {
                podeAtualizar = true;
                UpdateProp();
            }
        }
    }

    public void UpdateProp()
    {
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(Global.gameObjectName))
        {
            MeshRenderer meshRender = null;

            if (Global.propriedadePecas[Global.gameObjectName] is CuboPropriedadePeca)
            {
                meshRender = GameObject.Find((Global.propriedadePecas[Global.gameObjectName] as CuboPropriedadePeca).NomeCuboAmbiente).GetComponent<MeshRenderer>();
            }
            else if (Global.propriedadePecas[Global.gameObjectName] is PoligonoPropriedadePeca)
            {
                meshRender = GameObject.Find((Global.propriedadePecas[Global.gameObjectName] as PoligonoPropriedadePeca).PoligonoAmbiente).GetComponent<MeshRenderer>();
            }

            Global.propriedadePecas[Global.gameObjectName].Ativo = ativo.isOn;

            if (meshRender != null)
                meshRender.enabled = ativo.isOn;
        }
    }
}
