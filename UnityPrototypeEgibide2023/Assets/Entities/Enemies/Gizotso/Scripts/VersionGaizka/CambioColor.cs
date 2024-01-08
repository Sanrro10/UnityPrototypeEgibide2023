using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllIn1SpriteShader;

public class CambioColor : MonoBehaviour
{
    [SerializeField]
    Material matCambiar;
    [SerializeField]
    private Color[] coloresposibles;
    [SerializeField]
    private Color colorElegido;
    [SerializeField]
    private Color colorElegidoOscuro;
    [SerializeField]
    private Color colorOriginal;
    // Start is called before the first frame update
    void Start()
    {
        int indiceColor = Random.Range(0, coloresposibles.Length);
        colorElegido = coloresposibles[indiceColor];
        matCambiar = GetComponent<Renderer>().material;
        //matCambiar.SetFloat("_Alpha", 1f);
        //matCambiar.SetColor("_Color", new Color(0.5f, 1f, 0f, 1f));
        matCambiar.EnableKeyword("CHANGECOLOR_ON");
        matCambiar.SetColor("_ColorChangeNewCol", colorElegido);
        Color.RGBToHSV(colorElegido, out float h, out float s, out float v);
        v -= 0.4F;
        colorElegidoOscuro = Color.HSVToRGB(h, s, v);
        matCambiar.SetColor("_ColorChangeNewCol2", colorElegidoOscuro);
        //colorOriginal = matCambiar.color;
        //colorOriginal = colorElegido;
        //matCambiar.color = colorElegido;

    }
    
}
