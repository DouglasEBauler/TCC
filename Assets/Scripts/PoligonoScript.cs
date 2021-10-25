using UnityEngine;
using UnityEngine.UI;

public class PoligonoScript : MonoBehaviour
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
    GameObject panelTutorial;

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject slot;
    Tutorial tutorialScript;

    void Start()
    {
        scanPos = startPos = gameObject.transform.position;
        tutorialScript = panelTutorial.GetComponent<Tutorial>();
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
        ConfiguraPoligonoPropriedadePeca();
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
            AddPoligono();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("FormasSlot"))
        {
            slot = other.gameObject;
        }
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

    bool PodeGerarCopia()
    {
        return screenPoint.y > cam.pixelRect.height / 2;
    }

    bool CheckPanelIsActive()
    {
        return panelArquivo.activeSelf || panelPropPeca.activeSelf || panelAjuda.activeSelf;
    }

    public void EncaixaPecaAoSlot()
    {
        if (!tutorialScript.EstaExecutandoTutorial)
        {
            transform.parent = slot.transform;
            transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, slot.transform.position.z);
            gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
        }
    }

    public void AddPoligono()
    {
        EncaixaPecaAoSlot();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
                GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

            ConfigPoligonoAmb();
        }
        else
        {
            GameObject.Find("PoligonoAmbiente" + DropPeca.countObjetosGraficos.ToString()).GetComponent<MeshRenderer>().enabled = true;
        }

        CreatePropPeca(null);
    }

    void ConfigPoligonoAmb()
    {
        GameObject poligonoAmb = GameObject.Find("PoligonoAmbiente");
        if (poligonoAmb != null)
        {
            GameObject cloneFab = Instantiate(poligonoAmb, poligonoAmb.transform.position, poligonoAmb.transform.rotation, poligonoAmb.transform.parent);
            cloneFab.name = "PoligonoAmbiente";
            cloneFab.transform.position = new Vector3(poligonoAmb.transform.position.x, poligonoAmb.transform.position.y, poligonoAmb.transform.position.z);
            cloneFab.GetComponent<MeshRenderer>().enabled = false;

            poligonoAmb.name += DropPeca.countObjetosGraficos.ToString();
            poligonoAmb.GetComponent<MeshRenderer>().enabled = Global.propriedadePecas["ObjetoGraficoP" + DropPeca.countObjetosGraficos.ToString()].Ativo;
        }
    }

    public void ConfiguraPoligonoPropriedadePeca(PoligonoPropriedadePeca propPeca = null)
    {
        if (Global.listaEncaixes.ContainsKey(gameObject.name))
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedades.gameObject;

            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropPoligonoScript>().Inicializa();
            menuControl.GetComponent<MenuScript>().EnablePanelProp(Global.lastPressedButton.name);
        }
    }

    void CreatePropPeca(PoligonoPropriedadePeca propPeca = null)
    {
        if (!Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            PoligonoPropriedadePeca prPeca;

            if (propPeca != null)
            {
                prPeca = propPeca;
            }
            else
            {
                prPeca = new PoligonoPropriedadePeca()
                {
                    PoligonoAmbiente = "PoligonoAmbiente" + DropPeca.countObjetosGraficos.ToString()
                };
                prPeca.Nome = gameObject.name;
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
            gameObject.name += DropPeca.countObjetosGraficos.ToString();

            if (!Global.listaEncaixes.ContainsKey(gameObject.name))
            {
                Global.listaEncaixes.Add(gameObject.name, slot.name);
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
            cloneFab.name = Consts.POLIGONO;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
