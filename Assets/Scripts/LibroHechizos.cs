using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class LibroHechizos : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [Header("UI del Libro")]
    [SerializeField] TMP_Text textoMensaje;

    [Header("Palabras del Conjuro")]
    public string[] palabras = { "pescado", "casa", "manzana", "banana", "fuego", "agua", "elfo" };
    public int largoSecuenciaRevivir = 3;

    [Header("Costo de Maná")]
    public SistemaMana sistemaMana;
    public float costoRevivir = 3f; // Lo que cuesta lanzar este hechizo

    [Header("Aliados a Revivir")]
    public VidaAliado vidaTanque;
    public VidaAliado vidaEspadachin;
    public VidaAliado vidaPicaro;

    List<string> secuencia = new List<string>();
    int indiceActual = 0;

    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(palabras, ConfidenceLevel.Medium);
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        GenerarSecuencia();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Verificamos si hay maná antes de revivir con la tecla O
            if (sistemaMana != null && sistemaMana.IntentarGastarMana(costoRevivir))
            {
                Mostrar("¡CONJURO COMPLETADO!\nAliados revividos (Tecla O).");
                RevivirAliados();
                Invoke(nameof(GenerarSecuencia), 3f);
            }
            else
            {
                Mostrar("¡FALTA MANÁ!\nEl conjuro se desvanece.");
            }
        };
    }
    void GenerarSecuencia()
    {
        // Mezcla las palabras aleatoriamente y toma las indicadas sin repetir
        secuencia = palabras.OrderBy(p => Random.value).Take(largoSecuenciaRevivir).ToList();
        indiceActual = 0;

        Mostrar($"Hechizo de Resurrección\n(Costo: {costoRevivir} Maná)\n\nDi:\n" + string.Join("\n", secuencia));
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string dicha = args.text;
        string esperada = secuencia[indiceActual];

        if (dicha == esperada)
        {
            indiceActual++;

            if (indiceActual >= secuencia.Count)
            {
                // Al terminar de leer, revisamos si hay maná
                if (sistemaMana != null && sistemaMana.IntentarGastarMana(costoRevivir))
                {
                    Mostrar("¡CONJURO COMPLETADO!\nAliados revividos.");
                    RevivirAliados();
                }
                else
                {
                    Mostrar("¡FALTA MANÁ!\nEl conjuro se desvanece.");
                }

                // Genera nuevas palabras aleatorias después de 3 segundos
                Invoke(nameof(GenerarSecuencia), 3f);
            }
            else
            {
                Mostrar($"Correcto: {dicha}...\nSigue leyendo.");
            }
        }
        else
        {
            Mostrar($"¡Fallaste!\nDijiste '{dicha}'.\nEl hechizo se reinició.");
            Invoke(nameof(GenerarSecuencia), 2f);
        }
    }

    void RevivirAliados()
    {
        if (vidaTanque != null && vidaTanque.vidaActual <= 0)
            vidaTanque.RecibirCura(vidaTanque.vidaMaxima);

        if (vidaEspadachin != null && vidaEspadachin.vidaActual <= 0)
            vidaEspadachin.RecibirCura(vidaEspadachin.vidaMaxima);

        if (vidaPicaro != null && vidaPicaro.vidaActual <= 0)
            vidaPicaro.RecibirCura(vidaPicaro.vidaMaxima);

        Debug.Log("¡Los aliados han regresado a la vida a máxima salud!");
    }

    void Mostrar(string mensaje)
    {
        if (textoMensaje != null)
            textoMensaje.text = mensaje;
    }

    void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            if (keywordRecognizer.IsRunning) keywordRecognizer.Stop();
            keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Dispose();
        }
    }
}
