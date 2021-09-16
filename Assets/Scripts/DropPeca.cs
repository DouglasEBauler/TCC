using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPeca : MonoBehaviour
{
    enum Slots { CameraSlot, ObjGrafSlot, FormasSlot, TransformacoesSlot, IluminacaoSlot };

    const string AMB = "Amb";
    const string VIS = "Vis";

    public static int countTransformacoes;
    public static int countObjetosGraficos;
    public static int countFormas;

    [SerializeField]
    GameObject tutorial;
    [SerializeField]
    GameObject render;
    [SerializeField]
    GameObject propCamera;
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    GameObject fabricaPecas;

    GameObject objDrop;
    Controller controller;

    void Update()
    {
        if (Input.GetMouseButtonUp(0)
            && objDrop != null
            && controller != null)
        {
            ProcessaExclusao();
            objDrop = null;
            controller = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        objDrop = other.gameObject;
        controller = objDrop.GetComponent<Controller>();
    }

    void ProcessaExclusao()
    {
        if (controller != null)
        {
            string slotOrigem = Global.listaEncaixes[objDrop.name];

            bool podeReposicionar = false;

            if (Global.GetSlot(objDrop.name).Contains("TransformacoesSlot"))
            {
                GameObject objGrafico = GameObject.Find(Global.listaEncaixes[objDrop.name]).transform.parent.gameObject;

                foreach (Transform child in render.transform)
                {
                    if (podeReposicionar && child.name.Contains("ObjGraficoSlot"))
                    {
                        child.position = new Vector3(child.position.x, child.position.y + 3f, child.position.z);

                        foreach (var pair in Global.listaEncaixes)
                        {
                            if (child.name.Equals(pair.Value))
                            {
                                GameObject GO_Peca = GameObject.Find(pair.Key);
                                GO_Peca.transform.position = new Vector3(GO_Peca.transform.position.x, GO_Peca.transform.position.y + 3f, GO_Peca.transform.position.z);

                                foreach (Transform _child in child.transform)
                                {
                                    GameObject GO_PecaAux;
                                    foreach (var peca in Global.listaEncaixes)
                                    {
                                        if (_child.name.Equals(peca.Value))
                                        {
                                            GO_PecaAux = GameObject.Find(peca.Key);
                                            GO_PecaAux.transform.position = new Vector3(GO_PecaAux.transform.position.x, GO_PecaAux.transform.position.y + 3f, GO_PecaAux.transform.position.z);
                                            break;
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }

                    if (!child.name.Equals(objGrafico.transform.name)) continue;

                    foreach (Transform _child in objGrafico.transform)
                    {
                        if (!_child.name.Equals(slotOrigem)) continue;

                        GameObject GO_Slot = GameObject.Find(_child.name);
                        GameObject GO_Peca;

                        foreach (var pair in Global.listaEncaixes)
                        {
                            if (_child.name.Equals(pair.Value))
                            {
                                GO_Peca = GameObject.Find(pair.Key);
                                GO_Peca.transform.position = new Vector3(GO_Peca.transform.position.x, GO_Peca.transform.position.y + 3f, GO_Peca.transform.position.z);
                                break;
                            }
                        }

                        if (!GO_Slot.name.Contains("Base") && !GO_Slot.name.Contains("IluminacaoSlot"))
                        {
                            GO_Slot.transform.position = new Vector3(GO_Slot.transform.position.x, GO_Slot.transform.position.y + 3f, GO_Slot.transform.position.z);
                        }
                    }

                }

                RenderController.ResizeBases(GameObject.Find(slotOrigem), Consts.TRANSLADAR, false);

                ExcluiHierarquiaOuAlteraPropriedade(Slots.TransformacoesSlot);

                Destroy(GameObject.Find(Global.listaEncaixes[objDrop.name]));
                Destroy(GameObject.Find(objDrop.name));
                Global.listaSequenciaSlots.Remove(Global.listaEncaixes[objDrop.name]);
                Global.removeObject(objDrop);
                AtualizaTrasformGameObjectAmb(objDrop.name);
                Global.listaEncaixes.Remove(objDrop.name);
                Global.propriedadePecas.Remove(objDrop.name);

                configuraIluminacao("+");
            }
            else if (Global.GetSlot(objDrop.name).Contains("ObjGraficoSlot"))
            {
                ExcluiHierarquiaOuAlteraPropriedade(Slots.ObjGrafSlot);
                ExclusaoCompletaObjetos(Slots.ObjGrafSlot);

                if (!new PropIluminacaoPadrao().existeIluminacao())
                    GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Nothing");
            }
            else
            {
                Destroy(objDrop);
                Global.listaSequenciaSlots.Remove(Global.listaEncaixes[objDrop.name]);
                Global.removeObject(objDrop);

                if (Global.GetSlot(objDrop.name).Contains("FormasSlot"))
                {
                    ExcluiHierarquiaOuAlteraPropriedade(Slots.FormasSlot);
                    Global.listaEncaixes.Remove(objDrop.name);
                    Global.propriedadePecas.Remove(objDrop.name);
                }
                else if (Global.GetSlot(objDrop.name).Contains("IluminacaoSlot"))
                {
                    ExcluiHierarquiaOuAlteraPropriedade(Slots.IluminacaoSlot);

                    Global.listaEncaixes.Remove(objDrop.name);

                    if (!new PropIluminacaoPadrao().existeIluminacao())
                        GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Nothing");

                    Global.propriedadeIluminacao.Remove(objDrop.name);

                    if (objDrop.name.Length > "Iluminacao".Length)
                        Destroy(GameObject.Find("LightObjects" + objDrop.name));
                    else
                    {
                        if (Global.propriedadePecas.ContainsKey(objDrop.name))
                        {
                            PropIluminacaoPadrao luz = new PropIluminacaoPadrao();
                            luz.AtivaIluminacao(luz.GetTipoLuzPorExtenso(Global.propriedadePecas[objDrop.name].TipoLuz) + objDrop.name, false);
                        }
                    }

                    Global.propriedadePecas.Remove(objDrop.name);
                }
                else if (Global.GetSlot(objDrop.name).Contains("CameraSlot"))
                {
                    Global.propCameraGlobal.ExisteCamera = false; // seta 'false' para que não atualize as propriedades.

                    propCamera.GetComponent<PropCameraScript>().DemosntraCamera(false);

                    GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Nothing");

                    Global.listaEncaixes.Remove(objDrop.name);
                    Global.propriedadePecas.Remove(objDrop.name);
                }
            }
            menuControl.GetComponent<MenuScript>().EnablePanelFabPecas();
            ReactiveColliderPeca();
        }
    }

    void ExclusaoCompletaObjetos(Slots slot)
    {
        switch (slot)
        {
            case Slots.ObjGrafSlot:
                List<string> listaNomeObjGrafico = new List<string>();
                ArrayList listaPecas = new ArrayList();
                GameObject fabricaPecas = GameObject.Find("FabricaPecas");
                Vector3 posGO = Vector3.zero;
                Vector3 posPecas = Vector3.zero;
                GameObject go = null;
                GameObject goPecas = null;

                // Adiciona todos ObjGraficoSlots em uma lista 
                foreach (Transform child in render.transform)
                {
                    if (child.name.Contains("ObjGraficoSlot"))
                        listaNomeObjGrafico.Add(child.name);
                }

                for (int i = 0; i < listaNomeObjGrafico.Count; i++)
                {
                    // Verifica os objetos graficos da lista até chegar o objeto selecionado
                    if (!listaNomeObjGrafico[i].Equals(Global.listaEncaixes[objDrop.name]))
                        continue;

                    // Pega a posição do objeto após o objeto a ser excluído
                    posGO = GameObject.Find(listaNomeObjGrafico[i + 1]).transform.position;

                    // Instancia um GameObject vazio tendo Render como pai
                    go = Instantiate(new GameObject(), render.transform);
                    go.name = "GO_ObjetosGraficos";

                    goPecas = Instantiate(new GameObject(), fabricaPecas.transform);
                    goPecas.name = "GO_Pecas";

                    // Joga a posição do objeto a ser excluído para o GameObject vazio
                    go.transform.position = posGO;

                    goPecas.transform.position = posGO;

                    // Pega a posição do objeto a ser excluído
                    posGO = GameObject.Find(listaNomeObjGrafico[i]).transform.position;

                    // Remove peças dos slos
                    string value = string.Empty;
                    GameObject objPecaSlot = GameObject.Find(listaNomeObjGrafico[i]);
                    foreach (Transform child in objPecaSlot.transform)
                    {
                        foreach (var pair in Global.listaEncaixes)
                        {
                            if (child.name.Equals(pair.Value) && !child.name.Contains("ObjGraficoSlot"))
                            {
                                Destroy(GameObject.Find(Global.listaEncaixes[pair.Key]));
                                Destroy(GameObject.Find(pair.Key));
                                Global.listaSequenciaSlots.Remove(Global.listaEncaixes[pair.Key]);
                                Global.removeObject(GameObject.Find(pair.Key));
                                Global.listaEncaixes.Remove(pair.Key);

                                DestroyIluminacao(pair.Key);
                                break;
                            }
                        }
                    }

                    // Incrementa 'i' para pegar a partir do próximo objeto da lista
                    i++;

                    foreach (string nome in listaNomeObjGrafico)
                    {
                        // Instancia os próximos objetos da lista e coloca o GameObjet vazio como pai deles
                        GameObject goEmpty = GameObject.Find(nome);
                        goEmpty.transform.parent = go.transform;

                        foreach (var pair in Global.listaEncaixes)
                        {
                            if (getNumObjeto(pair.Value).Equals(getNumObjeto(nome)))
                                listaPecas.Add(pair.Key);
                        }
                    }
                    break;
                }
                // Atualiza a posição do GameObject com todos objetos para a posição do objeto excluído.
                go.transform.position = posGO;

                foreach (string peca in listaPecas)
                {
                    GameObject goEmpty = GameObject.Find(peca);
                    goEmpty.transform.parent = goPecas.transform;
                }

                // Atualiza a posição do GameObject com todos objetos para a posição do objeto excluído.
                goPecas.transform.position = posGO;

                //retira objetos dos GameObjects vazios
                GameObject GO_ObjGraf = GameObject.Find("GO_ObjetosGraficos");
                GameObject GO_Pecas = GameObject.Find("GO_Pecas");

                List<GameObject> listaGO_ObjGraf = new List<GameObject>();
                List<GameObject> listaGO_Pecas = new List<GameObject>();

                // Criada lista de objetos para troca de pais
                foreach (Transform child in GO_ObjGraf.transform)
                    listaGO_ObjGraf.Add(child.gameObject);

                // Criada lista de objetos para troca de pais
                foreach (Transform child in GO_Pecas.transform)
                    listaGO_Pecas.Add(child.gameObject);

                // Faz a troca dos pais
                foreach (GameObject child in listaGO_ObjGraf)
                    child.transform.parent = render.transform;

                Destroy(GO_ObjGraf);

                foreach (GameObject child in listaGO_Pecas)
                    child.transform.parent = fabricaPecas.transform;
                Destroy(GO_Pecas);

                Destroy(GameObject.Find(Global.listaEncaixes[objDrop.name]));
                Destroy(GameObject.Find(objDrop.name));
                Global.listaSequenciaSlots.Remove(Global.listaEncaixes[objDrop.name]);
                Global.removeObject(objDrop);
                Global.listaEncaixes.Remove(objDrop.name);
                Global.propriedadePecas.Remove(objDrop.name);
                break;

            case Slots.TransformacoesSlot:
                Destroy(GameObject.Find(Global.listaEncaixes[objDrop.name]));
                Destroy(GameObject.Find(objDrop.name));
                Global.listaSequenciaSlots.Remove(Global.listaEncaixes[objDrop.name]);
                Global.removeObject(objDrop);
                Global.listaEncaixes.Remove(objDrop.name);
                Global.propriedadePecas.Remove(objDrop.name);
                break;
        }
    }

    void ExcluiHierarquiaOuAlteraPropriedade(Slots tipoSlot)
    {
        MeshRenderer mr;
        string numObj = string.Empty;
        string valueOut = string.Empty;
        int val;

        switch (tipoSlot)
        {
            case Slots.ObjGrafSlot:
                if (Global.listaEncaixes.TryGetValue(objDrop.name, out valueOut))
                {
                    if (valueOut.Length > "ObjGraficoSlot".Length)
                    {
                        int.TryParse(valueOut.Substring(valueOut.IndexOf("Slot") + 4, 1), out val);

                        if (val > 0)
                            numObj = Convert.ToString(val);
                    }
                }

                Destroy(GameObject.Find("PosicaoAmb" + numObj));
                Destroy(GameObject.Find("CuboVisObjectMain" + numObj));
                break;

            case Slots.FormasSlot:
                mr = GameObject.Find("CuboAmbiente" + getNumObjeto(GameObject.Find(Global.listaEncaixes[objDrop.name]).transform.parent.name)).GetComponent<MeshRenderer>();
                mr.enabled = false;
                break;

            case Slots.TransformacoesSlot:
                GameObject goExcluido = GameObject.Find(objDrop.name + AMB);
                goExcluido.transform.GetChild(0).parent = goExcluido.transform.parent;
                Destroy(goExcluido);

                goExcluido = GameObject.Find(objDrop.name + VIS);
                goExcluido.transform.GetChild(0).parent = goExcluido.transform.parent;
                Destroy(goExcluido);
                break;
        }
    }

    public void AtualizaTrasformGameObjectAmb(string GOname)
    {
        string nomeAmb = "GameObjectAmb";

        if (!tutorial.GetComponent<Tutorial>().EstaExecutandoTutorial)
            nomeAmb += getNumObjeto(GameObject.Find(Global.listaEncaixes[GOname]).transform.parent.name);

        int cont = 0;
        int breakLoop = 50;

        Transform GoAmb = GameObject.Find(nomeAmb).transform;

        while (cont < breakLoop)
        {
            if (GoAmb.childCount > 0)
                GoAmb = GoAmb.GetChild(0);

            if (GoAmb.name.Contains("CuboAmbiente"))
            {
                Vector3 pos = Vector3.zero;
                Vector3 tam = new Vector3(1, 1, 1);
                Tamanho tamanho;
                Posicao posicao;

                if (Global.propriedadePecas.Count > 0)
                {
                    // Verifica posição e tamanho do cubo.
                    foreach (KeyValuePair<string, string> enc in Global.listaEncaixes)
                    {
                        if (Equals(enc.Value, "FormasSlot" + getNumeroSlotObjetoGrafico()))
                        {
                            if (Global.propriedadePecas.ContainsKey(enc.Key))
                            {
                                tamanho = Global.propriedadePecas[enc.Key].Tam;
                                posicao = Global.propriedadePecas[enc.Key].Pos;
                                tam = new Vector3(tamanho.X, tamanho.Y, tamanho.Z);
                                pos = new Vector3(posicao.X * -1, posicao.Y, posicao.Z);
                                break;
                            }
                        }
                    }
                }

                GoAmb.localPosition = pos;
                GoAmb.localRotation = Quaternion.Euler(0, 0, 0);
                GoAmb.localScale = tam;
                break;
            }

            // Verifica se o GO foi excluído então pega o próximo.
            if (GoAmb.childCount == 0 && !GoAmb.name.Contains("CuboAmbiente"))
            {
                if (GoAmb.parent.childCount > 1)
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

    void DestroyIluminacao(string key)
    {
        PropIluminacaoPadrao luz = new PropIluminacaoPadrao();

        if (key.Contains(Consts.ILUMINACAO))
        {
            Global.propriedadeIluminacao.Remove(key);

            if (key.Length > "Iluminacao".Length)
            {
                Destroy(GameObject.Find("LightObjects" + key));
            }
            else if (Global.propriedadePecas.ContainsKey(key))
            {
                luz.AtivaIluminacao(luz.GetTipoLuzPorExtenso(Global.propriedadePecas[key].TipoLuz) + key, false);
            }
        }
    }

    public string getNumeroSlotObjetoGrafico()
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

    void configuraIluminacao(string sinal)
    {
        float valorInc = 0;
        string iluminacao = "Iluminacao";

        valorInc = sinal.Equals("+") ? 3f : -3f;

        GameObject goObj = GameObject.Find("ObjGraficoSlot" + controller.GetComponent<Controller>().concatNumber);

        foreach (Transform child in goObj.transform)
        {
            if (child.name.Contains("IluminacaoSlot"))
            {
                foreach (var peca in Global.listaEncaixes)
                {
                    if (child.name.Equals(peca.Value))
                        iluminacao = peca.Key;
                }
            }
        }

        GameObject ilumunacao = GameObject.Find("IluminacaoSlot" + controller.GetComponent<Controller>().concatNumber);
        Vector3 pos = ilumunacao.transform.position;
        pos.y += valorInc;
        ilumunacao.transform.position = pos;

        // Se a peça "Iluminação já foi selecionada, será devidamente reposicionada"        
        GameObject IlumPeca = GameObject.Find(iluminacao);

        if (!tutorial.GetComponent<Tutorial>().EstaExecutandoTutorial)
        {
            if (Global.listaObjetos.Contains(IlumPeca))
                IlumPeca.transform.position = new Vector3(IlumPeca.transform.position.x, pos.y, IlumPeca.transform.position.z);
        }
    }

    void ReactiveColliderPeca()
    {
        Collider collider;

        foreach (Transform child in fabricaPecas.transform)
        {
            collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }
}
