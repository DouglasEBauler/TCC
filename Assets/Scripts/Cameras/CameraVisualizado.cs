using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVisualizado : MonoBehaviour {

    //public Camera camVisualizador;
    //public float posX, posY, posZ, rotX, rotY, rotZ;
    //public GameObject teste;
    public Transform lookAt;
    public Transform camTransform;
    public float posX, posY;
    public float distance = 0;
    public GameObject obj;

    private Camera cam;    
    private float currentX;
    private float currentY;
    private float sensivityX;
    private float sensivityY;


    void Start () {
        //camTransform = transform;
        //cam = Camera.main;
        posX = 168f; // transform.position.x;
        posY = -30f; // transform.position.y;
        distance = -30F;
        //posX = camVisualizador.transform.position.x;
        //posY = camVisualizador.transform.position.y;
        //posZ = camVisualizador.transform.position.z;
        //rotX = camVisualizador.transform.rotation.x;
        //rotY = camVisualizador.transform.rotation.y;
        //rotZ = camVisualizador.transform.rotation.z;
        obj.SetActive(false);
    }
	

	void Update () {
        //camVisualizador.transform.position = new Vector3(posX, posY, posZ);
        //camVisualizador.transform.rotation = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));
        //camVisualizador.transform.Rotate(new Vector3(rotX, rotY, rotZ), Space.World);
        //camVisualizador.transform.Translate(new Vector3(rotX, rotY, rotZ));

        //currentX = currentX -Input.GetAxis("Mouse X");
        //currentY = currentY -Input.GetAxis("Mouse Y");

        currentX = posX;
        currentY = posY;
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
