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
    GameObject tutorial;

    [HideInInspector]
    public GameObject slot;

    Vector3 offset, scanPos, startPos;
    GameObject slotTransfNext;
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
            else
            {
                Encaixa();
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

    public void AddTransformacao(PropriedadeTransformacao propPeca = null, bool tutorial = false)
    {
        Encaixa();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            string numSlot = GetNumSlot();

            GameObject forma = GameObject.Find(Consts.CUBO + numSlot);
            if (forma != null)
            {
                AddGameObjectTree(Consts.CUBO_AMB_OBJ + ((!tutorial) ? numSlot : Consts.TUTORIAL), Consts.AMB, forma.name, tutorial);
                //AddGameObjectTree("CuboVisObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.VIS, "CuboVis" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"));
            }
            else
            {
                forma = GameObject.Find(Consts.POLIGONO + numSlot);
                if (forma != null)
                {
                    AddGameObjectTree(Consts.POLIGONO_AMB_OBJ + ((!tutorial) ? numSlot : Consts.TUTORIAL), Consts.AMB, forma.name, tutorial);
                    //AddGameObjectTree("PoligonoVisObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.VIS, "CuboVis" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"));
                }
                else
                {
                    forma = GameObject.Find(Consts.SPLINE + numSlot);
                    if (forma != null)
                    {
                        AddGameObjectTree(Consts.SPLINE_AMB_OBJ + ((!tutorial) ? numSlot : Consts.TUTORIAL), Consts.AMB, forma.name, tutorial);
                        //AddGameObjectTree("SplineVisObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.VIS, "CuboVis" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"));
                    }
                }
            }


        }

        InstantiateNextSlot();
        CreatePropPeca(propPeca);
    }

    void InstantiateNextSlot()
    {
        int index = slot.transform.parent.transform.GetSiblingIndex();

        slotTransfNext.transform.SetSiblingIndex(index + 1);
    }

    string GetNumSlot()
    {
        return slot.name.Substring(slot.name.IndexOf("Slot") + 4, 1);
    }


    string GetNomeObjeto(string nomeObj)
    {
        if (nomeObj.Contains(Consts.ROTACIONAR)) return Consts.ROTACIONAR;
        else if (nomeObj.Contains(Consts.TRANSLADAR)) return Consts.TRANSLADAR;
        else if (nomeObj.Contains(Consts.ESCALAR)) return Consts.ESCALAR;
        else return string.Empty;
    }

    void AddGameObjectTree(string firstNameObject, string extensionName, string forma, bool tutorial = false)
    {
        string mainGameObject = string.Empty;

        string slot = GetNumSlot();

        if (forma.Contains(Consts.CUBO))
        {
            mainGameObject = Consts.CUBO_AMB + ((!tutorial) ? slot : Consts.TUTORIAL);
        }
        else if (forma.Contains(Consts.POLIGONO))
        {
            mainGameObject = Consts.POLIGONO_AMB + ((!tutorial) ? slot : Consts.TUTORIAL);
        }
        else if (forma.Contains(Consts.SPLINE))
        {
            mainGameObject = Consts.SPLINE_AMB + ((!tutorial) ? slot : Consts.TUTORIAL);
        }

        GameObject goFirst = GameObject.Find(firstNameObject);

        GameObject go = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);

        if (goFirst.transform.GetChild(0).name.Contains(Consts.CUBO_AMB)
            || goFirst.transform.GetChild(0).name.Contains(Consts.POLIGONO_AMB)
            || goFirst.transform.GetChild(0).name.Contains(Consts.SPLINE_AMB))
        {
            go.name = gameObject.name + extensionName;
            go.transform.parent = goFirst.transform;

            GameObject mainGO = GameObject.Find(mainGameObject);

            if (mainGO != null)
            {
                go.transform.localPosition = mainGO.transform.localPosition;
                go.transform.localRotation = mainGO.transform.localRotation;
                go.transform.localScale = mainGO.transform.localScale;
                mainGO.transform.parent = go.transform;
            }
        }
        else
        {
            goFirst.transform.GetChild(0).parent = go.transform;
            go.transform.parent = goFirst.transform;
        }
    }

    public void ConfiguraPropriedadePeca(PropriedadeTransformacao propPeca = null)
    {
        if (EstaEncaixado())
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = propriedades.gameObject;

            CreatePropPeca(propPeca);

            if (Global.gameObjectName.Contains(Consts.ROTACIONAR))
            {
                propriedades.GetComponent<PropRotacionarScript>().Inicializa();
            }
            else if (Global.gameObjectName.Contains(Consts.TRANSLADAR))
            {
                propriedades.GetComponent<PropTransladarScript>().Inicializa();
            }
            else if (Global.gameObjectName.Contains(Consts.ESCALAR))
            {
                propriedades.GetComponent<PropEscalarScript>().Inicializa();
            }
            menuControl.GetComponent<MenuScript>().EnablePanelProp(Global.lastPressedButton.name);
        }
    }

    public void CreatePropPeca(PropriedadeTransformacao propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            PropriedadeTransformacao propTransf;
            string forma = Util_VisEdu.GetPecaByName(gameObject.name);

            if (forma.Contains(Consts.CUBO)
                || forma.Contains(Consts.POLIGONO)
                || forma.Contains(Consts.SPLINE))
            {
                propTransf = new PropriedadeTransformacao()
                {
                    NomePeca = forma
                };
            }
            else if (propPeca == null)
            {
                propTransf = new PropriedadeTransformacao();
            }
            else
            {
                propTransf = propPeca;
            }
            propTransf.Nome = forma + gameObject.name;

            Global.propriedadePecas.Add(propTransf.Nome, propTransf);
        }
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

    public void CopiaPeca()
    {
        if (PodeGerarCopiaPeca() && !EstaEncaixado())
        {
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = GetNomeObjeto(gameObject.name);
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    public bool PodeGerarCopiaPeca()
    {
        return cam.WorldToScreenPoint(scanPos).y > cam.pixelRect.height / 2;
    }
}
