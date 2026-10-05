using UnityEngine;

public class CetroSigueMouse : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadSuavizado = 10f; // Qué tan rápido alcanza al cursor
    public float distanciaBajar = 150f;    // Cuántos píxeles baja el cetro

    private float yNormal; // Para recordar dónde estaba originalmente en Y

    void Start()
    {
        // Al darle Play, guardamos la altura original que le pusiste en el Inspector
        yNormal = transform.position.y;
    }

    void Update()
    {
        // 1. Calculamos la posición X objetivo (la del mouse)
        float objetivoX = Input.mousePosition.x;

        // 2. Calculamos la posición Y objetivo
        float objetivoY = yNormal;

        // Si el mouse está más abajo de la mitad de la pantalla...
        if (Input.mousePosition.y < Screen.height / 2f)
        {
            // ...el objetivo Y baja
            objetivoY = yNormal - distanciaBajar;
        }

        // 3. Creamos el vector con la posición a la que queremos ir
        Vector3 posicionObjetivo = new Vector3(objetivoX, objetivoY, transform.position.z);

        // 4. Movemos el cetro suavemente desde su posición actual hacia la posición objetivo
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * velocidadSuavizado);
    }
}
