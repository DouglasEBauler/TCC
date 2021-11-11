using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IluminacaoScript : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject posicaoColliderDestino;
    [SerializeField]
    GameObject abrePropriedade;
    [SerializeField]
    GameObject propCamera;
    [SerializeField]
    GameObject objGraficoSlot;
    [SerializeField]
    GameObject iluminacaoSlot;
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    GameObject panelArquivo;
    [SerializeField]
    GameObject panelPropPeca;
    [SerializeField]
    GameObject panelAjuda;

    [HideInInspector]
    public GameObject slot;

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject cloneFab;
    Tutorial tutorialScript;

    void Start()
    {
        scanPos = startPos = gameObject.transform.position;
        tutorialScript = GameObject.Find("PanelTutorial").GetComponent<Tutorial>();
    }

    void Update()
    {
        scanPos = gameObject.transform.position;
    }

    void OnMouseDown()
    {
        offset = scanPos - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));

        CopiaPeca();
        ConfiguraPropriedadePeca();
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = cam.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    void OnMouseUp()
    {
        if (PodeEncaixar())
        {
            AddIluminacao();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(Consts.ITERACAO_SLOT))
        {
            slot = other.gameObject;
        }
    }

    bool PodeGerarCopia()
    {
        return cam.WorldToScreenPoint(scanPos).y > cam.pixelRect.height / 2;
    }


    void AjustaPeca()
    {
        if (!tutorialScript.EstaExecutandoTutorial && !CheckPanelIsActive())
        {
            bool podeDestruir = PodeGerarCopia();

            //if (podeDestruir && !EstaEncaixado())
            //{
            //    StartCoroutine(RemovePeca());
            //}
            //else
            //{
            //    Encaixa();
            //}
        }
    }

    bool CheckPanelIsActive()
    {
        return panelArquivo.activeSelf || panelPropPeca.activeSelf || panelAjuda.activeSelf;
    }

    public IEnumerator RemovePeca()
    {
        while ((transform.position.y != startPos.y && transform.position.x != startPos.x))
        {
            transform.position =
                Vector3.Lerp(transform.position, startPos, Time.deltaTime * Consts.SPEED_DESLOC / Vector3.Distance(transform.position, startPos));

            yield return null;
        }

        Destroy(gameObject);
    }

    public void Encaixa()
    {
        StartCoroutine(EncaixaPecaAoSlot());
    }

    IEnumerator EncaixaPecaAoSlot()
    {
        if (!tutorialScript.EstaExecutandoTutorial)
        {
            while ((transform.position.y != slot.transform.position.y && transform.position.x != slot.transform.position.x))
            {
                transform.position =
                    Vector3.Lerp(transform.position, slot.transform.position, Time.deltaTime * Consts.SPEED_DESLOC / Vector3.Distance(transform.position, slot.transform.position));

                yield return null;
            }

            transform.parent = slot.transform;
            gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
        }
    }

    public void AddIluminacao(bool tutorial = false)
    {
        posicaoColliderDestino = GameObject.Find("IluminacaoSlot" + ((!tutorial) ? Global.countObjetosGraficos.ToString() : "Tutorial"));

        //Verifica se há câmera e aplica luz aos objetos com Layer "Formas"
        if (Global.cameraAtiva)
            GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            PropriedadePeca prPeca = new PropriedadePeca();
            prPeca.Nome = gameObject.name;
            //prPeca.NomeCuboAmbiente = "CuboAmbiente" + getNumObjeto(Global.listaEncaixes[gameObject.name]);
            //prPeca.NomeCuboVis = "CuboVis" + getNumObjeto(Global.listaEncaixes[gameObject.name]);
            prPeca.TipoLuz = 0;
            Global.propriedadePecas.Add(gameObject.name, prPeca);

            if (gameObject.name.Length > "Iluminacao".Length)
                CriaLightObject(gameObject.name.Substring("Iluminacao".Length, 1));
        }
    }

    public void ConfiguraPropriedadePeca(PropriedadePeca propPeca = null)
    {
        //if (EstaEncaixado())
        //{
        //    Global.gameObjectName = gameObject.name;
        //    Global.lastPressedButton?.SetActive(false);
        //    Global.lastPressedButton = propriedades.gameObject;

        //    CreatePropPeca(propPeca);

        //    propriedades.GetComponent<PropIluminacaoScript>().Inicializa();
        //    menuControl.GetComponent<MenuScript>().EnablePanelProp(Global.lastPressedButton.name);
        //}
    }

    public void CreatePropPeca(PropriedadePeca propPeca = null)
    {
        if (!Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            PropriedadePeca prPeca;

            //if (Global.gameObjectName.Contains(Consts.CUBO))
            //{
            //    prPeca = new CuboPropriedadePeca()
            //    {
            //        NomeCuboAmbiente = "CuboAmbiente" + getNumObjeto(Global.listaEncaixes[gameObject.name]),
            //        NomeCuboVis = "CuboVis" + getNumObjeto(Global.listaEncaixes[gameObject.name])
            //    };
            //}
            //else if (Global.gameObjectName.Contains(Consts.POLIGONO))
            //{
            //    prPeca = new PoligonoPropriedadePeca()
            //    {
            //        PoligonoAmbiente = "PoligonoAmbiente" + getNumObjeto(Global.listaEncaixes[gameObject.name]),
            //        //, NomeCuboVis = "CuboVis" + getNumObjeto(Global.listaEncaixes[gameObject.name])
            //    };
            //}
            //else if (propPeca == null)
            //{
            //    prPeca = new PropriedadePeca();
            //}
            //else
            //{
            //    prPeca = propPeca;
            //}
            //prPeca.Nome = gameObject.name;

            //Global.propriedadePecas.Add(gameObject.name, prPeca);
        }
    }

    public bool PodeEncaixar()
    {
        const float VALOR_APROXIMADO = 2;

        float pecaY = transform.position.y;

        //foreach (var slot in Global.listaPosicaoSlot)
        //{
        //    //Verifica se o encaixe existe na lista 
        //    if (slot.Key.Contains(Global.GetSlot(gameObject.name)))
        //    {
        //        //Verifica se a peça está próxima do encaixe e se o Slot ainda não está na lista de encaixes.
        //        if ((slot.Value + VALOR_APROXIMADO > pecaY) && (slot.Value - VALOR_APROXIMADO < pecaY) && !Global.listaEncaixes.ContainsValue(slot.Key))
        //        {
        //            if (!Global.listaEncaixes.ContainsKey(gameObject.transform.name) && (GameObject.Find(slot.Key) != null))
        //            {
        //                Global.listaEncaixes.Add(gameObject.transform.name, slot.Key);
        //            }
        //            else if (Global.listaEncaixes[gameObject.name] != slot.Key)
        //            {
        //                ReposicionaSlots(Global.listaEncaixes[gameObject.name], slot.Key);
        //                return false;
        //            }
        //            else
        //                return false;

        //            return GameObject.Find(slot.Key) != null;
        //        }
        //    }
        //}

        return false;
    }

    public void CopiaPeca()
    {
        cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
        cloneFab.name = Consts.PECA_ILUMINACAO;
        cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    void CriaLightObject(string numLight)
    {
        const string LightObjectsIluminacao = "LightObjectsIluminacao";

        if (!Global.lightObjectList.Contains(LightObjectsIluminacao + numLight))
        {
            GameObject cloneGO;
            GameObject GOCloneLightObjects = GameObject.Find(LightObjectsIluminacao);

            Global.lightObjectList.Add(LightObjectsIluminacao + numLight);

            if (LightObjectsIluminacao + numLight != LightObjectsIluminacao)
            {
                cloneGO = Instantiate(GOCloneLightObjects, GOCloneLightObjects.transform.position, GOCloneLightObjects.transform.rotation, GOCloneLightObjects.transform.parent);
                cloneGO.name = LightObjectsIluminacao + numLight;
                cloneGO.transform.position = new Vector3(GOCloneLightObjects.transform.position.x, GOCloneLightObjects.transform.position.y, GOCloneLightObjects.transform.position.z);

                RenomeiaLightObject(cloneGO, numLight);
            }
        }
    }

    void RenomeiaLightObject(GameObject go, string numLight)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            switch (i)
            {
                case 0:
                    go.transform.GetChild(i).name += numLight;
                    break;
                case 1:
                case 2:
                    go.transform.GetChild(i).name += numLight;
                    go.transform.GetChild(i).GetChild(0).name += numLight;
                    break;
                case 3:
                    go.transform.GetChild(i).name += numLight;
                    go.transform.GetChild(i).GetChild(0).name += numLight;
                    go.transform.GetChild(i).GetChild(0).GetChild(0).name += numLight;
                    break;
            }
        }
    }
}
