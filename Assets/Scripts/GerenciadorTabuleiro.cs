using UnityEngine;

public class GerenciadorTabuleiro : MonoBehaviour
{
    // Referência global para acessar o gerenciador
    public static GerenciadorTabuleiro instancia;

    // Lista das peças do puzzle
    public Peca[] pecas;

    // Guarda a posição atual do espaço vazio
    public int linhaVazia = 2;
    public int colunaVazia = 2;

    // Tamanho de cada célula do tabuleiro
    public float tamanhoCelula = 105f;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        // Organiza as peças ao iniciar o jogo
        OrganizarPecas();
    }

    private void OrganizarPecas()
    {
        for (int i = 0; i < pecas.Length; i++)
        {
            int linha = i / 3;
            int coluna = i % 3;

            pecas[i].linha = linha;
            pecas[i].coluna = coluna;

            pecas[i].transform.localPosition =
                CalcularPosicao(linha, coluna);
        }

        // Define a última posição como espaço vazio
        linhaVazia = 2;
        colunaVazia = 2;
    }

    private Vector3 CalcularPosicao(int linha, int coluna)
    {
        return new Vector3(
            coluna * tamanhoCelula - tamanhoCelula,
            -linha * tamanhoCelula + tamanhoCelula,
            0
        );
    }

    public void TentarMover(Peca peca)
    {
        int diferencaLinha =
            Mathf.Abs(peca.linha - linhaVazia);

        int diferencaColuna =
            Mathf.Abs(peca.coluna - colunaVazia);

        // Verifica se a peça está ao lado do espaço vazio
        bool estaAoLado =
            (diferencaLinha == 1 && diferencaColuna == 0) ||
            (diferencaLinha == 0 && diferencaColuna == 1);

        if (estaAoLado)
        {
            int linhaAntiga = peca.linha;
            int colunaAntiga = peca.coluna;

            // Move a peça para o espaço vazio
            peca.transform.localPosition =
                CalcularPosicao(linhaVazia, colunaVazia);

            // Atualiza os dados da peça
            peca.linha = linhaVazia;
            peca.coluna = colunaVazia;

            // Atualiza a nova posição vazia
            linhaVazia = linhaAntiga;
            colunaVazia = colunaAntiga;
        }
    }
}