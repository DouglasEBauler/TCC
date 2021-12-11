using SplineMesh;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplineScript : MonoBehaviour
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
    SplinePropriedadePeca propPeca;

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
            AddSpline();
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

    public void AddSpline(SplinePropriedadePeca propPeca = null)
    {
        Encaixa(propPeca != null);

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            ConfigSplineAmb();
            ConfigSplineVis();
        }
        else
        {
            GameObject.Find("segment 1 mesh").GetComponent<MeshRenderer>().enabled = true;
        }

        CreatePropPeca(propPeca);
    }

    void ConfigSplineVis()
    {
        GameObject objPecaVis = GameObject.Find(Consts.SPLINE_VIS_OBJ);
        if (objPecaVis != null)
        {
            GameObject cloneFab = Instantiate(objPecaVis, objPecaVis.transform.parent.position, objPecaVis.transform.parent.rotation, objPecaVis.transform.parent);
            cloneFab.name = Consts.SPLINE_VIS_OBJ;
            cloneFab.transform.position = new Vector3(objPecaVis.transform.position.x, objPecaVis.transform.position.y, objPecaVis.transform.position.z);
            
            objPecaVis.name += Util_VisEdu.GetNumSlot(slot.name); // SplineVisObject
            objPecaVis.transform.GetChild(0).name += Util_VisEdu.GetNumSlot(slot.name); //SplineVis

            // GameObject gerado pelo proprio componente SplineMesh
            // nao sendo possivel alterar seu nome, apenas o seu objeto pai pode ser redefinido
            Transform splineMesh = objPecaVis.transform.GetChild(0).GetChild(0).Find("segment 1 mesh");
            if (splineMesh != null)
            {
                splineMesh.gameObject.GetComponent<MeshRenderer>().enabled =
                    Global.propriedadePecas[Consts.OBJETOGRAFICO + Util_VisEdu.GetNumSlot(slot.name)].Ativo && Global.cameraAtiva;
            }
        }
    }

    void ConfigSplineAmb()
    {
        GameObject objPecaAmbiente = GameObject.Find(Consts.SPLINE_AMB_OBJ);
        if (objPecaAmbiente != null)
        {
            GameObject cloneFab = Instantiate(objPecaAmbiente, objPecaAmbiente.transform.parent.position, objPecaAmbiente.transform.parent.rotation, objPecaAmbiente.transform.parent);
            cloneFab.name = Consts.SPLINE_AMB_OBJ;
            cloneFab.transform.position = new Vector3(objPecaAmbiente.transform.position.x, objPecaAmbiente.transform.position.y, objPecaAmbiente.transform.position.z);

            objPecaAmbiente.name += Util_VisEdu.GetNumSlot(slot.name); //SplineAmbObject
            objPecaAmbiente.transform.GetChild(0).name += Util_VisEdu.GetNumSlot(slot.name); //SplineAmb

            // GameObject gerado pelo proprio componente SplineMesh
            // nao sendo possivel alterar seu nome, apenas o seu objeto pai pode ser redefinido
            Transform splineMesh = objPecaAmbiente.transform.GetChild(0).GetChild(0).Find("segment 1 mesh");
            if (splineMesh != null)
            {
                splineMesh.gameObject.GetComponent<MeshRenderer>().enabled =
                    Global.propriedadePecas[Consts.OBJETOGRAFICO + Util_VisEdu.GetNumSlot(slot.name)].Ativo;
            }
        }
    }

    void AjustaPeca()
    {
        if (!tutorialScript.EstaExecutandoTutorial && !CheckPanelIsActive())
        {
            bool podeDestruir = PodeGerarCopiaPeca();

            if (podeDestruir && !Global.listaEncaixes.ContainsKey(gameObject.name))
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

    public void ConfiguraPropriedadePeca(SplinePropriedadePeca propPeca = null, PropriedadeCamera camProp = null)
    {
        if (EstaEncaixado())
        {
            CreatePropPeca(propPeca);
            propriedades.GetComponent<PropSplineScript>().Inicializa(this.propPeca);
            menuControl.GetComponent<MenuScript>().EnablePanelProp(propriedades.name);
        }
    }

    void CreatePropPeca(SplinePropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            string numSlot = Util_VisEdu.GetNumSlot(slot.name);

            if (propPeca == null)
            {
                this.propPeca = new SplinePropriedadePeca();
            }
            else
            {
                this.propPeca = propPeca;
            }
            this.propPeca.Nome = Consts.SPLINE;
            this.propPeca.NomePeca = gameObject.name;
            this.propPeca.SplineAmb = Consts.SPLINE_AMB + numSlot;
            this.propPeca.SplineVis = Consts.SPLINE_VIS + numSlot;

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
        if (PodeGerarCopiaPeca() && !EstaEncaixado())
        {
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = Consts.SPLINE;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    bool PodeGerarCopiaPeca()
    {
        return cam.WorldToScreenPoint(scanPos).y > cam.pixelRect.height / 2;
    }
}
