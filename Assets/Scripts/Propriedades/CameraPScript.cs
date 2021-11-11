using UnityEngine;
using UnityEngine.UI;

public class CameraPScript : MonoBehaviour
{
    [SerializeField]
    Camera cam;
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

    [HideInInspector]
    public GameObject slot;

    Vector3 screenPoint, offset, scanPos, startPos;
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
            AddCamera();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(Consts.CAMERA_SLOT))
        {
            slot = other.gameObject;
        }   
    }

    bool PodeGerarCopiaPeca()
    {
        return cam.WorldToScreenPoint(scanPos).y > cam.pixelRect.height / 2;
    }

    void AjustaPeca()
    {
        if (!tutorialScript.EstaExecutandoTutorial && !CheckPanelIsActive())
        {
            bool podeDestruir = PodeGerarCopiaPeca();

            if (podeDestruir && !Global.listaEncaixes.ContainsKey(gameObject.name))
            {
                transform.position = startPos;
                Destroy(gameObject);
                screenPoint = cam.WorldToScreenPoint(scanPos);
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

    public void AddCamera(PropriedadeCamera proCam = null)
    {
        EncaixaPecaAoSlot();

        PropIluminacaoPadrao lightProperty = new PropIluminacaoPadrao();

        propriedades.GetComponent<PropCameraScript>().DemosntraCamera(true);

        if (lightProperty.existeIluminacao())
            GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

        // Verifica se existem cubos e iluminações mas a câmera ainda não foi colocada.
        Global.propCameraGlobal.ExisteCamera = true;

        CreatePropPeca(proCam);
    }

    public void ConfiguraPropriedadePeca()
    {
        ConfiguraPropriedadePeca(null);
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

    public void CreatePropPeca(PropriedadeCamera propPeca = null)
    {
        if (!Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            PropriedadeCamera prPeca;

            if (propPeca != null)
            {
                prPeca = propPeca;
            }
            else
            {
                prPeca = new PropriedadeCamera();
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
            slot.transform.parent.name += "1";
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
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = gameObject.name;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
