using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
   public void Play()
    {
        DatosJuego.ReiniciarDatos();
        SceneManager.LoadScene("Nivel1");
    }
    public void Return()
    {
        SceneManager.LoadScene("Inicio");
    }
}
