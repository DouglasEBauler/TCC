using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    public const string AMB = "Amb";
    public const string VIS = "Vis";
    public const string Rotacionar = "Rotacionar";
    public const string Transladar = "Transladar";
    public const string Escalar = "Escalar";
    public const string Iluminacao = "Iluminacao";
    public const string ObjetoGrafico = "ObjetoGraficoP";
    public const string Cubo = "Cubo";
    public const string Poligono = "Poligono";
    public const string Spline = "Spline";
    public const string Camera = "CameraP";
    public const string FabricaPecas = "FabricaPecas";

    public static bool IsTransformacao(string tranf)
    {
        return Rotacionar.Equals(tranf) || Transladar.Equals(tranf) || Escalar.Equals(tranf);
    }
}

public enum PecasSlot { Camera, ObjGrafico, Formas, Traformacoes, Iluminacao };

public enum Slots { CameraSlot, ObjGrafSlot, FormasSlot, TransformacoesSlot, IluminacaoSlot };

public enum InputSelected { InputPosX, InputPosY, InputPosZ, ToggleAtivo, InputIntensidade, InputValorX, InputValorY, InputValorZ, InputDistancia, InputAngulo, InputExpoente, InputEmpty };

public enum TipoIluminacao { Ambiente, Directional, Point, Spot };
