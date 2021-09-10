using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoPropPecas : MonoBehaviour
{
    [SerializeField]
    GameObject goFabricaPecas;
    [SerializeField]
    Material materialSemFoco;
    [SerializeField]
    Material materialComFoco;

    void OnMouseDown()
    {
        ActivePanelProp(string.Empty);
    }

    public void ActivePanelProp(string activePanelProp)
    {
        Util_VisEdu.EnableColliderFabricaPecas(false, true);

        foreach (Transform child in goFabricaPecas.transform)
        {
            if ("FabricaPecas".Equals(child.name)) continue;

            if (child.name.Equals("PropriedadePecas"))
            {
                gameObject.GetComponent<MeshRenderer>().materials[0] = materialComFoco;
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
                gameObject.GetComponent<MeshRenderer>().material = materialSemFoco;
                child.gameObject.SetActive(false);
            }
        }
    }
}
