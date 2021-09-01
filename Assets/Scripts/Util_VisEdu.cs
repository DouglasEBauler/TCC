using UnityEngine;

public class Util_VisEdu : MonoBehaviour
{
    static GameObject _fabricaPecas;

    public static GameObject CuboVisObjectMain;
    public static GameObject PosicaoAmb;

    static GameObject fabricaPecas
    {
        get
        {
            if (_fabricaPecas == null)
            {
                _fabricaPecas = GameObject.Find("FabricaPecas");
            }

            return _fabricaPecas;
        }
    }

    public static void CriaFormasVazias()
    {
        GameObject cuboVisObjectMain = GameObject.Find("CuboVisObjectMain");
        CuboVisObjectMain = Instantiate(cuboVisObjectMain, cuboVisObjectMain.transform.position, cuboVisObjectMain.transform.rotation, cuboVisObjectMain.transform.parent);

        GameObject posicaoAmb = GameObject.Find("PosicaoAmb");
        PosicaoAmb = Instantiate(posicaoAmb, posicaoAmb.transform.position, posicaoAmb.transform.rotation, posicaoAmb.transform.parent);

        Global.cuboVis = CuboVisObjectMain;
        Global.posAmb = PosicaoAmb;
    }

    public static string GetCuboByNomePeca(string nome)
    {
        string cubo = string.Empty;

        if (!string.Empty.Equals(nome))
        {
            foreach (Transform child in GameObject.Find(Global.listaEncaixes[nome]).transform.parent)
            {
                if ("FormasSlot".Equals(child.name))
                {
                    foreach (var pair in Global.listaEncaixes)
                    {
                        if (pair.Value.Equals(child.name)) return pair.Key;
                    }
                }
            }
        }

        return cubo;
    }

    public static void EnableColliderFabricaPecas(bool enable, bool enableChildren, bool render = false)
    {
        if (enableChildren)
        {
            foreach (Transform child in fabricaPecas.transform)
            {
                if (!"Spline".Equals(child.name) && !"Poligono".Equals(child.name)
                && !"GO_Pecas".Equals(child.name) && !"GO_ObjetosGraficos".Equals(child.name))
                {
                    // Se render for false é porque é pra alterar os colliders da fabrica de peças.
                    if (!render)
                    {
                        if (!Global.listaEncaixes.ContainsKey(child.name))
                            child.GetComponent<Collider>().enabled = enable;
                    }
                    else
                    {
                        // Senão dever alterar os colliders do render.
                        if (Global.listaEncaixes.ContainsKey(child.name))
                            child.GetComponent<Collider>().enabled = enable;
                    }
                }
            }
        }

        fabricaPecas.GetComponent<Collider>().enabled = enable;
    }
}
