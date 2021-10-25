using UnityEngine;
using UnityEngine.UI;

public class IteracaoScript : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject propriedade;
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

    Vector3 screenPoint, offset, scanPos;
    GameObject slot;
    Tutorial tutorialScript;

    void Start()
    {
        scanPos = gameObject.transform.position;
        tutorialScript = tutorial.GetComponent<Tutorial>();
    }

    void Update()
    {
        scanPos = gameObject.transform.position;
    }

    void OnMouseDown()
    {
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
            EncaixaPecaAoSlot();
            AddPecaIteracao();
            CreatePropPeca(null);
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
                if (Global.listaEncaixes.ContainsKey(gameObject.name))
                {
                    Global.listaEncaixes.Remove(gameObject.name);
                }
                Destroy(gameObject);
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
        }
    }

    public void AddPecaIteracao()
    {
        string numFormaSlot = Util_VisEdu.GetSlot(gameObject.name);

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            string numObjGrafSlot = Util_VisEdu.GetNumeroSlotObjetoGrafico(gameObject);

            if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
                GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

            GameObject cuboAmb = GameObject.Find("CuboAmbiente");
            if (cuboAmb != null)
            {
                GameObject cloneFab = Instantiate(cuboAmb, cuboAmb.transform.position, cuboAmb.transform.rotation, cuboAmb.transform.parent);
                cloneFab.name = "CuboAmbiente";
                cloneFab.transform.position = new Vector3(cuboAmb.transform.position.x, cuboAmb.transform.position.y, cuboAmb.transform.position.z);
                cloneFab.GetComponent<MeshRenderer>().enabled = false;

                cuboAmb.name += numFormaSlot;
                cuboAmb.GetComponent<MeshRenderer>().enabled = Global.propriedadePecas["ObjGraficoP" + numObjGrafSlot].Ativo;
            }

            GameObject cuboVis = GameObject.Find("CuboVis");
            if (cuboVis != null)
            {
                GameObject cloneFab = Instantiate(cuboVis, cuboVis.transform.position, cuboVis.transform.rotation, cuboVis.transform.parent);
                cloneFab.name = "CuboVis";
                cloneFab.transform.position = new Vector3(cuboVis.transform.position.x, cuboVis.transform.position.y, cuboVis.transform.position.z);
                cloneFab.GetComponent<MeshRenderer>().enabled = false;

                cuboVis.name += numObjGrafSlot;
                cuboVis.GetComponent<MeshRenderer>().enabled = Global.propriedadePecas["ObjGraficoP" + numObjGrafSlot].Ativo;
            }
        }
        else
        {
            GameObject.Find("CuboAmbiente" + numFormaSlot).GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void ConfiguraPropriedadePeca(PropriedadePeca propPeca = null, PropriedadeCamera camProp = null)
    {
        if (Global.listaEncaixes.ContainsKey(gameObject.name))
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedade.gameObject;

            CreatePropPeca(propPeca);

            propriedade.GetComponent<PropCuboScript>().Inicializa();
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
            if (!Global.listaEncaixes.ContainsKey(gameObject.transform.name))
            {
                Global.listaEncaixes.Add(gameObject.name+DropPeca.countObjetosGraficos.ToString(), slot.name);
            }
            else
            {
                Global.listaEncaixes[gameObject.transform.name] = slot.name;
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
