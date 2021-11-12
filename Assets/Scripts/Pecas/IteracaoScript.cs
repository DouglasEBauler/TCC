using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IteracaoScript : MonoBehaviour
{
    const float TIMER = 580f;

    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject propriedade;
    [SerializeField]
    PropRotacionarScript propriedadeRotacionar;
    [SerializeField]
    PropTransladarScript propriedadeTransladar;
    [SerializeField]
    PropEscalarScript propriedadeEscalar;
    [SerializeField]
    GameObject menuControl;
    [SerializeField]
    GameObject panelArquivo;
    [SerializeField]
    GameObject panelPropPeca;
    [SerializeField]
    GameObject panelAjuda;
    [SerializeField]
    Tutorial tutorialScript;

    [HideInInspector]
    public GameObject slot;

    Vector3 offset, scanPos, startPos;
    float timer;
    IteracaoPropriedadePeca propIteracao;

    void Start()
    {
        scanPos = startPos = gameObject.transform.position;
        timer = TIMER;
    }

    void FixedUpdate()
    {
        scanPos = gameObject.transform.position;

        Iteracoes();
    }

    void Iteracoes()
    {
        if (EstaEncaixado() && Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            string numSlot = Util_VisEdu.GetNumSlot(slot.name, true);

            if (Global.propriedadePecas.ContainsKey(Consts.ROTACIONAR + numSlot))
            {
                StartCoroutine(IteracaoRotacionar());
            }
            else if (Global.propriedadePecas.ContainsKey(Consts.TRANSLADAR + numSlot))
            {
                StartCoroutine(IteracaoTransladar());
            }
            else if (Global.propriedadePecas.ContainsKey(Consts.ESCALAR + numSlot))
            {
                StartCoroutine(IteracaoEscalar());
            }
        }
    }

    IEnumerator IteracaoRotacionar()
    {
        if (this.propIteracao != null && this.propIteracao.Ativo)
        {
            TransformacaoPropriedadePeca transformacao = Global.propriedadePecas[this.propIteracao.NomeTransformacao] as TransformacaoPropriedadePeca;
            if (transformacao != null)
            {
                while (timer > 0)
                {
                    timer--;
                    yield return new WaitForSeconds(1f);
                }

                if (this.propIteracao.AtivoX && (this.propIteracao.Intervalo.X + transformacao.Pos.X) < this.propIteracao.Max.X)
                {
                    if ((this.propIteracao.Intervalo.X + transformacao.Pos.X) > this.propIteracao.Max.X)
                    {
                        transformacao.Pos.X = this.propIteracao.Max.X;
                    }
                    else
                    {
                        transformacao.Pos.X += this.propIteracao.Intervalo.X;
                    }
                }
                else
                {
                    transformacao.Pos.X = this.propIteracao.Min.X;
                }

                if (this.propIteracao.AtivoY && (this.propIteracao.Intervalo.X + transformacao.Pos.Y) < this.propIteracao.Max.Y)
                {
                    if ((this.propIteracao.Intervalo.Y + transformacao.Pos.Y) > this.propIteracao.Max.Y)
                    {
                        transformacao.Pos.Y = this.propIteracao.Max.Y;
                    }
                    else
                    {
                        transformacao.Pos.Y += this.propIteracao.Intervalo.Y;
                    }
                }
                else
                {
                    transformacao.Pos.Y = this.propIteracao.Min.Y;
                }

                if (this.propIteracao.AtivoZ && (this.propIteracao.Intervalo.X + transformacao.Pos.Z) < this.propIteracao.Max.Z)
                {
                    if ((this.propIteracao.Intervalo.Z + transformacao.Pos.Z) > this.propIteracao.Max.Z)
                    {
                        transformacao.Pos.Z = this.propIteracao.Min.Z;
                    }
                    else
                    {
                        transformacao.Pos.Z += this.propIteracao.Intervalo.Z;
                    }
                }
                else
                {
                    transformacao.Pos.Z = this.propIteracao.Min.Z;
                }

                Global.propriedadePecas[transformacao.NomePeca] = transformacao;
                propriedadeRotacionar.Inicializa(transformacao);
                propriedadeRotacionar.UpdateProp(true);
                timer = TIMER;

                //yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator IteracaoTransladar()
    {
        if (this.propIteracao != null && this.propIteracao.Ativo)
        {
            TransformacaoPropriedadePeca transformacao = Global.propriedadePecas[this.propIteracao.NomeTransformacao] as TransformacaoPropriedadePeca;
            if (transformacao != null)
            {
                while (timer > 0)
                {
                    timer--;
                    yield return new WaitForSeconds(1f);
                }

                if (this.propIteracao.AtivoX && (this.propIteracao.Intervalo.X + transformacao.Pos.X) < this.propIteracao.Max.X)
                {
                    if ((this.propIteracao.Intervalo.X + transformacao.Pos.X) > this.propIteracao.Max.X)
                    {
                        transformacao.Pos.X = this.propIteracao.Max.X;
                    }
                    else
                    {
                        transformacao.Pos.X += this.propIteracao.Intervalo.X;
                    }
                }
                else
                {
                    transformacao.Pos.X = this.propIteracao.Min.X;
                }

                if (this.propIteracao.AtivoY && (this.propIteracao.Intervalo.X + transformacao.Pos.Y) < this.propIteracao.Max.Y)
                {
                    if ((this.propIteracao.Intervalo.Y + transformacao.Pos.Y) > this.propIteracao.Max.Y)
                    {
                        transformacao.Pos.Y = this.propIteracao.Max.Y;
                    }
                    else
                    {
                        transformacao.Pos.Y += this.propIteracao.Intervalo.Y;
                    }
                }
                else
                {
                    transformacao.Pos.Y = this.propIteracao.Min.Y;
                }

                if (this.propIteracao.AtivoZ && (this.propIteracao.Intervalo.X + transformacao.Pos.Z) < this.propIteracao.Max.Z)
                {
                    if ((this.propIteracao.Intervalo.Z + transformacao.Pos.Z) > this.propIteracao.Max.Z)
                    {
                        transformacao.Pos.Z = this.propIteracao.Min.Z;
                    }
                    else
                    {
                        transformacao.Pos.Z += this.propIteracao.Intervalo.Z;
                    }
                }
                else
                {
                    transformacao.Pos.Z = this.propIteracao.Min.Z;
                }

                Global.propriedadePecas[transformacao.NomePeca] = transformacao;
                propriedadeTransladar.Inicializa(transformacao);
                propriedadeTransladar.UpdateProp(true);
                timer = TIMER;
            }
        }
    }
    IEnumerator IteracaoEscalar()
    {
        if (this.propIteracao != null && this.propIteracao.Ativo)
        {
            TransformacaoPropriedadePeca transformacao = Global.propriedadePecas[this.propIteracao.NomeTransformacao] as TransformacaoPropriedadePeca;
            if (transformacao != null)
            {
                while (timer > 0)
                {
                    timer--;
                    yield return new WaitForSeconds(1f);
                }

                if (this.propIteracao.AtivoX && (this.propIteracao.Intervalo.X + transformacao.Pos.X) < this.propIteracao.Max.X)
                {
                    if ((this.propIteracao.Intervalo.X + transformacao.Pos.X) > this.propIteracao.Max.X)
                    {
                        transformacao.Pos.X = this.propIteracao.Max.X;
                    }
                    else
                    {
                        transformacao.Pos.X += this.propIteracao.Intervalo.X;
                    }
                }
                else
                {
                    transformacao.Pos.X = this.propIteracao.Min.X + 1;
                }

                if (this.propIteracao.AtivoY && (this.propIteracao.Intervalo.X + transformacao.Pos.Y) < this.propIteracao.Max.Y)
                {
                    if ((this.propIteracao.Intervalo.Y + transformacao.Pos.Y) > this.propIteracao.Max.Y)
                    {
                        transformacao.Pos.Y = this.propIteracao.Max.Y;
                    }
                    else
                    {
                        transformacao.Pos.Y += this.propIteracao.Intervalo.Y;
                    }
                }
                else
                {
                    transformacao.Pos.Y = this.propIteracao.Min.Y + 1;
                }

                if (this.propIteracao.AtivoZ && (this.propIteracao.Intervalo.X + transformacao.Pos.Z) < this.propIteracao.Max.Z)
                {
                    if ((this.propIteracao.Intervalo.Z + transformacao.Pos.Z) > this.propIteracao.Max.Z)
                    {
                        transformacao.Pos.Z = this.propIteracao.Min.Z;
                    }
                    else
                    {
                        transformacao.Pos.Z += this.propIteracao.Intervalo.Z;
                    }
                }
                else
                {
                    transformacao.Pos.Z = this.propIteracao.Min.Z + 1;
                }

                Global.propriedadePecas[transformacao.NomePeca] = transformacao;
                propriedadeEscalar.Inicializa(transformacao);
                propriedadeEscalar.UpdateProp(true);
                timer = TIMER;
            }
        }
    }

    void OnMouseDown()
    {
        offset = scanPos - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));

        CopiaPeca();
        ConfiguraPropriedadePeca();
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f);
        Vector3 curPosition = cam.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    void OnMouseUp()
    {
        if (PodeEncaixar())
        {
            AddIteracao();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(Consts.ITERACAO_SLOT))
        {
            slot = other.gameObject;
        }
    }

    bool PodeGerarCopia()
    {
        return cam.WorldToScreenPoint(scanPos).y > cam.pixelRect.height / 2;
    }

    void AjustaPeca()
    {
        if (!tutorialScript.EstaExecutandoTutorial && !CheckPanelIsActive())
        {
            bool podeDestruir = PodeGerarCopia();

            if (podeDestruir && !EstaEncaixado())
            {
                StartCoroutine(RemovePeca());
            }
            else if (EstaEncaixado())
            {
                Encaixa();
            }
            else
            {
                StartCoroutine(RemovePeca());
            }
        }
    }

    public IEnumerator RemovePeca()
    {
        while ((transform.position.y != startPos.y && transform.position.x != startPos.x))
        {
            transform.position =
                Vector3.Lerp(transform.position, startPos, Time.deltaTime * Consts.SPEED_DESLOC / Vector3.Distance(transform.position, startPos));

            yield return null;
        }

        Destroy(gameObject);
    }


    bool CheckPanelIsActive()
    {
        return panelArquivo.activeSelf || panelAjuda.activeSelf;
    }

    public void Encaixa()
    {
        StartCoroutine(EncaixaPecaAoSlot());
    }


    IEnumerator EncaixaPecaAoSlot()
    {
        if (!tutorialScript.EstaExecutandoTutorial)
        {
            while ((transform.position.y != slot.transform.position.y && transform.position.x != slot.transform.position.x))
            {
                transform.position =
                    Vector3.Lerp(transform.position, slot.transform.position, Time.deltaTime * Consts.SPEED_DESLOC / Vector3.Distance(transform.position, slot.transform.position));

                yield return null;
            }

            transform.parent = slot.transform;
            gameObject.GetComponentInChildren<RawImage>().texture = slot.GetComponentInChildren<RawImage>().texture;
        }
    }

    public void AddIteracao(IteracaoPropriedadePeca propPeca = null)
    {
        Encaixa();
        CreatePropPeca(propPeca);
    }

    public void ConfiguraPropriedadePeca(IteracaoPropriedadePeca propPeca = null, PropriedadeCamera camProp = null)
    {
        if (EstaEncaixado())
        {
            CreatePropPeca(propPeca);

            propriedade.GetComponent<PropIteracao>().Inicializa(this.propIteracao);
            menuControl.GetComponent<MenuScript>().EnablePanelProp(propriedade.name);
        }
    }

    public void CreatePropPeca(IteracaoPropriedadePeca propPeca = null)
    {
        if (EstaEncaixado() && !Global.propriedadePecas.ContainsKey(gameObject.name))
        {
            if (propPeca == null)
            {
                TransformacaoPropriedadePeca transformacao = GetTransformacao();

                this.propIteracao = new IteracaoPropriedadePeca()
                {
                    NomeTransformacao = transformacao.NomePeca
                };
            }
            else
            {
                this.propIteracao = propPeca;
            }
            this.propIteracao.NomePeca = gameObject.name;

            Global.propriedadePecas.Add(this.propIteracao.NomePeca, this.propIteracao);
        }
    }

    TransformacaoPropriedadePeca GetTransformacao()
    {
        string numSlot = Util_VisEdu.GetNumSlot(slot.name, true);

        if (Global.propriedadePecas.ContainsKey(Consts.ROTACIONAR + numSlot))
        {
            return Global.propriedadePecas[Consts.ROTACIONAR + numSlot] as TransformacaoPropriedadePeca;
        }

        if (Global.propriedadePecas.ContainsKey(Consts.TRANSLADAR + numSlot))
        {
            return Global.propriedadePecas[Consts.TRANSLADAR + numSlot] as TransformacaoPropriedadePeca;
        }

        if (Global.propriedadePecas.ContainsKey(Consts.ESCALAR + numSlot))
        {
            return Global.propriedadePecas[Consts.ESCALAR + numSlot] as TransformacaoPropriedadePeca;
        }

        return null;
    }

    public bool PodeEncaixar()
    {
        if ((slot != null)
            && (Vector3.Distance(slot.transform.position, gameObject.transform.position) < 4)
            && (GetTransformacao() != null)
            && !EstaEncaixado())
        {
            string numSlot = Util_VisEdu.GetNumSlot(slot.name, true);

            gameObject.name += numSlot;

            Destroy(slot.GetComponent<Rigidbody>());
            if (!EstaEncaixado())
            {
                Global.listaEncaixes.Add(gameObject.name, slot.name);
            }
            else
            {
                Global.listaEncaixes[gameObject.name] = slot.name;
                return false;
            }

            return true;
        }
        else
        {
            AjustaPeca();
            return false;
        }
    }

    public void CopiaPeca()
    {
        if (PodeGerarCopia() && !EstaEncaixado())
        {
            GameObject cloneFab = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
            cloneFab.name = Consts.ITERACAO;
            cloneFab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    bool EstaEncaixado()
    {
        return Global.listaEncaixes.ContainsKey(gameObject.name);
    }
}
