
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class nivel1 : MonoBehaviour
{
    [Header("Interfaz")]
    [SerializeField] GameObject warning;
    [SerializeField] Slider barraVida;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject warningDoor;

    [Header("Vidas")]
    [SerializeField] int lifes = 3;
    private const int vidasMaximas = 3;

    [Header("Sonidos")]
    [SerializeField] private AudioSource sonidoDaño;
    [SerializeField] private AudioSource sonidoLlave;
    [SerializeField] private AudioSource musicaFondo;
    [SerializeField] private AudioSource gameOverMusic;


    [Header("Invulnerabilidad")]
    [SerializeField] private SpriteRenderer spriteJugador;
    [SerializeField] private float tiempoInvulnerable = 1.5f;
    [SerializeField] private float intervaloParpadeo = 0.1f;

    private bool esInvulnerable = false;
    private float temporizadorInvulnerabilidad = 0f;
    private float temporizadorParpadeo = 0f;
    private bool haveKey = false;
    private Vector3 posicionOriginal;

    void Start()
    {
        posicionOriginal = transform.position;

        Time.timeScale = 1f;

        DatosJuego.ReiniciarDatos();

        // Comenzar con 3 vidas
        lifes = vidasMaximas;
        DatosJuego.vidas = lifes;

        ConfigurarBarraVida();
    }

    private void Update()
    {
        if (esInvulnerable)
        {
            temporizadorInvulnerabilidad -= Time.deltaTime;
            temporizadorParpadeo -= Time.deltaTime;

            // Alternar la visibilidad del jugador
            if (temporizadorParpadeo <= 0f)
            {
                spriteJugador.enabled = !spriteJugador.enabled;
                temporizadorParpadeo = intervaloParpadeo;
            }

            // Terminar la invulnerabilidad
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("death"))
        {
            if (esInvulnerable)
                return;

            if (lifes <= 0)
                return;

            sonidoDaño.Play();

            esInvulnerable = true;
            temporizadorInvulnerabilidad = tiempoInvulnerable;
            temporizadorParpadeo = intervaloParpadeo;
            spriteJugador.enabled = false;

            // Restar una vida
            lifes--;

            DatosJuego.vidas = lifes;

            // Actualizar barra de vida
            ActualizarBarraVida();

            if (lifes == 0)
            {
                gameOver.SetActive(true);
                Time.timeScale = 0f;
                musicaFondo.Stop();
                gameOverMusic.Play();
            }
            else
            {
                // Volver a la posición inicial
                transform.position = posicionOriginal;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("death"))
        {
            if (esInvulnerable)
                return;

            if (lifes <= 0)
                return;

            sonidoDaño.Play();

            esInvulnerable = true;
            temporizadorInvulnerabilidad = tiempoInvulnerable;
            temporizadorParpadeo = intervaloParpadeo;
            spriteJugador.enabled = false;

            // Restar una vida
            lifes--;

            DatosJuego.vidas = lifes;

            // Actualizar barra de vida
            ActualizarBarraVida();

            if (lifes == 0)
            {
                gameOver.SetActive(true);
                Time.timeScale = 0f;
                musicaFondo.Stop();
                gameOverMusic.Play();
            }
            else
            {
                // Volver a la posición inicial
                transform.position = posicionOriginal;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Recoger la llave
        if (collision.gameObject.CompareTag("key"))
        {
            collision.gameObject.SetActive(false);
            sonidoLlave.Play();
            haveKey = true;

            DatosJuego.tieneLlave = true;
        }

        // Comprobar si entra en la puerta
        if (collision.gameObject.CompareTag("door"))
        {
            if (haveKey && DatosJuego.puntuacion == 10)
            {
                SceneManager.LoadScene("Nivel2");
            }
            else
            {
                warningDoor.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("door"))
        {
            warningDoor.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("warning"))
        {
            warning.SetActive(true);
        }
        else
        {
            warning.SetActive(false);
        }
    }
}
