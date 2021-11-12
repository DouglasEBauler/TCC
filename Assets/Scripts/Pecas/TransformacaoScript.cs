using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransformacaoScript : MonoBehaviour
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
    GameObject render;
    [SerializeField]
    GameObject posAmb;
    [SerializeField]
    GameObject posVis;
    [SerializeField]
    GameObject tutorial;

    [HideInInspector]
    public GameObject slot;

    Vector3 offset, scanPos, startPos;
    GameObject slotTransfNext;
    Tutorial tutorialScript;
    TransformacaoPropriedadePeca propTransformacao;

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
            AddTransformacao();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(Consts.TRANSF_SLOT) && !EstaEncaixado())
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
            bool podeDestruir = PodeGerarCopiaPeca();

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

    public void AddTransformacao(TransformacaoPropriedadePeca propPeca = null, bool tutorial = false)
    {
        Encaixa();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            AddGameObjectTree(tutorial);
        }

        InstantiateNextSlot();
        CreatePropPeca(propPeca);
    }

    void InstantiateNextSlot()
    {
        int index = slot.transform.parent.transform.GetSiblingIndex();

        slotTransfNext.transform.SetSiblingIndex(index + 1);
    }

    string GetNomeObjeto()
    {
        if (gameObject.name.Contains(Consts.ROTACIONAR)) return Consts.ROTACIONAR;
        else if (gameObject.name.Contains(Consts.TRANSLADAR)) return Consts.TRANSLADAR;
        else if (gameObject.name.Contains(Consts.ESCALAR)) return Consts.ESCALAR;
        else return string.Empty;
    }

    void AddGameObjectTree(bool tutorial = false)
    {
        string numFormaSlot = Util_VisEdu.GetNumSlot(slot.name);
        GameObject transformacaoAmbVis, pecaAmbVis;

        pecaAmbVis = GameObject.Find(Consts.CUBO_AMB + numFormaSlot);
        if (pecaAmbVis != null)
        {
            transformacaoAmbVis = Instantiate(new GameObject(), pecaAmbVis.transform.parent);
            transformacaoAmbVis.name = gameObject.name + Consts.AMB;
            pecaAmbVis.transform.parent = transformacaoAmbVis.transform;
        }
        else
        {
            pecaAmbVis = GameObject.Find(Consts.POLIGONO_AMB + numFormaSlot);
            if (pecaAmbVis != null)
            {
                transformacaoAmbVis = Instantiate(new GameObject(), pecaAmbVis.transform.parent);
                transformacaoAmbVis.name = gameObject.name + Consts.AMB;
                pecaAmbVis.transform.parent = transformacaoAmbVis.transform;
            }
            else
            {
                pecaAmbVis = GameObject.Find(Consts.SPLINE_AMB + numFormaSlot);
                if (pecaAmbVis != null)
                {
                    transformacaoAmbVis = Instantiate(new GameObject(), pecaAmbVis.transform.parent);
                    transformacaoAmbVis.name = gameObject.name + Consts.AMB;
                    pecaAmbVis.transform.parent = transformacaoAmbVis.transform;
                }
            }
        }

        pecaAmbVis = GameObject.Find(Consts.CUBO_VIS + numFormaSlot);
        if (pecaAmbVis != null)
        {
            transformacaoAmbVis = Instantiate(new GameObject(), pecaAmbVis.transform.parent);
            transformacaoAmbVis.name = gameObject.name + Consts.VIS;
            pecaAmbVis.transform.parent = transformacaoAmbVis.transform;
        }
        else
        {
            pecaAmbVis = GameObject.Find(Consts.POLIGONO_VIS + numFormaSlot);
            if (pecaAmbVis != null)
            {
                transformacaoAmbVis = Instantiate(new GameObject(), pecaAmbVis.transform.parent);
                transformacaoAmbVis.name = gameObject.name + Consts.VIS;
                pecaAmbVis.transform.parent = transformacaoAmbVis.transform;
            }
            else
            {
                pecaAmbVis = GameObject.Find(Consts.SPLINE_VIS + numFormaSlot);
                if (pecaAmbVis != null)
                {
                    transformacaoAmbVis = Instantiate(new GameObject(), pecaAmbVis.transform.parent);
                    transformacaoAmbVis.name = gameObject.name + Consts.VIS;
                    pecaAmbVis.transform.parent = transformacaoAmbVis.transform;
                }
            }
        }
    }

    public void ConfiguraPropriedadePeca(TransformacaoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado())
        {
            CreatePropPeca(propPeca);

            if (this.propTransformacao.NomePeca.Contains(Consts.ROTACIONAR))
            {
                propriedades.GetComponent<PropRotacionarScript>().Inicializa(this.propTransformacao);
            }
            else if (this.propTransformacao.NomePeca.Contains(Consts.TRANSLADAR))
            {
                propriedades.GetComponent<PropTransladarScript>().Inicializa(this.propTransformacao);
            }
            else if (this.propTransformacao.NomePeca.Contains(Consts.ESCALAR))
            {
                propriedades.GetComponent<PropEscalarScript>().Inicializa(this.propTransformacao);
            }
            menuControl.GetComponent<MenuScript>().EnablePanelProp(propriedades.name);
        }
    }

    public void CreatePropPeca(TransformacaoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            if (propPeca == null)
            {
                this.propTransformacao = new TransformacaoPropriedadePeca()
                {
                    NomePeca = GetPeca(),
                    NomePecaAmb = gameObject.name + Consts.AMB,
                    NomePecaVis = gameObject.name + Consts.VIS
                };
            }
            else
            {
                this.propTransformacao = propPeca;
            }
            this.propTransformacao.Nome = GetNomeObjeto();
            this.propTransformacao.NomePeca = gameObject.name;

            Global.propriedadePecas.Add(this.propTransformacao.NomePeca, propTransformacao);
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

    public bool PodeEncaixar()
    {
        if ((slot != null) 
            && (Vector3.Distance(slot.transform.position, gameObject.transform.position) < 4)
            && ExisteFormaEncaixado()
            && !EstaEncaixado())
        {
            string numSlotGraf = Util_VisEdu.GetNumSlot(slot.name);

            slotTransfNext = Instantiate(slot.transform.parent.gameObject, render.transform);
            slotTransfNext.name = Consts.SLOT_TRANSF + numSlotGraf;

            Destroy(slot.GetComponent<Rigidbody>());
            gameObject.name += numSlotGraf + "_" + (++Global.countTransformacoes).ToString();
            slot.name += "_" + numSlotGraf;
            slot.transform.parent.name += "_" + Global.countTransformacoes.ToString();
            GameObject.Find(Consts.ITERACAO_SLOT).name += numSlotGraf + "_" + Global.countTransformacoes.ToString();

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

    bool ExisteFormaEncaixado()
    {
        return !string.Empty.Equals(GetPeca());
    }

    public void CopiaPeca()
    {
        if (PodeGerarCopiaPeca() && !EstaEncaixado())
        {
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = GetNomeObjeto();
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    public bool PodeGerarCopiaPeca()
    {
        return cam.WorldToScreenPoint(scanPos).y > cam.pixelRect.height / 2;
    }
}
