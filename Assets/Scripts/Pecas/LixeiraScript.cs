using UnityEngine;

public class LixeiraScript : MonoBehaviour
{
    [SerializeField]
    GameObject tutorial;
    [SerializeField]
    GameObject render;
    [SerializeField]
    PropCameraScript propCamera;
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    GameObject fabricaPecas;
    [SerializeField]
    GameObject posicaoAmb;
    [SerializeField]
    GameObject posicaoVis;

    [HideInInspector]
    public GameObject objDrop;

    void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0) && objDrop != null)
        {
            RemovePeca();
            objDrop = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        objDrop = other.gameObject;
    }

    public void RemovePeca()
    {
        EnabledColliderPecas(false);
        try
        {
            if (objDrop.name.Contains(Consts.CAMERA))
            {
                RemoveCameraP();
            }
            else if (objDrop.name.Contains(Consts.OBJETOGRAFICO))
            {
                RemoveObjetoGrafico();
            }
            else if (objDrop.name.Contains(Consts.CUBO) || objDrop.name.Contains(Consts.POLIGONO) || objDrop.name.Contains(Consts.SPLINE))
            {
                RemoveForma();
            }
            else if (objDrop.name.Contains(Consts.ROTACIONAR) || objDrop.name.Contains(Consts.ESCALAR) || objDrop.name.Contains(Consts.TRANSLADAR))
            {
                RemoveTransformacao();
            }
            else if (objDrop.name.Contains(Consts.ITERACAO))
            {
                RemoveIteracao();
            }
            else if (objDrop.name.Contains(Consts.ILUMINACAO))
            {
                RemoveIluminacao();
            }

            menuControl.GetComponent<MenuScript>().EnablePanelFabPecas();
            ReactiveColliderPeca();
        }
        finally
        {
            EnabledColliderPecas(true);
        }
    }

    void RemoveCameraP()
    {
        propCamera.DemosntraCamera(false);
        Global.propriedadePecas.Remove(objDrop.name);
        Global.listaEncaixes.Remove(objDrop.name);
        Destroy(objDrop);
    }

    void RemoveIluminacao()
    {
        string numFormaSlot = Util_VisEdu.GetNumSlot(objDrop.transform.parent.name);
        Transform slot, peca;

        foreach (Transform child in render.transform)
        {
            if (child.name.Contains(Consts.SLOT_ILUMINACAO + numFormaSlot))
            {
                slot = child.transform.Find(Consts.ILUMINACAO_SLOT + numFormaSlot);

                if (slot != null)
                {
                    peca = slot.transform.Find(Consts.ILUMINACAO + numFormaSlot);

                    if (peca)
                    {
                        Global.listaEncaixes.Remove(peca.name);
                        Global.propriedadePecas.Remove(peca.name);
                        Destroy(peca.gameObject);

                        GameObject objGameLightIluminacao = GameObject.Find(Consts.LIGHT_OBJECTS_ILUMINACAO + numFormaSlot);
                        if (objGameLightIluminacao != null)
                        {
                            Destroy(objGameLightIluminacao);
                        }

                        Rigidbody rigiBody = slot.gameObject.AddComponent<Rigidbody>();
                        rigiBody.useGravity = false;
                        rigiBody.constraints = RigidbodyConstraints.FreezeAll;
                    }
                    break;
                }
            }
        }
    }

    void RemoveIteracao()
    {
        string numTransfSlot = Util_VisEdu.GetNumSlot(objDrop.transform.parent.name, true);
        Transform peca;

        foreach (Transform child in render.transform)
        {
            if (child.name.Contains(Consts.SLOT_TRANSF + numTransfSlot))
            {
                Transform _child = child.GetChild(0).Find(Consts.ITERACAO_SLOT + numTransfSlot);
                if (_child != null)
                {
                    peca = _child.transform.Find(Consts.ITERACAO + numTransfSlot);

                    if (peca != null)
                    {
                        Global.listaEncaixes.Remove(peca.gameObject.name);
                        Global.propriedadePecas.Remove(peca.gameObject.name);
                        Destroy(peca.gameObject);
                    }

                    Rigidbody rigiBody = _child.gameObject.AddComponent<Rigidbody>();
                    rigiBody.useGravity = false;
                    rigiBody.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
    }

    void EnabledColliderPecas(bool status)
    {
        foreach (var item in Global.listaEncaixes)
        {
            GameObject.Find(item.Key).GetComponent<BoxCollider>().enabled = status;
        }
    }

    void RemoveObjetoGrafico()
    {
        GameObject forma = GetForma();
        string numFormaSlot = Util_VisEdu.GetNumSlot(objDrop.transform.parent.name);
        Transform slot;

        foreach (Transform child in posicaoAmb.transform)
        {
            if (child.name.Equals(Consts.CUBO_AMB_OBJ + numFormaSlot)
                || child.name.Equals(Consts.POLIGONO_AMB_OBJ + numFormaSlot)
                || child.name.Equals(Consts.SPLINE_AMB_OBJ + numFormaSlot))
            {
                Destroy(child.gameObject);
            }
        }

        foreach (Transform child in posicaoVis.transform)
        {
            if (child.name.Equals(Consts.CUBO_VIS_OBJ + numFormaSlot)
                || child.name.Equals(Consts.POLIGONO_VIS_OBJ + numFormaSlot)
                || child.name.Equals(Consts.SPLINE_VIS_OBJ + numFormaSlot))
            {
                Destroy(child.gameObject);
            }
        }

        foreach (Transform child in render.transform)
        {
            if (child.name.Contains(Consts.SLOT_TRANSF + numFormaSlot))
            {
                slot = child.transform.GetChild(0);

                foreach (Transform _slot in slot.transform)
                {
                    if (_slot.name.Contains(Consts.ROTACIONAR)
                        || _slot.name.Contains(Consts.TRANSLADAR)
                        || _slot.name.Contains(Consts.ESCALAR))
                    {
                        Global.listaEncaixes.Remove(_slot.name);
                        Global.propriedadePecas.Remove(_slot.name);
                    }
                }
                Destroy(child.gameObject);
            }
            else if (child.name.Contains(Consts.SLOT_FORMA + numFormaSlot))
            {
                slot = child.transform.GetChild(0);

                foreach (Transform _slot in slot.transform)
                {
                    if (_slot.name.Contains(Consts.CUBO)
                        || _slot.name.Contains(Consts.POLIGONO)
                        || _slot.name.Contains(Consts.SPLINE))
                    {
                        Global.listaEncaixes.Remove(_slot.name);
                        Global.propriedadePecas.Remove(_slot.name);
                    }
                }
                Destroy(child.gameObject);
            }
            else if (child.name.Contains(Consts.SLOT_ILUMINACAO + numFormaSlot))
            {
                slot = child.transform.GetChild(0);

                if (slot != null)
                {
                    Transform peca = slot.transform.Find(Consts.ILUMINACAO + numFormaSlot);

                    if (peca)
                    {
                        Global.listaEncaixes.Remove(peca.name);
                        Global.propriedadePecas.Remove(peca.name);
                        Destroy(peca.gameObject);

                        GameObject objGameLightIluminacao = GameObject.Find(Consts.LIGHT_OBJECTS_ILUMINACAO + numFormaSlot);
                        if (objGameLightIluminacao != null)
                        {
                            Destroy(objGameLightIluminacao);
                        }
                    }

                    Destroy(child.gameObject);
                }
            }
            else if (child.name.Contains(Consts.SLOT_OBJ_GRAFICO + numFormaSlot))
            {
                slot = child.transform.GetChild(0);

                foreach (Transform _slot in slot.transform)
                {
                    if (_slot.name.Contains(Consts.OBJETOGRAFICO))
                    {
                        Global.listaEncaixes.Remove(_slot.name);
                        Global.propriedadePecas.Remove(_slot.name);
                    }
                }
                Destroy(child.gameObject);
            }
        }
    }

    void RemoveTransformacao()
    {
        string numSlot = Util_VisEdu.GetNumSlot(objDrop.transform.parent.name);
        string numTransfSlot = Util_VisEdu.GetNumSlot(objDrop.transform.parent.name, true);
        Transform slot, child;

        child = posicaoAmb.transform.GetChild(0);
        while (child != null)
        {
            if (child.GetChild(0).name.Contains(objDrop.name))
            {
                if (child.GetChild(0).gameObject.name.Contains(Consts.ROTACIONAR))
                {
                    child.GetChild(0).localEulerAngles = new Vector3(0, ((GameObject.Find(Consts.POLIGONO + numSlot) != null) ? 180 : 0), 0);
                }
                else if (child.GetChild(0).gameObject.name.Contains(Consts.TRANSLADAR))
                {
                    child.GetChild(0).localPosition = new Vector3(0, 0, 0);
                }
                else if (child.GetChild(0).gameObject.name.Contains(Consts.ESCALAR))
                {
                    child.GetChild(0).localScale = new Vector3(1, 1, 1);
                }

                if (child.GetChild(0).childCount > 0)
                {
                    child.GetChild(0).GetChild(0).parent = child.GetChild(0).parent;
                }                
                Destroy(child.GetChild(0).gameObject);
                break;
            }

            child = child.GetChild(0);
        }

        child = posicaoVis.transform.GetChild(0);
        while (child != null)
        {
            if (child.GetChild(0).name.Contains(objDrop.name))
            {
                if (child.GetChild(0).gameObject.name.Contains(Consts.ROTACIONAR))
                {
                    child.GetChild(0).localEulerAngles = new Vector3(0, ((GameObject.Find(Consts.POLIGONO + numSlot) != null) ? 180 : 0), 0);
                }
                else if (child.GetChild(0).gameObject.name.Contains(Consts.TRANSLADAR))
                {
                    child.GetChild(0).localPosition = new Vector3(0, 0, 0);
                }
                else if (child.GetChild(0).gameObject.name.Contains(Consts.ESCALAR))
                {
                    child.GetChild(0).localScale = new Vector3(1, 1, 1);
                }

                if (child.GetChild(0).childCount > 0)
                {
                    child.GetChild(0).GetChild(0).parent = child.GetChild(0).parent;
                }
                Destroy(child.GetChild(0).gameObject);
                break;
            }

            child = child.GetChild(0);
        }

        foreach (Transform _child in render.transform)
        {
            if (_child.name.Contains(Consts.SLOT_TRANSF + numTransfSlot))
            {
                slot = _child.transform.GetChild(0);

                foreach (Transform _slot in slot.transform)
                {
                    if (_slot.name.Contains(objDrop.name))
                    {
                        Global.listaEncaixes.Remove(_slot.name);
                        Global.propriedadePecas.Remove(_slot.name);
                        break;
                    }
                }
                Destroy(_child.gameObject);
                break;
            }
        }
    }

    void RemoveForma()
    {
        GameObject forma = GetForma();
        string numFormaSlot = Util_VisEdu.GetNumSlot(forma.transform.parent.name);
        Transform slot;

        foreach (Transform child in posicaoAmb.transform)
        {
            if (child.name.Contains(Consts.CUBO_AMB_OBJ + numFormaSlot)
                || child.name.Contains(Consts.POLIGONO_AMB_OBJ + numFormaSlot)
                || child.name.Contains(Consts.SPLINE_AMB_OBJ + numFormaSlot))
            {
                Destroy(child.gameObject);
                break;
            }
        }

        foreach (Transform child in posicaoVis.transform)
        {
            if (child.name.Contains(Consts.CUBO_VIS_OBJ + numFormaSlot)
                || child.name.Contains(Consts.POLIGONO_VIS_OBJ + numFormaSlot)
                || child.name.Contains(Consts.SPLINE_VIS_OBJ + numFormaSlot))
            {
                Destroy(child.gameObject);
                break;
            }
        }

        foreach (Transform child in render.transform)
        {
            if (child.name.Contains(Consts.SLOT_TRANSF + numFormaSlot + "_"))
            {
                slot = child.transform.GetChild(0);

                foreach (Transform _slot in slot.transform)
                {
                    if (_slot.name.Contains(Consts.ROTACIONAR)
                        || _slot.name.Contains(Consts.TRANSLADAR)
                        || _slot.name.Contains(Consts.ESCALAR))
                    {
                        Global.listaEncaixes.Remove(_slot.name);
                        Global.propriedadePecas.Remove(_slot.name);
                    }
                }
                Destroy(child.gameObject);
            }
            else if (child.name.Contains(Consts.SLOT_FORMA + numFormaSlot))
            {
                slot = child.transform.GetChild(0);

                foreach (Transform _slot in slot.transform)
                {
                    if (_slot.name.Contains(Consts.CUBO)
                        || _slot.name.Contains(Consts.POLIGONO)
                        || _slot.name.Contains(Consts.SPLINE))
                    {
                        Global.listaEncaixes.Remove(_slot.name);
                        Global.propriedadePecas.Remove(_slot.name);
                        Destroy(_slot.gameObject);
                    }
                }
            }
            else if (child.name.Contains(Consts.SLOT_ILUMINACAO + numFormaSlot))
            {
                Transform peca = child.transform.GetChild(0).transform.Find(Consts.ILUMINACAO + numFormaSlot);

                if (peca)
                {
                    Global.listaEncaixes.Remove(peca.name);
                    Global.propriedadePecas.Remove(peca.name);
                    Destroy(peca.gameObject);

                    GameObject objGameLightIluminacao = GameObject.Find(Consts.LIGHT_OBJECTS_ILUMINACAO + numFormaSlot);
                    if (objGameLightIluminacao != null)
                    {
                        Destroy(objGameLightIluminacao);
                    }

                    Rigidbody rigiBody = child.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
                    rigiBody.useGravity = false;
                    rigiBody.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }

        if (forma.transform.parent.name.Contains(Consts.FORMA_SLOT))
        {
            Rigidbody rigiBody = forma.transform.parent.gameObject.AddComponent<Rigidbody>();
            rigiBody.useGravity = false;
            rigiBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    GameObject GetForma()
    {
        string numForma = Util_VisEdu.GetNumSlot(objDrop.transform.parent.name);

        GameObject forma = GameObject.Find(Consts.CUBO + numForma);
        if (forma != null)
        {
            return forma;
        }

        forma = GameObject.Find(Consts.POLIGONO + numForma);
        if (forma != null)
        {
            return forma;
        }

        forma = GameObject.Find(Consts.SPLINE + numForma);
        if (forma != null)
        {
            return forma;
        }

        return null;
    }

    void ReactiveColliderPeca()
    {
        BoxCollider collider;

        foreach (Transform child in fabricaPecas.transform)
        {
            collider = child.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }
}
