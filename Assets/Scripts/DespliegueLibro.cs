using UnityEngine;

public class DespliegueLibro : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform rectLibro;

    [Header("Posiciones Exactas (X, Y)")]
    public Vector2 posicionOculta;   // Ej: X = -800, Y = 0
    public Vector2 posicionVisible;  // Ej: X = 0, Y = 0

    [Header("Configuración")]
    [Tooltip("Distancia desde el borde izquierdo. Prueba con 150 o 200.")]
    public float margenDeteccion = 150f;
    public float velocidadMovimiento = 10f;

    [Header("Estado Actual (Mira esto al darle Play)")]
    public bool estaDesplegado = false;

    void Update()
    {
        // 1. Obtenemos las coordenadas del mouse
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        // 2. Verificamos que el mouse esté DENTRO de la pantalla y pegado a la izquierda
        bool mouseEnPantalla = mouseX >= 0 && mouseX <= Screen.width && mouseY >= 0 && mouseY <= Screen.height;

        if (mouseEnPantalla && mouseX <= margenDeteccion)
        {
            estaDesplegado = true; // El mouse está en la zona izquierda
        }
        else
        {
            estaDesplegado = false; // El mouse salió de la zona o de la pantalla
        }

        // 3. Elegimos hacia dónde debe ir el libro
        Vector2 destinoActual = estaDesplegado ? posicionVisible : posicionOculta;

        // 4. Movemos el libro suavemente
        if (rectLibro != null)
        {
            rectLibro.anchoredPosition = Vector2.Lerp(rectLibro.anchoredPosition, destinoActual, Time.deltaTime * velocidadMovimiento);
        }
    }
}