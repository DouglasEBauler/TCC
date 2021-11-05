using System;
using System.Collections;
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

    [HideInInspector]
    public GameObject slot;

    Vector3 offset, scanPos, startPos;
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
            AddPoligono();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(Consts.FORMA_SLOT) && !EstaEncaixado())
        {
            slot = other.gameObject;
        }
    }

    bool EstaEncaixado()
    {
        return Global.listaEncaixes.ContainsKey(gameObject.name);
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

    bool PodeGerarCopia()
    {
        return cam.WorldToScreenPoint(scanPos).y > cam.pixelRect.height / 2;
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

    public void AddPoligono(PoligonoPropriedadePeca propPeca = null)
    {
        Encaixa();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
                GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

            ConfigPoligonoAmb();
        }
        else
        {
            GameObject.Find(Consts.POLIGONO_AMB + Global.countObjetosGraficos.ToString()).GetComponent<MeshRenderer>().enabled = true;
        }

        CreatePropPeca(propPeca);
    }

    void ConfigPoligonoAmb()
    {
        GameObject poligonoAmb = GameObject.Find(Consts.POLIGONO_AMB);
        if (poligonoAmb != null)
        {
            GameObject cloneFab = Instantiate(poligonoAmb, poligonoAmb.transform.position, poligonoAmb.transform.rotation, poligonoAmb.transform.parent);
            cloneFab.name = Consts.POLIGONO_AMB;
            cloneFab.transform.position = new Vector3(poligonoAmb.transform.position.x, poligonoAmb.transform.position.y, poligonoAmb.transform.position.z);
            cloneFab.GetComponent<MeshRenderer>().enabled = false;

            poligonoAmb.name += Global.countObjetosGraficos.ToString();
            poligonoAmb.GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[Consts.OBJETOGRAFICO + Global.countObjetosGraficos.ToString()].Ativo;
        }
    }

    public void ConfiguraPropriedadePeca(PoligonoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado())
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
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            PoligonoPropriedadePeca prPeca;

            if (propPeca != null)
            {
                prPeca = propPeca;
            }
            else
            {
                prPeca = new PoligonoPropriedadePeca();
            }
            prPeca.Nome = gameObject.name;
            prPeca.PoligonoAmbiente = Consts.POLIGONO_AMB + Util_VisEdu.GetSlot(gameObject.name);

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
            Destroy(slot.GetComponent<Rigidbody>());
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
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = Consts.POLIGONO;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
