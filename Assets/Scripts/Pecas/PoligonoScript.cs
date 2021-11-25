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
    PoligonoPropriedadePeca propPeca;
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
            ConfigPoligonoAmb();
            ConfigPoligonoVis();
        }
        else
        {
            GameObject.Find(Consts.POLIGONO_AMB + Util_VisEdu.GetNumSlot(slot.name)).GetComponent<MeshRenderer>().enabled = true;
        }

        CreatePropPeca(propPeca);
    }

    void ConfigPoligonoAmb()
    {
        GameObject objPecaAmbiente = GameObject.Find(Consts.POLIGONO_AMB_OBJ);
        if (objPecaAmbiente != null)
        {
            GameObject cloneFab = Instantiate(objPecaAmbiente, objPecaAmbiente.transform.parent.position, objPecaAmbiente.transform.parent.rotation, objPecaAmbiente.transform.parent);
            cloneFab.name = Consts.POLIGONO_AMB_OBJ;
            cloneFab.transform.position = new Vector3(objPecaAmbiente.transform.position.x, objPecaAmbiente.transform.position.y, objPecaAmbiente.transform.position.z);
            cloneFab.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false; //PoligonoAmbiente

            objPecaAmbiente.name += Util_VisEdu.GetNumSlot(slot.name);
            objPecaAmbiente.transform.GetChild(0).name += Util_VisEdu.GetNumSlot(slot.name);
            objPecaAmbiente.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[Consts.OBJETOGRAFICO + Util_VisEdu.GetNumSlot(slot.name)].Ativo;
        }
    }
    void ConfigPoligonoVis()
    {
        GameObject objPecaVis = GameObject.Find(Consts.POLIGONO_VIS_OBJ);
        if (objPecaVis != null)
        {
            GameObject cloneFab = Instantiate(objPecaVis, objPecaVis.transform.position, objPecaVis.transform.rotation, objPecaVis.transform.parent);
            cloneFab.name = Consts.POLIGONO_VIS_OBJ;
            cloneFab.transform.position = new Vector3(objPecaVis.transform.position.x, objPecaVis.transform.position.y, objPecaVis.transform.position.z);
            cloneFab.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false; //PoligonoVis

            objPecaVis.transform.name += Util_VisEdu.GetNumSlot(slot.name);
            objPecaVis.transform.GetChild(0).name += Util_VisEdu.GetNumSlot(slot.name);
            objPecaVis.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[Consts.OBJETOGRAFICO + Util_VisEdu.GetNumSlot(slot.name)].Ativo && Global.cameraAtiva;
        }
    }

    public void ConfiguraPropriedadePeca(PoligonoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado())
        {
            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropPoligonoScript>().Inicializa(this.propPeca);
            menuControl.GetComponent<MenuScript>().EnablePanelProp(propriedades.name);
        }
    }

    void CreatePropPeca(PoligonoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            string numSlot = Util_VisEdu.GetNumSlot(slot.name);

            if (propPeca != null)
            {
                this.propPeca = propPeca;
            }
            else
            {
                this.propPeca = new PoligonoPropriedadePeca();
            }
            this.propPeca.Nome = Consts.POLIGONO;
            this.propPeca.NomePeca = gameObject.name;
            this.propPeca.PoligonoAmb = Consts.POLIGONO_AMB + numSlot;
            this.propPeca.PoligonoVis = Consts.POLIGONO_AMB + numSlot;

            Global.propriedadePecas.Add(this.propPeca.NomePeca, this.propPeca);
        }
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
            cloneFab.name = Consts.POLIGONO;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
