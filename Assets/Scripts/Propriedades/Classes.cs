using System;
using System.Collections.Generic;
using UnityEngine;

public class Valores
{
    public float X;
    public float Y;
    public float Z;
}

public class Tamanho : Valores
{
    public Tamanho() : base() { }
}

public class Posicao : Valores
{
    public Posicao() : base() { }
}

public class ValorIluminacao : Valores
{
    public ValorIluminacao() : base() { }
}

[Serializable]
public class PropriedadePeca
{
    public string Nome;
    [NonSerialized]
    public string NomePeca;
    public Color Cor;
    [NonSerialized]
    public Dictionary<Property, string> ListPropLocks;
    public bool Ativo;

    public PropriedadePeca()
    {
        Nome = string.Empty;
        NomePeca = string.Empty;
        Cor = Color.white;
        ListPropLocks = new Dictionary<Property, string>();
    }
}

[Serializable]
public class IluminacaoPropriedadePeca : PropriedadePeca
{
    public Posicao Pos;
    [NonSerialized]
    public string NomePecaAmbiente;
    [NonSerialized]
    public string NomePecaVis;
    public TipoIluminacao TipoLuz;
    public float Intensidade;
    public ValorIluminacao ValorIluminacao;
    public float Distancia;
    public float Angulo;
    public float Expoente;
    public int UltimoIndexLuz;

    public IluminacaoPropriedadePeca() : base() 
    {
        Pos = new Posicao();
        TipoLuz = TipoIluminacao.Ambiente;
        ValorIluminacao = new ValorIluminacao();
    }
}

[Serializable]
public class TransformacaoPropriedadePeca : PropriedadePeca
{
    public Posicao Pos;
    [NonSerialized]
    public string NomePecaAmb;
    [NonSerialized]
    public string NomePecaVis;

    public TransformacaoPropriedadePeca()
    {
        Pos = new Posicao();
    }
}

[Serializable]
public class CuboPropriedadePeca : PropriedadePeca
{
    [NonSerialized]
    public string NomeCuboAmb;
    [NonSerialized]
    public string NomeCuboVis;
    public Posicao Pos;
    public Tamanho Tam;
    public Texture Textura;

    public CuboPropriedadePeca() : base() 
    {
        NomeCuboAmb = string.Empty;
        NomeCuboVis = string.Empty;
        Pos = new Posicao();
        Tam = new Tamanho();
    }
}

[Serializable]
public class PoligonoPropriedadePeca : PropriedadePeca
{
    [NonSerialized]
    public string PoligonoAmb;
    [NonSerialized]
    public string PoligonoVis;
    public Posicao Pos;
    public int Pontos;
    public TipoPrimitiva Primitiva;

    public PoligonoPropriedadePeca() : base() 
    {
        this.Pos = new Posicao();
        this.Pontos = 3;
        this.Primitiva = TipoPrimitiva.Preenchido;
        this.PoligonoAmb = string.Empty;
        this.PoligonoVis = string.Empty;
    }
}

[Serializable]
public class SplinePropriedadePeca : PropriedadePeca
{
    public Posicao P1;
    public Posicao P2;
    public Posicao P3;
    public Posicao P4;
    public Posicao P5;
    public int QuantidadePontos;
    [NonSerialized]
    public string SplineAmb;
    [NonSerialized]
    public string SplineVis;

    public SplinePropriedadePeca() : base() 
    {
        P1 = new Posicao();
        P2 = new Posicao();
        P3 = new Posicao();
        P4 = new Posicao();
        P5 = new Posicao();
        QuantidadePontos = 30;
        SplineAmb = string.Empty;
        SplineVis = string.Empty;
    }
}

[Serializable]
public class IteracaoPropriedadePeca : PropriedadePeca
{
    public Posicao Intervalo;
    public Posicao Min;
    public Posicao Max;
    [NonSerialized]
    public string NomeTransformacao;
    public bool AtivoX;
    public bool AtivoY;
    public bool AtivoZ;

    public IteracaoPropriedadePeca() : base()
    {
        Intervalo = new Posicao();
        Min = new Posicao();
        Max = new Posicao();
        AtivoX = true;
        AtivoY = false;
        AtivoZ = false;
    }
}

[Serializable]
public class PropriedadeCamera : PropriedadePeca
{
    public Posicao Pos;
    public float FOV;
    public bool CameraAtiva;
    public PropriedadeCameraInicial PropInicial;

    public PropriedadeCamera() : base()
    {
        this.Pos = new Posicao();
        this.FOV = 45f;
    }
}

[Serializable]
public struct PropriedadeCameraInicial
{
    public float PosX;
    public float PosY;
    public float PosZ;
    public Vector2 FOV;
}

