using UnityEngine;
using UnityEngine.UI;

public class PropTransladarScript : MonoBehaviour
{
    [SerializeField]
    GameObject labelZ;
    [SerializeField]
    InputField nomePeca;
    [SerializeField]
    InputField posX;
    [SerializeField]
    InputField posY;
    [SerializeField]
    InputField posZ;
    [SerializeField]
    Toggle toggleField;
    [SerializeField]
    Toggle toggleLockPosX;
    [SerializeField]
    Toggle toggleLockPosY;
    [SerializeField]
    Toggle toggleLockPosZ;

    PropriedadeTransformacao prPeca;
    bool podeAtualizar;

    public void Inicializa()
    {
        prPeca = Global.propriedadePecas[Util_VisEdu.GetPecaByName(Global.gameObjectName) + Global.gameObjectName] as PropriedadeTransformacao;
        prPeca.Ativo = true;
        podeAtualizar = true;

        PreencheCampos();
    }

    void PreencheCampos()
    {
        podeAtualizar = false;
        try
        {
            nomePeca.text = prPeca.Nome;

            if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
            {
                posX.text = prPeca.Pos.X.ToString();
                posY.text = prPeca.Pos.Y.ToString();
                posZ.text = prPeca.Pos.Z.ToString();
                toggleField.isOn = prPeca.Ativo;
            }
        }
        finally
        {
            podeAtualizar = true;
            UpdateProp();
        }
    }

    public void UpdateProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome) && podeAtualizar)
        {
            prPeca.Pos.X = Util_VisEdu.ConvertField(posX.text);
            prPeca.Pos.Y = Util_VisEdu.ConvertField(posY.text);
            prPeca.Pos.Z = Util_VisEdu.ConvertField(posZ.text);
            prPeca.Ativo = toggleField.isOn;

            if (prPeca.Ativo)
            {
                GameObject goTransformacaoAmb = GameObject.Find(prPeca.Nome + "Amb");
                if (goTransformacaoAmb != null)
                {
                    goTransformacaoAmb.transform.localPosition = new Vector3(prPeca.Pos.X * -1, prPeca.Pos.Y, prPeca.Pos.Z);
                }

                GameObject goTransformacaoVis = GameObject.Find(prPeca.Nome + "Vis");
                if (goTransformacaoVis != null)
                {
                    goTransformacaoVis.transform.localPosition = new Vector3(prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
                }
            }
        }

        AtualizaListaProp();
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            Global.propriedadePecas[prPeca.Nome] = prPeca;
        }
    }

    public void UpdateLockFields()
    {
        if (!toggleLockPosX.isOn)
        {
            prPeca.ListPropLocks.Remove("PosX");
            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (prPeca.ListPropLocks.ContainsKey("PosX"))
            {
                prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(posX.text).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(posX.text).ToString());
            }

            toggleLockPosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosY.isOn)
        {
            prPeca.ListPropLocks.Remove("PosY");
            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (prPeca.ListPropLocks.ContainsKey("PosY"))
            {
                prPeca.ListPropLocks.Remove("PosY");
            }
            else
            {
                prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(posY.text).ToString());
            }

            toggleLockPosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }

        if (!toggleLockPosZ.isOn)
        {
            prPeca.ListPropLocks.Remove("PosZ");
            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (prPeca.ListPropLocks.ContainsKey("PosZ"))
            {
                prPeca.ListPropLocks.Remove("PosZ");
            }
            else
            {
                prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(posZ.text).ToString());
            }

            toggleLockPosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }
}
