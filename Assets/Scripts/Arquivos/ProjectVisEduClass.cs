using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProjectVisEduClass
{
    public CameraPecaProject Camera;
    public List<ObjetoGraficoProject> ObjetosGraficos;

    public ProjectVisEduClass()
    {
        this.Camera = new CameraPecaProject();
        this.ObjetosGraficos = new List<ObjetoGraficoProject>();
    }
}

[Serializable]
public class ValoresProject
{
    public string X;
    public string Y;
    public string Z;
}

[Serializable]
public class TamanhoProject : ValoresProject
{
    public TamanhoProject() : base() { }
}

[Serializable]
public class PosicaoProject : ValoresProject
{
    public PosicaoProject() : base() { }
}

[Serializable]
public class ValorIluminacaoProject : ValoresProject
{
    public ValorIluminacaoProject() : base() { }
}


[Serializable]
public class CameraPecaProject
{
    public PropriedadeCameraProject Propriedades;

    public CameraPecaProject()
    {
        this.Propriedades = new PropriedadeCameraProject();
    }
}

[Serializable]
public class ObjetoGraficoProject
{
    public PropriedadePeca Propriedades;
    public CuboProject Cubo;
    public PoligonoProject Poligono;
    public SplineProject Spline;

    public ObjetoGraficoProject()
    {
        this.Propriedades = new PropriedadePeca();
    }
}

[Serializable]
public class CuboProject
{
    public CuboPropriedadePecaProject Propriedades;
    public List<TransformacaoProject> Transformacoes;
    public IluminacaoProject Iluminacao;

    public CuboProject()
    {
        this.Propriedades = new CuboPropriedadePecaProject();
    }
}

[Serializable]
public class PoligonoProject
{
    public PoligonoPropriedadePecaProject Propriedades;
    public List<TransformacaoProject> Transformacoes;
    public IluminacaoProject Iluminacao;

    public PoligonoProject()
    {
        this.Propriedades = new PoligonoPropriedadePecaProject();
    }
}

[Serializable]
public class SplineProject
{
    public SplinePropriedadePecaProject Propriedades;
    public List<TransformacaoProject> Transformacoes;
    public IluminacaoProject Iluminacao;

    public SplineProject()
    {
        this.Propriedades = new SplinePropriedadePecaProject();
    }
}

[Serializable]
public class TransformacaoProject
{
    public TransformacaoPropriedadePecaProject Propriedades;
    public TypeTransform TipoTransformacao;
    public IteracaoProject Iteracao;

    public TransformacaoProject(TypeTransform tipoTransformacao)
    {
        this.Propriedades = new TransformacaoPropriedadePecaProject();
        this.TipoTransformacao = tipoTransformacao;
        this.Iteracao = new IteracaoProject();
    }
}

[Serializable]
public class IteracaoProject
{
    public IteracaoPropriedadePecaProject Propriedades;
    public string NomeTransformacao;

    public IteracaoProject() : base()
    {
        Propriedades = new IteracaoPropriedadePecaProject();
    }
}

[Serializable]
public class IteracaoPropriedadePecaProject : PropriedadePecaProject
{
    public PosicaoProject Intervalo;
    public PosicaoProject Min;
    public PosicaoProject Max;

    public IteracaoPropriedadePecaProject()
    {
        this.Intervalo = new PosicaoProject();
        this.Min = new PosicaoProject();
        this.Max = new PosicaoProject();
    }
}

[Serializable]
public class IluminacaoProject
{
    public PropriedadeIluminacaoPecaProject Propriedades;

    public IluminacaoProject()
    {
        this.Propriedades = new PropriedadeIluminacaoPecaProject();
    }
}

[Serializable]
public class PropriedadePecaProject
{
    public string Nome;
    public Color Cor;
    public bool Ativo;
    [NonSerialized]
    public Dictionary<string, float> listaValores;
}

[Serializable]
public class TransformacaoPropriedadePecaProject : PropriedadePecaProject
{
    public PosicaoProject Pos;
    public string NomePeca;
    public string NomePecaAmb;
    public string NomePecaVis;

    public TransformacaoPropriedadePecaProject() : base()
    {
        this.Pos = new PosicaoProject();
    }
}

[Serializable]
public class CuboPropriedadePecaProject : PropriedadePecaProject
{
    public string NomeCuboAmbiente;
    public string NomeCuboVis;
    public PosicaoProject Pos;
    public TamanhoProject Tam;
    public Texture Textura;

    public CuboPropriedadePecaProject() : base()
    {
        this.NomeCuboAmbiente = string.Empty;
        this.NomeCuboVis = string.Empty;
        this.Pos = new PosicaoProject();
        this.Tam = new TamanhoProject();
    }
}

[Serializable]
public class PoligonoPropriedadePecaProject : PropriedadePecaProject
{
    public string PoligonoAmb;
    public string PoligonoVis;
    public PosicaoProject Pos;
    public string Pontos;
    public TipoPrimitiva Primitiva;

    public PoligonoPropriedadePecaProject() : base()
    {
        this.PoligonoAmb = string.Empty;
        this.PoligonoVis = string.Empty;
        this.Pos = new PosicaoProject();
        this.Pontos = string.Empty;
        this.Primitiva = TipoPrimitiva.Cheio;
    }
}

[Serializable]
public class SplinePropriedadePecaProject : PropriedadePecaProject
{
    public PosicaoProject P1;
    public PosicaoProject P2;
    public PosicaoProject P3;
    public PosicaoProject P4;
    public PosicaoProject P5;
    public string QuantidadePontos;
    public string SplineAmb;
    public string SplineVis;

    public SplinePropriedadePecaProject() : base()
    {
        P1 = new PosicaoProject();
        P2 = new PosicaoProject();
        P3 = new PosicaoProject();
        P4 = new PosicaoProject();
        QuantidadePontos = string.Empty;
        SplineAmb = string.Empty;
        SplineVis = string.Empty;
    }
}

[Serializable]
public class PropriedadeIluminacaoPecaProject : PropriedadePecaProject
{
    public PosicaoProject Pos;
    public string NomePecaAmbiente;
    public string NomePecaVis;
    public TipoIluminacao TipoLuz;
    public ValorIluminacaoProject ValorIluminacao;
    public string Intensidade;
    public string Distancia;
    public string Angulo;
    public string Expoente;
    public string UltimoIndexLuz;

    public PropriedadeIluminacaoPecaProject() : base()
    {
        Pos = new PosicaoProject();
        ValorIluminacao = new ValorIluminacaoProject();
    }
}

[Serializable]
public class PropriedadeCameraProject
{
    public string Nome;
    public string PosX;
    public string PosY;
    public string PosZ;
    public string FOV;
    public bool CameraAtiva;
    public bool JaIniciouValores;
    public bool ExisteCamera;

    public PropriedadeCameraProject() { }
}