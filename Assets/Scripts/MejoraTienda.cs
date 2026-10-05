using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; // Agregamos la librería

public class MejoraTienda : MonoBehaviour
{
    [Header("Interfaz")]
    public GameObject panelTienda;

    [Header("Costos de Mejora")]
    public float costoMaxMana = 10f;
    public float costoRegenMana = 15f;
    public float mejoraRegenMana = 0.5f;

    [Header("Textos UI - Maná Máximo")]
    public TMP_Text textoCostoMaxMana;
    public TMP_Text textoNivelMaxMana;

    [Header("Textos UI - Regeneración")]
    public TMP_Text textoCostoRegen;
    public TMP_Text textoNivelRegen;

    void Start()
    {
        // Nos aseguramos de que el panel empiece cerrado
        if (panelTienda != null) panelTienda.SetActive(false);
    }

    // Abre el panel al hacer clic en el modelo 3D de la fogata
    void OnMouseDown()
    {
        // El escudo protector contra clics accidentales
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (panelTienda != null)
        {
            panelTienda.SetActive(true);
            ActualizarTextos();
        }
    }

    // Función para el botón Cerrar
    public void CerrarTienda()
    {
        if (panelTienda != null) panelTienda.SetActive(false);
    }

    // Función para el botón de Maná Máximo
    public void ComprarMaxMana()
    {
        if (SistemaOro.oroAcumulado >= costoMaxMana)
        {
            SistemaOro.oroAcumulado -= costoMaxMana;
            SistemaMana.manaMaximoGlobal += 1;

            ActualizarTextos();
        }
        else
        {
            Debug.Log("No hay oro suficiente.");
        }
    }

    // Función para el botón de Regeneración
    public void ComprarRegenMana()
    {
        if (SistemaOro.oroAcumulado >= costoRegenMana)
        {
            SistemaOro.oroAcumulado -= costoRegenMana;
            SistemaMana.regeneracionPorSegundoGlobal += mejoraRegenMana; // Aumenta medio punto por segundo

            ActualizarTextos();
        }
        else
        {
            Debug.Log("No hay oro suficiente.");
        }
    }

    private void ActualizarTextos()
    {
        if (textoCostoMaxMana != null)
            textoCostoMaxMana.text = "Costo: " + costoMaxMana + " Oro";

        if (textoNivelMaxMana != null)
            textoNivelMaxMana.text = "Actual: " + SistemaMana.manaMaximoGlobal + " Mana Maximo";

        if (textoCostoRegen != null)
            textoCostoRegen.text = "Costo: " + costoRegenMana + " Oro";

        if (textoNivelRegen != null)
            textoNivelRegen.text = "Actual: " + SistemaMana.regeneracionPorSegundoGlobal + " / seg";
    }
}
