using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropIluminacaoScript : MonoBehaviour
{
    const float VALOR_INICIAL_ROTACAO_X = 71.819f;
    const float VALOR_INICIAL_ROTACAO_Y = 90f;

    [SerializeField]
    InputField nome;
    [SerializeField]
    Dropdown tipoLuz;
    [SerializeField]
    InputField posX;
    [SerializeField]
    InputField posY;
    [SerializeField]
    InputField posZ;
    [SerializeField]
    InputField itensidade;
    [SerializeField]
    InputField distancia;
    [SerializeField]
    InputField angulo;
    [SerializeField]
    InputField expoente;
    [SerializeField]
    FlexibleColorPicker corSelecionado;
    [SerializeField]
    GameObject seletorCor;
    [SerializeField]
    InputField valorX;
    [SerializeField]
    InputField valorY;
    [SerializeField]
    InputField valorZ;
    [SerializeField]
    Toggle ativo;

    [SerializeField]
    Toggle lockPosX;
    [SerializeField]
    Toggle lockPosY;
    [SerializeField]
    Toggle lockPosZ;
    [SerializeField]
    Toggle lockItensidade;
    [SerializeField]
    Toggle lockDistancia;
    [SerializeField]
    Toggle lockAngulo;
    [SerializeField]
    Toggle lockExpoente;
    [SerializeField]
    Toggle lockValX;
    [SerializeField]
    Toggle lockValY;
    [SerializeField]
    Toggle lockValZ;

    [SerializeField]
    GameObject grupoItensidadeDistancia;
    [SerializeField]
    GameObject objItensidade;
    [SerializeField]
    GameObject objDistancia;
    [SerializeField]
    GameObject grupoAnguloExpoente;
    [SerializeField]
    GameObject objAngulo;
    [SerializeField]
    GameObject objExpoente;
    [SerializeField]
    GameObject objValores;

    IluminacaoPropriedadePeca prPeca;
    Dictionary<Property, InputField> propList;
    Dictionary<Property, Toggle> lockList;
    bool podeAtualizar;

    void FixedUpdate()
    {
        if (prPeca != null)
        {
            EnableLabelZ();
            UpdateColor();
        }
    }

    public void Inicializa(IluminacaoPropriedadePeca propIluminacao)
    {
        prPeca = propIluminacao;
        prPeca.Ativo = true;

        PreencheCampos();
    }

    void InicializaListas()
    {
        propList = new Dictionary<Property, InputField>()
        {
            { Property.PosX, posX },
            { Property.PosY, posY },
            { Property.PosZ, posZ },

            { Property.Intensidade, itensidade },
            { Property.Distancia, distancia },
            { Property.Angulo, angulo },
            { Property.Expoente, expoente },

            { Property.ValorX, valorX },
            { Property.ValorY, valorY },
            { Property.ValorZ, valorZ }
        };

        lockList = new Dictionary<Property, Toggle>()
        {
            { Property.PosX, lockPosX },
            { Property.PosY, lockPosY },
            { Property.PosZ, lockPosZ },

            { Property.Intensidade, lockItensidade },
            { Property.Distancia, lockDistancia },
            { Property.Angulo, lockAngulo },
            { Property.Expoente, lockExpoente },

            { Property.ValorX, lockValX },
            { Property.ValorY, lockValY },
            { Property.ValorZ, lockValZ }
        };
    }

    void AtualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
        {
            Global.propriedadePecas[this.prPeca.NomePeca] = prPeca;
        }
    }

    void UpdateColor()
    {
        this.prPeca.Cor = corSelecionado.color;
        seletorCor.GetComponent<Image>().material.color = this.prPeca.Cor;
        seletorCor.GetComponent<Image>().material.SetColor("_EmissionColor", this.prPeca.Cor);
    }

    public void ControlCorPanel()
    {
        corSelecionado.gameObject.SetActive(!corSelecionado.gameObject.activeSelf);
    }

    void EnableLabelZ()
    {
        GameObject GOLuzAmbiente = GameObject.Find("LuzAmbiente");
        GameObject GOLuzDirectional = GameObject.Find("LuzDirectional");
        GameObject GOLuzPoint = GameObject.Find("LuzPoint");
        GameObject GOLuzSpot = GameObject.Find("LuzSpot");

        float posZ = 0.000111648f;

        if (Global.Grafico2D)
        {
            if (GOLuzAmbiente != null)
                GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x, GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, 100000);

            if (GOLuzDirectional != null)
            {
                GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x, GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, 100000);

                GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition.x, GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition.y, 100000);
            }

            if (GOLuzPoint != null)
                GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x, GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, 100000);

            if (GOLuzSpot != null)
            {
                GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x, GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, 100000);

                GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition.x, GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition.y, 100000);
            }
        }
        else
        {
            if (GOLuzAmbiente != null)
                GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x, GOLuzAmbiente.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, posZ);

            if (GOLuzDirectional != null)
            {
                GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x, GOLuzDirectional.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, posZ);

                GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition.x, GOLuzDirectional.transform.GetChild(1).GetChild(1).GetChild(2).transform.localPosition.y, posZ);
            }

            if (GOLuzPoint != null)
                GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x, GOLuzPoint.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, posZ);

            if (GOLuzSpot != null)
            {
                GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.x, GOLuzSpot.transform.GetChild(0).GetChild(0).GetChild(2).transform.localPosition.y, posZ);

                GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition =
                    new Vector3(GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition.x, GOLuzSpot.transform.GetChild(1).GetChild(4).GetChild(2).transform.localPosition.y, posZ);
            }
        }
    }

    public void UpdateProp()
    {
        UpdateProp(InputSelectedIluminacao.None);
    }

    public void UpdateProp(int inputSelected)
    {
        UpdateProp((InputSelectedIluminacao)inputSelected);
    }

    public void UpdateProp(InputSelectedIluminacao inputSelected)
    {
        if (podeAtualizar && Global.propriedadePecas.ContainsKey(this.prPeca.NomePeca))
        {
            podeAtualizar = false;
            try
            {
                prPeca.Nome = nome.text;
                prPeca.Pos.X = Util_VisEdu.ConvertField(posX.text);
                prPeca.Pos.Y = Util_VisEdu.ConvertField(posY.text);
                prPeca.Pos.Z = Util_VisEdu.ConvertField(posZ.text);
                prPeca.Intensidade = Util_VisEdu.ConvertField(itensidade.text);
                prPeca.ValorIluminacao.X = Util_VisEdu.ConvertField(valorX.text);
                prPeca.ValorIluminacao.Y = Util_VisEdu.ConvertField(valorY.text);
                prPeca.ValorIluminacao.Z = Util_VisEdu.ConvertField(valorZ.text);
                prPeca.Distancia = Util_VisEdu.ConvertField(distancia.text);
                prPeca.Angulo = Util_VisEdu.ConvertField(angulo.text);
                prPeca.Expoente = Util_VisEdu.ConvertField(expoente.text);
                prPeca.TipoLuz = (TipoIluminacao)tipoLuz.value;
                prPeca.Ativo = ativo.isOn;

                GameObject lightObject = null;
                bool podeAtualizarValor = false;

                switch (prPeca.TipoLuz)
                {
                    case TipoIluminacao.Ambiente:
                        lightObject = GameObject.Find("Ambiente" + this.prPeca.NomePeca);

                        foreach (Transform child in lightObject.transform)
                        {
                            child.GetComponent<Light>().color = prPeca.Cor;
                            child.GetComponent<Light>().enabled = prPeca.Ativo;
                        }

                        AtivaCamera();

                        break;

                    case TipoIluminacao.Directional:
                        lightObject = GameObject.Find("Directional" + this.prPeca.NomePeca);

                        //Se o Valor X estiver sendo alterado, atualiza Y e Z com o mesmo valor
                        if (InputSelectedIluminacao.ValorX.Equals(inputSelected))
                        {
                            podeAtualizarValor = true;

                            prPeca.ValorIluminacao.Y = prPeca.ValorIluminacao.X;
                            prPeca.ValorIluminacao.Z = prPeca.ValorIluminacao.X;
                            valorX.text = prPeca.ValorIluminacao.X.ToString();
                            valorY.text = prPeca.ValorIluminacao.X.ToString();
                        }
                        //Se o Valor Y estiver sendo alterado, atualiza X e Z com o mesmo valor
                        else if (InputSelectedIluminacao.ValorY.Equals(inputSelected))
                        {
                            podeAtualizarValor = true;

                            prPeca.ValorIluminacao.X = prPeca.ValorIluminacao.Y;
                            prPeca.ValorIluminacao.Z = prPeca.ValorIluminacao.Y;
                            valorX.text = prPeca.ValorIluminacao.Y.ToString();
                            valorZ.text = prPeca.ValorIluminacao.Y.ToString();
                        }
                        //Se o Valor Z estiver sendo alterado, atualiza X e Y com o mesmo valor
                        else if (InputSelectedIluminacao.ValorY.Equals(inputSelected))
                        {
                            podeAtualizarValor = true;

                            prPeca.ValorIluminacao.X = prPeca.ValorIluminacao.Z;
                            prPeca.ValorIluminacao.Y = prPeca.ValorIluminacao.Z;
                            valorX.text = prPeca.ValorIluminacao.Z.ToString();
                            valorY.text = prPeca.ValorIluminacao.Z.ToString();
                        }

                        if (podeAtualizarValor)
                        {
                            lightObject.GetComponent<CameraObjectScript>().enabled =
                                0.Equals(prPeca.ValorIluminacao.X) && 0.Equals(prPeca.ValorIluminacao.Y) && 0.Equals(prPeca.ValorIluminacao.Z);

                            lightObject.transform.localRotation =
                                Quaternion.Euler(prPeca.ValorIluminacao.X + VALOR_INICIAL_ROTACAO_X, VALOR_INICIAL_ROTACAO_Y, 0);
                        }

                        break;

                    case TipoIluminacao.Point:
                        lightObject = GameObject.Find("Point" + this.prPeca.NomePeca);
                        lightObject.GetComponent<Light>().range = prPeca.Distancia;
                        break;

                    case TipoIluminacao.Spot:
                        lightObject = GameObject.Find("Spot" + this.prPeca.NomePeca);
                        lightObject.GetComponent<Light>().range = prPeca.Distancia;

                        // Altera escala de acordo com o range.
                        GameObject objSpot = GameObject.Find("ObjSpot" + this.prPeca.NomePeca);

                        float scaleObjSpot = prPeca.Distancia / 1000;

                        objSpot.transform.localScale = new Vector3(scaleObjSpot, scaleObjSpot, scaleObjSpot);
                        objSpot.transform.localPosition = Vector3.zero;

                        lightObject.GetComponent<Light>().spotAngle = prPeca.Angulo;

                        // Altera o ângulo de acordo com o SpotAngle.
                        GameObject SpotIlum = GameObject.Find("Spot" + this.prPeca.NomePeca);

                        float scaleMeshSpot = prPeca.Angulo * 0.033f;

                        SpotIlum.transform.localScale = new Vector3(scaleMeshSpot, scaleMeshSpot, SpotIlum.transform.localScale.z);

                        //Se o Valor X estiver sendo alterado, atualiza Y e Z com o mesmo valor
                        if (InputSelectedIluminacao.ValorX.Equals(inputSelected))
                        {
                            podeAtualizarValor = true;

                            prPeca.ValorIluminacao.Y = prPeca.ValorIluminacao.X;
                            prPeca.ValorIluminacao.Z = prPeca.ValorIluminacao.X;
                            valorY.text = prPeca.ValorIluminacao.X.ToString();
                            valorZ.text = prPeca.ValorIluminacao.X.ToString();
                        }
                        //Se o Valor Y estiver sendo alterado, atualiza X e Z com o mesmo valor
                        else if (InputSelectedIluminacao.ValorY.Equals(inputSelected))
                        {
                            podeAtualizarValor = true;

                            prPeca.ValorIluminacao.X = prPeca.ValorIluminacao.Y;
                            prPeca.ValorIluminacao.Z = prPeca.ValorIluminacao.Y;
                            valorX.text = prPeca.ValorIluminacao.Y.ToString();
                            valorZ.text = prPeca.ValorIluminacao.Y.ToString();
                        }
                        //Se o Valor Z estiver sendo alterado, atualiza X e Y com o mesmo valor
                        else if (InputSelectedIluminacao.ValorZ.Equals(inputSelected))
                        {
                            podeAtualizarValor = true;

                            prPeca.ValorIluminacao.X = prPeca.ValorIluminacao.Z;
                            prPeca.ValorIluminacao.Y = prPeca.ValorIluminacao.Z;
                            valorX.text = prPeca.ValorIluminacao.Z.ToString();
                            valorY.text = prPeca.ValorIluminacao.Z.ToString();
                        }

                        if (podeAtualizarValor)
                        {
                            lightObject.GetComponent<CameraObjectScript>().enabled =
                                0.Equals(prPeca.ValorIluminacao.X) && 0.Equals(prPeca.ValorIluminacao.Y) && 0.Equals(prPeca.ValorIluminacao.Z);

                            lightObject.transform.localRotation =
                                Quaternion.Euler(prPeca.ValorIluminacao.X + VALOR_INICIAL_ROTACAO_X, VALOR_INICIAL_ROTACAO_Y, 0);
                        }
                        break;
                }

                // Propriedades comuns entre todas iluminações, exceto iluminação "Ambiente".
                if (!TipoIluminacao.Ambiente.Equals(prPeca.TipoLuz))
                {
                    lightObject.transform.localPosition = new Vector3(-prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
                    lightObject.GetComponent<Light>().color = prPeca.Cor;
                    lightObject.GetComponent<Light>().intensity = prPeca.Intensidade;
                    AtivaIluminacao(GetTipoLuzPorExtenso(prPeca.TipoLuz) + this.prPeca.NomePeca);
                }

                UpdateAllLockFields();
                AtivaCamera();
                AtualizaListaProp();
            }
            finally
            {
                podeAtualizar = true;
                EnabledFields();
            }
        }
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

    public void AjustaCameraEmX()
    {
        GameObject go = GameObject.Find("CameraObjetoMain");
        go.transform.GetComponent<RectTransform>().transform.position =
            new Vector3(go.transform.GetComponent<RectTransform>().transform.position.x + 1,
                        go.transform.GetComponent<RectTransform>().transform.position.y,
                        go.transform.GetComponent<RectTransform>().transform.position.z);
    }

    public void PreencheCampos()
    {
        GameObject lightObject = null;
        podeAtualizar = false;
        try
        {
            nome.text = prPeca.Nome;
            posX.text = prPeca.Pos.X.ToString();
            posY.text = prPeca.Pos.Y.ToString();
            posZ.text = prPeca.Pos.Z.ToString();
            corSelecionado.color = prPeca.Cor;
            itensidade.text = prPeca.Intensidade.ToString();
            distancia.text = prPeca.Distancia.ToString();
            angulo.text = prPeca.Angulo.ToString();
            expoente.text = prPeca.Expoente.ToString();
            valorX.text = prPeca.ValorIluminacao.X.ToString();
            valorY.text = prPeca.ValorIluminacao.Y.ToString();
            valorZ.text = prPeca.ValorIluminacao.Z.ToString();
            ativo.isOn = prPeca.Ativo;

            switch (prPeca.TipoLuz)
            {
                case TipoIluminacao.Ambiente:
                    tipoLuz.value = 0;
                    AtivaCamera();

                    break;

                case TipoIluminacao.Directional:
                    tipoLuz.value = 1;

                    // Alterações do Objeto e Iluminação "Directional".
                    lightObject = GameObject.Find("Directional" + this.prPeca.NomePeca);
                    lightObject.transform.localPosition = new Vector3(-prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
                    lightObject.GetComponent<Light>().color = prPeca.Cor;
                    lightObject.GetComponent<Light>().intensity = prPeca.Intensidade;

                    AtivaCamera();

                    break;

                case TipoIluminacao.Point:
                    tipoLuz.value = 2;

                    // Alterações do Objeto e Iluminação "Point".
                    lightObject = GameObject.Find("Point" + this.prPeca.NomePeca);
                    lightObject.transform.localPosition = new Vector3(-prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
                    lightObject.GetComponent<Light>().color = prPeca.Cor;
                    lightObject.GetComponent<Light>().intensity = prPeca.Intensidade;
                    lightObject.GetComponent<Light>().range = prPeca.Distancia;

                    AtivaCamera();

                    break;

                case TipoIluminacao.Spot:
                    tipoLuz.value = 3;

                    // Alterações do Objeto e Iluminação "Spot".
                    lightObject = GameObject.Find("Spot" + this.prPeca.NomePeca);
                    lightObject.transform.localPosition = new Vector3(-prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
                    lightObject.GetComponent<Light>().color = prPeca.Cor;
                    lightObject.GetComponent<Light>().intensity = prPeca.Intensidade;
                    lightObject.GetComponent<Light>().range = prPeca.Distancia;

                    AtivaCamera();

                    // Altera escala de acordo com o range.
                    float scaleObjSpot = prPeca.Distancia / 1000;
                    Transform objSpot = lightObject.transform.GetChild(0).transform;
                    objSpot.localScale = new Vector3(scaleObjSpot, scaleObjSpot, scaleObjSpot);
                    objSpot.localPosition = Vector3.zero;

                    break;
            }

            if (lightObject != null)
            {
                AtivaIluminacao(lightObject.name);
            }
        }
        finally
        {
            podeAtualizar = true;
            UpdateProp();
        }
    }

    public string GetTipoLuzPorExtenso(TipoIluminacao tipoLuz)
    {
        switch (tipoLuz)
        {
            case TipoIluminacao.Ambiente: return "Ambiente";
            case TipoIluminacao.Directional: return "Directional";
            case TipoIluminacao.Point: return "Point";
            case TipoIluminacao.Spot: return "Spot";
        }

        return string.Empty;
    }

    public void AtivaIluminacao(string nomeIluminacao)
    {
        if (!nomeIluminacao.Contains("Ambiente"))
        {
            GameObject.Find(nomeIluminacao).GetComponent<Light>().enabled = prPeca.Ativo;

            GameObject GO = GameObject.Find(nomeIluminacao).transform.GetChild(0).gameObject;

            if (nomeIluminacao.Contains("Spot"))
            {
                GO.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = ativo;

                foreach (Transform child in GO.transform.GetChild(0).GetChild(0))
                {
                    child.GetComponent<MeshRenderer>().enabled = ativo;
                }
            }
            else
            {
                foreach (Transform child in GO.transform)
                {
                    child.GetComponent<MeshRenderer>().enabled = ativo;
                }
            }
        }
        else
        {
            foreach (Transform child in GameObject.Find(nomeIluminacao).transform)
            {
                child.GetComponent<Light>().enabled = ativo;
            }
        }
    }

    public void AtivaCamera()
    {
        GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask =
            1 << LayerMask.NameToLayer((prPeca.Ativo && Global.cameraAtiva) ? "Formas" : "Nothing");
    }

    public void EnabledFields()
    {
        podeAtualizar = false;
        try
        {
            TipoIluminacao tipo = (TipoIluminacao)tipoLuz.value;

            switch (tipo)
            {
                case TipoIluminacao.Ambiente:
                    objItensidade.SetActive(false);
                    objDistancia.SetActive(false);
                    objAngulo.SetActive(false);
                    objExpoente.SetActive(false);
                    objValores.SetActive(false);
                    break;

                case TipoIluminacao.Directional:
                    objItensidade.SetActive(true);
                    objDistancia.SetActive(false);
                    objAngulo.SetActive(false);
                    objExpoente.SetActive(false);
                    objValores.SetActive(true);
                    break;

                case TipoIluminacao.Point:
                    objItensidade.SetActive(true);
                    objDistancia.SetActive(true);
                    objAngulo.SetActive(false);
                    objExpoente.SetActive(false);
                    objValores.SetActive(false);
                    break;

                case TipoIluminacao.Spot:
                    objItensidade.SetActive(true);
                    objDistancia.SetActive(true);
                    objAngulo.SetActive(true);
                    objExpoente.SetActive(true);
                    objValores.SetActive(true);
                    break;
            }

            grupoItensidadeDistancia.SetActive(objItensidade.activeSelf || objDistancia.activeSelf);
            grupoAnguloExpoente.SetActive(objAngulo.activeSelf || objExpoente.activeSelf);
        }
        finally
        {
            podeAtualizar = true;
        }
    }

    void AtivaIluminacao(TipoIluminacao tipo)
    {
        Light lightDirectional = GameObject.Find("Directional" + this.prPeca.NomePeca).GetComponent<Light>();
        Light lightPoint = GameObject.Find("Point" + this.prPeca.NomePeca).GetComponent<Light>();
        Light lightSpot = GameObject.Find("Spot" + this.prPeca.NomePeca).GetComponent<Light>();

        switch (tipo)
        {
            case TipoIluminacao.Ambiente:
                LightAmbienteEnable(true);
                lightDirectional.enabled = false;
                lightPoint.enabled = false;
                lightSpot.enabled = false;

                EnableMeshRendered(false, false, false);

                break;
            
            case TipoIluminacao.Directional:
                LightAmbienteEnable(false);
                lightDirectional.enabled = true;
                lightPoint.enabled = false;
                lightSpot.enabled = false;

                if (prPeca.Ativo)
                    EnableMeshRendered(true, false, false);
                else
                    EnableMeshRendered(false, false, false);
                
                break;

            case TipoIluminacao.Point:
                LightAmbienteEnable(false);
                lightDirectional.enabled = false;
                lightPoint.enabled = true;
                lightSpot.enabled = false;

                if (prPeca.Ativo)
                    EnableMeshRendered(false, true, false);
                else
                    EnableMeshRendered(false, false, false);
            
                break;
            
            case TipoIluminacao.Spot:
                LightAmbienteEnable(false);
                lightDirectional.enabled = false;
                lightPoint.enabled = false;
                lightSpot.enabled = true;

                if (prPeca.Ativo)
                    EnableMeshRendered(false, false, true);
                else
                    EnableMeshRendered(false, false, false);

                break;
        }
    }

    void EnableMeshRendered(bool directional, bool point, bool spot)
    {
        GameObject goAmbiente = GameObject.Find("Ambiente" + this.prPeca.NomePeca);
        GameObject goDirectional = GameObject.Find("ObjDirectional" + this.prPeca.NomePeca);
        GameObject goPoint = GameObject.Find("ObjPoint" + this.prPeca.NomePeca);
        GameObject goSpot = GameObject.Find("ObjSpot" + this.prPeca.NomePeca);

        foreach (Transform child in goDirectional.transform)
        {
            child.GetComponent<MeshRenderer>().enabled = directional;
        }

        foreach (Transform child in goPoint.transform)
        {
            child.GetComponent<MeshRenderer>().enabled = point;
        }

        goSpot.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = spot;
        foreach (Transform child in goSpot.transform.GetChild(0).GetChild(0))
        {
            child.GetComponent<MeshRenderer>().enabled = spot;
        }

        switch (prPeca.TipoLuz)
        {
            case TipoIluminacao.Ambiente:
                foreach (Transform child in goAmbiente.transform)
                {
                    child.GetComponent<Light>().enabled = prPeca.Ativo;
                }
                AtivaCamera();
                break;

            case TipoIluminacao.Directional:
                goDirectional.transform.parent.GetComponent<Light>().enabled = prPeca.Ativo;
                AtivaCamera();
                break;

            case TipoIluminacao.Point:
                goPoint.transform.parent.GetComponent<Light>().enabled = prPeca.Ativo;
                AtivaCamera();
                break;

            case TipoIluminacao.Spot:
                goSpot.transform.parent.GetComponent<Light>().enabled = prPeca.Ativo;
                AtivaCamera();
                break;
        }

        AtivaCamera();
    }

    void LightAmbienteEnable(bool status)
    {
        GameObject goAmbiente = GameObject.Find("Ambiente" + this.prPeca.NomePeca);

        foreach (Transform child in goAmbiente.transform)
            child.GetComponent<Light>().enabled = status;
    }
}