
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nivel2 : MonoBehaviour
{
    [Header("Interfaz")]
    [SerializeField] private TextMeshProUGUI panelPuntuacion;
    [SerializeField] private TextMeshProUGUI lifesTxt;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject warning;

    [Header("Victoria")]
    [SerializeField] private ParticleSystem fireworks;

    [Header("Vidas")]
    [SerializeField] private int lifes = 3;

    private Rigidbody2D rb;
    private Vector3 posicionOriginal;
    private bool yaReproducido = false;
    private int puntuacion = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        posicionOriginal = transform.position;

        Time.timeScale = 1f;

        // Recuperar la puntuación del nivel anterior
        puntuacion = DatosJuego.puntuacion;

        if (panelPuntuacion != null)
        {
            panelPuntuacion.text =
                "Puntuación: " + puntuacion;
        }

        if (lifesTxt != null)
        {
            lifesTxt.text = "Vidas: " + lifes;
        }

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOver != null)
            gameOver.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobar llegada a la meta
        if (collision.CompareTag("goal"))
        {
            ComprobarVictoria();
        }
        if (collision.CompareTag("death"))
        {
            PerderVida();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Comprobar si sigue dentro de la meta
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

            if (winPanel != null)
                winPanel.SetActive(true);

            if (fireworks != null)
                fireworks.Play();

            // Detener al personaje
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
        // Comprobar si toca una zona mortal
        if (collision.gameObject.CompareTag("death"))
        {
            PerderVida();
        }
    }

    private void PerderVida()
    {
        if (lifes <= 0)
            return;

        lifes--;

        DatosJuego.vidas = lifes;

        if (lifesTxt != null)
        {
            lifesTxt.text = "Vidas: " + lifes;
        }

        if (lifes == 0)
        {
            if (gameOver != null)
                gameOver.SetActive(true);

            Time.timeScale = 0f;
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
}
