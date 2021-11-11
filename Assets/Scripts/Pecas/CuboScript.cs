using System.Collections;
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

    [HideInInspector]
    public GameObject slot;

    Vector3 offset, scanPos, startPos;
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
            AddCubo();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(Consts.FORMA_SLOT) && !EstaEncaixado())
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

    bool CheckPanelIsActive()
    {
        return panelArquivo.activeSelf || panelAjuda.activeSelf;
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

    public void AddCubo(CuboPropriedadePeca propPeca = null)
    {
        Encaixa();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
                GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

            ConfigCuboAmb();
            ConfigCuboVis();
        }
        else
        {
            GameObject.Find(Consts.CUBO_AMB + Util_VisEdu.GetSlot(gameObject.name)).GetComponent<MeshRenderer>().enabled = true;
        }

        CreatePropPeca(propPeca);
    }

    void ConfigCuboVis()
    {
        GameObject cuboVis = GameObject.Find(Consts.CUBO_VIS);
        if (cuboVis != null)
        {
            GameObject cloneFab = Instantiate(cuboVis, cuboVis.transform.position, cuboVis.transform.rotation, cuboVis.transform.parent);
            cloneFab.name = Consts.CUBO_VIS;
            cloneFab.transform.position = new Vector3(cuboVis.transform.position.x, cuboVis.transform.position.y, cuboVis.transform.position.z);
            cloneFab.GetComponent<MeshRenderer>().enabled = false;

            cuboVis.name += Global.countObjetosGraficos.ToString();
            cuboVis.GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[Consts.OBJETOGRAFICO + Util_VisEdu.GetSlot(gameObject.name)].Ativo;
        }
    }

    void ConfigCuboAmb()
    {
        GameObject cuboAmbObj = GameObject.Find(Consts.CUBO_AMB_OBJ);
        if (cuboAmbObj != null)
        {
            GameObject cloneFab = Instantiate(cuboAmbObj, cuboAmbObj.transform.position, cuboAmbObj.transform.rotation, cuboAmbObj.transform.parent);
            cloneFab.name = Consts.CUBO_AMB_OBJ;
            cloneFab.transform.position = new Vector3(cuboAmbObj.transform.position.x, cuboAmbObj.transform.position.y, cuboAmbObj.transform.position.z);
            cloneFab.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            cuboAmbObj.name += Global.countObjetosGraficos.ToString();
            cuboAmbObj.transform.GetChild(0).name += Global.countObjetosGraficos.ToString();
            cuboAmbObj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[Consts.OBJETOGRAFICO + Util_VisEdu.GetSlot(gameObject.name)].Ativo;
        }
    }

    public void ConfiguraPropriedadePeca(CuboPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado())
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedades.gameObject;

            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropCuboScript>().Inicializa();
            menuControl.GetComponent<MenuScript>().EnablePanelProp(Global.lastPressedButton.name);
        }
    }

    public void CreatePropPeca(CuboPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            CuboPropriedadePeca prPeca;

            if (propPeca == null)
            {
                prPeca = new CuboPropriedadePeca();
            }
            else
            {
                prPeca = propPeca;
            }
            prPeca.Nome = gameObject.name;
            prPeca.NomeCuboAmbiente = Consts.CUBO_AMB + Util_VisEdu.GetSlot(gameObject.name);
            prPeca.NomeCuboVis = Consts.CUBO_VIS + Util_VisEdu.GetSlot(gameObject.name);

            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
        }
    }

    bool EstaEncaixado()
    {
        return Global.listaEncaixes.ContainsKey(gameObject.name);
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
            cloneFab.name = Consts.CUBO;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    string GetNumeroSlotObjetoGrafico(Transform transformObj)
    {
        if (transformObj.name.Contains(Consts.OBJ_GRAFICO_SLOT))
        {
            return gameObject.name;
        }

        return GetNumeroSlotObjetoGrafico(gameObject.transform.parent);
    }
}
