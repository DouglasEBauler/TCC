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

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject slot;
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
            AddSpline();
        }
    }

    void AddSpline()
    {
        EncaixaPecaAoSlot();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
                GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

            ConfigSplineAmb();
        }
        else
        {
            GameObject.Find("SplineMesh" + DropPeca.countObjetosGraficos.ToString()).GetComponent<MeshRenderer>().enabled = true;
        }

        CreatePropPeca(null);
    }

    void ConfigSplineAmb()
    {
        GameObject splineAmb = GameObject.Find("SplineAmbiente" + DropPeca.countObjetosGraficos.ToString());
        if (splineAmb != null)
        {
            splineAmb.name += DropPeca.countObjetosGraficos.ToString();
        }

        // GameObject gerado pelo proprio componente SplineMesh
        // nao sendo possivel alterar seu nome, apenas o seu objeto pai pode ser redefinido
        splineAmb = GameObject.Find("segment 1 mesh");
        if (splineAmb != null)
        {
            MeshRenderer mr = splineAmb.GetComponent<MeshRenderer>();
            mr.enabled = Global.propriedadePecas["ObjetoGraficoP" + DropPeca.countObjetosGraficos.ToString()].Ativo;
        }
    }

    void AjustaPeca()
    {
        if (!tutorialScript.EstaExecutandoTutorial && !CheckPanelIsActive())
        {
            bool podeDestruir = PodeGerarCopiaPeca();

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

    public void EncaixaPecaAoSlot()
    {
        if (!tutorialScript.EstaExecutandoTutorial)
        {
            transform.parent = slot.transform;
            transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, slot.transform.position.z);
            gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
        }
    }

    public void ConfiguraPropriedadePeca(SplinePropriedadePeca propPeca = null, PropriedadeCamera camProp = null)
    {
        if (Global.listaEncaixes.ContainsKey(gameObject.name))
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedades.gameObject;

            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropSplineScript>().Inicializa();
            menuControl.GetComponent<MenuScript>().EnablePanelProp(Global.lastPressedButton.name);
        }
    }

    public void CreatePropPeca(SplinePropriedadePeca propPeca = null)
    {
        if (!Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            SplinePropriedadePeca prPeca;

            if (propPeca == null)
            {
                prPeca = new SplinePropriedadePeca()
                {
                    SplineAmbiente = "SplineAmbiente" + DropPeca.countObjetosGraficos
                };
                prPeca.Nome = gameObject.name;
            }
            else
            {
                prPeca = propPeca;
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

    public void GeraCopiaPeca()
    {
        if (PodeGerarCopiaPeca())
        {
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = Consts.SPLINE;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    bool PodeGerarCopiaPeca()
    {
        return screenPoint.y > cam.pixelRect.height / 2;
    }
}
