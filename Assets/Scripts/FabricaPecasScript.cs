using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricaPecasScript : MonoBehaviour 
{
    string[] nomeObj = new string[9];
    List<string> listaPecas;

    void Start()
    {
        listaPecas = new List<string>();

        //switch (gameObject.name)
        //{
        //    case "CameraSlot":
        //        nomeObj[0] = "CameraP";
        //        break;
        //    case "ObjGraficoSlot":
        //        nomeObj[0] = "ObjetoGraficoP";
        //        break;
        //    case "FormasSlot":
        //        nomeObj[0] = "Cubo";
        //        nomeObj[1] = "Poligono";
        //        nomeObj[2] = "Spline";
        //        break;
        //    case "TraformacoesSlot":
        //        nomeObj[0] = "Transladar";
        //        nomeObj[1] = "Rotacionar";
        //        nomeObj[2] = "Escalar";
        //        break;
        //    case "IluminacaoSlot":
        //        nomeObj[0] = "Iluminacao";
        //        break;
        //}

        nomeObj[0] = "CameraP";
        nomeObj[1] = "ObjetoGraficoP";
        nomeObj[2] = "Cubo";
        nomeObj[3] = "Poligono";
        nomeObj[4] = "Spline";
        nomeObj[5] = "Transladar";
        nomeObj[6] = "Rotacionar";
        nomeObj[7] = "Escalar";
        nomeObj[8] = "Iluminacao";
    }

    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < nomeObj.Length; i++)
        {
            if (other.name == nomeObj[i])
            {
                listaPecas.Add(nomeObj[i]);
            }
        }
    }
}
