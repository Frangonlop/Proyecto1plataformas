
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class nivel2 : MonoBehaviour
{
    [Header("Interfaz")]
    [SerializeField] private TextMeshProUGUI panelPuntuacion;
    [SerializeField] private Slider barraVida;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject warning;

    [Header("Victoria")]
    [SerializeField] private ParticleSystem fireworks;

    [Header("Vidas")]
    private int lifes = 3;
    private const int vidasMaximas = 3;

    [Header("Sonidos")]
    [SerializeField] AudioSource musicGameOver;
    [SerializeField] AudioSource backMusic;
    [SerializeField] AudioSource damageSound;
    [SerializeField] AudioSource eliminarSound;
    [SerializeField] AudioSource winMusic;

    [Header("Invulnerabilidad")]
    [SerializeField] private SpriteRenderer spriteJugador;
    [SerializeField] private float tiempoInvulnerable = 1.5f;
    [SerializeField] private float intervaloParpadeo = 0.1f;

    private bool esInvulnerable = false;
    private float temporizadorInvulnerabilidad = 0f;
    private float temporizadorParpadeo = 0f;

    private Rigidbody2D rb;
    private Vector3 posicionOriginal;
    private bool yaReproducido = false;
    private int puntuacion = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        posicionOriginal = transform.position;

        Time.timeScale = 1f;

        // Recuperar puntuación y vidas del nivel anterior
        puntuacion = DatosJuego.puntuacion;
        lifes = Mathf.Clamp(DatosJuego.vidas, 0, vidasMaximas);

        if (panelPuntuacion != null)
        {
            panelPuntuacion.text =
                "Puntuación: " + puntuacion;
        }

        ConfigurarBarraVida();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOver != null)
            gameOver.SetActive(false);

        // Por si el jugador llega sin vidas
        if (lifes == 0)
        {
            MostrarGameOver();
        }
    }

    private void Update()
    {
        if (esInvulnerable)
        {
            temporizadorInvulnerabilidad -= Time.deltaTime;
            temporizadorParpadeo -= Time.deltaTime;

            if (temporizadorParpadeo <= 0f)
            {
                spriteJugador.enabled = !spriteJugador.enabled;
                temporizadorParpadeo = intervaloParpadeo;
            }

            if (temporizadorInvulnerabilidad <= 0f)
            {
                esInvulnerable = false;
                spriteJugador.enabled = true;
            }
        }
    }
    private void ConfigurarBarraVida()
    {
        if (barraVida != null)
        {
            barraVida.minValue = 0;
            barraVida.maxValue = vidasMaximas;
            barraVida.wholeNumbers = true;
            barraVida.value = lifes;
        }
    }

    private void ActualizarBarraVida()
    {
        if (barraVida != null)
        {
            barraVida.value = lifes;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
        {
            ComprobarVictoria();
        }

        if (collision.CompareTag("death"))
        {
            PerderVida();
        }

        if (collision.CompareTag("eliminar"))
        {
            Destroy(collision.transform.parent.gameObject);
            eliminarSound.Play();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
        {
            ComprobarVictoria();
        }

        if (collision.CompareTag("warning"))
        {
            warning.SetActive(true);
        }
        else
        {
            warning.SetActive(false);
        }
    }

    private void ComprobarVictoria()
    {
        if (yaReproducido)
            return;

        if (puntuacion >= 10)
        {
            yaReproducido = true;
            backMusic.Stop();
            winMusic.Play();
            if (winPanel != null)
                winPanel.SetActive(true);

            if (fireworks != null)
                fireworks.Play();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
        {
            if (!yaReproducido && winPanel != null)
                winPanel.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("death"))
        {
            PerderVida();
        }
    }

    private void PerderVida()
    {
        if (esInvulnerable)
            return;

        esInvulnerable = true;
        temporizadorInvulnerabilidad = tiempoInvulnerable;
        temporizadorParpadeo = intervaloParpadeo;
        spriteJugador.enabled = false;

        if (lifes <= 0)
            return;

        lifes--;

        damageSound.Play();

        DatosJuego.vidas = lifes;

        ActualizarBarraVida();

        if (lifes == 0)
        {
            MostrarGameOver();
        }
        else
        {
            // Reaparecer en la posición inicial
            transform.position = posicionOriginal;

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    private void MostrarGameOver()
    {
        if (gameOver != null)
            gameOver.SetActive(true);

        backMusic.Stop();
        musicGameOver.Play();

        Time.timeScale = 0f;
    }
}
