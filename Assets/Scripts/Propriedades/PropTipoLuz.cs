using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropTipoLuz : MonoBehaviour {

    public enum TipoIluminacao { Ambiente, Directional, Point, Spot };

    private const int AMBIENTE = 0;
    private const int DIRECTIONAL = 1;
    private const int POINT = 2;
    private const int SPOT = 3;

    public TMP_Dropdown mainDropdown;
    public GameObject Ambiente;
    public GameObject Directional;
    public GameObject Point;
    public GameObject Spot;
    private PropIluminacaoPadrao propIluminacao = new PropIluminacaoPadrao();

    void Start () {
        if (mainDropdown != null)
            mainDropdown.onValueChanged.AddListener(delegate { AdicionaValorPropriedade(); });
    }

    public void AdicionaValorPropriedade()
    {
        switch (gameObject.GetComponent<TMP_Dropdown>().value)
        {
            case 0: //Ambiente
                Ambiente.SetActive(true);
                Directional.SetActive(false);
                Point.SetActive(false);
                Spot.SetActive(false);
                Global.propriedadePecas[Global.gameObjectName].TipoLuz = AMBIENTE;
                propIluminacao.preencheCamposIluminacao(AMBIENTE);
                Global.propriedadePecas[Global.gameObjectName].UltimoIndexLuz = AMBIENTE;
                AtivaIluminacao(AMBIENTE);
                break;
            case 1: //Directional
                Ambiente.SetActive(false);
                Directional.SetActive(true);
                Point.SetActive(false);
                Spot.SetActive(false);
                Global.propriedadePecas[Global.gameObjectName].TipoLuz = DIRECTIONAL;
                propIluminacao.preencheCamposIluminacao(DIRECTIONAL);
                Global.propriedadePecas[Global.gameObjectName].UltimoIndexLuz = DIRECTIONAL;
                AtivaIluminacao(DIRECTIONAL);
                break;
            case 2: //Point
                Ambiente.SetActive(false);
                Directional.SetActive(false);
                Point.SetActive(true);
                Spot.SetActive(false);
                Global.propriedadePecas[Global.gameObjectName].TipoLuz = POINT;
                propIluminacao.preencheCamposIluminacao(POINT);
                Global.propriedadePecas[Global.gameObjectName].UltimoIndexLuz = POINT;
                AtivaIluminacao(POINT);
                break;
            case 3: //Spot
                Ambiente.SetActive(false);
                Directional.SetActive(false);
                Point.SetActive(false);
                Spot.SetActive(true);
                Global.propriedadePecas[Global.gameObjectName].TipoLuz = SPOT;
                propIluminacao.preencheCamposIluminacao(SPOT);
                Global.propriedadePecas[Global.gameObjectName].UltimoIndexLuz = SPOT;
                AtivaIluminacao(SPOT);
                break;
        }
    }

    private void AtivaIluminacao(int idxIluminacao)
    {
        Light lDirectional = GameObject.Find("Directional" + Global.gameObjectName).GetComponent<Light>();
        Light lPoint = GameObject.Find("Point" + Global.gameObjectName).GetComponent<Light>();
        Light lSpot = GameObject.Find("Spot" + Global.gameObjectName).GetComponent<Light>();        

        switch (idxIluminacao)
        {
            case 0: //Ambiente
                LightAmbienteEnable(true);
                lDirectional.enabled = false;
                lPoint.enabled = false;
                lSpot.enabled = false;                

                EnableMeshRendered(false, false, false);
                break;
            case 1: //Directional
                LightAmbienteEnable(false);
                lDirectional.enabled = true;
                lPoint.enabled = false;
                lSpot.enabled = false;

                if (Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Ativo)
                    EnableMeshRendered(true, false, false);
                else
                    EnableMeshRendered(false, false, false);
                break;
            case 2: //Point
                LightAmbienteEnable(false);
                lDirectional.enabled = false;
                lPoint.enabled = true;
                lSpot.enabled = false;

                if (Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Ativo)
                    EnableMeshRendered(false, true, false);
                else
                    EnableMeshRendered(false, false, false);
                break;
            case 3: //Spot
                LightAmbienteEnable(false);
                lDirectional.enabled = false;
                lPoint.enabled = false;
                lSpot.enabled = true;

                if (Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Ativo)
                    EnableMeshRendered(false, false, true);
                else
                    EnableMeshRendered(false, false, false);
                break;
        }
    }

    private void EnableMeshRendered(bool directional, bool point, bool spot)
    {
        GameObject goAmbiente = GameObject.Find("Ambiente" + Global.gameObjectName);
        GameObject goDirectional = GameObject.Find("ObjDirectional" + Global.gameObjectName);
        GameObject goPoint = GameObject.Find("ObjPoint" + Global.gameObjectName);
        GameObject goSpot = GameObject.Find("ObjSpot" + Global.gameObjectName);

        PropIluminacaoPadrao propIluminacao = new PropIluminacaoPadrao();
        
        for (int i = 0; i < goDirectional.transform.childCount; i++)
            goDirectional.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = directional;

        for (int i = 0; i < goPoint.transform.childCount; i++)
            goPoint.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = point;

        for (int i = 0; i < goSpot.transform.GetChild(0).GetChild(0).childCount; i++)
        {
            goSpot.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = spot;
            goSpot.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = spot;
        }

        if (Global.propriedadePecas[Global.gameObjectName].TipoLuz == AMBIENTE)
        {
            for (int i = 0; i < goAmbiente.transform.childCount; i++)
                goAmbiente.transform.GetChild(i).GetComponent<Light>().enabled = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Ativo;
            
            //propIluminacao.AtivaCamera(Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Ambiente].Ativo);
        }
        else if (Global.propriedadePecas[Global.gameObjectName].TipoLuz == DIRECTIONAL)
        {
            goDirectional.transform.parent.GetComponent<Light>().enabled = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Directional].Ativo;
            //propIluminacao.AtivaCamera(directional);
        }            
        else if (Global.propriedadePecas[Global.gameObjectName].TipoLuz == POINT)
        {
            goPoint.transform.parent.GetComponent<Light>().enabled = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Point].Ativo;
            //propIluminacao.AtivaCamera(point);
        }            
        else if (Global.propriedadePecas[Global.gameObjectName].TipoLuz == SPOT)
        {
            goSpot.transform.parent.GetComponent<Light>().enabled = Global.propriedadeIluminacao[Global.gameObjectName][(int)TipoIluminacao.Spot].Ativo;
            //propIluminacao.AtivaCamera(spot);
        }

        //propIluminacao.AtualizaCamera();

    }

    private void LightAmbienteEnable(bool status)
    {
        GameObject goAmbiente = GameObject.Find("Ambiente" + Global.gameObjectName);

        for (int i = 0; i < goAmbiente.transform.childCount; i++)
            goAmbiente.transform.GetChild(i).GetComponent<Light>().enabled = status;
    }


}
