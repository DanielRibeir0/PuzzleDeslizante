using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorMenu : MonoBehaviour
{
    public void JogarCasual()
    {
        DadosJogo.tempoDaPartida = 300f;
        //Debug.Log("CASUAL - 300 segundos");
        SceneManager.LoadScene("Jogo");
    }

    public void JogarContraOTempo()
    {
        DadosJogo.tempoDaPartida = 40f;
        //Debug.Log("CONTRA O TEMPO - 40 segundos");
        SceneManager.LoadScene("Jogo");
    }
}