using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nivel1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float velocidadX = 0.1f;
    [SerializeField] float velocidadY = 0.6f;
    [SerializeField] TextMeshProUGUI panelPuntuacion;
    [SerializeField] GameObject warning;
    [SerializeField] ParticleSystem explosionFX;
    [SerializeField] TextMeshProUGUI lifesTxt;
    [SerializeField] int lifes=3;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject warningDoor;

    private bool haveKey = false;
    private float movimientox = 0f;
    private float movimientoy = 0f;
    private Rigidbody2D rb;
    private bool tocandoSuelo = false;
    private Vector3 posicionOriginal;

    private int puntuacion = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionOriginal = transform.position;
        Time.timeScale = 1f;


    }

    // Update is called once per frame
    void Update()
    {
        // En cada ciclo del juego veo si hay que mover el personaje
        movimientox = Input.GetAxis("Horizontal");
        movimientoy = Input.GetAxis("Vertical");
        if (!tocandoSuelo)
            movimientoy = 0f;

        rb.AddForce(new Vector2(movimientox * velocidadX, movimientoy * velocidadY), ForceMode2D.Impulse);

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "moneda")
        {
            collision.gameObject.SetActive(false);
            puntuacion++;
            DatosJuego.puntuacion = puntuacion;
            panelPuntuacion.text = "Puntuación: " + puntuacion.ToString();
            Destroy(collision.gameObject);
            explosionFX.transform.position = transform.position;
            explosionFX.Play();

        }
        if (collision.gameObject.tag == "Suelo")
            tocandoSuelo = true;

        // Coger la llave y se guarda que la ha cogido
        if (collision.gameObject.tag == "key")
        {
            collision.gameObject.SetActive(false);
            haveKey = true;
            DatosJuego.tieneLlave = true;

        }
        if(collision.gameObject.tag == "door")
        {
            if (haveKey && puntuacion == 10)
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
        if (collision.gameObject.tag == "Suelo")
            tocandoSuelo = false;
        if(collision.gameObject.tag == "door")
            warningDoor.SetActive(false);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("moneda"))
            Debug.Log("dentro de la moneda");

        if (collision.gameObject.tag == "Suelo")
            tocandoSuelo = true;
        if (collision.gameObject.tag == "warning")
            warning.SetActive(true);
        else
            warning.SetActive(false);

       
            

    }

    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
