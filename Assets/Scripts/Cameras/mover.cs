using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover : MonoBehaviour 
{
    float moveX = 0.0f;
    float moveY = 0.0f;
    float sensitivity = 150f;
    static float sensLateral = 0.5f;
    Vector3 posInicial, rotacaoInicial;

    void Start()
    {
        posInicial = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        rotacaoInicial = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
    }

    void Update()
    {       
        moveAmbienteBotaoDireito();
        rotacionaAmbiente();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            getPosicaoInicial();
        }
    }

    void moveAmbienteBotaoDireito()
    {
        if (Input.GetAxis("Mouse X") != 0 && Input.GetButton("Fire2"))
        {
            moveX += Input.GetAxis("Mouse X") * sensLateral;
            moveY += Input.GetAxis("Mouse Y") * sensLateral;
            transform.Translate(Vector3.right * moveX);
            transform.Translate(Vector3.up * moveY);
        }
        moveX = 0.0f;
        moveY = 0.0f;
    }

    void rotacionaAmbiente()
    {
        if (Input.GetButton("Fire1"))
        {
            float rotx = Input.GetAxis("Mouse X") * -sensitivity * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * sensitivity * Mathf.Deg2Rad;
            gameObject.transform.Rotate(rotY, rotx, 0f, Space.Self);
        }
    }

    void getPosicaoInicial()
    {
        transform.position = posInicial;
        transform.rotation = Quaternion.Euler(rotacaoInicial);
    }
}
