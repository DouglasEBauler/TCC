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
                prPeca.ListPropLocks.Remove("P1PosX");
                lockP1PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P1PosX"))
                {
                    prPeca.ListPropLocks["P1PosX"] = Util_VisEdu.ConvertField(p1X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P1PosX", Util_VisEdu.ConvertField(p1X.text).ToString());
                }

                lockP1PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP1PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("P1PosY");
                lockP1PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P1PosY"))
                {
                    prPeca.ListPropLocks["P1PosY"] = Util_VisEdu.ConvertField(p1Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P1PosY", Util_VisEdu.ConvertField(p1Y.text).ToString());
                }

                lockP1PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP1PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("P1PosZ");
                lockP1PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P1PosZ"))
                {
                    prPeca.ListPropLocks["P1PosZ"] = Util_VisEdu.ConvertField(p1Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P1PosZ", Util_VisEdu.ConvertField(p1Z.text).ToString());
                }

                lockP1PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP2PosX.isOn)
            {
                prPeca.ListPropLocks.Remove("P2PosX");
                lockP2PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P2PosX"))
                {
                    prPeca.ListPropLocks["P2PosX"] = Util_VisEdu.ConvertField(p2X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P2PosX", Util_VisEdu.ConvertField(p2X.text).ToString());
                }

                lockP2PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP2PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("P2PosY");
                lockP2PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P2PosY"))
                {
                    prPeca.ListPropLocks["P2PosY"] = Util_VisEdu.ConvertField(p2Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P2PosY", Util_VisEdu.ConvertField(p2Y.text).ToString());
                }

                lockP2PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP2PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("P2PosZ");
                lockP2PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P2PosZ"))
                {
                    prPeca.ListPropLocks["P2PosZ"] = Util_VisEdu.ConvertField(p2Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P2PosZ", Util_VisEdu.ConvertField(p2Z.text).ToString());
                }

                lockP2PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP3PosX.isOn)
            {
                prPeca.ListPropLocks.Remove("P3PosX");
                lockP3PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P3PosX"))
                {
                    prPeca.ListPropLocks["P3PosX"] = Util_VisEdu.ConvertField(p3X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P3PosX", Util_VisEdu.ConvertField(p3X.text).ToString());
                }

                lockP3PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP3PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("P3PosY");
                lockP3PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P3PosY"))
                {
                    prPeca.ListPropLocks["P3PosY"] = Util_VisEdu.ConvertField(p3Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P3PosY", Util_VisEdu.ConvertField(p3Y.text).ToString());
                }

                lockP3PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP3PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("P3PosZ");
                lockP3PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P3PosZ"))
                {
                    prPeca.ListPropLocks["P3PosZ"] = Util_VisEdu.ConvertField(p3Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P3PosZ", Util_VisEdu.ConvertField(p3Z.text).ToString());
                }

                lockP3PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP4PosX.isOn)
            {
                prPeca.ListPropLocks.Remove("P4PosX");
                lockP4PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P4PosX"))
                {
                    prPeca.ListPropLocks["P4PosX"] = Util_VisEdu.ConvertField(p4X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P4PosX", Util_VisEdu.ConvertField(p4X.text).ToString());
                }

                lockP4PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP4PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("P4PosY");
                lockP4PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P4PosY"))
                {
                    prPeca.ListPropLocks["P4PosY"] = Util_VisEdu.ConvertField(p4Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P4PosY", Util_VisEdu.ConvertField(p4Y.text).ToString());
                }

                lockP4PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP4PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("P4PosZ");
                lockP4PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P4PosZ"))
                {
                    prPeca.ListPropLocks["P4PosZ"] = Util_VisEdu.ConvertField(p4Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P4PosZ", Util_VisEdu.ConvertField(p4Z.text).ToString());
                }

                lockP4PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP5PosX.isOn)
            {
                prPeca.ListPropLocks.Remove("P5PosX");
                lockP5PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P5PosX"))
                {
                    prPeca.ListPropLocks["P5PosX"] = Util_VisEdu.ConvertField(p4X.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P5PosX", Util_VisEdu.ConvertField(p4X.text).ToString());
                }

                lockP5PosX.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP5PosY.isOn)
            {
                prPeca.ListPropLocks.Remove("P5PosY");
                lockP5PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P5PosY"))
                {
                    prPeca.ListPropLocks["P5PosY"] = Util_VisEdu.ConvertField(p4Y.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P5PosY", Util_VisEdu.ConvertField(p4Y.text).ToString());
                }

                lockP5PosY.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
            }

            if (!lockP5PosZ.isOn)
            {
                prPeca.ListPropLocks.Remove("P5PosZ");
                lockP5PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
            }
            else
            {
                if (prPeca.ListPropLocks.ContainsKey("P5PosZ"))
                {
                    prPeca.ListPropLocks["P5PosZ"] = Util_VisEdu.ConvertField(p4Z.text).ToString();
                }
                else
                {
                    prPeca.ListPropLocks.Add("P5PosZ", Util_VisEdu.ConvertField(p4Z.text).ToString());
                }

                lockP5PosZ.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
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
