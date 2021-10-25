using UnityEngine;
using UnityEngine.UI;

public class CameraPScript : MonoBehaviour
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
    GameObject tutorial;
    [SerializeField]
    GameObject panelArquivo;
    [SerializeField]
    GameObject panelPropPeca;
    [SerializeField]
    GameObject panelAjuda;

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject cloneFab;
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

        CopiaPeca();
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
            AddCamera();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("CameraSlot"))
        {
            slot = other.gameObject;
        }   
    }

    bool PodeGerarCopiaPeca()
    {
        return screenPoint.y > cam.pixelRect.height / 2;
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

    public void AddCamera()
    {
        EncaixaPecaAoSlot();

        PropIluminacaoPadrao lightProperty = new PropIluminacaoPadrao();

        propriedades.GetComponent<PropCameraScript>().DemosntraCamera(true);

        if (lightProperty.existeIluminacao())
            GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

        // Verifica se existem cubos e iluminações mas a câmera ainda não foi colocada.
        Global.propCameraGlobal.ExisteCamera = true;

        CreatePropPeca(null);
    }

    public void ConfiguraPropriedadePeca(PropriedadeCamera camProp = null)
    {
        if (Global.listaEncaixes.ContainsKey(gameObject.name))
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedades.gameObject;

            CreatePropPeca();

            if (camProp != null)
            {
                Global.propCameraGlobal = camProp;
            }
            propriedades.GetComponent<PropCameraScript>().PreencheCampos();
            propriedades.GetComponent<PropCameraScript>().UpdateProp();
            propriedades.GetComponent<PropCameraScript>().DemosntraCamera(true);

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
            slot.name += "1";
            gameObject.name += "1";

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

    public void CopiaPeca()
    {
        if (PodeGerarCopiaPeca())
        {
            cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = gameObject.name;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
