using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorResultado : MonoBehaviour
{
    public void ReiniciarPartida()
    {
        SceneManager.LoadScene("Jogo");
    }

    public void JogarNovamente()
    {
        ReiniciarPartida();
    }

    public void VoltarMenu()
    {
        PlayerPrefs.DeleteKey("TempoDaPartida");
        PlayerPrefs.Save();

        SceneManager.LoadScene("MenuInicial");
    }
}