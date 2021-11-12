using System.Collections;
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

    Vector3 offset, scanPos, startPos;
    Tutorial tutorialScript;
    PropriedadeCamera propCamera;

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
        if (other.gameObject.name.Contains(Consts.CAMERA_SLOT) && !EstaEncaixado())
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


    public void AddCamera(PropriedadeCamera proCam = null)
    {
        Encaixa();

        propriedades.GetComponent<PropCameraScript>().DemosntraCamera(true);

        //if (lightProperty.existeIluminacao())
        //    GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

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
        if (EstaEncaixado())
        {
            CreatePropPeca();

            if (camProp != null)
            {
                Global.propCameraGlobal = camProp;
            }
            propriedades.GetComponent<PropCameraScript>().PreencheCampos();
            propriedades.GetComponent<PropCameraScript>().UpdateProp();
            propriedades.GetComponent<PropCameraScript>().DemosntraCamera(true);

            menuControl.GetComponent<MenuScript>().EnablePanelProp(propriedades.name);
        }
    }

    public void CreatePropPeca(PropriedadeCamera propPeca = null)
    {
        if (!EstaEncaixado())
        {
            if (propPeca != null)
            {
                this.propCamera = propPeca;
            }
            else
            {
                this.propCamera = new PropriedadeCamera();
            }
            this.propCamera.NomePeca = gameObject.name;

            Global.propriedadePecas.Add(gameObject.name, this.propCamera);
        }
    }

    bool EstaEncaixado()
    {
        return Global.listaEncaixes.ContainsKey(gameObject.name);
    }

    public bool PodeEncaixar()
    {
        if ((slot != null) 
            && (Vector3.Distance(transform.position, slot.transform.position) < 4) 
            && !EstaEncaixado())
        {
            slot.name += "1";
            slot.transform.parent.name += "1";
            gameObject.name += "1";
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
        if (PodeGerarCopiaPeca() && !EstaEncaixado())
        {
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = gameObject.name;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
