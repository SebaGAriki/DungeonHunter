using UnityEngine;

public class SistemaOro : MonoBehaviour
{
    // Al ser "static", este valor NO se borra al cambiar al campamento o al nivel
    public static float oroAcumulado = 0f;

    [Header("Visualización (Solo para el Inspector)")]
    public float oroActual; // Usamos esta variable normal solo para que puedas ver cuánto oro tienes en Unity

    void Start()
    {
        // Al cargar la escena, actualizamos el número del Inspector con lo que hay en la memoria global
        oroActual = oroAcumulado;
    }

    public void GanarOro(float cantidad)
    {
        // Sumamos el oro a la memoria global
        oroAcumulado += cantidad;

        // Actualizamos el número del Inspector para que lo veas subir en tiempo real
        oroActual = oroAcumulado;
    }
}