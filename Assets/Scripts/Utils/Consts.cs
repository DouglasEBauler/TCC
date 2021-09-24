using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    public const string AMB = "Amb";
    public const string VIS = "Vis";
    public const string ROTACIONAR = "Rotacionar";
    public const string TRANSLADAR = "Transladar";
    public const string ESCALAR = "Escalar";
    public const string ILUMINACAO = "Iluminacao";
    public const string OBJETOGRAFICO = "ObjetoGraficoP";
    public const string CUBO = "Cubo";
    public const string POLIGONO = "Poligono";
    public const string SPLINE = "Spline";
    public const string CAMERA = "CameraP";
    public const string FABRICAPECAS = "FabricaPecas";
    public const string PATH_IMG_UNLOCK = "Textures/UnLock";
    public const string PATH_IMG_LOCK = "Textures/Lock";

    public static bool IsTransformacao(string tranf)
    {
        return ROTACIONAR.Equals(tranf) || TRANSLADAR.Equals(tranf) || ESCALAR.Equals(tranf);
    }
}

public enum PecasSlot { Camera, ObjGrafico, Formas, Traformacoes, Iluminacao };

public enum Slots { CameraSlot, ObjGrafSlot, FormasSlot, TransformacoesSlot, IluminacaoSlot };

public enum Property { PosX, PosY, PosZ, TamX, TamY, TamZ, ToggleActive, Intensity, Distancy, Angle, Expoent, FOV, Empty };

public enum TipoIluminacao { Ambiente, Directional, Point, Spot };

public enum TipoPrimitiva { Vertices, Aberto, Fechado, Cheio };
