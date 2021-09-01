using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCor : MonoBehaviour {

    public Image imagemTela;
    public Texture2D texturaImagemTela;
    public Vector2 dimensoesImagem;
    public RectTransform rectImagemTela;
    public Vector3 posicaoImagem;
    public Vector2 posicaoClick;

    public Vector2 fatorImagemTextura;
    public Vector2 pixelClicado;

    public Color corSelecionada;

    public float x, y;

	void Start () {
        rectImagemTela = imagemTela.rectTransform;
        texturaImagemTela = imagemTela.sprite.texture;        
    }
	
	void Update () {
        mudaCor();
    }

    private void mudaCor()
    {
        dimensoesImagem = rectImagemTela.anchorMax - rectImagemTela.anchorMin;
        dimensoesImagem = new Vector2(Screen.width * dimensoesImagem.x, Screen.height * dimensoesImagem.y);

        posicaoImagem = rectImagemTela.position;
        posicaoClick = Input.mousePosition - posicaoImagem;

        fatorImagemTextura = new Vector2(dimensoesImagem.x / texturaImagemTela.width, dimensoesImagem.y / texturaImagemTela.height);

        pixelClicado = new Vector2(posicaoClick.x/* / fatorImagemTextura.x*/, posicaoClick.y/* / fatorImagemTextura.y*/);
        x = pixelClicado.x;
        y = pixelClicado.y;

        corSelecionada = texturaImagemTela.GetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y);
    }
}
