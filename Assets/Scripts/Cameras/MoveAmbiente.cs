using UnityEngine;

public class MoveAmbiente : MonoBehaviour 
{
    [SerializeField]
    GameObject target;
    [SerializeField]
    Camera cameraGraf;

    float speed = 5f;
    float minFov = 15f;
    float maxFov = 100f;
    float sensitivity = 17f;

    private Vector3 positionInicial;
    private Vector3 rotaionInicial;

    void Start () {
        positionInicial = new Vector3(cameraGraf.transform.position.x, cameraGraf.transform.position.y, cameraGraf.transform.position.z);
        rotaionInicial = new Vector3(cameraGraf.transform.rotation.x, cameraGraf.transform.rotation.y, cameraGraf.transform.rotation.z);
    }
	
	void Update () {        

        if (MoveAmbienteGrafico.entrouAmbienteGrafico)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                transform.RotateAround(targetPos, transform.up, Input.GetAxis("Mouse X") * speed);
                transform.RotateAround(targetPos, transform.right, -Input.GetAxis("Mouse Y") * speed);
            }

            float fov = cameraGraf.fieldOfView;
            fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            cameraGraf.fieldOfView = fov;
        }        

        if (Input.GetKey(KeyCode.Space))
        {
            cameraGraf.transform.position = positionInicial;
            cameraGraf.transform.localRotation = new Quaternion(rotaionInicial.x, rotaionInicial.y, rotaionInicial.z, 0f);
        }            
    }    
}
