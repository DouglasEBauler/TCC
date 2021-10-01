using UnityEngine;

public class RenderScript : MonoBehaviour 
{
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    GameObject propRenderer;

    void OnMouseDown()
    {
        menuControl.GetComponent<MenuScript>().EnablePanelProp("PropRenderer");
    }

    //void OnMouseOver()
    //{        
    //Vector3 pos = gameObject.transform.position;
    //pos.y -= Input.mouseScrollDelta.y * 0.5f;

    //// Limita o máximo a ser "scrollado" para cima e para baixo no painel Render. 
    //if (pos.y > 619.8 && pos.y < 673)
    //{
    //    gameObject.transform.position = pos;
    //    moveObjetos();
    //}
    //}

    //private void moveObjetos()
    //{
    //if (Global.listaObjetos != null)
    //{
    //    foreach (GameObject go in Global.listaObjetos)
    //    {
    //        Vector3 pos = go.transform.position;
    //        pos.y -= Input.mouseScrollDelta.y * 0.5f;
    //        go.transform.position = pos;
    //    }
    //}
    //}
}
