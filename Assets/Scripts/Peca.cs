using UnityEngine;

public class Peca : MonoBehaviour
{
    // Posição lógica da peça na grade
    public int linha;
    public int coluna;

    // Chamado ao clicar na peça
    public void Clicar()
    {
        GerenciadorTabuleiro.instancia.TentarMover(this);
    }
}