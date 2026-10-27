using UnityEngine;
using TMPro;

public class playerControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float velocidadX = 0.1f;
    [SerializeField] float velocidadY = 0.6f;
    [SerializeField] TextMeshProUGUI panelPuntuacion;
    [SerializeField] TextMeshProUGUI panelTiempo;


    private float movimientox = 0f;
    private float movimientoy = 0f;
    private Rigidbody2D rb;
    private bool tocandoSuelo = false;

    private int puntuacion = 0;
    private float tiempoTotal = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // En cada ciclo del juego veo si hay que mover el personaje
        movimientox = Input.GetAxis("Horizontal");
        movimientoy = Input.GetAxis("Vertical");
        if (!tocandoSuelo)
            movimientoy = 0f;

        rb.AddForce( new Vector2(movimientox * velocidadX, movimientoy * velocidadY), ForceMode2D.Impulse);
       
        //  Controlo el tiempo pasado
        tiempoTotal = tiempoTotal + Time.deltaTime;
 
        //panelTiempo.text = "Tiempo: " + ((int)tiempoTotal).ToString() + " s";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bola")
        {
            collision.gameObject.SetActive(false);
            puntuacion++;
            panelPuntuacion.text = "Puntuación: " + puntuacion.ToString();
            Debug.Log(puntuacion);
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("moneda"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Suelo")
          tocandoSuelo = true;

        

        if (collision.gameObject.CompareTag("moneda"))
            Debug.Log("choque con moneda");
        else if (collision.gameObject.CompareTag("ParedInvisible"))
            Debug.Log("choque con pared invisble");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("moneda"))
            Debug.Log("salgo de la moneda");

        if (collision.gameObject.tag == "Suelo")
            tocandoSuelo = false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("moneda"))
            Debug.Log("dentro de la moneda");

        if (collision.gameObject.tag == "Suelo")
            tocandoSuelo = true;

    }

}
