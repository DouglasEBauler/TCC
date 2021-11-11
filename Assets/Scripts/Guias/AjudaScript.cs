using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AjudaScript : MonoBehaviour
{
    [SerializeField]
    GameObject panels;
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    Material materialSemFoco;
    [SerializeField]
    Material materialComFoco;

    public void EnablePanel(GameObject panel)
    {
        foreach (Transform child in panels.transform)
        {
            child.gameObject.SetActive(panel.name.Equals(child.name));
        }
    }

    public void EnableButton(GameObject btn)
    {
        foreach (Transform child in menuControl.transform)
        {
            child.GetComponent<Image>().material = btn.name.Equals(child.name) ? materialComFoco : materialSemFoco;
        }
    }
}
