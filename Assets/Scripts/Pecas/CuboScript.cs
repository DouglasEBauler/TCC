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
    CuboPropriedadePeca propPeca;

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
            //if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
            //    GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

            ConfigCuboAmb();
            ConfigCuboVis();
        }
        else
        {
            GameObject.Find(Consts.CUBO_AMB + Util_VisEdu.GetNumSlot(slot.name)).GetComponent<MeshRenderer>().enabled = true;
        }

        CreatePropPeca(propPeca);
    }

    void ConfigCuboVis()
    {
        GameObject cuboVisObj = GameObject.Find(Consts.CUBO_VIS_OBJ);
        if (cuboVisObj != null)
        {
            GameObject cloneFab = Instantiate(cuboVisObj, cuboVisObj.transform.position, cuboVisObj.transform.rotation, cuboVisObj.transform.parent);
            cloneFab.name = Consts.CUBO_VIS_OBJ;
            cloneFab.transform.position = new Vector3(cuboVisObj.transform.position.x, cuboVisObj.transform.position.y, cuboVisObj.transform.position.z);
            cloneFab.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            cuboVisObj.name += Util_VisEdu.GetNumSlot(slot.name);
            cuboVisObj.transform.GetChild(0).name += Util_VisEdu.GetNumSlot(slot.name);
            cuboVisObj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[Consts.OBJETOGRAFICO + Util_VisEdu.GetNumSlot(slot.name)].Ativo && Global.cameraAtiva;
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

            cuboAmbObj.name += Util_VisEdu.GetNumSlot(slot.name);
            cuboAmbObj.transform.GetChild(0).name += Util_VisEdu.GetNumSlot(slot.name);
            cuboAmbObj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[Consts.OBJETOGRAFICO + Util_VisEdu.GetNumSlot(slot.name)].Ativo;
        }
    }

    public void ConfiguraPropriedadePeca(CuboPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado())
        {
            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropCuboScript>().Inicializa(this.propPeca);
            menuControl.GetComponent<MenuScript>().EnablePanelProp(propriedades.name);
        }
    }

    public void CreatePropPeca(CuboPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            CuboPropriedadePeca prPeca;

            if (propPeca == null)
            {
                this.propPeca = new CuboPropriedadePeca();
            }
            else
            {
                this.propPeca = propPeca;
            }
            this.propPeca.Nome = Consts.CUBO;
            this.propPeca.NomePeca = gameObject.name;
            this.propPeca.NomeCuboAmb = Consts.CUBO_AMB + Util_VisEdu.GetNumSlot(slot.name);
            this.propPeca.NomeCuboVis = Consts.CUBO_VIS + Util_VisEdu.GetNumSlot(slot.name);

            Global.propriedadePecas.Add(this.propPeca.NomePeca, this.propPeca);
        }
    }

    bool EstaEncaixado()
    {
        return Global.listaEncaixes.ContainsKey(gameObject.name);
    }

    public bool PodeEncaixar()
    {
        if ((slot != null)
            && (Vector3.Distance(slot.transform.position, gameObject.transform.position) < 4)
            && !EstaEncaixado())
        {
            Destroy(slot.GetComponent<Rigidbody>());
            gameObject.name += Util_VisEdu.GetNumSlot(slot.name);

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
}
