using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject goFabricaPecas;
    [SerializeField]
    Material materialSemFoco;
    [SerializeField]
    Material materialComFoco;
    [SerializeField]
    Tutorial tutorialScript;

    public void EnablePanelAjuda()
    {
        Util_VisEdu.EnableColliderFabricaPecas(false, false);
        Util_VisEdu.EnableGOAmbienteGrafico(false);
        Util_VisEdu.EnableGOVisualizador(false);

        foreach (Transform child in goFabricaPecas.transform)
        {
            child.gameObject.SetActive(!Consts.FABRICAPECAS.Equals(child.name) && Consts.AJUDA.Equals(child.name));
        }

        EnableButtons(Consts.BTN_AJUDA);
    }

    public void EnablePanelArquivo()
    {
        Util_VisEdu.EnableColliderFabricaPecas(false);

        foreach (Transform child in goFabricaPecas.transform)
        {
            if (Consts.FABRICAPECAS.Equals(child.name)) continue;

            child.gameObject.SetActive(Consts.ARQUIVO.Equals(child.name));
        }

        EnableButtons(Consts.BTN_ARQUIVO);
    }

    public void EnablePanelProp(string activePanelProp)
    {
        Util_VisEdu.EnableColliderFabricaPecas(false);

        EnableButtons(Consts.BTN_PROP_PECAS);

        foreach (Transform child in goFabricaPecas.transform)
        {
            if (child.name.Equals(Consts.PROPRIEDADEPECAS))
            {
                child.gameObject.SetActive(true);

                foreach (Transform _child in child)
                {
                    if (!_child.name.Equals(Consts.ILUMINACAO))
                    {
                        _child.gameObject.SetActive(
                            (!string.Empty.Equals(activePanelProp) && _child.name.Equals(activePanelProp))
                            || (string.Empty.Equals(activePanelProp) && _child.name.Equals("LabelNomeCamera"))
                        );
                    }
                }
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void EnablePanelFabPecas()
    {
        if (tutorialScript.EstaExecutandoTutorial)
        {
            tutorialScript.Nivel = "4.2";
        }

        foreach (Transform child in goFabricaPecas.transform)
        {
            child.gameObject.SetActive(Consts.FABRICAPECAS.Equals(child.name));
        }

        EnableButtons(Consts.BTN_FAB_PECAS);

        Util_VisEdu.EnableColliderFabricaPecas(true);
        Util_VisEdu.EnableGOAmbienteGrafico(true);
        Util_VisEdu.EnableGOVisualizador(true);
    }

    void EnableButtons(string btnName)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.GetComponent<Image>().material = btnName.Equals(child.name) ? materialComFoco : materialSemFoco;
        }
    }
}

