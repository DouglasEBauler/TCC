using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoPropPadrao : MonoBehaviour
{
    [SerializeField]
    GameObject botaoArquivo;
    [SerializeField]
    GameObject botaoFabrica;
    [SerializeField]
    GameObject botaoPropriedade;
    [SerializeField]
    GameObject botaoAjuda;
    [SerializeField]
    GameObject propriedadePecas;
    [SerializeField]
    GameObject go_fabricaPecas;

    private void SetButton(GameObject go, GameObject painel, bool clicouBotao = false)
    {
        Util_VisEdu.EnableColliderFabricaPecas(false, true);

        //botaoArquivo.transform.localScale = new Vector3(botaoArquivo.transform.localScale.x, 30, botaoArquivo.transform.localScale.z);
        //botaoFabrica.transform.localScale = new Vector3(botaoFabrica.transform.localScale.x, 30, botaoFabrica.transform.localScale.z);
        //botaoPropriedade.transform.localScale = new Vector3(botaoPropriedade.transform.localScale.x, 30, botaoPropriedade.transform.localScale.z);
        //botaoAjuda.transform.localScale = new Vector3(botaoAjuda.transform.localScale.x, 30, botaoAjuda.transform.localScale.z);

        //go.transform.localScale = new Vector3(go.transform.localScale.x, 45, go.transform.localScale.z);

        foreach (Transform child in go_fabricaPecas.transform)
        {
            if (!"FabricaPecas".Equals(child.name))
            {
                child.gameObject.SetActive(false);
            }

            if ((painel != null)
                && "BtnPropPecas".Equals(go.name)
                && child.name.Contains("Prop"))
            {
                foreach (Transform _child in child)
                {
                    _child.gameObject.SetActive(_child.name.Equals(painel.name));
                }
            }
        }

        if (!"BtnFabPecas".Equals(go.name))
        {
            if (clicouBotao && (painel == null))
            {
                if (Global.lastPressedButton == null) // Se for null é porque nenhum objeto foi selecionado ainda
                {
                    Global.lastPressedButton = this.propriedadePecas;
                }

                this.propriedadePecas.SetActive(true);

                foreach (Transform child in this.propriedadePecas.transform)
                {
                    child.gameObject.SetActive(child.name.Equals("LabelNomeCamera"));
                }
            }
            else
            {
                if (painel != null)
                {
                    switch (painel.name)
                    {
                        case "Arquivo":
                        case "Ajuda":
                            painel.SetActive(true); break;

                        case "Iluminacao":
                            {
                                foreach (Transform child in painel.transform.GetChild(2))
                                    child.gameObject.SetActive(false);

                                painel.transform.GetChild(2).GetChild(Global.propriedadePecas[Global.gameObjectName].TipoLuz).gameObject.SetActive(true);
                                break;
                            }
                        default: propriedadePecas.SetActive(true); break;
                    }
                }
                else
                {
                    propriedadePecas.SetActive(true);
                }
            }
        }
    }
}
