using UnityEngine;
using UnityEngine.UI;

public class VidaJefe : MonoBehaviour
{
    [Header("Estadísticas")]
    public float vidaMaxima = 500f;
    public float vidaActual;

    [Header("Interfaz UI")]
    public Image barraVidaJefe; // Aquí irá tu imagen VidaJefe.png

    [Header("Referencias")]
    public ProgresoNivel sistemaNivel; // Para avisarle al enjambre que ya morimos

    void Start()
    {
        vidaActual = vidaMaxima;
        if (barraVidaJefe != null) barraVidaJefe.fillAmount = 1f;
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;

        // Actualizar la barra visual
        if (barraVidaJefe != null)
        {
            barraVidaJefe.fillAmount = vidaActual / vidaMaxima;
        }

        // Si la vida llega a 0, el jefe muere
        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        if (sistemaNivel != null)
        {
            sistemaNivel.jefeActivo = false; // Despausamos el daño al enjambre
        }

        // Apagamos al jefe (puedes cambiar esto por una animación de muerte a futuro)
        gameObject.SetActive(false);
    }
}
