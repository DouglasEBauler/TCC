using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FabricaPecas : MonoBehaviour 
{
    [SerializeField]
    Camera mainCamera;

    static bool fabrica, render = false;
    bool podeAlterar = true;

    void LateUpdate () 
    {
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(gameObject.transform.position);

        if (Input.mousePosition.y > mainCamera.pixelRect.height 
            || Input.mousePosition.x > (mainCamera.pixelRect.width))
            podeAlterar = false;

        if (podeAlterar)
        {
            if (Input.mousePosition.y > (mainCamera.pixelRect.height / 2) 
                && Input.mousePosition.x < (mainCamera.pixelRect.width / 2))
            {
                if (!fabrica)
                {
                    Util_VisEdu.EnableColliderFabricaPecas(false, true, true);
                    fabrica = true;
                    render = false;
                }
            }
            else
            {
                if (!render)
                {
                    Util_VisEdu.EnableColliderFabricaPecas(true, true, true);
                    render = true;
                    fabrica = false;
                }
            }
        }                 
    }    
}
