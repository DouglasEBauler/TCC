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
    InputField quantidadePontos;
    [SerializeField]
    FlexibleColorPicker corSelecionado;
    [SerializeField]
    GameObject seletorCor;
    [SerializeField]
    PropCameraScript propCamera;
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
    [SerializeField]
    Toggle lockQuantidadePontos;

    SplinePropriedadePeca prPeca;
    Dictionary<Property, InputField> propList;
    Dictionary<Property, Toggle> lockList;
    Spline splineAmb;
    Spline splineVis;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (this.prPeca != null)
        {
            UpdateColor();
            FocusSplineVis();
        }
    }

    void FocusSplineVis()
    {
        if (splineVis != null && Global.cameraAtiva && this.prPeca.Ativo)
        {
            propCamera.EnabledPecasVis(this.prPeca.NomePeca, false);
            splineVis.transform.GetChild(0).Find("segment 1 mesh").GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            splineVis.transform.GetChild(0).Find("segment 1 mesh").GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void Inicializa(SplinePropriedadePeca propPeca)
    {
        prPeca = propPeca;
        prPeca.Ativo = true;

        PreencheCampos();
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.NomePeca))
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
                p5X.text = prPeca.P5.X.ToString();
                p5Y.text = prPeca.P5.Y.ToString();
                p5Z.text = prPeca.P5.Z.ToString();
                quantidadePontos.text = prPeca.QuantidadePontos.ToString();
                corSelecionado.color = prPeca.Cor;

                splineAmb = GameObject.Find(prPeca.SplineAmb).GetComponent<Spline>();
                splineVis = GameObject.Find(prPeca.SplineVis).GetComponent<Spline>();
            }
            finally
            {
                podeAtualizar = true;
                UpdateProp();
            }
        }
    }

    void InicializaListas()
    {
        propList = new Dictionary<Property, InputField>()
        {
            { Property.P1PosX, p1X },
            { Property.P1PosY, p1Y },
            { Property.P1PosZ, p1Z },
            { Property.P2PosX, p2X },
            { Property.P2PosY, p2Y },
            { Property.P2PosZ, p2Z },
            { Property.P3PosX, p3X },
            { Property.P3PosY, p3Y },
            { Property.P3PosZ, p3Z },
            { Property.P4PosX, p4X },
            { Property.P4PosY, p4Y },
            { Property.P4PosZ, p4Z },
            { Property.P5PosX, p5X },
            { Property.P5PosY, p5Y },
            { Property.P5PosZ, p5Z },
            { Property.QuantidadePontos, quantidadePontos }
        };

        lockList = new Dictionary<Property, Toggle>()
        {
            { Property.P1PosX, lockP1PosX },
            { Property.P1PosY, lockP1PosY },
            { Property.P1PosZ, lockP1PosZ },
            { Property.P2PosX, lockP2PosX },
            { Property.P2PosY, lockP2PosY },
            { Property.P2PosZ, lockP2PosZ },
            { Property.P3PosX, lockP3PosX },
            { Property.P3PosY, lockP3PosY },
            { Property.P3PosZ, lockP3PosZ },
            { Property.P4PosX, lockP4PosX },
            { Property.P4PosY, lockP4PosY },
            { Property.P4PosZ, lockP4PosZ },
            { Property.P5PosX, lockP5PosX },
            { Property.P5PosY, lockP5PosY },
            { Property.P5PosZ, lockP5PosZ },
            { Property.QuantidadePontos, lockQuantidadePontos }
        };
    }


    public void UpdateProp()
    {
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(prPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                prPeca.Nome = nomePeca.text;
                prPeca.P1.X = Util_VisEdu.ConvertField(p1X.text) * -1;
                prPeca.P1.Y = Util_VisEdu.ConvertField(p1Y.text);
                prPeca.P1.Z = Util_VisEdu.ConvertField(p1Z.text);
                prPeca.P2.X = Util_VisEdu.ConvertField(p2X.text) * -1;
                prPeca.P2.Y = Util_VisEdu.ConvertField(p2Y.text);
                prPeca.P2.Z = Util_VisEdu.ConvertField(p2Z.text);
                prPeca.P3.X = Util_VisEdu.ConvertField(p3X.text) * -1;
                prPeca.P3.Y = Util_VisEdu.ConvertField(p3Y.text);
                prPeca.P3.Z = Util_VisEdu.ConvertField(p3Z.text);
                prPeca.P4.X = Util_VisEdu.ConvertField(p4X.text) * -1;
                prPeca.P4.Y = Util_VisEdu.ConvertField(p4Y.text);
                prPeca.P4.Z = Util_VisEdu.ConvertField(p4Z.text);
                prPeca.P5.X = Util_VisEdu.ConvertField(p5X.text) * -1;
                prPeca.P5.Y = Util_VisEdu.ConvertField(p5Y.text);
                prPeca.P5.Z = Util_VisEdu.ConvertField(p5Z.text);
                prPeca.QuantidadePontos = Util_VisEdu.ConvertField(quantidadePontos.text);
                prPeca.Ativo = ativo.isOn;

                if (splineAmb != null)
                {
                    splineAmb.nodes[0].Position =
                        new Vector3(splineAmb.nodes[0].Position.x + prPeca.P1.X, splineAmb.nodes[0].Position.y + prPeca.P1.Y, splineAmb.nodes[0].Position.z + prPeca.P1.Z);
                    splineAmb.nodes[1].Position =
                        new Vector3(splineAmb.nodes[1].Position.x + prPeca.P2.X, splineAmb.nodes[1].Position.y + prPeca.P2.Y, splineAmb.nodes[1].Position.z + prPeca.P2.Z);
                    splineAmb.nodes[2].Position =
                        new Vector3(splineAmb.nodes[2].Position.x + prPeca.P3.X, splineAmb.nodes[2].Position.y + prPeca.P3.Y, splineAmb.nodes[2].Position.z + prPeca.P3.Z);
                    splineAmb.nodes[3].Position =
                        new Vector3(splineAmb.nodes[3].Position.x + prPeca.P4.X, splineAmb.nodes[3].Position.y + prPeca.P4.Y, splineAmb.nodes[3].Position.z + prPeca.P4.Z);
                    splineAmb.nodes[4].Position =
                        new Vector3(splineAmb.nodes[4].Position.x + prPeca.P5.X, splineAmb.nodes[4].Position.y + prPeca.P5.Y, splineAmb.nodes[4].Position.z + prPeca.P5.Z);
                    splineAmb.transform.GetChild(0).Find("segment 1 mesh").GetComponent<MeshRenderer>().enabled = prPeca.Ativo;

                    if ((Util_VisEdu.ConvertField(quantidadePontos.text) / 100f) < 100)
                    {
                        splineAmb.GetComponent<SplineSmoother>().curvature = Util_VisEdu.ConvertField(quantidadePontos.text) / 100f;
                    }
                    else
                    {
                        splineAmb.GetComponent<SplineSmoother>().curvature += 99 / 100f;
                        prPeca.QuantidadePontos = 99;
                        quantidadePontos.text = (prPeca.QuantidadePontos / 100f).ToString();
                    }
                }

                if (splineVis != null)
                {
                    splineVis.nodes[0].Position =
                        new Vector3(splineVis.nodes[0].Position.x + prPeca.P1.X, splineVis.nodes[0].Position.y + prPeca.P1.Y, splineVis.nodes[0].Position.z + prPeca.P1.Z);
                    splineVis.nodes[1].Position =
                        new Vector3(splineVis.nodes[1].Position.x + prPeca.P2.X, splineVis.nodes[1].Position.y + prPeca.P2.Y, splineVis.nodes[1].Position.z + prPeca.P2.Z);
                    splineVis.nodes[2].Position =
                        new Vector3(splineVis.nodes[2].Position.x + prPeca.P3.X, splineVis.nodes[2].Position.y + prPeca.P3.Y, splineVis.nodes[2].Position.z + prPeca.P3.Z);
                    splineVis.nodes[3].Position =
                        new Vector3(splineVis.nodes[3].Position.x + prPeca.P4.X, splineVis.nodes[3].Position.y + prPeca.P4.Y, splineVis.nodes[3].Position.z + prPeca.P4.Z);
                    splineVis.nodes[4].Position =
                        new Vector3(splineVis.nodes[4].Position.x + prPeca.P5.X, splineVis.nodes[4].Position.y + prPeca.P5.Y, splineVis.nodes[4].Position.z + prPeca.P5.Z);
                    splineVis.transform.GetChild(0).Find("segment 1 mesh").GetComponent<MeshRenderer>().enabled = prPeca.Ativo && Global.cameraAtiva;

                    if ((Util_VisEdu.ConvertField(quantidadePontos.text) / 100f) < 100)
                    {
                        splineVis.GetComponent<SplineSmoother>().curvature = Util_VisEdu.ConvertField(quantidadePontos.text) / 100f;
                    }
                    else
                    {
                        splineVis.GetComponent<SplineSmoother>().curvature = 99 / 100f;
                        prPeca.QuantidadePontos = 99;
                        quantidadePontos.text = (prPeca.QuantidadePontos / 100f).ToString();
                    }

                    FocusSplineVis();
                }

                UpdateAllLockFields();
                UpdateColor();
                AtualizaListaProp();
            }
            finally
            {
                podeAtualizar = true;
            }
        }
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
        {
            Global.propriedadePecas[this.prPeca.NomePeca] = prPeca;
        }
    }

    public void ControlCorPanel()
    {
        corSelecionado.gameObject.SetActive(!corSelecionado.gameObject.activeSelf);
    }

    void UpdateAllLockFields()
    {
        if (lockList == null || propList == null)
        {
            InicializaListas();
        }

        foreach (var lockItem in lockList)
        {
            UpdateLockFields((lockItem.Key));
        }
    }

    public void UpdateLockFields(int typeProperty)
    {
        UpdateLockFields((Property)typeProperty);
    }

    void UpdateLockFields(Property typeProperty)
    {
        if (lockList == null || propList == null)
        {
            InicializaListas();
        }

        if (!lockList[typeProperty].isOn)
        {
            prPeca.ListPropLocks.Remove(typeProperty);
            lockList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_UNLOCK);
        }
        else
        {
            if (prPeca.ListPropLocks.ContainsKey(typeProperty))
            {
                prPeca.ListPropLocks[typeProperty] = Util_VisEdu.ConvertField(propList[typeProperty].text).ToString();
            }
            else
            {
                prPeca.ListPropLocks.Add(typeProperty, Util_VisEdu.ConvertField(propList[typeProperty].text).ToString());
            }

            lockList[typeProperty].GetComponent<RawImage>().texture = Resources.Load<Texture2D>(Consts.PATH_IMG_LOCK);
        }
    }

    void UpdateColor()
    {
        seletorCor.GetComponent<Image>().material.color = corSelecionado.color;

        if (splineAmb != null)
        {
            splineAmb.GetComponent<SplineMeshTiling>().material.color = corSelecionado.color;
            splineAmb.GetComponent<SplineMeshTiling>().material.SetColor("_EmissionColor", corSelecionado.color);
        }
        if (splineVis != null)
        {
            splineVis.GetComponent<SplineMeshTiling>().material.color = corSelecionado.color;
            splineVis.GetComponent<SplineMeshTiling>().material.SetColor("_EmissionColor", corSelecionado.color);
        }
    }
}
