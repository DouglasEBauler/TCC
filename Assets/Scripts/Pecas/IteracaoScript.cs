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
    [SerializeField]
    Tutorial tutorialScript;

    [HideInInspector]
    public GameObject slot;

    Vector3 offset, scanPos, startPos;

    void Start()
    {
        scanPos = startPos = gameObject.transform.position;
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
            AddIteracao();
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

            if (podeDestruir && !EstaEncaixado())
            {
                transform.position = startPos;
                Destroy(gameObject);
            }
            else
            {
                EncaixaPecaAoSlot();
            }
        }
    }

    bool CheckPanelIsActive()
    {
        return panelArquivo.activeSelf || panelAjuda.activeSelf;
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

    public void AddIteracao(IteracaoPropriedadePeca propPeca = null)
    {
        EncaixaPecaAoSlot();
        CreatePropPeca(propPeca);
    }

    public void ConfiguraPropriedadePeca(IteracaoPropriedadePeca propPeca = null, PropriedadeCamera camProp = null)
    {
        if (EstaEncaixado())
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedade.gameObject;

            CreatePropPeca(propPeca);

            propriedade.GetComponent<PropIteracao>().Inicializa();
            menuControl.GetComponent<MenuScript>().EnablePanelProp(Global.lastPressedButton.name);
        }
    }

    public void CreatePropPeca(IteracaoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            PropriedadePeca prPeca;

            if (propPeca == null)
            {
                string forma = Util_VisEdu.GetPecaByName(gameObject.name, true);

                prPeca = new IteracaoPropriedadePeca()
                {
                    NomeTransformacao = forma
                };
            }
            else
            {
                prPeca = propPeca;
            }
            prPeca.Nome = gameObject.name;

            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
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
            string numSlot = Util_VisEdu.GetNumSlot(slot.name, true);

            gameObject.name += numSlot;

            Destroy(slot.GetComponent<Rigidbody>());
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
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = Consts.ITERACAO;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    bool EstaEncaixado()
    {
        return Global.listaEncaixes.ContainsKey(gameObject.name);
    }
}
