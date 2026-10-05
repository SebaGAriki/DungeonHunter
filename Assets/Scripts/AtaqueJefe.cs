using UnityEngine;

public class AtaqueJefe : MonoBehaviour
{
    [Header("Tiempos del Ataque")]
    public float tiempoEntreAtaques = 6f; // El cooldown (Fase 0)
    public float tiempoPreparacion = 3f;  // El tiempo de aviso visual (Fase 1)

    [Header("Daño")]
    public float dañoMasivo = 40f;

    [Header("Objetivos Aliados")]
    public VidaAliado vidaTanque;
    public VidaAliado vidaEspadachin;
    public VidaAliado vidaPicaro;

    private Animator animador;
    private float temporizador = 0f;
    private int faseActual = 0; // 0 = Cooldown, 1 = Preparando

    void Awake()
    {
        animador = GetComponent<Animator>();
    }

    // Se ejecuta automáticamente cada vez que el jefe aparece (al llegar al 50%)
    void OnEnable()
    {
        faseActual = 0;
        temporizador = 0f;
    }

    void Update()
    {
        // FASE 0: Cooldown (El jefe pelea normal o espera)
        if (faseActual == 0)
        {
            temporizador += Time.deltaTime;
            if (temporizador >= tiempoEntreAtaques)
            {
                IniciarPreparacion();
            }
        }
        // FASE 1: Preparación (Aviso visual para usar el hechizo "Proteger")
        else if (faseActual == 1)
        {
            temporizador += Time.deltaTime;
            if (temporizador >= tiempoPreparacion)
            {
                EjecutarAtaque();
            }
        }
    }

    void IniciarPreparacion()
    {
        faseActual = 1;
        temporizador = 0f;

        if (animador != null) animador.SetTrigger("prepararAtaqueJefe");
        Debug.Log("¡El Jefe se prepara para un ataque devastador!");
    }

    void EjecutarAtaque()
    {
        faseActual = 0;
        temporizador = 0f;

        if (animador != null) animador.SetTrigger("ataqueJefe");
    }

    // Llama a esta función mediante un Evento de Animación en el fotograma del impacto
    public void AplicarDañoAtodos()
    {
        // El daño masivo solo afecta a los aliados que sigan vivos
        if (vidaTanque != null && vidaTanque.vidaActual > 0) vidaTanque.RecibirDaño(dañoMasivo);
        if (vidaEspadachin != null && vidaEspadachin.vidaActual > 0) vidaEspadachin.RecibirDaño(dañoMasivo);
        if (vidaPicaro != null && vidaPicaro.vidaActual > 0) vidaPicaro.RecibirDaño(dañoMasivo);

        Debug.Log("¡PUM! El Jefe lanzó su ataque masivo a todos.");
    }
}