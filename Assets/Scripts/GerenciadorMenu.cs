using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorMenu : MonoBehaviour
{
    public void JogarCasual()
    {
        DadosJogo.contraOTempo = false;
        SceneManager.LoadScene("Jogo");
    }

    public void JogarContraOTempo()
    {
        DadosJogo.contraOTempo = true;
        SceneManager.LoadScene("Jogo");
    }
}