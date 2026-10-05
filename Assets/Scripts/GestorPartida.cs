using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escenas

public class GestorPartida : MonoBehaviour
{
    [Header("Condiciones de Victoria/Derrota")]
    public ProgresoNivel progresoNivel; // Para saber la vida enemiga
    public VidaAliado tanque;           // Para saber si están vivos
    public VidaAliado picaro;
    public VidaAliado espadachin;

    [Header("Economía")]
    public SistemaOro sistemaOro;
    public float bonoVictoria = 50f; // El premio extra por llegar al cofre

    [Header("Interfaz")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    private bool juegoTerminado = false;

    void Start()
    {
        // Nos aseguramos de que los paneles estén ocultos y el tiempo corra normal
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Si el juego ya terminó, dejamos de comprobar
        if (juegoTerminado) return;

        // 1. Verificar Victoria (Vida enemiga en 0)
        if (progresoNivel != null && progresoNivel.saludActual <= 0)
        {
            Victoria();
        }
        // 2. Verificar Derrota (Los 3 aliados tienen vida 0)
        else if (tanque.vidaActual <= 0 && picaro.vidaActual <= 0 && espadachin.vidaActual <= 0)
        {
            Derrota();
        }
    }

    void Victoria()
    {
        juegoTerminado = true;
        // Sumamos el bono
        if (sistemaOro != null)
        {
            sistemaOro.GanarOro(bonoVictoria);
        }

        if (panelVictoria != null) panelVictoria.SetActive(true);
        Time.timeScale = 0f; // Congela el juego
    }

    void Derrota()
    {
        juegoTerminado = true;
        if (panelDerrota != null) panelDerrota.SetActive(true);
        Time.timeScale = 0f; // Congela el juego
    }

    // Esta función será llamada al presionar los botones
    public void VolverAlCampamento()
    {
        Time.timeScale = 1f; // Restauramos el tiempo a la normalidad

        // Carga la escena llamada "Campamento" (Asegúrate de que se llame exactamente así)
        SceneManager.LoadScene("Campamento");
    }
}
