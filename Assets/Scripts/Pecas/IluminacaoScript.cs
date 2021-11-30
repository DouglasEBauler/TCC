using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IluminacaoScript : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject propriedades;
    [SerializeField]
    GameObject propCamera;
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    GameObject panelArquivo;
    [SerializeField]
    GameObject panelAjuda;
    [SerializeField]
    Tutorial tutorialScript;

    [HideInInspector]
    public GameObject slot;

    Vector3 offset, scanPos, startPos;
    GameObject cloneFab;
    IluminacaoPropriedadePeca propIluminacao;

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
            AddIluminacao();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(Consts.ILUMINACAO_SLOT))
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
        }

        transform.parent = slot.transform;
        gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
    }

    public void AddIluminacao()
    {
        Encaixa();
        CreatePropPeca();
        CriaLightObject();
    }

    public void ConfiguraPropriedadePeca(IluminacaoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado())
        {
            CreatePropPeca(propPeca);

            propriedades.GetComponent<PropIluminacaoScript>().Inicializa(this.propIluminacao);
            menuControl.GetComponent<MenuScript>().EnablePanelProp(propriedades.name);
        }
    }

    public void CreatePropPeca(IluminacaoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            if (propPeca == null)
            {
                this.propIluminacao = new IluminacaoPropriedadePeca();
                this.propIluminacao.TipoLuz = 0;
            }
            else
            {
                this.propIluminacao = propPeca;
            }
            this.propIluminacao.Nome = Consts.ILUMINACAO;
            this.propIluminacao.NomePeca = gameObject.name;
            this.propIluminacao.NomePecaAmbiente = GetNomePecaAmbiente() + Util_VisEdu.GetNumSlot(gameObject.name);
            this.propIluminacao.NomePecaVis = GetNomePecaVis() + Util_VisEdu.GetNumSlot(gameObject.name);

            Global.propriedadePecas.Add(this.propIluminacao.NomePeca, this.propIluminacao);
        }
    }

    public bool PodeEncaixar()
    {
        if ((tutorialScript.EstaExecutandoTutorial)
            || ((slot != null)
            && (Vector3.Distance(slot.transform.position, gameObject.transform.position) < 4)
            && ExisteFormaEncaixado()
            && !EstaEncaixado()))
        {
            if (tutorialScript.EstaExecutandoTutorial)
            {
                slot = GameObject.Find(Consts.ILUMINACAO_SLOT + "1");
            }

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

    string GetNomePecaAmbiente()
    {
        if (Global.listaEncaixes.ContainsKey(Consts.CUBO + Util_VisEdu.GetNumSlot(slot.name)))
        {
            return Consts.CUBO_AMB;
        }
        else if (Global.listaEncaixes.ContainsKey(Consts.POLIGONO + Util_VisEdu.GetNumSlot(slot.name)))
        {
            return Consts.POLIGONO_AMB;
        }
        else if (Global.listaEncaixes.ContainsKey(Consts.SPLINE + Util_VisEdu.GetNumSlot(slot.name)))
        {
            return Consts.SPLINE_AMB;
        }

        return string.Empty;
    }

    string GetNomePecaVis()
    {
        if (Global.listaEncaixes.ContainsKey(Consts.CUBO + Util_VisEdu.GetNumSlot(slot.name)))
        {
            return Consts.CUBO_VIS;
        }
        else if (Global.listaEncaixes.ContainsKey(Consts.POLIGONO + Util_VisEdu.GetNumSlot(slot.name)))
        {
            return Consts.POLIGONO_VIS;
        }
        else if (Global.listaEncaixes.ContainsKey(Consts.SPLINE + Util_VisEdu.GetNumSlot(slot.name)))
        {
            return Consts.SPLINE_VIS;
        }

        return string.Empty;
    }

    bool EstaEncaixado()
    {
        return Global.listaEncaixes.ContainsKey(gameObject.name);
    }

    public void CopiaPeca()
    {
        if (PodeGerarCopia() && !EstaEncaixado())
        {
            cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = Consts.ILUMINACAO;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    void CriaLightObject()
    {
        GameObject GOCloneLightObjects = GameObject.Find(Consts.LIGHT_OBJECTS_ILUMINACAO);
        if (GOCloneLightObjects != null)
        {
            GameObject cloneGO = Instantiate(GOCloneLightObjects, GOCloneLightObjects.transform.position, GOCloneLightObjects.transform.rotation, GOCloneLightObjects.transform.parent);
            cloneGO.name = Consts.LIGHT_OBJECTS_ILUMINACAO;
            cloneGO.transform.position = new Vector3(GOCloneLightObjects.transform.position.x, GOCloneLightObjects.transform.position.y, GOCloneLightObjects.transform.position.z);

            GOCloneLightObjects.name += Util_VisEdu.GetNumSlot(slot.name);
            RenomeiaLightObject(GOCloneLightObjects);
        }
    }

    string GetPeca()
    {
        string numSlot = Util_VisEdu.GetNumSlot(slot.name);

        if (Global.listaEncaixes.ContainsKey(Consts.CUBO + numSlot))
        {
            return Consts.CUBO + numSlot;
        }
        else if (Global.listaEncaixes.ContainsKey(Consts.POLIGONO + numSlot))
        {
            return Consts.POLIGONO + numSlot;
        }
        else if (Global.listaEncaixes.ContainsKey(Consts.SPLINE + numSlot))
        {
            return Consts.SPLINE + numSlot;
        }

        return string.Empty;
    }

    bool ExisteFormaEncaixado()
    {
        return !string.Empty.Equals(GetPeca());
    }

    void RenomeiaLightObject(GameObject go)
    {
        string numLight = Util_VisEdu.GetNumSlot(slot.name);

        for (int i = 0; i < go.transform.childCount; i++)
        {
            switch (i)
            {
                case 0:
                    go.transform.GetChild(i).name += numLight;
                    break;
                case 1:
                case 2:
                    go.transform.GetChild(i).name += numLight;
                    go.transform.GetChild(i).GetChild(0).name += numLight;
                    break;
                case 3:
                    go.transform.GetChild(i).name += numLight;
                    go.transform.GetChild(i).GetChild(0).name += numLight;
                    go.transform.GetChild(i).GetChild(0).GetChild(0).name += numLight;
                    break;
            }
        }
    }
}
