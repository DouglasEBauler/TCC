using System;
using UnityEngine;
using UnityEngine.UI;

public class PropObjetoGraficoScript : MonoBehaviour
{
    [SerializeField]
    InputField nomePeca;
    [SerializeField]
    Text matrix4x4;
    [SerializeField]
    Toggle ativo;

    PropriedadePeca propPeca;
    bool podeAtualizar;

    void FixedUpdate()
    {
        UpdateMatrix();
    }

    void UpdateMatrix()
    {
        GameObject pecaAmb = GameObject.Find(GetPecaAmb());
        if (pecaAmb != null)
        {
            matrix4x4.text = pecaAmb.transform.worldToLocalMatrix.ToString();
        }
    }

    public void Inicializa(PropriedadePeca propObjGrafico)
    {
        propPeca = propObjGrafico;
        propPeca.Ativo = true;

        PreencheCampos();
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(this.propPeca.NomePeca))
        {
            Global.propriedadePecas[this.propPeca.NomePeca] = propPeca;
        }    
    }

    void PreencheCampos()
    {
        if (Global.propriedadePecas.ContainsKey(this.propPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                nomePeca.text = propPeca.Nome;
                ativo.isOn = propPeca.Ativo;
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
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(this.propPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                Global.propriedadePecas[this.propPeca.NomePeca].Ativo = ativo.isOn;

                MeshRenderer meshRenderAmb = null, meshRenderVis = null;

                string pecaName = GetPecaAmb();
                string numSlot = this.propPeca.NomePeca.Substring(this.propPeca.NomePeca.IndexOf("P") + 1);

                if (!string.Empty.Equals(pecaName))
                {
                    if (pecaName.Contains(Consts.CUBO))
                    {
                        meshRenderAmb = GameObject.Find((Global.propriedadePecas[Consts.CUBO + numSlot] as CuboPropriedadePeca).NomeCuboAmb).GetComponent<MeshRenderer>();
                        meshRenderVis = GameObject.Find((Global.propriedadePecas[Consts.CUBO + numSlot] as CuboPropriedadePeca).NomeCuboVis).GetComponent<MeshRenderer>();
                    }
                    else if (pecaName.Contains(Consts.POLIGONO))
                    {
                        meshRenderAmb = GameObject.Find((Global.propriedadePecas[Consts.POLIGONO + numSlot] as PoligonoPropriedadePeca).PoligonoAmb).GetComponent<MeshRenderer>();
                        meshRenderVis = GameObject.Find((Global.propriedadePecas[Consts.POLIGONO + numSlot] as PoligonoPropriedadePeca).PoligonoVis).GetComponent<MeshRenderer>();
                    }
                    else if (pecaName.Contains(Consts.SPLINE))
                    {
                        meshRenderAmb = GameObject.Find((Global.propriedadePecas[Consts.SPLINE + numSlot] as SplinePropriedadePeca).SplineAmb).GetComponent<MeshRenderer>();
                        meshRenderVis = GameObject.Find((Global.propriedadePecas[Consts.SPLINE + numSlot] as SplinePropriedadePeca).SplineVis).GetComponent<MeshRenderer>();
                    }

                    if (meshRenderAmb != null)
                    {
                        meshRenderAmb.enabled = Global.propriedadePecas[this.propPeca.NomePeca].Ativo;
                    }
                    if (meshRenderVis != null)
                    {
                        meshRenderVis.enabled = Global.propriedadePecas[this.propPeca.NomePeca].Ativo && Global.cameraAtiva;
                    }
                }

                AtualizaListaProp();
            }
            finally
            {
                podeAtualizar = true;
            }
        }
    }

    string GetPecaAmb()
    {
        if (this.propPeca != null)
        {
            string numSlot = this.propPeca.NomePeca.Substring(this.propPeca.NomePeca.IndexOf("P") + 1);

            if (Global.listaEncaixes.ContainsKey(Consts.CUBO + numSlot))
            {
                return Consts.CUBO + Consts.AMB + numSlot;
            }
            else if (Global.listaEncaixes.ContainsKey(Consts.POLIGONO + numSlot))
            {
                return Consts.POLIGONO + Consts.AMB + numSlot;
            }
            else if (Global.listaEncaixes.ContainsKey(Consts.SPLINE + numSlot))
            {
                return Consts.SPLINE + Consts.AMB + numSlot;
            }
        }

        return string.Empty;
    }
}
