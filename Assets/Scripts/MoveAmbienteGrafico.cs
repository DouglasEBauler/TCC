using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAmbienteGrafico : MonoBehaviour
{

    float moveX = 0.0f;
    float moveY = 0.0f;
    float sensitivity = 200f;
    static float sensLateral = 1.5f;
    Vector3 posInicial, rotacaoInicial;
    public GameObject mainObject;
    bool entrouAmbienteGrafico;
    public Camera cameraAmbiente;
    public float posX, posY, posZ;

    private void Start()
    {
        posInicial = new Vector3(mainObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        rotacaoInicial = new Vector3(mainObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
        posX = cameraAmbiente.transform.position.x;
        posY = cameraAmbiente.transform.position.y;
        posZ = cameraAmbiente.transform.position.z;
    }

    void OnMouseEnter()
    {
        entrouAmbienteGrafico = true;
    }

    private void OnMouseExit()
    {
        entrouAmbienteGrafico = false;
    }


    void Update()
    {
       if (entrouAmbienteGrafico)
        {
            moveAmbienteBotaoDireito();
            rotacionaAmbiente();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                getPosicaoInicial();
            }
        }

        cameraAmbiente.transform.position = new Vector3(posX, posY, posZ);
    }

    private void moveAmbienteBotaoDireito()
    {
        if (Input.GetAxis("Mouse X") != 0 && Input.GetButton("Fire2"))
        {
            moveX += Input.GetAxis("Mouse X") * sensLateral;
            moveY += Input.GetAxis("Mouse Y") * sensLateral;
            mainObject.transform.Translate(Vector3.right * moveX);
            mainObject.transform.Translate(Vector3.up * moveY);
        }
        moveX = 0.0f;
        moveY = 0.0f;
    }

    private void rotacionaAmbiente()
    {
        if (Input.GetButton("Fire1"))
        {
            float rotx = Input.GetAxis("Mouse X") * -sensitivity * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * sensitivity * Mathf.Deg2Rad;
            mainObject.transform.Rotate(rotY, rotx, 0f, Space.Self);
        }
    }

    private void getPosicaoInicial()
    {
        mainObject.transform.position = posInicial;
        mainObject.transform.rotation = Quaternion.Euler(rotacaoInicial);
    }
}

