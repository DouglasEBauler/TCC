using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour 
{
    [SerializeField]
    Transform target;
    
    float x, z;
    bool foward, left;   

    void Update () 
    {
        RectTransform rectTransf = transform.GetComponent<RectTransform>();
        transform.LookAt(target);

        if (!left && !foward)
            rectTransf.transform.position = 
                new Vector3(rectTransf.transform.position.x + 1, rectTransf.transform.position.y, rectTransf.transform.position.z);

        if (left) transform.LookAt(target, Vector3.left);

        if (foward) transform.LookAt(target, Vector3.forward);

        if (!rectTransf.transform.position.x.Equals(x))
        {
            transform.LookAt(target, Vector3.left);
            left = true;
            foward = !left;
        }
        else if (!rectTransf.transform.position.z.Equals(z))
        {
            transform.LookAt(target, Vector3.forward);
            foward = true;
            left = !foward;
        }

        x = rectTransf.transform.position.x;
        z = rectTransf.transform.position.z;
    }
}
