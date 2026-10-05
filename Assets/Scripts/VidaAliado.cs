using UnityEngine;
using UnityEngine.UI; // Muy importante para poder modificar la "Image" de la barra

public class VidaAliado : MonoBehaviour
{
    [Header("Estadísticas de Vida")]
    public float vidaMaxima = 100f;
    public float vidaActual;

    [Header("Interfaz UI")]
    public Image barraRelleno; // Aquí arrastraremos la imagen "RellenoVida"

    [Header("Efectos de Cura")]
    public bool esInvulnerable = false;
    public AudioClip sonidoCura;
    public GameObject particulasCuraPrefab;

    [Header("Efectos de Fantasma")]
    public float alturaFlote = 0.5f;     // Qué tan alto sube el fantasma
    public float velocidadFlote = 3f;    // Qué tan rápido hace el balanceo
    //public float subirCollider = 1f;     // Cuánto sube el área verde para apuntarle

    private AudioSource generadorSonido;
    private Animator animador; // Variable para controlar la animación
    private float yOriginal; // Variables para recordar dónde estaba originalmente el personaje

    //private BoxCollider colisionador
    //private Vector3 centroColliderOriginal;

    void Start()
    {
        // Al empezar, la vida está llena
        vidaActual = vidaMaxima;

        // Le añadimos un componente de audio invisible al personaje para que pueda "hablar"
        generadorSonido = gameObject.AddComponent<AudioSource>();

        // Obtenemos el componente Animator del personaje
        animador = GetComponent<Animator>();

        // Guardamos las posiciones originales para saber a dónde volver cuando reviva
        yOriginal = transform.position.y;

        // Obtenemos el Box Collider
        //colisionador = GetComponent<BoxCollider>();
        //if (colisionador != null) centroColliderOriginal = colisionador.center;
    }

    void Update()
    {
        // Si el personaje aún tiene vida...
        if (vidaActual > 0)
        {
            // --- ESTADO: VIVO ---

            // Actualizamos visualmente la barra (fillAmount requiere un valor entre 0 y 1)
            if (barraRelleno != null) barraRelleno.fillAmount = vidaActual / vidaMaxima;

            // Si está vivo, le decimos al animador que NO está muerto
            if (animador != null) animador.SetBool("estaMuerto", false);

            // Anclamos al personaje al suelo original
            transform.position = new Vector3(transform.position.x, yOriginal, transform.position.z);

            // Devolvemos el collider a su posición original (en el cuerpo)
            //if (colisionador != null) colisionador.center = centroColliderOriginal; 
        }
        else
        {
            // --- ESTADO: FANTASMA ---

            if (barraRelleno != null) barraRelleno.fillAmount = 0;

            // Si la vida llega a 0, le decimos al animador que SÍ está muerto
            if (animador != null) animador.SetBool("estaMuerto", true);

            // 1. Efecto de flotar usando el tiempo del juego y la función Seno
            float nuevoY = yOriginal + (Mathf.Sin(Time.time * velocidadFlote) * alturaFlote);

            // Aplicamos un pequeño offset extra (alturaFlote) para que el centro del balanceo sea más arriba del suelo
            transform.position = new Vector3(transform.position.x, nuevoY + alturaFlote, transform.position.z);

            // 2. Subimos el Collider para que puedas apuntarle con el mouse al fantasma
            //if (colisionador != null) colisionador.center = centroColliderOriginal + new Vector3(0, subirCollider, 0); 
        }

    }
    public void RecibirDaño(float cantidad)
    {
        if (esInvulnerable) return; // Si el escudo está activo, ignoramos el daño y salimos de la función
        if (vidaActual > 0)
        {
            vidaActual -= cantidad;
            vidaActual = Mathf.Max(vidaActual, 0);
        }
    }
    public void HacerInvulnerable(float duracion)
    {
        esInvulnerable = true;
        // Cancela cualquier temporizador viejo por si le tiras dos escudos seguidos
        CancelInvoke("QuitarInvulnerabilidad");
        // Programa que el escudo se apague después de 'duracion' segundos
        Invoke("QuitarInvulnerabilidad", duracion);
    }

    public void QuitarInvulnerabilidad()
    {
        esInvulnerable = false;
        CancelInvoke("QuitarInvulnerabilidad");
    }

    public void RecibirCura(float cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;

        if (sonidoCura != null && generadorSonido != null) generadorSonido.PlayOneShot(sonidoCura); // 1. Reproducir el sonido

        
        if (particulasCuraPrefab != null && barraRelleno != null) // 2. Mostrar las partículas
        {
            // Creamos las partículas un poco más arriba de la posición central del personaje
            Vector3 posicionEfecto = barraRelleno.transform.position + new Vector3(0, 0, -1f);
            Instantiate(particulasCuraPrefab, posicionEfecto, Quaternion.identity);
        }
    }
}