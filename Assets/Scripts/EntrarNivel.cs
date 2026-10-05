using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena
using UnityEngine.EventSystems;

public class EntrarNivel : MonoBehaviour
{
    [Header("Configuración")]
    // Aquí pondremos el nombre de la escena a la que queremos viajar
    public string nombreEscenaNivel = "SampleScene";

    // Esta función de Unity se ejecuta automáticamente cuando 
    // el jugador hace clic izquierdo sobre el Collider de este objeto
    void OnMouseDown()
    {
        // Si el mouse está sobre cualquier elemento de la interfaz (como tu Panel), abortamos el clic
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Debug.Log("Viajando al nivel...");
        SceneManager.LoadScene(nombreEscenaNivel);
    }
}