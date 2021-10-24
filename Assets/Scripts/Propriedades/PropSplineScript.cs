using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropSplineScript : MonoBehaviour
{
    [SerializeField]
    InputField nomePeca;
    [SerializeField]
    InputField p1X;
    [SerializeField]
    InputField p1Y;
    [SerializeField]
    InputField p1Z;
    [SerializeField]
    InputField p2X;
    [SerializeField]
    InputField p2Y;
    [SerializeField]
    InputField p2Z;
    [SerializeField]
    InputField p3X;
    [SerializeField]
    InputField p3Y;
    [SerializeField]
    InputField p3Z;
    [SerializeField]
    InputField p4X;
    [SerializeField]
    InputField p4Y;
    [SerializeField]
    InputField p4Z;
    [SerializeField]
    InputField p5X;
    [SerializeField]
    InputField p5Y;
    [SerializeField]
    InputField p5Z;
    [SerializeField]
    FlexibleColorPicker corSelecionado;
    [SerializeField]
    GameObject seletorCor;
    [SerializeField]
    Toggle ativo;
    [SerializeField]
    Toggle lockP1PosX;
    [SerializeField]
    Toggle lockP1PosY;
    [SerializeField]
    Toggle lockP1PosZ;
    [SerializeField]
    Toggle lockP2PosX;
    [SerializeField]
    Toggle lockP2PosY;
    [SerializeField]
    Toggle lockP2PosZ;
    [SerializeField]
    Toggle lockP3PosX;
    [SerializeField]
    Toggle lockP3PosY;
    [SerializeField]
    Toggle lockP3PosZ;
    [SerializeField]
    Toggle lockP4PosX;
    [SerializeField]
    Toggle lockP4PosY;
    [SerializeField]
    Toggle lockP4PosZ;
    [SerializeField]
    Toggle lockP5PosX;
    [SerializeField]
    Toggle lockP5PosY;
    [SerializeField]
    Toggle lockP5PosZ;

    SplinePropriedadePeca prPeca;
    Spline splineAmb;
    //GameObject splineVis;
    bool podeAtualizar;

    public void Inicializa()
    {
        prPeca = Global.propriedadePecas[Global.gameObjectName] as SplinePropriedadePeca;
        prPeca.Ativo = true;

        AtualizaListaProp();
        PreencheCampos();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            nomePeca.text = prPeca.Nome;
            ativo.isOn = prPeca.Ativo;

            podeAtualizar = false;
            try
            {
                p1X.text = prPeca.P1.X.ToString();
                p1Y.text = prPeca.P1.Y.ToString();
                p1Z.text = prPeca.P1.Z.ToString();
                p2X.text = prPeca.P2.X.ToString();
                p2Y.text = prPeca.P2.Y.ToString();
                p2Z.text = prPeca.P2.Z.ToString();
                p3X.text = prPeca.P3.X.ToString();
                p3Y.text = prPeca.P3.Y.ToString();
                p3Z.text = prPeca.P3.Z.ToString();
                p4X.text = prPeca.P4.X.ToString();
                p4Y.text = prPeca.P4.Y.ToString();
                p4Z.text = prPeca.P4.Z.ToString();
                p5X.text = prPeca.P4.X.ToString();
                p5Y.text = prPeca.P4.Y.ToString();
                p5Z.text = prPeca.P4.Z.ToString();
                corSelecionado.color = prPeca.Cor;

                splineAmb = GameObject.Find(prPeca.SplineAmbiente).GetComponent<Spline>();
                //splineVis = GameObject.Find(prPeca.NomeCuboVis);
            }
            finally
            {
                podeAtualizar = true;
                UpdateProp();
            }
        }
    }

    public void UpdateProp()
    {
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            prPeca.Nome = nomePeca.text;
            prPeca.P1.X = Util_VisEdu.ConvertField(p1X.text);
            prPeca.P1.Y = Util_VisEdu.ConvertField(p1Y.text);
            prPeca.P1.Z = Util_VisEdu.ConvertField(p1Z.text);
            prPeca.P2.X = Util_VisEdu.ConvertField(p2X.text);
            prPeca.P2.Y = Util_VisEdu.ConvertField(p2Y.text);
            prPeca.P2.Z = Util_VisEdu.ConvertField(p2Z.text);
            prPeca.P3.X = Util_VisEdu.ConvertField(p3X.text);
            prPeca.P3.Y = Util_VisEdu.ConvertField(p3Y.text);
            prPeca.P3.Z = Util_VisEdu.ConvertField(p3Z.text);
            prPeca.P4.X = Util_VisEdu.ConvertField(p4X.text);
            prPeca.P4.Y = Util_VisEdu.ConvertField(p4Y.text);
            prPeca.P4.Z = Util_VisEdu.ConvertField(p4Z.text);
            prPeca.P5.X = Util_VisEdu.ConvertField(p5X.text);
            prPeca.P5.Y = Util_VisEdu.ConvertField(p5Y.text);
            prPeca.P5.Z = Util_VisEdu.ConvertField(p5Z.text);
            prPeca.Ativo = ativo.isOn;

            if (prPeca.Ativo)
            {
                if (splineAmb != null)
                {
                    splineAmb.nodes[0].Position =
                        new Vector3(splineAmb.nodes[0].Position.x + prPeca.P1.X, splineAmb.nodes[0].Position.y + prPeca.P1.Y, splineAmb.nodes[0].Position.z + prPeca.P1.Z);
                    splineAmb.nodes[1].Position =
                        new Vector3(splineAmb.nodes[0].Position.x + prPeca.P2.X, splineAmb.nodes[0].Position.y + prPeca.P2.Y, splineAmb.nodes[0].Position.z + prPeca.P2.Z);
                    splineAmb.nodes[2].Position =
                        new Vector3(splineAmb.nodes[0].Position.x + prPeca.P3.X, splineAmb.nodes[0].Position.y + prPeca.P3.Y, splineAmb.nodes[0].Position.z + prPeca.P3.Z);
                    splineAmb.nodes[3].Position =
                        new Vector3(splineAmb.nodes[0].Position.x + prPeca.P4.X, splineAmb.nodes[0].Position.y + prPeca.P4.Y, splineAmb.nodes[0].Position.z + prPeca.P4.Z);
                    splineAmb.nodes[4].Position =
                        new Vector3(splineAmb.nodes[0].Position.x + prPeca.P5.X, splineAmb.nodes[0].Position.y + prPeca.P5.Y, splineAmb.nodes[0].Position.z + prPeca.P5.Z);
                }

                podeAtualizar = false;
                try
                {
                    UpdateLockFields();
                    UpdateColor();
                }
                finally
                {
                    podeAtualizar = true;
                }
            }
        }
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            Global.propriedadePecas[prPeca.Nome] = prPeca;
        }
    }

    public void ControlCorPanel()
    {
        corSelecionado.gameObject.SetActive(!corSelecionado.gameObject.activeSelf);
    }

    public void UpdateLockFields()
    {
        if (podeAtualizar)
        {
            if (!lockP1PosX.isOn)
            {
                prPeca.ListPropLocks.Remove("PosX");
                lockP1PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosX"))
                {
                    prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(p1X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(p1X.text).ToString());
                }

                lockP1PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP1PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("PosY");
                lockP1PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosY"))
                {
                    prPeca.ListPropLocks["PosY"] = Util_VisEdu.ConvertField(p1Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(p1Y.text).ToString());
                }

                lockP1PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP1PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("PosZ");
                lockP1PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosZ"))
                {
                    prPeca.ListPropLocks["PosZ"] = Util_VisEdu.ConvertField(p1Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(p1Z.text).ToString());
                }

                lockP1PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP2PosX.isOn)
            {
                prPeca.ListPropLocks.Remove("PosX");
                lockP2PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosX"))
                {
                    prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(p2X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(p2X.text).ToString());
                }

                lockP2PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP2PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("PosY");
                lockP2PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosY"))
                {
                    prPeca.ListPropLocks["PosY"] = Util_VisEdu.ConvertField(p2Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(p2Y.text).ToString());
                }

                lockP2PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP2PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("PosZ");
                lockP2PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosZ"))
                {
                    prPeca.ListPropLocks["PosZ"] = Util_VisEdu.ConvertField(p2Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(p2Z.text).ToString());
                }

                lockP2PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP3PosX.isOn)
            {
                prPeca.ListPropLocks.Remove("PosX");
                lockP3PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosX"))
                {
                    prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(p3X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(p3X.text).ToString());
                }

                lockP3PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP3PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("PosY");
                lockP3PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosY"))
                {
                    prPeca.ListPropLocks["PosY"] = Util_VisEdu.ConvertField(p3Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(p3Y.text).ToString());
                }

                lockP3PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP3PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("PosZ");
                lockP3PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosZ"))
                {
                    prPeca.ListPropLocks["PosZ"] = Util_VisEdu.ConvertField(p3Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(p3Z.text).ToString());
                }

                lockP3PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP4PosX.isOn)
            {
                prPeca.ListPropLocks.Remove("PosX");
                lockP4PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosX"))
                {
                    prPeca.ListPropLocks["PosX"] = Util_VisEdu.ConvertField(p4X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosX", Util_VisEdu.ConvertField(p4X.text).ToString());
                }

                lockP4PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP4PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("PosY");
                lockP4PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosY"))
                {
                    prPeca.ListPropLocks["PosY"] = Util_VisEdu.ConvertField(p4Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosY", Util_VisEdu.ConvertField(p4Y.text).ToString());
                }

                lockP4PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP4PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("PosZ");
                lockP4PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("PosZ"))
                {
                    prPeca.ListPropLocks["PosZ"] = Util_VisEdu.ConvertField(p4Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("PosZ", Util_VisEdu.ConvertField(p4Z.text).ToString());
                }

                lockP4PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }
        }
    }

    void UpdateColor()
    {
        seletorCor.GetComponent<Image>().material.color = corSelecionado.color;

        if (splineAmb != null)
        {
            splineAmb.GetComponent<SplineMeshTiling>().material.color = corSelecionado.color;
        }
        //if (splineVis != null)
        //{
        //  splineVis.GetComponent<SplineMeshTiling>().material.color = corSelecionado.color;
        //}
    }
}
