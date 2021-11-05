using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjetoGraficoScript : MonoBehaviour
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
    [SerializeField]
    GameObject render;

    [HideInInspector]
    public GameObject slot;

    Vector3 offset, scanPos, startPos;
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
        offset = scanPos - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));

        CopiaPeca();
        ConfiguraPropriedadePeca();
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f);
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
        if (other.gameObject.name.Contains(Consts.OBJ_GRAFICO_SLOT) && !EstaEncaixado())
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

            if (podeDestruir && !EstaEncaixado())
            {
                StartCoroutine(RemovePeca());
            }
            else
            {
                Encaixa();
            }
        }
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

    bool EstaEncaixado()
    {
        return Global.listaEncaixes.ContainsKey(gameObject.name);
    }

    bool CheckPanelIsActive()
    {
        return panelArquivo.activeSelf || panelAjuda.activeSelf;
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

    public void AddObjGrafico(PropriedadePeca propPeca = null, bool tutorial = false)
    {
        Encaixa();
        SetActivatedSlots(tutorial);
        InstantiateNextSlot();
        CreatePropPeca(propPeca);
    }

    void InstantiateNextSlot()
    {
        slotObjGrafNext.transform.SetAsLastSibling();

        GameObject slotNext;

        GameObject slots = GameObject.Find(Consts.SLOT_FORMA + Global.countObjetosGraficos.ToString());
        if (slots != null)
        {
            slotNext = Instantiate(slots, slots.transform.position, slots.transform.rotation, render.transform);
            slotNext.name = Consts.SLOT_FORMA;
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot(Consts.FORMA_SLOT + Global.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = Consts.FORMA_SLOT;
        }

        slots = GameObject.Find(Consts.SLOT_TRANSF + Global.countObjetosGraficos.ToString());
        if (slots != null)
        {
            slotNext = Instantiate(slots, slots.transform.position, slots.transform.rotation, render.transform);
            slotNext.name = Consts.SLOT_TRANSF;
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot(Consts.TRANSF_SLOT + Global.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = Consts.TRANSF_SLOT;
        }

        slots = GameObject.Find(Consts.SLOT_ILUMINACAO + Global.countObjetosGraficos.ToString());
        if (slots != null)
        {
            slotNext = Instantiate(slots, slots.transform.position, slots.transform.rotation, render.transform);
            slotNext.name = Consts.SLOT_ILUMINACAO;
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot(Consts.ILUMINACAO_SLOT + Global.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = Consts.ILUMINACAO_SLOT;
        }
    }

    void SetActivatedSlots(bool tutorial = false)
    {
        GameObject slot = FindSlot(Consts.SLOT_FORMA);
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += ((!tutorial) ? Global.countObjetosGraficos.ToString() : Consts.TUTORIAL);

            slot = FindSlot(Consts.FORMA_SLOT, slot);
            slot.name += ((!tutorial) ? Global.countObjetosGraficos.ToString() : Consts.TUTORIAL);
        }

        slot = FindSlot(Consts.SLOT_TRANSF);
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += ((!tutorial) ? Global.countObjetosGraficos.ToString() : Consts.TUTORIAL);

            slot = FindSlot(Consts.TRANSF_SLOT, slot);
            slot.name += ((!tutorial) ? Global.countObjetosGraficos.ToString() : Consts.TUTORIAL);
        }

        slot = FindSlot(Consts.SLOT_ILUMINACAO);
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += ((!tutorial) ? Global.countObjetosGraficos.ToString() : Consts.TUTORIAL);

            slot = FindSlot(Consts.ILUMINACAO_SLOT, slot);
            slot.name += ((!tutorial) ? Global.countObjetosGraficos.ToString() : Consts.TUTORIAL);
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
        if (EstaEncaixado())
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
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
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
            && !EstaEncaixado())
        {
            slotObjGrafNext = Instantiate(slot.transform.parent.gameObject, render.transform);
            slotObjGrafNext.name = Consts.SLOT_OBJ_GRAFICO;

            Destroy(slot.GetComponent<Rigidbody>());
            slot.name += (++Global.countObjetosGraficos).ToString();
            slot.transform.parent.name += Global.countObjetosGraficos.ToString();
            gameObject.name += Global.countObjetosGraficos.ToString();

            if (!EstaEncaixado())
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
        if (PodeGerarCopia() && !EstaEncaixado())
        {
            cloneObjGraficoFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneObjGraficoFab.name = Consts.OBJETOGRAFICO;
            cloneObjGraficoFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
