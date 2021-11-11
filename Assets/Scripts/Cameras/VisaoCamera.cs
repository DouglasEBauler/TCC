using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisaoCamera : MonoBehaviour {

    public Camera cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        cam.GetComponent<Camera>();
        Matrix4x4 m = cam.previousViewProjectionMatrix;        
	}
}
