using UnityEngine;
using TMPro; // Librería obligatoria para usar TextMeshPro

public class MostrarOro : MonoBehaviour
{
    private TMP_Text textoOro;

    void Start()
    {
        // El script busca automáticamente el componente de texto en el mismo objeto
        textoOro = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (textoOro != null)
        {
            // Actualizamos el texto leyendo la memoria estática.
            // El ToString("0") hace que el número se vea sin decimales (ej. 15 en vez de 15.5)
            textoOro.text = SistemaOro.oroAcumulado.ToString("0");
        }
    }
}