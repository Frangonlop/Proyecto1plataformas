
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class playerControl : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float velocidadX = 5f;
    [SerializeField] float fuerzaSalto = 7f;

    [Header("Doble salto")]
    [SerializeField] float tiempoRecargaDobleSalto = 5f;

    [Header("Controles - Nuevo Input System")]
    [SerializeField] InputActionReference movimiento;
    [SerializeField] InputActionReference salto;

    [Header("Interfaz")]
    [SerializeField] TextMeshProUGUI panelPuntuacion;
    [SerializeField] TextMeshProUGUI panelTiempo;
    [SerializeField] TextMeshProUGUI panelRetardoSalto;

    private Rigidbody2D rb;

    private float movimientox = 0f;
    private bool tocandoSuelo = false;
    private bool solicitarSalto = false;

    private int puntuacion = 0;
    private float tiempoTotal = 0f;

    private bool dobleSaltoDisponible = false;
    private bool yaHaSaltado = false;

    private float tiempoRecarga = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Comenzar la recarga al iniciar el nivel
        tiempoRecarga = tiempoRecargaDobleSalto;
    }

    private void OnEnable()
    {
        if (movimiento != null)
            movimiento.action.Enable();

        if (salto != null)
            salto.action.Enable();
    }

    private void OnDisable()
    {
        if (movimiento != null)
            movimiento.action.Disable();

        if (salto != null)
            salto.action.Disable();
    }

    void Update()
    {
        // Leer movimiento horizontal
        if (movimiento != null)
        {
            Vector2 inputMovimiento =
                movimiento.action.ReadValue<Vector2>();

            movimientox = inputMovimiento.x;
        }

        // Detectar pulsación del salto
        if (salto != null && salto.action.WasPressedThisFrame())
        {
            solicitarSalto = true;
        }

        // Contador general del juego
        tiempoTotal += Time.deltaTime;

        if (panelTiempo != null)
        {
            panelTiempo.text =
                "Tiempo: " + ((int)tiempoTotal) + " s";
        }

        // Recarga global del doble salto
        if (!dobleSaltoDisponible)
        {
            tiempoRecarga -= Time.deltaTime;

            if (tiempoRecarga <= 0f)
            {
                tiempoRecarga = 0f;
                dobleSaltoDisponible = true;
            }
        }

        // Mostrar contador del doble salto
        if (panelRetardoSalto != null)
        {
            if (dobleSaltoDisponible)
            {
                panelRetardoSalto.text =
                    "¡Doble salto disponible!";
            }
            else
            {
                panelRetardoSalto.text =
                    "Doble salto en: " +
                    tiempoRecarga.ToString("F1") + " s";
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        // Movimiento horizontal
        rb.linearVelocity = new Vector2(
            movimientox * velocidadX,
            rb.linearVelocity.y
        );

        // Recuperar el estado al tocar el suelo
        if (tocandoSuelo && rb.linearVelocity.y <= 0.01f)
        {
            yaHaSaltado = false;
        }

        // Realizar salto
        if (solicitarSalto)
        {
            // Primer salto
            if (tocandoSuelo && !yaHaSaltado)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    fuerzaSalto
                );

                yaHaSaltado = true;
                tocandoSuelo = false;

            }
            // Segundo salto, si la recarga global ha terminado
            else if (yaHaSaltado && dobleSaltoDisponible)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    fuerzaSalto
                );

                dobleSaltoDisponible = false;
                tiempoRecarga = tiempoRecargaDobleSalto;

            }

            solicitarSalto = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("moneda"))
        {
            Destroy(collision.gameObject);

            puntuacion++;
            DatosJuego.puntuacion++;

            if (panelPuntuacion != null)
            {
                panelPuntuacion.text =
                    "Puntuacion: " + puntuacion;
            }
        }

        if (collision.gameObject.CompareTag("Suelo"))
        {
            tocandoSuelo = true;
        }

        if (collision.gameObject.CompareTag("ParedInvisible"))
        {
            Debug.Log("choque con pared invisible");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            tocandoSuelo = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            tocandoSuelo = true;
        }
    }
}

