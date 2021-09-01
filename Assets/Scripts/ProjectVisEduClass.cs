using System;
using System.Collections.Generic;

[Serializable]
public class ProjectVisEduClass
{
    public CameraPeca Camera;
    public List<ObjetoGrafico> ObjetosGraficos;

    public ProjectVisEduClass()
    {
        this.Camera = new CameraPeca();
        this.ObjetosGraficos = new List<ObjetoGrafico>();
    }
}

[Serializable]
public class CameraPeca
{
    public PropriedadeCamera Propriedades;

    public CameraPeca()
    {
        this.Propriedades = new PropriedadeCamera();
    }
}

[Serializable]
public class ObjetoGrafico
{
    public PropriedadePeca Propriedades;
    public Forma Forma;
    public List<Transformacao> Transformacoes;
    public Iluminacao Iluminacao;

    public ObjetoGrafico()
    {
        this.Propriedades = new PropriedadePeca();
        this.Forma = new Forma();
        this.Transformacoes = new List<Transformacao>();
        this.Iluminacao = new Iluminacao();
    }
}

[Serializable]
public class Forma
{
    public PropriedadePeca Propriedades;

    public Forma()
    {
        this.Propriedades = new PropriedadePeca();
    }
}

[Serializable]
public class Cubo : Forma 
{
    public Cubo() : base() { }
}

[Serializable]
public class Poligono : Forma
{
    public Poligono() : base() { }
}

[Serializable]
public class Spline : Forma 
{
    public Spline() : base() { }
}

[Serializable]
public class Transformacao
{
    public PropriedadePeca Propriedades;
    
    public Transformacao()
    {
        this.Propriedades = new PropriedadePeca();
    }
}

[Serializable]
public class Escalar : Transformacao { }

[Serializable]
public class Rotacionar : Transformacao { }

[Serializable]
public class Transladar : Transformacao { }

[Serializable]
public class Iluminacao
{
    public PropriedadePeca Propriedades;
    
    public Iluminacao()
    {
        this.Propriedades = new PropriedadePeca();
    }
}