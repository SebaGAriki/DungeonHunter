using UnityEngine;

public class AtaqueAliado : MonoBehaviour
{
    [Header("Ataque")]
    public ProgresoNivel enjambreEnemigo; // El nivel/enemigos a los que ataca
    public float dañoPorSegundo = 3f;     // Cuánto daño hace este aliado

    private VidaAliado miVida;

    void Start()
    {
        miVida = GetComponent<VidaAliado>();
    }

    void Update()
    {
        // ¡Solo hace daño si el aliado está vivo (no es fantasma)!
        if (miVida != null && miVida.vidaActual > 0 && enjambreEnemigo != null)
        {
            enjambreEnemigo.RecibirDaño(dañoPorSegundo * Time.deltaTime);
        }
    }
}