using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorMenu : MonoBehaviour
{
    // Carrega a cena principal do jogo
    public void Jogar()
    {
        SceneManager.LoadScene("Jogo");
    }
}