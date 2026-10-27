
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class nivel2 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI panelPuntuacion;
    [SerializeField] private TextMeshProUGUI lifesTxt;

    [SerializeField] private int puntuacion = 10;
    [SerializeField] private int lifes = 3;
    [SerializeField] ParticleSystem fireworks;
    private bool yaReproducido = false;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject gameOver;


    [SerializeField] float velocidadX = 0.1f;
    [SerializeField] float velocidadY = 1.1f;
    private float movimientox = 0f;
    private float movimientoy = 0f;
    private Rigidbody2D rb;
    private bool tocandoSuelo = false;
    private Vector3 posicionOriginal;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionOriginal = transform.position;
        Time.timeScale = 1f;
        panelPuntuacion.text = "Puntuación: " + puntuacion;
        lifesTxt.text = "Vidas: " + lifes;
    }

    private void Update()
    {
        // En cada ciclo del juego veo si hay que mover el personaje
        movimientox = Input.GetAxis("Horizontal");
        movimientoy = Input.GetAxis("Vertical");
        if (!tocandoSuelo)
            movimientoy = 0f;

        rb.AddForce(new Vector2(movimientox * velocidadX, movimientoy * velocidadY), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Suelo")
            tocandoSuelo = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            if (yaReproducido) return;
            if (puntuacion == 10)
            {
                winPanel.SetActive(true);
                fireworks.Play();
                yaReproducido = true;
                rb.linearVelocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Suelo")
            tocandoSuelo = false;

        if (collision.gameObject.tag == "goal")
        {
            if (puntuacion == 10)
            {
                winPanel.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "death")
        {
            transform.position = posicionOriginal;
            if (lifes > 0)
            {
                lifes--;
                DatosJuego.vidas = lifes;
                lifesTxt.text = "Vidas: " + lifes;
            }
            if (lifes == 0)
            {
                gameOver.SetActive(true);
                Time.timeScale = 0f;
            }

        }
    }
}
