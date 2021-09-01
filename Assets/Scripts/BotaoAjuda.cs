using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoAjuda : MonoBehaviour
{
    [SerializeField]
    GameObject go_fabricaPecas;
    [SerializeField]
    Material materialSemFoco;
    [SerializeField]
    Material materialComFoco;

    void OnMouseDown()
    {
        ActivePanel();
    }

    void ActivePanel()
    {
        Util_VisEdu.EnableColliderFabricaPecas(false, true);

        foreach (Transform child in go_fabricaPecas.transform)
        {
            if (!"FabricaPecas".Equals(child.name))
            {
                if ("Ajuda".Equals(child.name))
                {
                    gameObject.GetComponent<MeshRenderer>().material = materialComFoco;
                    child.gameObject.SetActive(true);
                }
                else
                {
                    gameObject.GetComponent<MeshRenderer>().material = materialSemFoco;
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
