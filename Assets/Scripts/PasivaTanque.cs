using UnityEngine;
using UnityEngine.UI;

public class PasivaTanque : MonoBehaviour
{
    [Header("Tiempos de la Habilidad")]
    public float tiempoEntreProvocaciones = 20f;
    public float tiempoPreparacion = 2f;
    public float duracionDistraccion = 5f;
    public float duracionProteccion = 7f;

    [Header("Interfaz UI")]
    public Image barraEnfriamiento;

    [Header("Referencias a Compañeros")]
    public VidaAliado vidaEspadachin;
    public VidaAliado vidaPicaro;

    private Animator animador;
    private VidaAliado miVida;

    private float temporizador = 0f;
    private int faseActual = 0;

    void Start()
    {
        animador = GetComponent<Animator>();
        miVida = GetComponent<VidaAliado>();
        if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 0f;
    }

    void Update()
    {
        if (miVida != null && miVida.vidaActual <= 0)
        {
            // Si el tanque muere exactamente MIENTRAS estaba distrayendo (Fase 2)
            if (faseActual == 2)
            {
                if (vidaEspadachin != null) vidaEspadachin.QuitarInvulnerabilidad();
                if (vidaPicaro != null) vidaPicaro.QuitarInvulnerabilidad();
                Debug.Log("El Tanque cayó. ¡Protección cancelada!");
            }

            faseActual = 0;
            temporizador = 0f;
            if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 0f;
            return;
        }

        // FASE 0: Enfriamiento
        if (faseActual == 0)
        {
            temporizador += Time.deltaTime;
            if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = temporizador / tiempoEntreProvocaciones;

            if (temporizador >= tiempoEntreProvocaciones)
            {
                IniciarFase1();
            }
        }
        // FASE 1: Preparación
        else if (faseActual == 1)
        {
            temporizador += Time.deltaTime;
            if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 1f;

            if (temporizador >= tiempoPreparacion)
            {
                IniciarFase2();
            }
        }
        // FASE 2: Distracción y drenado de barra
        else if (faseActual == 2)
        {
            temporizador += Time.deltaTime;

            if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 1f - (temporizador / duracionDistraccion);

            if (temporizador >= duracionDistraccion)
            {
                TerminarFase2();
            }
        }
    }

    void IniciarFase1()
    {
        faseActual = 1;
        temporizador = 0f;
        if (animador != null) animador.SetTrigger("prepararDistraccion");
    }

    void IniciarFase2()
    {
        faseActual = 2;
        temporizador = 0f; // Reiniciamos para poder contar los 5s de distracción

        if (animador != null) animador.SetTrigger("activarDistraccion");

        // Activamos la protección desde el código directamente, sin depender del Animator
        ProtegerAliados();
    }

    void TerminarFase2()
    {
        faseActual = 0;
        temporizador = 0f;
        if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 0f;

        if (animador != null) animador.SetTrigger("finalizarDistraccion");
    }

    public void ProtegerAliados()
    {
        if (vidaEspadachin != null) vidaEspadachin.HacerInvulnerable(duracionProteccion);
        if (vidaPicaro != null) vidaPicaro.HacerInvulnerable(duracionProteccion);

        Debug.Log("¡El Tanque atrae los ataques! Aliados protegidos.");
    }
}
