using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjetoGraficoScript : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject slot;
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
    [SerializeField]
    GameObject render;

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject cloneObjGraficoFab;
    Tutorial tutorialScript;
    GameObject slotObjGrafNext;

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

        GeraCopiaPeca();
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
            AddObjGrafico();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("ObjGraficoSlot"))
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

    public void EncaixaSlot()
    {
        if (!tutorialScript.EstaExecutandoTutorial)
        {
            transform.parent = slot.transform;
            transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, slot.transform.position.z);
            gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
        }
    }

    public void AddObjGrafico(bool tutorial = false)
    {
        EncaixaSlot();
        SetActivatedSlots(tutorial);
        InstantiateNextSlot();
        CreatePropPeca(null);
    }

    void InstantiateNextSlot()
    {
        slotObjGrafNext.transform.SetAsLastSibling();

        GameObject slotNext;

        GameObject slots = GameObject.Find("SlotForma" + DropPeca.countObjetosGraficos.ToString());
        if (slots != null)
        {
            slotNext = Instantiate(slots, slots.transform.position, slots.transform.rotation, render.transform);
            slotNext.name = "SlotForma";
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot("FormasSlot" + DropPeca.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = "FormasSlot";
        }

        slots = GameObject.Find("SlotTransf" + DropPeca.countObjetosGraficos.ToString());
        if (slots != null)
        {
            slotNext = Instantiate(slots, slots.transform.position, slots.transform.rotation, render.transform);
            slotNext.name = "SlotTransf";
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot("TransformacoesSlot" + DropPeca.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = "TransformacoesSlot";
        }

        slots = GameObject.Find("SlotIluminacao" + DropPeca.countObjetosGraficos.ToString());
        if (slots != null)
        {
            slotNext = Instantiate(slots, slots.transform.position, slots.transform.rotation, render.transform);
            slotNext.name = "SlotIluminacao";
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot("IluminacaoSlot" + DropPeca.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = "IluminacaoSlot";
        }
    }

    void SetActivatedSlots(bool tutorial = false)
    {
        GameObject slot = FindSlot("SlotForma");
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial");

            slot = FindSlot("FormasSlot", slot);
            slot.name += ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial");
        }

        slot = FindSlot("SlotTransf");
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial");

            slot = FindSlot("TransformacoesSlot", slot);
            slot.name += ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial");
        }

        slot = FindSlot("SlotIluminacao");
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial");

            slot = FindSlot("IluminacaoSlot", slot);
            slot.name += ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial");
        }
    }

    GameObject FindSlot(string slotName, GameObject slot)
    {
        foreach (Transform child in slot.transform)
        {
            if (child.name.Equals(slotName))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    GameObject FindSlot(string slotName)
    {
        foreach (Transform child in render.transform)
        {
            if (child.name.Equals(slotName))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    public void ConfiguraPropriedadePeca(PropriedadePeca propPeca = null)
    {
        if (Global.listaEncaixes.ContainsKey(gameObject.name))
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedades.gameObject;

            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropObjetoGraficoScript>().Inicializa();
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
                prPeca = new PropriedadePeca();
            }
            else
            {
                prPeca = propPeca;
            }
            prPeca.Nome = gameObject.name;
            prPeca.Ativo = true;

            Global.propriedadePecas.Add(gameObject.name, prPeca);
        }
    }

    public bool PodeEncaixar()
    {
        const float VALOR_APROXIMADO = 2;
        float pecaY = transform.position.y;

        if ((slot != null)
            && (slot.transform.position.y + VALOR_APROXIMADO > pecaY)
            && (slot.transform.position.y - VALOR_APROXIMADO < pecaY)
            && !Global.listaEncaixes.ContainsKey(gameObject.name))
        {
            slotObjGrafNext = Instantiate(slot.transform.parent.gameObject, render.transform);
            slotObjGrafNext.name = "SlotObjGraf";

            slot.name += (++DropPeca.countObjetosGraficos).ToString();
            slot.transform.parent.name += DropPeca.countObjetosGraficos.ToString();
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

    public void GeraCopiaPeca()
    {
        if (PodeGerarCopia())
        {
            cloneObjGraficoFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneObjGraficoFab.name = Consts.OBJETOGRAFICO;
            cloneObjGraficoFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
