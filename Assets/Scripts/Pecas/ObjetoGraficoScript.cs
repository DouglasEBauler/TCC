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
    PropriedadePeca propObjGrafico;

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
            else if (EstaEncaixado())
            {
                Encaixa();
            }
            else
            {
                StartCoroutine(RemovePeca());
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

    public void Encaixa(bool isImport = false)
    {
        if (!isImport)
        {
            StartCoroutine(EncaixaPecaAoSlot());
        }
        else
        {
            EncaixaPecaImportada();
        }
    }

    void EncaixaPecaImportada()
    {
        transform.position = slot.transform.position;
        transform.parent = slot.transform;
        gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
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
        }

        transform.parent = slot.transform;
        gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
    }

    public void AddObjGrafico(PropriedadePeca propPeca = null)
    {
        if (tutorialScript.EstaExecutandoTutorial)
        {
            PodeEncaixar();
        }

        Encaixa(propPeca != null);
        SetActivatedSlots();
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
            slotNext = Instantiate(slots, render.transform, false);
            slotNext.name = Consts.SLOT_FORMA;
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot(Consts.FORMA_SLOT + Global.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = Consts.FORMA_SLOT;
        }

        slots = GameObject.Find(Consts.SLOT_TRANSF + Global.countObjetosGraficos.ToString());
        if (slots != null)
        {
            slotNext = Instantiate(slots, render.transform, false);
            slotNext.name = Consts.SLOT_TRANSF;
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot(Consts.TRANSF_SLOT + Global.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = Consts.TRANSF_SLOT;
        }

        slots = GameObject.Find(Consts.SLOT_ILUMINACAO + Global.countObjetosGraficos.ToString());
        if (slots != null)
        {
            slotNext = Instantiate(slots, render.transform, false);
            slotNext.name = Consts.SLOT_ILUMINACAO;
            slotNext.gameObject.SetActive(false);

            slotNext = FindSlot(Consts.ILUMINACAO_SLOT + Global.countObjetosGraficos.ToString(), slotNext);
            slotNext.name = Consts.ILUMINACAO_SLOT;
        }
    }

    void SetActivatedSlots()
    {
        GameObject slot = FindSlot(Consts.SLOT_FORMA);
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += Global.countObjetosGraficos.ToString();

            slot = FindSlot(Consts.FORMA_SLOT, slot);
            slot.name += Global.countObjetosGraficos.ToString();
        }

        slot = FindSlot(Consts.SLOT_TRANSF);
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += Global.countObjetosGraficos.ToString();

            slot = FindSlot(Consts.TRANSF_SLOT, slot);
            slot.name += Global.countObjetosGraficos.ToString();
        }

        slot = FindSlot(Consts.SLOT_ILUMINACAO);
        if (slot != null)
        {
            slot.gameObject.SetActive(true);
            slot.name += Global.countObjetosGraficos.ToString();

            slot = FindSlot(Consts.ILUMINACAO_SLOT, slot);
            slot.name += Global.countObjetosGraficos.ToString();
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
            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropObjetoGraficoScript>().Inicializa(this.propObjGrafico);
            menuControl.GetComponent<MenuScript>().EnablePanelProp(propriedades.name);
        }
    }

    public void CreatePropPeca(PropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            if (propPeca == null)
            {
                this.propObjGrafico = new PropriedadePeca();
            }
            else
            {
                this.propObjGrafico = propPeca;
            }
            this.propObjGrafico.Nome = Consts.OBJETOGRAFICO;
            this.propObjGrafico.NomePeca = gameObject.name;
            this.propObjGrafico.Ativo = true;

            Global.propriedadePecas.Add(this.propObjGrafico.NomePeca, this.propObjGrafico);
        }
    }

    public bool PodeEncaixar()
    {
        if (tutorialScript.EstaExecutandoTutorial
            || ((slot != null)
                && (Vector3.Distance(slot.transform.position, gameObject.transform.position) < 4)
                && !EstaEncaixado()))
        {
            if (tutorialScript.EstaExecutandoTutorial)
            {
                slot = GameObject.Find(Consts.OBJ_GRAFICO_SLOT);
            }

            slotObjGrafNext = Instantiate(slot.transform.parent.gameObject, render.transform, false);
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
