using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{
    [Header("Objetivos Aliados")]
    public VidaAliado tanque;
    public float dañoAlTanque = 5f; // Recibe más daño

    public VidaAliado picaro;
    public float dañoAlPicaro = 1f;

    public VidaAliado espadachin;
    public float dañoAlEspadachin = 2.5f;

    void Update()
    {
        // El enjambre ataca a todos los que sigan vivos
        if (tanque != null) tanque.RecibirDaño(dañoAlTanque * Time.deltaTime);
        if (picaro != null) picaro.RecibirDaño(dañoAlPicaro * Time.deltaTime);
        if (espadachin != null) espadachin.RecibirDaño(dañoAlEspadachin * Time.deltaTime);
    }
}