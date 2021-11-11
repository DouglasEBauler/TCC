using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObjectScript : MonoBehaviour 
{
    [SerializeField]
    Transform target;
		
	void Update () 
    {
        transform.LookAt(target);
    }
}
