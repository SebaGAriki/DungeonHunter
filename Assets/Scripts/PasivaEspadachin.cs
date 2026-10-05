using UnityEngine;
using UnityEngine.UI; // Necesario para la interfaz gráfica

public class PasivaEspadachin : MonoBehaviour
{
    [Header("Tiempos de la Habilidad")]
    public float tiempoEntreAtaques = 10f;
    public float tiempoPreparacion = 5f;

    [Header("Daño")]
    public float dañoAtaqueFuerte = 50f;

    [Header("Interfaz UI")]
    public Image barraEnfriamiento; // La barra que mostrará el progreso

    [Header("Referencias")]
    public ProgresoNivel enjambreEnemigo;

    private Animator animador;
    private VidaAliado miVida;

    private float temporizador = 0f;
    private int faseActual = 0; // 0 = Esperando (10s), 1 = Preparando (2s), 2 = Atacando

    void Start()
    {
        animador = GetComponent<Animator>();
        miVida = GetComponent<VidaAliado>();
        if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 0f;
    }

    void Update()
    {
        // Si está convertido en fantasma (vida 0), el reloj se pausa y no hace la pasiva
        if (miVida != null && miVida.vidaActual <= 0)
        {
            faseActual = 0;
            temporizador = 0f;
            if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 0f;
            return; // Detiene la lectura del resto del código mientras sea fantasma
        }

        // FASE 0: Esperando a que pasen los 10 segundos
        if (faseActual == 0)
        {
            temporizador += Time.deltaTime;
            if (barraEnfriamiento != null)
            {
                barraEnfriamiento.fillAmount = temporizador / tiempoEntreAtaques;
            }

            if (temporizador >= tiempoEntreAtaques)
            {
                IniciarFase1();
            }
        }
        // FASE 1: Animación de carga (2 segundos)
        else if (faseActual == 1)
        {
            temporizador += Time.deltaTime;
            if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 1f;

            if (temporizador >= tiempoPreparacion)
            {
                IniciarFase2();
            }
        }
    }

    void IniciarFase1()
    {
        faseActual = 1;
        temporizador = 0f; // Reiniciamos el reloj para medir los 2s de preparación

        // Avisamos al Animator que cambie a la animación de carga
        if (animador != null) animador.SetTrigger("prepararAtaque");
    }

    void IniciarFase2()
    {
        // 1. Solo lanzamos la animación y reiniciamos la barra
        if (animador != null) animador.SetTrigger("ataqueFuerte");

        faseActual = 0;
        temporizador = 0f;
        if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 0f;
    }

    // 2. NUEVA FUNCIÓN: La animación llamará a esto en el fotograma que tú elijas
    public void AplicarDañoFuerte()
    {
        if (enjambreEnemigo != null)
        {
            enjambreEnemigo.RecibirDaño(dañoAtaqueFuerte);
            Debug.Log("¡Daño aplicado exactamente en el golpe!");
        }
    }
}