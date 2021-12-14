
public static class Consts
{
    public const float SPEED_DESLOC = 24f;

    public const string AMB = "Amb";
    public const string AMBIENTE = "Ambiente";
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
    public const string ITERACAO = "Iteracao";

    public const string ARQUIVO = "Arquivo";
    public const string FABRICAPECAS = "FabricaPecas";
    public const string PROPRIEDADEPECAS = "PropriedadePecas";
    public const string AJUDA = "Ajuda";

    public const string BTN_ARQUIVO = "BtnArquivo";
    public const string BTN_FAB_PECAS = "BtnFabPecas";
    public const string BTN_PROP_PECAS = "BtnPropPecas";
    public const string BTN_AJUDA = "BtnAjuda";

    public const string CUBO_AMB_OBJ = "CuboAmbObject";
    public const string POLIGONO_AMB_OBJ = "PoligonoAmbObject";
    public const string SPLINE_AMB_OBJ = "SplineAmbObject";
    public const string CUBO_AMB = "CuboAmb";
    public const string POLIGONO_AMB = "PoligonoAmb";
    public const string SPLINE_AMB = "SplineAmb";

    public const string CUBO_VIS_OBJ = "CuboVisObject";
    public const string POLIGONO_VIS_OBJ = "PoligonoVisObject";
    public const string SPLINE_VIS_OBJ = "SplineVisObject";
    public const string CUBO_VIS = "CuboVis";
    public const string POLIGONO_VIS = "PoligonoVis";
    public const string SPLINE_VIS = "SplineVis";

    public const string PECA_CAM = "CameraP";
    public const string PECA_OBJ_GRAFICO = "ObjetoGraficoP";
    public const string PECA_CUBO = "Cubo";
    public const string PECA_POLIGONO = "Poligono";
    public const string PECA_SPLINE = "Spline";
    public const string PECA_TRANSF_ROTACIONAR = "Rotacionar";
    public const string PECA_TRANSF_ESCALAR = "Escalar";
    public const string PECA_TRANSF_TRANSLADAR = "Transladar";
    public const string PECA_ITERACAO = "IteracaoP";
    public const string PECA_ILUMINACAO = "IluminacaoP";

    public const string CAMERA_SLOT = "CameraSlot";
    public const string OBJ_GRAFICO_SLOT = "ObjGraficoSlot";
    public const string FORMA_SLOT = "FormasSlot";
    public const string TRANSF_SLOT = "TransformacoesSlot";
    public const string ITERACAO_SLOT = "IteracaoSlot";
    public const string ILUMINACAO_SLOT = "IluminacaoSlot";

    public const string SLOT_CAM = "SlotCam";
    public const string SLOT_OBJ_GRAFICO = "SlotGrafObj";
    public const string SLOT_FORMA = "SlotForma";
    public const string SLOT_TRANSF = "SlotTransf";
    public const string SLOT_ITERACAO = "SlotIteracao";
    public const string SLOT_ILUMINACAO = "SlotIluminacao";

    public const string LIGHT_OBJECTS_ILUMINACAO = "LightObjectsIluminacao";

    public const string TUTORIAL = "Tutorial";

    public const string PATH_IMG_UNLOCK = "Textures/UnLock";
    public const string PATH_IMG_LOCK = "Textures/Lock";

    public const string INITIAL_FILE_PROJECT = "ProjectVisEdu.json";
    public const string BTN_SELECT_PROJECT = "Selecionar";
    public const string TITLE_SELECT_EXPORT_DIRECTORY = "Selecione o diretório a ser exportado o projeto";
    public const string TITLE_SELECT_IMPORT_DIRECTORY = "Selecione o arquivo do projeto a ser importado";
}

public enum PecasSlot { Camera, ObjGrafico, Formas, Traformacoes, Iluminacao };

public enum Slots { CameraSlot, ObjGrafSlot, FormasSlot, TransformacoesSlot, IluminacaoSlot };

public enum Property
{
    None,
    PosX, PosY, PosZ,
    TamX, TamY, TamZ,
    Intensidade,
    Distancia,
    Angulo,
    Expoente,
    FOV,
    Pontos,
    P1PosX, P1PosY, P1PosZ,
    P2PosX, P2PosY, P2PosZ,
    P3PosX, P3PosY, P3PosZ,
    P4PosX, P4PosY, P4PosZ,
    P5PosX, P5PosY, P5PosZ,
    IntervaloX, IntervaloY, IntervaloZ,
    MinX, MinY, MinZ,
    MaxX, MaxY, MaxZ,
    ValorX, ValorY, ValorZ,
    QuantidadePontos
};

public enum TipoIluminacao { Ambiente, Directional, Point, Spot };

public enum TipoPrimitiva { Vertices, Aberto, Fechado, Preenchido };

public enum TypeTransform { Escalar, Rotacionar, Transladar };

public enum InputSelectedIluminacao { ValorX, ValorY, ValorZ, None }
