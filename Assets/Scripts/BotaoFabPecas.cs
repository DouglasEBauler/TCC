using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoFabPecas : MonoBehaviour
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

    void OnMouseDown()
    {
        if (tutorialScript.EstaExecutandoTutorial)
        {
            tutorialScript.Nivel = "4.2";
        }

        ActivePanel();
        Util_VisEdu.EnableColliderFabricaPecas(true, true);
    }

    public void ActivePanel()
    {
        gameObject.GetComponent<MeshRenderer>().material = materialComFoco;

        foreach (Transform child in goFabricaPecas.transform)
        {
            if (!"FabricaPecas".Equals(child.name))
            {
                gameObject.GetComponent<MeshRenderer>().material = materialSemFoco;
                child.gameObject.SetActive(false);
            }
        }
    }
}
