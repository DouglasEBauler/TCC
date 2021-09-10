using UnityEngine;

public class BotaoArquivo : MonoBehaviour
{
    [SerializeField]
    GameObject goFabricaPecas;
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

        foreach (Transform child in goFabricaPecas.transform)
        {
            if (!"FabricaPecas".Equals(child.name))
            {
                if ("Arquivo".Equals(child.name))
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
