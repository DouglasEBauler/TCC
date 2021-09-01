using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Valores
{
    public float X;
    public float Y;
    public float Z;
}

[Serializable]
public class Tamanho : Valores
{
    public Tamanho() : base() { }
}

[Serializable]
public class Posicao : Valores
{
    public Posicao() : base() { }
}

[Serializable]
public class ValorIluminacao : Valores
{
    public ValorIluminacao() : base() { }
}

[Serializable]
public class PropriedadePeca
{
    public string Nome;
    public bool PodeAtualizar;
    public bool JaIniciouValores;
    public Tamanho Tam;
    public Posicao Pos;
    public bool Ativo;
    public string NomeCuboAmbiente;
    public string NomeCuboVis;
    public bool JaInstanciou;
    public Color Cor;
    public Texture Textura;

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
    }
}

[Serializable]
public class PropriedadeCamera
{
    public float Nome;
    public float PosX;
    public float PosY;
    public float PosZ;
    public Vector2 FOV;
    public bool CameraAtiva;
    public bool JaIniciouValores;
    public bool ExisteCamera;
    public PropriedadeCameraInicial PropInicial;

    public PropriedadeCamera() { }
}

[Serializable]
public class PropriedadeCameraInicial
{
    public float PosX;
    public float PosY;
    public float PosZ;
    public Vector2 FOV;

    public PropriedadeCameraInicial() { }
}

[Serializable]
public class PropriedadeIluminacao
{

}


