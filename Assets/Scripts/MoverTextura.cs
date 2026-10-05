using UnityEngine;

public class MoverTextura : MonoBehaviour
{
    [Header("Ajustes de Velocidad")]
    public float velocidad = 0.2f;

    private Renderer renderizador;

    void Start()
    {
        // Obtenemos el componente que dibuja el material en el cilindro
        renderizador = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calculamos cuánto debe moverse basado en el tiempo del juego
        float desplazamiento = Time.time * velocidad;

        // Desplazamos la textura en el eje Y (verticalmente en la imagen)
        renderizador.material.mainTextureOffset = new Vector2(0, desplazamiento);
    }
}
