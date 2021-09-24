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
}

[Serializable]
public class CuboProject : FormaProject
{
    public CuboProject()
    {
        base.Propriedades = new CuboPropriedadePecaProject();
    }
}

[Serializable]
public class PoligonoProject : FormaProject
{
    public PoligonoProject() 
    {
        base.Propriedades = new PoligonoPropriedadePecaProject();
    }
}

[Serializable]
public class SplineProject : FormaProject
{
    public SplineProject()
    {
        base.Propriedades = new SplinePropriedadePecaProject();
    }
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
    public Color Cor;
    public bool Ativo;
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
public class CuboPropriedadePecaProject : PropriedadePecaProject
{
    public string NomeCuboAmbiente;
    public string NomeCuboVis;
}

[Serializable]
public class PoligonoPropriedadePecaProject : PropriedadePecaProject
{
    public string PoligonoAmbiente;
}

[Serializable]
public class SplinePropriedadePecaProject : PropriedadePecaProject
{

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