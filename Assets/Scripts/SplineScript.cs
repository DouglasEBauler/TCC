using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineScript : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject posicaoColliderDestino;
    [SerializeField]
    GameObject abrePropriedade;
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

    public static Collider colliderPecas;

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject cloneFab;
    float posColliderDestinoX, posColliderDestinoY, posColliderDestinoZ;
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
        Global.AtualizaListaSlot();

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
            AddSplineTransform();
            ConfiguraColliderDestino();
            CreatePropPeca(null);
        }
        else
        {
            AjustaPeca();
        }
    }

    void AddSplineTransform()
    {
        string numSlotObjGraf = Util_VisEdu.GetNumeroSlotObjetoGrafico(gameObject);

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            posicaoColliderDestino = GameObject.Find("FormasSlot" + (++DropPeca.countFormas));

            if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
                GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

            string pecaObjGrafSlot = GetPecaObjGrafSlot();

            GameObject splineAmb = RenameSplineAmb(numSlotObjGraf);
            if (splineAmb != null)
            {
                MeshRenderer mr = splineAmb.GetComponent<MeshRenderer>();
                if (mr != null && !string.Empty.Equals(pecaObjGrafSlot))
                {
                    mr.enabled = Global.propriedadePecas[pecaObjGrafSlot].Ativo;
                }
            }
        }
        else
        {
            GameObject.Find("SplineMesh" + numSlotObjGraf).GetComponent<MeshRenderer>().enabled = true;
        }
    }

    string GetPecaObjGrafSlot()
    {
        // Verificar se o Objeto Gráfico pai está ativo para demonstrar o cubo.
        string goObjGraficoSlot = GameObject.Find(Global.listaEncaixes[gameObject.name]).transform.parent.name;

        // Verifica peça conectada ao slot
        foreach (var pecas in Global.listaEncaixes)
        {
            if (pecas.Value.Equals(goObjGraficoSlot))
            {
                return pecas.Key;
            }
        }

        return string.Empty;
    }

    GameObject RenameSplineAmb(string numSlotObjGraf)
    {
        GameObject splineAmb = GameObject.Find("SplineAmbiente" + numSlotObjGraf);
        if (splineAmb != null)
        {
            splineAmb.name += DropPeca.countFormas;

            // GameObject gerado pelo proprio componente SplineMesh
            // nao sendo possivel alterar seu nome, apenas o seu objeto pai pode ser redefinido
            return GameObject.Find("segment 1 mesh");
        }

        return null;
    }

    void AjustaPeca()
    {
        if (!tutorialScript.EstaExecutandoTutorial && !CheckPanelIsActive())
        {
            bool podeDestruir = screenPoint.y > cam.pixelRect.height / 2;

            if (podeDestruir && !Global.listaEncaixes.ContainsKey(gameObject.name))
            {
                transform.position = startPos;
                Destroy(cloneFab);
                screenPoint = cam.WorldToScreenPoint(scanPos);
            }
            else
            {
                if (!Global.listaEncaixes.ContainsKey(gameObject.name))
                    transform.position = new Vector3(posColliderDestinoX, posColliderDestinoY, posColliderDestinoZ);

                GameObject newPosition = GameObject.Find(Global.listaEncaixes[gameObject.name]);

                if (newPosition != null)
                {
                    float incX = 0;
                    float incY = 0;

                    SetSpline(ref incX, ref incY);
                    transform.position = new Vector3(newPosition.transform.position.x + incX, newPosition.transform.position.y - incY, newPosition.transform.position.z);
                }
            }
        }
    }

    bool CheckPanelIsActive()
    {
        return panelArquivo.activeSelf || panelPropPeca.activeSelf || panelAjuda.activeSelf;
    }

    public void ConfiguraColliderDestino()
    {
        float incX = 0;
        float incY = 0;

        if (!tutorialScript.EstaExecutandoTutorial)
            Global.AddObject(gameObject);

        SetSpline(ref incX, ref incY);

        // A posição x é incrementada para que a peça fique no local correto.
        if (!tutorialScript.EstaExecutandoTutorial)
        {
            posColliderDestinoX = posicaoColliderDestino.transform.position.x + incX;
            posColliderDestinoY = posicaoColliderDestino.transform.position.y - incY;
            posColliderDestinoZ = posicaoColliderDestino.transform.position.z;

            transform.position = new Vector3(posColliderDestinoX, posColliderDestinoY, posColliderDestinoZ);
        }
    }

    void SetSpline(ref float x, ref float y)
    {
        x = 2.33f;
    }

    public void ConfiguraPropriedadePeca(SplinePropriedadePeca propPeca = null, PropriedadeCamera camProp = null)
    {
        if (!Input.GetMouseButtonUp(0) && Global.listaObjetos != null && Global.listaObjetos.Contains(gameObject))
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = abrePropriedade.gameObject;

            CreatePropPeca(propPeca);

            abrePropriedade.GetComponent<PropSplineScript>().Inicializa();
            menuControl.GetComponent<MenuScript>().EnablePanelProp(Global.lastPressedButton.name);
        }
    }

    public void CreatePropPeca(SplinePropriedadePeca propPeca = null)
    {
        if (!Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            SplinePropriedadePeca prPeca;

            if (propPeca != null)
            {
                prPeca = propPeca;
            }
            else
            {
                prPeca = new SplinePropriedadePeca()
                {
                    SplineAmbiente = "SplineAmbiente" + Util_VisEdu.GetSlot(gameObject.name)
                };
                prPeca.Nome = gameObject.name;
            }

            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
        }
    }

    public bool PodeEncaixar()
    {
        const float VALOR_APROXIMADO = 2;

        float pecaY = transform.position.y;

        foreach (var slot in Global.listaPosicaoSlot)
        {
            if (Global.listaPosicaoSlot.ContainsKey(slot.Key)
                && slot.Key.Contains("FormasSlot")
                && (slot.Value + VALOR_APROXIMADO > pecaY) && (slot.Value - VALOR_APROXIMADO < pecaY))
            {
                if (!Global.listaEncaixes.ContainsKey(gameObject.transform.name))
                {
                    Global.listaEncaixes.Add(gameObject.transform.name, slot.Key);
                }
                else
                {
                    Global.listaEncaixes[gameObject.transform.name] = slot.Key;
                    return false;
                }

                return true;
            }
        }

        return false;
    }

    public void GeraCopiaPeca()
    {
        if (PodeGerarCopiaPeca())
        {
            cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = Consts.SPLINE + DropPeca.countFormas.ToString();
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    bool PodeGerarCopiaPeca()
    {
        return screenPoint.y > cam.pixelRect.height / 2;
    }
}
