
using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public float distancia = 3f;

    private Vector2 posicionInicial;
    private int direccion = 1;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Movimiento del enemigo
        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);

        // Comprobar si ha llegado al límite derecho
        if (transform.position.x >= posicionInicial.x + distancia)
        {
            direccion = -1;
            Girar();
        }

        // Comprobar si ha llegado al límite izquierdo
        if (transform.position.x <= posicionInicial.x - distancia)
        {
            direccion = 1;
            Girar();
        }
    }

    void Girar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}
