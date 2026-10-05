using UnityEngine;
using UnityEngine.UI;

public class PasivaPicaro : MonoBehaviour
{
    [Header("Tiempos de la Habilidad")]
    public float tiempoEntreBusquedas = 30f;
    public float tiempoPreparacion = 3f;

    [Header("Recompensa")]
    public float oroEncontrado = 100f;

    [Header("Interfaz UI")]
    public Image barraEnfriamiento;

    [Header("Referencias")]
    public SistemaOro sistemaOro; // Tu conexión directa al banco

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
        // Si es fantasma, se reinicia y pausa
        if (miVida != null && miVida.vidaActual <= 0)
        {
            faseActual = 0;
            temporizador = 0f;
            if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 0f;
            return;
        }

        // FASE 0: Esperando 30s
        if (faseActual == 0)
        {
            temporizador += Time.deltaTime;

            if (barraEnfriamiento != null)
            {
                barraEnfriamiento.fillAmount = temporizador / tiempoEntreBusquedas;
            }

            if (temporizador >= tiempoEntreBusquedas)
            {
                IniciarFase1();
            }
        }
        // FASE 1: Animación de aviso (3s)
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
        temporizador = 0f;
        if (animador != null) animador.SetTrigger("prepararBusqueda");
    }

    void IniciarFase2()
    {
        if (animador != null) animador.SetTrigger("encontrarTesoro");

        faseActual = 0;
        temporizador = 0f;
        if (barraEnfriamiento != null) barraEnfriamiento.fillAmount = 0f;
    }

    // Llama a esta función desde el Evento de Animación al finalizar su acción
    public void ReclamarTesoro()
    {
        if (sistemaOro != null)
        {
            sistemaOro.GanarOro(oroEncontrado);
            Debug.Log("¡Pícaro encontró " + oroEncontrado + " de oro!");
        }
    }
}
