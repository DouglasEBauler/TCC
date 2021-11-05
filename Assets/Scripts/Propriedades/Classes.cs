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
    public Color Cor;
    public Dictionary<string, string> ListPropLocks;
    public bool Ativo;

    //Iluminação
    public int TipoLuz;
    public float Intensidade;
    public ValorIluminacao ValorIluminacao;
    public float Distancia;
    public float Angulo;
    public float Expoente;
    public int UltimoIndexLuz;

    public PropriedadePeca()
    {
        ListPropLocks = new Dictionary<string, string>();
    }
}

[Serializable]
public class PropriedadeTransformacao : PropriedadePeca
{
    public Posicao Pos;
    public string NomePeca;

    public PropriedadeTransformacao()
    {
        Pos = new Posicao();
    }
}

[Serializable]
public class CuboPropriedadePeca : PropriedadePeca
{
    public string NomeCuboAmbiente;
    public string NomeCuboVis;
    public Posicao Pos;
    public Tamanho Tam;
    public Texture Textura;

    public CuboPropriedadePeca() : base() 
    {
        NomeCuboAmbiente = string.Empty;
        NomeCuboVis = string.Empty;
        Pos = new Posicao();
        Tam = new Tamanho();
    }
}

[Serializable]
public class PoligonoPropriedadePeca : PropriedadePeca
{
    public string PoligonoAmbiente;
    public Posicao Pos;
    public int Pontos;
    public TipoPrimitiva Primitiva;

    public PoligonoPropriedadePeca() : base() 
    {
        this.PoligonoAmbiente = string.Empty;
        this.Pos = new Posicao();
        this.Pontos = 3;
        this.Primitiva = TipoPrimitiva.Cheio;
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
    public string SplineAmbiente;

    public SplinePropriedadePeca() : base() 
    {
        P1 = new Posicao();
        P2 = new Posicao();
        P3 = new Posicao();
        P4 = new Posicao();
        SplineAmbiente = string.Empty;
    }
}

[Serializable]
public class IteracaoPropriedadePeca : PropriedadePeca
{
    public Posicao Intervalo;
    public Posicao Min;
    public Posicao Max;
    public string NomeTransformacao;

    public IteracaoPropriedadePeca() : base()
    {
        Intervalo = new Posicao();
        Min = new Posicao();
        Max = new Posicao();
    }
}

[Serializable]
public class PropriedadeCamera : PropriedadePeca
{
    public float PosX;
    public float PosY;
    public float PosZ;
    public float FOV;
    public bool CameraAtiva;
    public bool JaIniciouValores;
    public bool ExisteCamera;
    public PropriedadeCameraInicial PropInicial;
    public Dictionary<string, string> ListPropCamLocks;

    public PropriedadeCamera() 
    {
        this.ListPropCamLocks = new Dictionary<string, string>();
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

