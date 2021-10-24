using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformacaoScript : MonoBehaviour
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

    public static Collider colliderPecas;
    public string concatNumber;

    Vector3 screenPoint, offset, scanPos, startPos;
    GameObject cloneFab;
    float posColliderDestinoX, posColliderDestinoY, posColliderDestinoZ;
    string objName;
    Tutorial tutorialScript;
    private string nomePeca;

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
            AddTransformacoesSlot();
            ConfiguraColliderDestino();
            CreatePropPeca(null);
        }
        else
        {
            AjustaPeca();
        }
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

                    AjustaPecaAoSlot(ref incX, ref incY);
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

        AjustaPecaAoSlot(ref incX, ref incY);

        // A posição x é incrementada para que a peça fique no local correto.
        if (!tutorialScript.EstaExecutandoTutorial)
        {
            posColliderDestinoX = posicaoColliderDestino.transform.position.x + incX;
            posColliderDestinoY = posicaoColliderDestino.transform.position.y - incY;
            posColliderDestinoZ = posicaoColliderDestino.transform.position.z;

            transform.position = new Vector3(posColliderDestinoX, posColliderDestinoY, posColliderDestinoZ);
        }
    }

    public void AddTransformacoesSlot(bool tutorial = false)
    {
        posicaoColliderDestino = GameObject.Find((tutorialScript.EstaExecutandoTutorial) ? "TransformacoesSlot" : "TransformacoesSlot" + (++DropPeca.countTransformacoes));
        GameObject cloneTrans = Instantiate(posicaoColliderDestino, posicaoColliderDestino.transform.position, posicaoColliderDestino.transform.rotation, posicaoColliderDestino.transform.parent);
        cloneTrans.name = "TransformacoesSlot" + ((!tutorial) ? (DropPeca.countObjetosGraficos + "_" + (++DropPeca.countTransformacoes)) : "Tutorial");
        cloneTrans.transform.position = new Vector3(posicaoColliderDestino.transform.position.x, posicaoColliderDestino.transform.position.y - 3f, posicaoColliderDestino.transform.position.z);

        AddTransformacoeSequenciaSlots(cloneTrans.name, tutorial);

        RenderController.ResizeBases(posicaoColliderDestino, Consts.TRANSLADAR, true, tutorial); // o Segundo parâmetro pode ser qualquer tranformação 

        concatNumber = DropPeca.countObjetosGraficos.ToString();

        if (!tutorialScript.EstaExecutandoTutorial)
        {
            string forma = Util_VisEdu.GetPecaByName(gameObject.name);

            if (forma.Contains(Consts.CUBO))
            {
                AddGameObjectTree("CuboAmbObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.AMB);
                //AddGameObjectTree("CuboVisObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.VIS, "CuboVis" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"));
            }
            else if (forma.Contains(Consts.POLIGONO))
            {
                AddGameObjectTree("PoligonoAmbObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.AMB);
                //AddGameObjectTree("CuboVisObject" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"), Consts.VIS, "CuboVis" + ((!tutorial) ? GetNumeroSlotObjetoGrafico() : "Tutorial"));
            }
        }

        ConfiguraIluminacao("-", tutorial);

        ReorganizaObjetos(tutorial);
    }

    void AjustaPecaAoSlot(ref float incX, ref float incY)
    {
        switch (GetNomeObjeto(gameObject.name))
        {
            case Consts.ROTACIONAR: SetRotacionar(ref incX, ref incY); break;
            case Consts.TRANSLADAR: SetTransladar(ref incX, ref incY); break;
            default: incX = 2.95f; incY = 0; break;
        }
    }

    void SetRotacionar(ref float x, ref float y)
    {
        x = 3.8f;
        y = 0;
    }

    void SetTransladar(ref float x, ref float y)
    {
        x = 3.8f;
        y = 0;
    }

    string GetNomeObjeto(string nomeObj)
    {
        if (nomeObj.Contains(Consts.ROTACIONAR)) return Consts.ROTACIONAR;
        else if (nomeObj.Contains(Consts.TRANSLADAR)) return Consts.TRANSLADAR;
        else if (nomeObj.Contains(Consts.ESCALAR)) return Consts.ESCALAR;
        else if (nomeObj.Contains(Consts.ILUMINACAO)) return Consts.ILUMINACAO;
        else if (nomeObj.Contains(Consts.OBJETOGRAFICO)) return Consts.OBJETOGRAFICO;
        else if (nomeObj.Contains(Consts.CUBO)) return Consts.CUBO;
        else if (nomeObj.Contains(Consts.POLIGONO)) return Consts.POLIGONO;
        else if (nomeObj.Contains(Consts.SPLINE)) return Consts.SPLINE;
        else if (nomeObj.Contains(Consts.CAMERA)) return Consts.CAMERA;
        else return string.Empty;
    }

    void ConfiguraIluminacao(string sinal, bool tutorial = false)
    {
        float valorInc = 0;
        string iluminacao = "Iluminacao";

        valorInc = sinal.Equals("+") ? 3f : -3f;

        GameObject goObj = GameObject.Find("ObjGraficoSlot" + ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial"));

        foreach (Transform child in goObj.transform)
        {
            if (child.name.Contains("IluminacaoSlot"))
            {
                foreach (var peca in Global.listaEncaixes)
                {
                    if (child.name.Equals(peca.Value)) iluminacao = peca.Key;
                }
            }
        }

        GameObject obj_iluminacao = GameObject.Find("IluminacaoSlot" + ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial"));
        Vector3 pos = obj_iluminacao.transform.position;
        pos.y += valorInc;
        obj_iluminacao.transform.position = pos;

        // Se a peça "Iluminação já foi selecionada, será devidamente reposicionada"        
        GameObject IlumPeca = GameObject.Find(iluminacao);

        if (!tutorialScript.EstaExecutandoTutorial && (Global.listaObjetos.Contains(IlumPeca)))
        {
            IlumPeca.transform.position = new Vector3(IlumPeca.transform.position.x, pos.y, IlumPeca.transform.position.z);
        }
    }

    // firstNameObject = primeiro GameObject pai
    // extensionName   = concatena com o nome do GameObject
    // mainGameObject  = nome do GameObject principal
    void AddGameObjectTree(string firstNameObject, string extensionName, bool tutorial = false)
    {
        string mainGameObject = string.Empty;

        string forma = Util_VisEdu.GetPecaByName(gameObject.name);
        string slot = Util_VisEdu.GetSlot(gameObject.name);

        if (forma.Contains(Consts.CUBO))
        {
            mainGameObject = "CuboAmbiente" + ((!tutorial) ? slot : "Tutorial");
        }
        else if (forma.Contains(Consts.POLIGONO))
        {
            mainGameObject = "PoligonoAmbiente" + ((!tutorial) ? slot : "Tutorial");
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

        //AtualizaTrasformGameObjectAmb(gameObject.name);
    }

    public void ConfiguraPropriedadePeca(PropriedadeTransformacao propPeca = null)
    {
        if (!Input.GetMouseButtonUp(0) && Global.listaObjetos != null && Global.listaObjetos.Contains(gameObject))
        {
            Global.gameObjectName = gameObject.name;
            Global.lastPressedButton?.SetActive(false);
            Global.lastPressedButton = abrePropriedade.gameObject;

            CreatePropPeca(propPeca);

            if (Global.gameObjectName.Contains(Consts.ROTACIONAR))
            {
                abrePropriedade.GetComponent<PropRotacionarScript>().Inicializa();
            }
            else if (Global.gameObjectName.Contains(Consts.TRANSLADAR))
            {
                abrePropriedade.GetComponent<PropTransladarScript>().Inicializa();
            }
            else if (Global.gameObjectName.Contains(Consts.ESCALAR))
            {
                abrePropriedade.GetComponent<PropEscalarScript>().Inicializa();
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

        foreach (var slot in Global.listaPosicaoSlot)
        {
            //Verifica se o encaixe existe na lista 
            if (slot.Key.Contains(Global.GetSlot(gameObject.name)))
            {
                //Verifica se a peça está próxima do encaixe e se o Slot ainda não está na lista de encaixes.
                if ((slot.Value + VALOR_APROXIMADO > pecaY) && (slot.Value - VALOR_APROXIMADO < pecaY) && !Global.listaEncaixes.ContainsValue(slot.Key))
                {
                    if (!Global.listaEncaixes.ContainsKey(gameObject.transform.name) && (GameObject.Find(slot.Key) != null))
                    {
                        Global.listaEncaixes.Add(gameObject.transform.name, slot.Key);
                    }
                    else if (Global.listaEncaixes[gameObject.name] != slot.Key)
                    {
                        ReposicionaSlots(Global.listaEncaixes[gameObject.name], slot.Key);
                        return false;
                    }
                    else
                        return false;

                    return GameObject.Find(slot.Key) != null;
                }
            }
        }

        return false;
    }

    public void ReposicionaSlots(string slotOrigem, string slotDestino)
    {
        //Permite reposicionar o slot somente se for um 'TransformacoesSlot' e os slots estiverem dentro do mesmo Objeto Gráfico
        if (slotOrigem.Contains("TransformacoesSlot") && getNumObjeto(slotOrigem).Equals(getNumObjeto(slotDestino)))
        {
            float incX = 0;
            float incY = 0;

            AjustaPecaAoSlot(ref incX, ref incY);

            GameObject slot = GameObject.Find(slotDestino);

            transform.position = new Vector3(slot.transform.position.x + incX, slot.transform.position.y - incY, slot.transform.position.z);

            Destroy(GameObject.Find(slotOrigem));

            GameObject t = GameObject.Find(slotDestino);

            int val = 0;
            string countTransformacoes = string.Empty;
            int.TryParse(slotDestino.Substring(slotDestino.IndexOf("_") + 1), out val);

            countTransformacoes = (val > 0) ? (val + 1).ToString() : "1";

            GameObject cloneTrans = Instantiate(t, t.transform.position, t.transform.rotation, t.transform.parent);
            cloneTrans.name = "TransformacoesSlot" + GetNumeroSlotObjetoGrafico() + "_" + countTransformacoes;
            cloneTrans.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - 3f, t.transform.position.z);

            AddTransformacoeSequenciaSlots(cloneTrans.name);

            posicaoColliderDestino = t;

            bool podeReposicionar = false;

            foreach (string _slot in Global.listaSequenciaSlots)
            {
                if (_slot.Contains("TransformacoesSlot" + GetNumeroSlotObjetoGrafico()) && getNumObjeto(_slot).Equals(GetNumeroSlotObjetoGrafico()))
                {
                    if (_slot.Equals(slotOrigem))
                    {
                        podeReposicionar = true;
                        continue;
                    }

                    if (podeReposicionar)
                    {
                        GameObject GO_Slot = GameObject.Find(_slot);
                        GameObject GO_Peca;

                        foreach (var pair in Global.listaEncaixes)
                        {
                            if (pair.Value.Equals(_slot))
                            {
                                GO_Peca = GameObject.Find(pair.Key);
                                GO_Peca.transform.position = new Vector3(GO_Peca.transform.position.x, GO_Peca.transform.position.y + 3f, GO_Peca.transform.position.z);
                                break;
                            }
                        }

                        GO_Slot.transform.position = new Vector3(GO_Slot.transform.position.x, GO_Slot.transform.position.y + 3f, GO_Slot.transform.position.z);
                    }
                }
            }

            Global.listaSequenciaSlots.Remove(slotOrigem);
            Global.listaEncaixes.Remove(gameObject.name);
            Global.listaEncaixes.Add(gameObject.name, slotDestino);

            ReposicionaPosicaoAmbCuboVis();

            AtualizaTrasformGameObjectAmb(gameObject.name);
            //AtualizaTrasformGameObjectAmb(gameObject.name);
        }
    }

    void AddTransformacoeSequenciaSlots(string slot, bool tutorial = false)
    {
        string numObj = GetNumeroSlotObjetoGrafico();
        bool encontrouTransf = false;
        bool isTransf = false;

        for (int i = 0; i < Global.listaSequenciaSlots.Count; i++)
        {
            isTransf = Global.listaSequenciaSlots[i].Contains("TransformacoesSlot" + ((!tutorial) ? numObj : "Tutorial"));

            if (Global.listaSequenciaSlots[i].Contains("TransformacoesSlot" + ((!tutorial) ? numObj : "Tutorial")))
            {
                encontrouTransf = true;
                continue;
            }
            else if (encontrouTransf && !isTransf)
            {
                Global.listaSequenciaSlots.Insert(i, slot);
                break;
            }
        }
    }

    public void GeraCopiaPeca()
    {
        if (PodeGerarCopiaPeca())
        {
            objName = GetNomeObjeto(gameObject.name);

            cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = objName + DropPeca.countTransformacoes.ToString();
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    public bool PodeGerarCopiaPeca()
    {
        return screenPoint.y > cam.pixelRect.height / 2;
    }

    public string GetNumeroSlotObjetoGrafico()
    {
        GameObject render = GameObject.Find("Render");
        float value = 0f;
        string numSlotObjGrafico = string.Empty;

        foreach (Transform child in render.transform)
        {
            if (child.name.Contains("ObjGraficoSlot")
                && Global.listaPosicaoSlot.ContainsKey(child.name)
                && Global.listaPosicaoSlot.TryGetValue(child.name, out value)
                && (transform.position.y < value)
                && (child.name.Length > "ObjGraficoSlot".Length))
            {
                numSlotObjGrafico = child.name.Substring(child.name.IndexOf("Slot") + 4, 1);
            }
        }

        return numSlotObjGrafico;
    }

    void ReorganizaObjetos(bool tutorial = false)
    {
        GameObject goRender = GameObject.Find("Render");

        foreach (Transform child in goRender.transform)
        {
            if (child.name.Equals("ObjGraficoSlot" + ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial")))
            {
                Vector3 pos = child.position;
                pos.y -= 3;
                child.position = pos;
            }
        }
    }

    public string getNumObjeto(string objeto)
    {
        int val = 0;
        string numObj = string.Empty;

        if (objeto.Length > objeto.Substring(0, objeto.IndexOf("Slot") + 4).Length)
            int.TryParse(objeto.Substring(objeto.IndexOf("Slot") + 4, 1), out val);

        if (val > 0)
            numObj = val.ToString();

        return numObj;
    }

    void ReposicionaPosicaoAmbCuboVis()
    {
        GameObject goAmb = GameObject.Find("GameObjectAmb" + GetNumeroSlotObjetoGrafico());
        GameObject goTroca = GameObject.Find(gameObject.name + Consts.AMB);

        if (!Equals(goAmb.transform.GetChild(0).name, goTroca.name))
        {
            goTroca.transform.GetChild(0).parent = goTroca.transform.parent;
            goTroca.transform.parent = goAmb.transform;

            goAmb.transform.GetChild(0).parent = goTroca.transform;
        }

    }

    public void AtualizaTrasformGameObjectAmb(string GOname)
    {
        string nomeAmb = Util_VisEdu.GetForma(gameObject.name) + "AmbObject";

        if (!tutorialScript.EstaExecutandoTutorial)
            nomeAmb += getNumObjeto(GameObject.Find(Global.listaEncaixes[GOname]).transform.parent.name);

        int cont = 0;
        int breakLoop = 50;

        Transform GoAmb = GameObject.Find(nomeAmb).transform;

        while (cont < breakLoop)
        {
            if (GoAmb.childCount > 0)
                GoAmb = GoAmb.GetChild(0);

            if (GoAmb.name.Contains("CuboAmbiente")
                || GoAmb.name.Contains("PoligonoAmbiente")
                || GoAmb.name.Contains("SplineAmbiente"))
            {
                Vector3 pos = Vector3.zero;
                Vector3 tam = new Vector3(1, 1, 1);
                Tamanho tamanho;
                Posicao posicao;

                if (Global.propriedadePecas.Count > 0)
                {
                    // Verifica posição e tamanho do cubo.
                    foreach (var enc in Global.listaEncaixes)
                    {
                        if (enc.Value.Equals(Global.listaEncaixes[Util_VisEdu.GetPecaByName(gameObject.name)])
                            && (Global.propriedadePecas.ContainsKey(enc.Key)))
                        {
                            tamanho = Global.propriedadePecas[enc.Key].Tam;
                            posicao = Global.propriedadePecas[enc.Key].Pos;
                            tam = new Vector3(tamanho.X, tamanho.Y, tamanho.Z);
                            pos = new Vector3(posicao.X * -1, posicao.Y, posicao.Z);
                            break;
                        }
                    }
                }

                GoAmb.localPosition = pos;
                GoAmb.localRotation = Quaternion.Euler(0, 0, 0);
                GoAmb.localScale = tam;
                break;
            }

            // Verifica se o GO foi excluído então pega o próximo.
            if (0.Equals(GoAmb.childCount)
                && !GoAmb.name.Contains("CuboAmbiente")
                && (GoAmb.parent.childCount > 1))
            {
                GoAmb = GoAmb.parent.GetChild(1);
            }

            if (GoAmb.name.Contains(Consts.TRANSLADAR))
            {
                if (!Global.propriedadePecas.ContainsKey(GOname))
                    GoAmb.localPosition = Vector3.zero;

                GoAmb.localRotation = Quaternion.Euler(0, 0, 0);
                GoAmb.localScale = new Vector3(1, 1, 1);
            }
            else if (GoAmb.name.Contains(Consts.ROTACIONAR))
            {
                if (!Global.propriedadePecas.ContainsKey(GOname))
                    GoAmb.localRotation = Quaternion.Euler(0, 0, 0);

                GoAmb.localPosition = Vector3.zero;
                GoAmb.localScale = new Vector3(1, 1, 1);
            }
            else if (GoAmb.name.Contains(Consts.ESCALAR))
            {
                if (!Global.propriedadePecas.ContainsKey(GOname))
                    GoAmb.localScale = new Vector3(1, 1, 1);

                GoAmb.localPosition = Vector3.zero;
                GoAmb.localRotation = Quaternion.Euler(0, 0, 0);
            }

            cont++;
        }
    }
}
