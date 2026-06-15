using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorResultado : MonoBehaviour
{
    public void JogarNovamente()
    {
        Debug.Log("Clicou em Jogar Novamente");
        SceneManager.LoadScene("Jogo");
    }

    public void VoltarMenu()
    {
        Debug.Log("Clicou em Voltar Menu");
        SceneManager.LoadScene("MenuInicial");
    }
}