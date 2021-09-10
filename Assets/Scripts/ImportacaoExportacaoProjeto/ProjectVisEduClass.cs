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
    public FormaProject Forma;
    public List<TransformacaoProject> Transformacoes;
    public IluminacaoProject Iluminacao;

    public ObjetoGraficoProject()
    {
        this.Propriedades = new PropriedadePeca();
        this.Forma = new FormaProject();
        this.Transformacoes = new List<TransformacaoProject>();
        this.Iluminacao = new IluminacaoProject();
    }
}

[Serializable]
public class FormaProject
{
    public PropriedadePecaProject Propriedades;

    public FormaProject()
    {
        this.Propriedades = new PropriedadePecaProject();
    }
}

[Serializable]
public class CuboProject : FormaProject
{
    public CuboProject() : base() { }
}

[Serializable]
public class PoligonoProject : FormaProject
{
    public PoligonoProject() : base() { }
}

[Serializable]
public class SplineProject : FormaProject
{
    public SplineProject() : base() { }
}

[Serializable]
public class TransformacaoProject
{
    public PropriedadePecaProject Propriedades;
    
    public TransformacaoProject()
    {
        this.Propriedades = new PropriedadePecaProject();
    }
}

[Serializable]
public class EscalarProject : TransformacaoProject { }

[Serializable]
public class RotacionarProject : TransformacaoProject { }

[Serializable]
public class TransladarProject : TransformacaoProject { }

[Serializable]
public class IluminacaoProject
{
    public PropriedadePecaProject Propriedades;
    
    public IluminacaoProject()
    {
        this.Propriedades = new PropriedadePecaProject();
    }
}

[Serializable]
public class PropriedadePecaProject
{
    public string Nome;
    public TamanhoProject Tam;
    public PosicaoProject Pos;
    public bool Ativo;
    public string NomeCuboAmbiente;
    public string NomeCuboVis;
    public Color Cor;
    public Texture Textura;
    [NonSerialized]
    public Dictionary<string, float> listaValores;

    //Iluminação
    public int TipoLuz;
    public string Intensidade;
    public ValorIluminacaoProject ValorIluminacao;
    public string Distancia;
    public string Angulo;
    public string Expoente;
    public int UltimoIndexLuz;

    public PropriedadePecaProject()
    {
        Tam = new TamanhoProject();
        Pos = new PosicaoProject();
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
    public PropriedadeCameraInicialProject PropInicial;

    public PropriedadeCameraProject() { }
}

[Serializable]
public class PropriedadeCameraInicialProject
{
    public string PosX;
    public string PosY;
    public string PosZ;
    public Vector2 FOV;

    public PropriedadeCameraInicialProject() { }
}
