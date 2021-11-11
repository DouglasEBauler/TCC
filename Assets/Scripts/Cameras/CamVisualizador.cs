using UnityEngine;

public class CamVisualizador : MonoBehaviour
{
    [SerializeField]
    float inc;

    Vector3 posIni;
    Vector3 rotIni;
    float valorAnt = 0;

    void Start()
    {
        posIni = transform.position;
        rotIni = transform.eulerAngles;
    }

    void Update()
    {
        if (2.2f.Equals(inc))
        {
            inc = 0;
        }

        if ((inc >= 0) && (valorAnt != inc && inc < 2.2f))
        {
            posIni.x -= inc * 14f;
            posIni.y += (inc * 5);
            rotIni.x += inc;

            transform.position = posIni;
            transform.eulerAngles = rotIni;
        }
        else if ((valorAnt != inc) && (inc > -2.2f))
        {
            posIni.x += inc * 14f;
            posIni.y -= (inc * 5);
            rotIni.x -= inc;

            transform.position = posIni;
            transform.eulerAngles = rotIni;
        }

        valorAnt = inc;
    }
}
