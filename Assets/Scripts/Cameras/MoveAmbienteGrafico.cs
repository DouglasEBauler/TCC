using UnityEngine;

public class MoveAmbienteGrafico : MonoBehaviour
{
    public static bool entrouAmbienteGrafico;

    float moveX = 0.0f;
    float moveY = 0.0f;
    Vector3 posInicial, rotacaoInicial;

    void Start()
    {
        posInicial = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        rotacaoInicial = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
    }

    void Update()
    {
        if (entrouAmbienteGrafico)
        {
            MoveAmbienteBotaoDireito();
            RotacionaAmbiente();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetPosicaoInicial();
        }
    }

    void OnMouseEnter()
    {
        entrouAmbienteGrafico = true;
    }

    private void OnMouseExit()
    {
        entrouAmbienteGrafico = false;
    }

    void MoveAmbienteBotaoDireito()
    {
        if (Input.GetAxis("Mouse X") != 0 && Input.GetButton("Fire2"))
        {
            const float sensLateral = 1.5f;

            moveX += Input.GetAxis("Mouse X") * sensLateral;
            moveY += Input.GetAxis("Mouse Y") * sensLateral;
            gameObject.transform.Translate(Vector3.right * moveX);
            gameObject.transform.Translate(Vector3.up * moveY);
        }
        moveX = 0.0f;
        moveY = 0.0f;
    }

    void RotacionaAmbiente()
    {
        if (Input.GetButton("Fire1"))
        {
            const float sensitivity = 200f;

            float rotx = Input.GetAxis("Mouse X") * -sensitivity * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * sensitivity * Mathf.Deg2Rad;
            gameObject.transform.Rotate(rotY, rotx, 0f, Space.Self);
        }
    }

    void GetPosicaoInicial()
    {
        gameObject.transform.position = posInicial;
        gameObject.transform.rotation = Quaternion.Euler(rotacaoInicial);
    }
}

