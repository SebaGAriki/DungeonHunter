using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SistemaMana : MonoBehaviour
{
    [Header("Estadísticas de Maná")]
    public static int manaMaximoGlobal = 3;
    public float manaActual;
    public static float regeneracionPorSegundoGlobal = 0.5f; // Cuánto maná recuperas cada segundo

    [Header("Interfaz de Bloques")]
    public GameObject bloqueManaPrefab; // El prefab "FondoBloque"
    public Transform contenedorMana;    // El "ContenedorMana" de la UI
    private List<Image> rellenosDeBloques = new List<Image>();

    void Start()
    {
        // Al empezar el nivel, tienes el maná al máximo
        manaActual = manaMaximoGlobal;
        ActualizarCantidadDeBloques();

    }

    void Update()
    {
        // Si el maná actual es menor al máximo, lo regeneramos constantemente
        if (manaActual < manaMaximoGlobal)
        {
            manaActual += regeneracionPorSegundoGlobal * Time.deltaTime;

            // Nos aseguramos de que no supere el límite máximo
            manaActual = Mathf.Min(manaActual, manaMaximoGlobal);
        }
        ActualizarCantidadDeBloques();
        ActualizarVisualizacionDeBloques();
    }

    // Función que los hechizos llamarán para intentar gastar maná
    public bool IntentarGastarMana(float costo)
    {
        if (manaActual >= costo)
        {
            // Si nos alcanza, lo restamos y avisamos que la operación fue un éxito
            manaActual -= costo;
            return true;
        }
        else
        {
            // Si no nos alcanza, avisamos que falló
            return false;
        }
    }

    public void ActualizarCantidadDeBloques()
    {
        foreach (Transform hijo in contenedorMana)
        {
            Destroy(hijo.gameObject);
        }
        rellenosDeBloques.Clear();

        for (int i = 0; i < manaMaximoGlobal; i++)
        {
            GameObject nuevoBloque = Instantiate(bloqueManaPrefab, contenedorMana);
            nuevoBloque.transform.SetAsFirstSibling();
            Image relleno = nuevoBloque.transform.Find("RellenoBloque").GetComponent<Image>();
            rellenosDeBloques.Add(relleno);
        }
    }

    void ActualizarVisualizacionDeBloques()
    {
        for (int i = 0; i < rellenosDeBloques.Count; i++)
        {
            float llenadoDeEsteBloque = Mathf.Clamp01(manaActual - i);
            rellenosDeBloques[i].fillAmount = llenadoDeEsteBloque;
        }
    }
}
