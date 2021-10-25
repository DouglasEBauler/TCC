using System;
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

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject slot;
    GameObject slotTransfNext;
    Tutorial tutorialScript;

    void Start()
    {
        scanPos = startPos = gameObject.transform.position;
        tutorialScript = GameObject.Find("PanelTutorial").GetComponent<Tutorial>();
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
            AddTransformacao();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("TransformacoesSlot"))
        {
            slot = other.gameObject;
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

    public void AddTransformacao(bool tutorial = false)
    {
        EncaixaPecaAoSlot();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            string numSlot = GetNumSlot();

            GameObject forma = GameObject.Find("Cubo" + numSlot);
            if (forma != null)
            {
                AddGameObjectTree("CuboAmbObject" + ((!tutorial) ? numSlot : "Tutorial"), Consts.AMB, forma.name, tutorial);
                //AddGameObjectTree("CuboVisObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.VIS, "CuboVis" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"));
            }
            else
            {
                forma = GameObject.Find("Poligono" + numSlot);
                if (forma != null)
                {
                    AddGameObjectTree("PoligonoAmbObject" + ((!tutorial) ? numSlot : "Tutorial"), Consts.AMB, forma.name, tutorial);
                    //AddGameObjectTree("PoligonoVisObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.VIS, "CuboVis" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"));
                }
                else
                {
                    forma = GameObject.Find("Spline" + numSlot);
                    if (forma != null)
                    {
                        AddGameObjectTree("SplineAmbObject" + ((!tutorial) ? numSlot : "Tutorial"), Consts.AMB, forma.name, tutorial);
                        //AddGameObjectTree("SplineVisObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.VIS, "CuboVis" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"));
                    }
                }
            }


        }

        InstantiateNextSlot();
        CreatePropPeca(null);
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
            mainGameObject = "CuboAmbiente" + ((!tutorial) ? slot : "Tutorial");
        }
        else if (forma.Contains(Consts.POLIGONO))
        {
            mainGameObject = "PoligonoAmbiente" + ((!tutorial) ? slot : "Tutorial");
        }
        else if (forma.Contains(Consts.SPLINE))
        {
            mainGameObject = "SplineAmbiente" + ((!tutorial) ? slot : "Tutorial");
        }

        GameObject goFirst = GameObject.Find(firstNameObject);

        GameObject go = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);

        if (goFirst.transform.GetChild(0).name.Contains("CuboAmbiente")
            || goFirst.transform.GetChild(0).name.Contains("PoligonoAmbiente")
            || goFirst.transform.GetChild(0).name.Contains("SplineAmbiente"))
        {
            go.name = forma + gameObject.name + extensionName;
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
        if (Global.listaEncaixes.ContainsKey(gameObject.name))
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
        if (!Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            PropriedadeTransformacao propTransf;
            string forma = Util_VisEdu.GetPecaByName(gameObject.name);

            if (forma.Contains(Consts.CUBO)
                || forma.Contains(Consts.POLIGONO)
                || forma.Contains(Consts.SPLINE))
            {
                propTransf = new PropriedadeTransformacao()
                {
                    PropPeca = Global.propriedadePecas[forma]
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

            if (Global.propriedadePecas.ContainsKey(propTransf.Nome))
            {
                Global.propriedadePecas[propTransf.Nome] = propTransf;
            }
            else
            {
                Global.propriedadePecas.Add(propTransf.Nome, propTransf);
            }
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
            slotTransfNext = Instantiate(slot.transform.parent.gameObject, render.transform);
            slotTransfNext.name = "SlotTransf"+DropPeca.countObjetosGraficos;

            gameObject.name += DropPeca.countObjetosGraficos.ToString() + "_" + (++DropPeca.countTransformacoes).ToString();
            slot.name += "_" + DropPeca.countTransformacoes.ToString();
            slot.transform.parent.name += "_" + DropPeca.countTransformacoes.ToString();

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
            cloneFab.name = GetNomeObjeto(gameObject.name);
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    public bool PodeGerarCopiaPeca()
    {
        return screenPoint.y > cam.pixelRect.height / 2;
    }
}
