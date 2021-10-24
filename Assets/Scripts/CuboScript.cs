using UnityEngine;
using UnityEngine.UI;

public class CuboScript : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject propriedades;
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    GameObject panelArquivo;
    [SerializeField]
    GameObject panelPropPeca;
    [SerializeField]
    GameObject panelAjuda;
    [SerializeField]
    GameObject tutorial;

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject slot;
    Tutorial tutorialScript;

    void Start()
    {
        scanPos = startPos = gameObject.transform.position;
        tutorialScript = tutorial.GetComponent<Tutorial>();
    }

    void Update()
    {
        scanPos = gameObject.transform.position;
    }

    void OnMouseDown()
    {
        Global.AtualizaListaSlot();

        screenPoint = cam.WorldToScreenPoint(scanPos);

        offset = scanPos - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

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
            AddCubo();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("FormasSlot"))
        {
            slot = other.gameObject;
        }   
    }

    bool PodeGerarCopia()
    {
        return screenPoint.y > cam.pixelRect.height / 2;
    }

    void AjustaPeca()
    {
        if (!tutorialScript.EstaExecutandoTutorial && !CheckPanelIsActive())
        {
            bool podeDestruir = PodeGerarCopia();

            if (podeDestruir)
            {
                transform.position = startPos;
                Destroy(gameObject);
                screenPoint = cam.WorldToScreenPoint(scanPos);
            }
        }
    }

    bool CheckPanelIsActive()
    {
        return panelArquivo.activeSelf || panelPropPeca.activeSelf || panelAjuda.activeSelf;
    }

    public void EncaixaPecaAoSlot()
    {
        if (!tutorialScript.EstaExecutandoTutorial)
        {
            name += DropPeca.countObjetosGraficos.ToString();
            transform.parent = slot.transform;
            transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, slot.transform.position.z);
            gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
            Global.AddObject(gameObject);
        }
    }

    public void AddCubo()
    {
        EncaixaPecaAoSlot();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
                GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

            ConfigCuboAmb();
            ConfigCuboVis();
        }
        else
        {
            GameObject.Find("CuboAmbiente" + Util_VisEdu.GetSlot(gameObject.name)).GetComponent<MeshRenderer>().enabled = true;
        }

        CreatePropPeca(null);
    }

    void ConfigCuboVis()
    {
        GameObject cuboVis = GameObject.Find("CuboVis");
        if (cuboVis != null)
        {
            string numSlot = Util_VisEdu.GetSlot(gameObject.name);

            GameObject cloneFab = Instantiate(cuboVis, cuboVis.transform.position, cuboVis.transform.rotation, cuboVis.transform.parent);
            cloneFab.name = "CuboVis";
            cloneFab.transform.position = new Vector3(cuboVis.transform.position.x, cuboVis.transform.position.y, cuboVis.transform.position.z);
            cloneFab.GetComponent<MeshRenderer>().enabled = false;

            cuboVis.name += numSlot;
            cuboVis.GetComponent<MeshRenderer>().enabled = Global.propriedadePecas["ObjetoGraficoP" + numSlot].Ativo;
        }
    }

    void ConfigCuboAmb()
    {
        GameObject cuboAmb = GameObject.Find("CuboAmbiente");
        if (cuboAmb != null)
        {
            string numSlot = Util_VisEdu.GetSlot(gameObject.name);

            GameObject cloneFab = Instantiate(cuboAmb, cuboAmb.transform.position, cuboAmb.transform.rotation, cuboAmb.transform.parent);
            cloneFab.name = "CuboAmbiente";
            cloneFab.transform.position = new Vector3(cuboAmb.transform.position.x, cuboAmb.transform.position.y, cuboAmb.transform.position.z);
            cloneFab.GetComponent<MeshRenderer>().enabled = false;

            cuboAmb.name += numSlot;
            cuboAmb.GetComponent<MeshRenderer>().enabled = Global.propriedadePecas["ObjetoGraficoP" + numSlot].Ativo;
        }
    }

    public void ConfiguraPropriedadePeca(PropriedadePeca propPeca = null, PropriedadeCamera camProp = null)
    {
        if (!Input.GetMouseButtonUp(0) && Global.listaObjetos != null && Global.listaObjetos.Contains(gameObject))
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedades.gameObject;

            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropCuboScript>().Inicializa();
            menuControl.GetComponent<MenuScript>().EnablePanelProp(Global.lastPressedButton.name);
        }
    }

    public void CreatePropPeca(PropriedadePeca propPeca = null)
    {
        if (!Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            PropriedadePeca prPeca;

            if (propPeca == null)
            {
                prPeca = new CuboPropriedadePeca()
                {
                    NomeCuboAmbiente = "CuboAmbiente" + Util_VisEdu.GetSlot(gameObject.name),
                    NomeCuboVis = "CuboVis" + Util_VisEdu.GetSlot(gameObject.name)
                };
                prPeca.Nome = gameObject.name;
            }
            else
            {
                prPeca = propPeca;
            }

            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
        }
    }

    public bool PodeEncaixar()
    {
        const float VALOR_APROXIMADO = 2;
        float pecaY = transform.position.y;

        if ((slot != null) 
            && (slot.transform.position.y + VALOR_APROXIMADO > pecaY) 
            && (slot.transform.position.y - VALOR_APROXIMADO < pecaY))
        {
            if (!Global.listaEncaixes.ContainsKey(gameObject.name))
            {
                Global.listaEncaixes.Add(gameObject.name+DropPeca.countObjetosGraficos.ToString(), slot.name);
            }
            else
            {
                Global.listaEncaixes[gameObject.name] = slot.name;
                return false;
            }

            return true;
        }
        else
        {
            AjustaPeca();
            return false;
        }
    }

    public void CopiaPeca()
    {
        if (PodeGerarCopia())
        {
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = Consts.CUBO;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    string GetNumeroSlotObjetoGrafico(Transform transformObj)
    {
        if (transformObj.name.Contains("ObjGraficoSlot"))
        {
            return gameObject.name;
        }

        return GetNumeroSlotObjetoGrafico(gameObject.transform.parent);
    }
}
