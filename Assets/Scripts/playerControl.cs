
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

    [SerializeField] private ParticleSystem efectoMoneda;
    [SerializeField] private AudioSource sonidoMoneda;
    public AudioClip blinMoneda;

    [SerializeField] private AudioSource sonidoSalto;
    private Rigidbody2D rb;


    [SerializeField] float velocidadHielo = 10f;
    private bool sobreHielo = false;


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

        // Comenzar la recarga del doble salto al iniciar el nivel
        tiempoRecarga = tiempoRecargaDobleSalto;

        tiempoTotal = DatosJuego.tiempo;
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

        // Detectar salto
        if (salto != null && salto.action.WasPressedThisFrame())
        {
            solicitarSalto = true;
        }

        // Contador de tiempo del juego
        tiempoTotal += Time.deltaTime;
        DatosJuego.tiempo = (int)tiempoTotal;

        if (panelTiempo != null)
        {
            panelTiempo.text =
                "Tiempo: " + ((int)tiempoTotal) + " s";
        }

        // Recarga del doble salto
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

        float velocidadActual = sobreHielo ? velocidadHielo : velocidadX;

        rb.linearVelocity = new Vector2(
            movimientox * velocidadActual,
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
                sonidoSalto.Play();
                yaHaSaltado = true;
                tocandoSuelo = false;

            }
            // Segundo salto, solo si la recarga de tiempo ha terminado
            else if (yaHaSaltado && dobleSaltoDisponible)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    fuerzaSalto
                );
                sonidoSalto.Play();

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
            efectoMoneda.transform.position = collision.transform.position;
            efectoMoneda.Play();
            sonidoMoneda.PlayOneShot(blinMoneda);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("hielo"))
        {
            sobreHielo = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("hielo"))
        {
            sobreHielo = false;
        }
    }
}

