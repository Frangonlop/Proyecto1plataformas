
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nivel1 : MonoBehaviour
{
    [Header("Interfaz")]
    [SerializeField] GameObject warning;
    [SerializeField] TextMeshProUGUI lifesTxt;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject warningDoor;

    [Header("Vidas")]
    [SerializeField] int lifes = 3;

    private bool haveKey = false;
    private Vector3 posicionOriginal;

    void Start()
    {
        posicionOriginal = transform.position;

        Time.timeScale = 1f;

        DatosJuego.ReiniciarDatos();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobar si el personaje toca una zona mortal
        if (collision.gameObject.CompareTag("death"))
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
        // Recoger la llave
        if (collision.gameObject.CompareTag("key"))
        {
            collision.gameObject.SetActive(false);

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
        // Ocultar aviso al salir de la puerta
        if (collision.gameObject.CompareTag("door"))
        {
            warningDoor.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Mostrar aviso
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
