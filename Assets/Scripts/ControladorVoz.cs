using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class ControladorVoz : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    [Header("Referencias para Hechizos")]
    public SistemaMana sistemaMana; // Conexión con el nuevo script
    public VidaAliado vidaTanque;
    public VidaAliado vidaEspadachin;
    public VidaAliado vidaPicaro;

    [Header("Curaura")]
    public float cantidadCura = 10f;
    public float costoCurar = 20f;  // Cuánto maná cuesta usar la cura

    [Header("Proteger")]
    public float duracionHechizoProteccion = 5f; // Duración del escudo mágico
    public float costoProteger = 2f; // Costo en bloques de maná

    [Header("Efecto Visual")]
    public GameObject escudoVisual; // Aquí conectaremos tu objeto 2D/3D

    void Start()
    {
        // Creamos la palabra clave "curar", tal como en tu VoiceRecognizer[cite: 11]
        keywords.Add("curar", () =>
        {
            IntentarCurar(cantidadCura); // Llamamos a la función de curar con 30 puntos
        });

        keywords.Add("defensa", () =>
        {
            IntentarProteccion(duracionHechizoProteccion);
        });

        // Inicializamos el reconocedor con las palabras del diccionario[cite: 11]
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            IntentarCurar(cantidadCura);
        };
        if (Input.GetKeyDown(KeyCode.I))
        {
            IntentarProteccion(duracionHechizoProteccion);
        };
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        // Si detecta la palabra, ejecuta la acción asociada[cite: 11]
        if (keywords.TryGetValue(args.text, out System.Action keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void IntentarCurar(float cantidadCura)
    {
        // 1. Disparamos un rayo desde la cámara hacia donde está el mouse en la pantalla
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit golpe;

        // 2. Si el rayo choca con un Collider...
        if (Physics.Raycast(rayo, out golpe))
        {
            // 3. Verificamos si el objeto golpeado tiene el script VidaAliado
            VidaAliado aliado = golpe.collider.GetComponent<VidaAliado>();

            if (aliado != null)
            {
                // NUEVO: Verificamos si el aliado está vivo antes de curarlo
                if (aliado.vidaActual > 0)
                {
                    // Solo gastamos maná y curamos si tiene vida mayor a 0
                    if (sistemaMana != null && sistemaMana.IntentarGastarMana(costoCurar))
                    {
                        aliado.RecibirCura(cantidadCura);
                        Debug.Log("¡Aliado curado!");
                    }
                }
                else
                {
                    // Opcional: Un mensaje de aviso en consola
                    Debug.Log("Este aliado es un fantasma. Usa el hechizo de resurrección del libro.");
                }
            }
        }
    }

    // Apagamos el micrófono al cerrar para evitar errores, basado en tu DetectorCommandControl[cite: 12]
    void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            if (keywordRecognizer.IsRunning) keywordRecognizer.Stop();
            keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Dispose();
        }
    }

    public void IntentarProteccion(float duracionHechizoProteccion)
    {
        // 1. Verificamos si hay maná suficiente (IntentarGastarMana ya hace el descuento si devuelve true)
        if (sistemaMana != null && sistemaMana.IntentarGastarMana(costoProteger))
        {
            // 2. Otorgamos el escudo mágico a los aliados que sigan vivos
            if (vidaTanque != null && vidaTanque.vidaActual > 0)
                vidaTanque.HacerInvulnerable(duracionHechizoProteccion);

            if (vidaEspadachin != null && vidaEspadachin.vidaActual > 0)
                vidaEspadachin.HacerInvulnerable(duracionHechizoProteccion);

            if (vidaPicaro != null && vidaPicaro.vidaActual > 0)
                vidaPicaro.HacerInvulnerable(duracionHechizoProteccion);

            // Activar el feedback visual
            if (escudoVisual != null)
            {
                escudoVisual.SetActive(true);
                CancelInvoke("ApagarEscudoVisual"); // Por si lanzas el hechizo 2 veces seguidas
                Invoke("ApagarEscudoVisual", duracionHechizoProteccion);
            }
        }
        else
        {
        }
    }
    private void ApagarEscudoVisual()
    {
        if (escudoVisual != null)
        {
            escudoVisual.SetActive(false);
        }
    }
}
