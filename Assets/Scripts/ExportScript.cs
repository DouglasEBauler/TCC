using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExportScript : MonoBehaviour
{
    [SerializeField]
    GameObject render;

    void OnMouseDown()
    {
        string path = EditorUtility.SaveFilePanel("Selecione o local a ser salvo o projeto", "", "ProjectVisEdu", "json");

        if (path.Length != 0)
        {
            var projectJson = GenerateProject();

            using (var streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(projectJson);
            }
        }
    }

    string GenerateProject()
    {
        var project = new ProjectVisEduClass();

        foreach (Transform child in render.transform)
        {
            if (child.name.Contains("CameraSlot"))
            {
                project.Camera.Propriedades = Global.propCameraGlobal;
            }
            else if (child.name.Contains("ObjGraficoSlot"))
            {
                GameObject objGrafico = GameObject.Find(child.name);
                ObjetoGrafico objGrafPeca = new ObjetoGrafico();
                string key = GetPeca(child.name);
                if (!string.Empty.Equals(key))
                {
                    objGrafPeca.Propriedades = Global.propriedadePecas[key];

                    foreach (Transform _child in objGrafico.transform)
                    {
                        if (_child.name.Contains("FormasSlot")
                            || _child.name.Contains("TransformacoesSlot")
                            || _child.name.Contains("IluminacaoSlot"))
                        {
                            key = GetPeca(_child.name);
                            if (!string.Empty.Equals(key))
                            {
                                var prop = Global.propriedadePecas[key];

                                if (prop != null)
                                {
                                    if (prop.Nome.Contains("Cubo"))
                                    {
                                        objGrafPeca.Forma = new Cubo();
                                        objGrafPeca.Forma.Propriedades = prop;
                                    }
                                    else if (prop.Nome.Contains("Poligono"))
                                    {
                                        objGrafPeca.Forma = new Poligono();
                                        objGrafPeca.Forma.Propriedades = prop;
                                    }
                                    else if (prop.Nome.Contains("Spline"))
                                    {
                                        objGrafPeca.Forma = new Spline();
                                        objGrafPeca.Forma.Propriedades = prop;
                                    }
                                    else if (prop.Nome.Contains("Iluminacao"))
                                    {
                                        objGrafPeca.Iluminacao = new Iluminacao();
                                        objGrafPeca.Iluminacao.Propriedades = prop;
                                    }
                                    else if (prop.Nome.Contains("Rotacionar"))
                                    {
                                        objGrafPeca.Transformacoes.Add(new Rotacionar() { Propriedades = prop });
                                    }
                                    else if (prop.Nome.Contains("Transladar"))
                                    {
                                        objGrafPeca.Transformacoes.Add(new Transladar() { Propriedades = prop });
                                    }
                                    else if (prop.Nome.Contains("Escalar"))
                                    {
                                        objGrafPeca.Transformacoes.Add(new Escalar() { Propriedades = prop });
                                    }
                                }
                            }
                        }
                    }
                    project.ObjetosGraficos.Add(objGrafPeca);
                }
            }
        }

        return JsonUtility.ToJson(project, true);
    }

    string GetPeca(string slot)
    {
        foreach (var peca in Global.listaEncaixes)
        {
            if (peca.Value.Equals(slot)) return peca.Key;
        }

        return string.Empty;
    }
}