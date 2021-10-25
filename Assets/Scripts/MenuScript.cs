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

    Tutorial tutorialScript;

    void Start()
    {
        tutorialScript = GameObject.Find("PanelTutorial").GetComponent<Tutorial>();
    }

    public void EnablePanelAjuda()
    {
        Util_VisEdu.EnableColliderFabricaPecas(false, false);
        Util_VisEdu.EnableGOAmbienteGrafico(false);
        Util_VisEdu.EnableGOVisualizador(false);

        foreach (Transform child in goFabricaPecas.transform)
        {
            child.gameObject.SetActive(!"FabricaPecas".Equals(child.name) && "Ajuda".Equals(child.name));
        }

        EnableButtons("BtnAjuda");
    }

    public void EnablePanelArquivo()
    {
        Util_VisEdu.EnableColliderFabricaPecas(false);

        foreach (Transform child in goFabricaPecas.transform)
        {
            if ("FabricaPecas".Equals(child.name)) continue;

            child.gameObject.SetActive("Arquivo".Equals(child.name));
        }

        EnableButtons("BtnArquivo");
    }

    public void EnablePanelProp(string activePanelProp)
    {
        Util_VisEdu.EnableColliderFabricaPecas(false);

        EnableButtons("BtnPropPecas");

        foreach (Transform child in goFabricaPecas.transform)
        {
            if ("FabricaPecas".Equals(child.name)) continue;

            if (child.name.Equals("PropriedadePecas"))
            {
                child.gameObject.SetActive(true);

                foreach (Transform _child in child)
                {
                    if (!_child.name.Equals("Iluminacao"))
                    {
                        _child.gameObject.SetActive(
                            (!string.Empty.Equals(activePanelProp) && _child.name.Equals(activePanelProp))
                            || (string.Empty.Equals(activePanelProp) && _child.name.Equals("LabelNomeCamera"))
                        );
                    }
                    else
                    {
                        foreach (Transform __child in _child.transform.GetChild(2))
                            __child.gameObject.SetActive(false);

                        _child.transform.GetChild(2).GetChild(Global.propriedadePecas[Global.gameObjectName].TipoLuz).gameObject.SetActive(true);
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
            child.gameObject.SetActive("FabricaPecas".Equals(child.name));
        }

        EnableButtons("BtnFabPecas");

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
