using UnityEngine;
using UnityEngine.UI;

public class ProgresoNivel : MonoBehaviour
{
    [Header("Datos del Nivel (Enjambre Enemigo)")]
    public float saludTotalEnemigos = 100f;
    public float saludActual;

    [Header("Interfaz UI")]
    public RectTransform imagenEquipo; // Aquí arrastraremos a "EquipoMoviendose"
    public Image barraDeAvance; // Aquí arrastraremos la línea de color (RellenoAvance)
    private float anchoImagen;

    [Header("Economía")]
    public SistemaOro sistemaOro;
    public float multiplicadorOro = 0.001f; // Ganas 0.5 de oro por cada 1 de daño

    [Header("Configuración del Jefe")]
    public GameObject objetoJefe; // El modelo 3D/2D del jefe en la escena
    public VidaJefe scriptJefe;   // Su script de vida
    public bool jefeActivo = false;
    private bool jefeYaAparecio = false; // ¡Añade esta línea!

    void Start()
    {
        // Al iniciar, los enemigos tienen toda la vida (el nivel acaba de empezar)
        saludActual = saludTotalEnemigos;

        // Guardamos el ancho de la imagen para saber exactamente cuánto debemos deslizarla
        if (imagenEquipo != null) anchoImagen = imagenEquipo.rect.width;
    }

    void Update()
    {
        if (saludActual > 0)
        {
            // 1. Calculamos el progreso del nivel.
            // Si la vida es 100, el progreso es 0. Si la vida es 0, el progreso es 1 (100%).
            float porcentajeProgreso = 1f - (saludActual / saludTotalEnemigos);

            // 2. Mueve el equipo visualmente
            if (imagenEquipo != null)
            {
                // Deslizamos la imagen. Empieza corrida hacia la izquierda (-anchoImagen) y termina en el centro (0).
                float nuevaPosicionX = Mathf.Lerp(-anchoImagen, 0, porcentajeProgreso);
                imagenEquipo.anchoredPosition = new Vector2(nuevaPosicionX, imagenEquipo.anchoredPosition.y);
            }

            // 3. Rellenamos la barra visualmente
            if (barraDeAvance != null)
            {
                barraDeAvance.fillAmount = porcentajeProgreso;
            }
        }
        else
        {
            // Aquí puedes agregar lo que pasa cuando ganan (ej: pantalla de victoria)
            // Debug.Log("¡Nivel superado, llegaste al cofre!");
        }
    }

    public void RecibirDaño(float daño)
    {
        if (saludActual > 0)
        {
            // 1. Si el jefe está activo, él recibe el daño y pausamos al enjambre
            if (jefeActivo && scriptJefe != null)
            {
                scriptJefe.RecibirDaño(daño);
                return;
            }

            // 2. Si no hay jefe, restamos el daño al enjambre normalmente
            saludActual -= daño;

            // 3. Chequeamos si el enjambre bajó al 50% por primera vez
            if (!jefeYaAparecio && saludActual <= (saludTotalEnemigos / 2f))
            {
                jefeYaAparecio = true;
                ActivarJefe();
            }

            // Pagar al jugador
            if (sistemaOro != null)
            {
                sistemaOro.GanarOro(daño * multiplicadorOro);
            }
        }
    }
    void ActivarJefe()
    {
        jefeActivo = true;

        if (objetoJefe != null)
        {
            objetoJefe.SetActive(true); // Enciende al Jefe
        }
    }
}