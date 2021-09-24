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
    public Tamanho Tam;
    public Posicao Pos;
    public bool Ativo;
    public Color Cor;
    public Texture Textura;
    public Dictionary<string, string> ListPropLocks;

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
        Tam = new Tamanho();
        Pos = new Posicao();
        ListPropLocks = new Dictionary<string, string>();
    }
}

[Serializable]
public class CuboPropriedadePeca : PropriedadePeca
{
    public string NomeCuboAmbiente;
    public string NomeCuboVis;

    public CuboPropriedadePeca() : base() {}
}

[Serializable]
public class PoligonoPropriedadePeca : PropriedadePeca
{
    public int Pontos;
    public TipoPrimitiva Primitiva;
    public string PoligonoAmbiente;

    public PoligonoPropriedadePeca() : base() 
    {
        this.Primitiva = TipoPrimitiva.Cheio;
    }
}

[Serializable]
public class SplinePropriedadePeca : PropriedadePeca
{
    public SplinePropriedadePeca() : base() { }
}

public class PropriedadeCamera
{
    public string Nome;
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

public struct PropriedadeCameraInicial
{
    public float PosX;
    public float PosY;
    public float PosZ;
    public Vector2 FOV;
}

