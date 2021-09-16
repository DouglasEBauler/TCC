using UnityEngine;

public class RenderController : MonoBehaviour 
{
    const float INC_BASE_VERDE = 0.53f; // Valor alcançado através de testes visuais.
    const float INC_BASE_CINZA = 1.4f;  // Valor alcançado através de testes visuais.

    static GameObject otherBase;
    static int mudaSinal;

    public static void ResizeBases(GameObject baseRender, string tipoPeca, bool incrementa, bool tutorial = false)
    {
        string numObj = string.Empty;
        int val = 0;
        mudaSinal = 1;

        if (!incrementa)
            mudaSinal = -1;

        if (Consts.OBJETOGRAFICO.Equals(tipoPeca))
        {
            otherBase = GameObject.Find("BaseRenderLateralGO" + ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial"));
            otherBase.transform.GetChild(0).gameObject.SetActive(true); //Base lateral cinza

            otherBase = GameObject.Find("BaseObjetoGraficoGO" + ((!tutorial) ? DropPeca.countObjetosGraficos.ToString() : "Tutorial"));
            otherBase.transform.GetChild(0).gameObject.SetActive(true); //Base objeto gráfico verde
        }
        else if (Consts.IsTransformacao(tipoPeca))
        {
            if (baseRender.name.Length > "TransformacoesSlot".Length)
            {
                int.TryParse(baseRender.name.Substring(baseRender.name.IndexOf("Slot") + 4, 1), out val);                

                if (val > 0)
                    numObj = val.ToString();
            }

            // Redimensiona base verde
            otherBase = GameObject.Find("BaseObjetoGraficoGO" + ((!tutorial) ? numObj : "Tutorial"));

            float ScaleY = otherBase.transform.localScale.y;
            otherBase.transform.localScale = new Vector3(otherBase.transform.localScale.x, ScaleY + (INC_BASE_VERDE * mudaSinal), otherBase.transform.localScale.z);

            // Redimensiona base cinza
            otherBase = GameObject.Find("BaseRenderLateralGO" + ((!tutorial) ? numObj : "Tutorial"));

            ScaleY = otherBase.transform.localScale.y;
            otherBase.transform.localScale = new Vector3(otherBase.transform.localScale.x, ScaleY + (INC_BASE_CINZA * mudaSinal), otherBase.transform.localScale.z);            
        }
    }
}
