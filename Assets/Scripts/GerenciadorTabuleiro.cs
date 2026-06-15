using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorTabuleiro : MonoBehaviour
{
    public static GerenciadorTabuleiro instancia;

    // Lista com todas as peças do puzzle
    public Peca[] pecas;

    // Guarda a posição atual do espaço vazio
    public int linhaVazia = 2;
    public int colunaVazia = 2;

    // Distância entre as células do tabuleiro
    public float tamanhoCelula = 105f;

    // Velocidade da animação das peças
    public float velocidadeMovimento = 8f;

    public TMP_Text textoMovimentos;
    public TMP_Text textoTempo;

    private int quantidadeMovimentos = 0;
    private float tempoRestante = 20f;

    private bool jogoFinalizado = false;
    private bool estaEmbaralhando = false;
    private bool estaAnimando = false;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        OrganizarPecas();

        // Embaralha tudo antes do jogador conseguir interagir
        StartCoroutine(EmbaralharTabuleiro());

        AtualizarTextoMovimentos();

        if (DadosJogo.contraOTempo)
        {
            textoTempo.gameObject.SetActive(true);
            AtualizarTextoTempo();
        }
        else
        {
            textoTempo.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!DadosJogo.contraOTempo || jogoFinalizado || estaEmbaralhando)
        {
            return;
        }

        tempoRestante -= Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            AtualizarTextoTempo();

            jogoFinalizado = true;
            SceneManager.LoadScene("Derrota");
            return;
        }

        AtualizarTextoTempo();
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

        linhaVazia = 2;
        colunaVazia = 2;
    }

    private IEnumerator EmbaralharTabuleiro()
    {
        estaEmbaralhando = true;

        // Quantidade de movimentos aleatórios usados para misturar o puzzle
        int quantidadeMisturas = 100;

        for (int i = 0; i < quantidadeMisturas; i++)
        {
            // Busca apenas peças vizinhas ao espaço vazio
            Peca[] pecasPossiveis = System.Array.FindAll(
                pecas,
                p => Mathf.Abs(p.linha - linhaVazia) +
                     Mathf.Abs(p.coluna - colunaVazia) == 1
            );

            // Escolhe uma peça aleatória dentre as válidas
            Peca pecaEscolhida =
                pecasPossiveis[Random.Range(0, pecasPossiveis.Length)];

            // Move sem animação e sem contabilizar como jogada do jogador
            MoverPecaInstantaneamente(pecaEscolhida);
        }

        // Garante que o jogo nunca comece resolvido
        if (EstaResolvido())
        {
            yield return StartCoroutine(EmbaralharTabuleiro());
            yield break;
        }

        quantidadeMovimentos = 0;
        AtualizarTextoMovimentos();

        estaEmbaralhando = false;

        yield return null;
    }

    private void MoverPecaInstantaneamente(Peca peca)
    {
        // Guarda a posição lógica atual da peça
        int linhaAntiga = peca.linha;
        int colunaAntiga = peca.coluna;

        // Move para a posição vazia
        peca.transform.localPosition =
            CalcularPosicao(linhaVazia, colunaVazia);

        // Atualiza a posição lógica da peça
        peca.linha = linhaVazia;
        peca.coluna = colunaVazia;

        // Atualiza a nova posição vazia
        linhaVazia = linhaAntiga;
        colunaVazia = colunaAntiga;
    }

    private IEnumerator AnimarMovimento(Peca peca, Vector3 destino)
    {
        estaAnimando = true;

        while (Vector3.Distance(peca.transform.localPosition, destino) > 0.1f)
        {
            peca.transform.localPosition = Vector3.Lerp(
                peca.transform.localPosition,
                destino,
                velocidadeMovimento * Time.deltaTime
            );

            yield return null;
        }

        peca.transform.localPosition = destino;
        estaAnimando = false;
    }

    private bool EstaResolvido()
    {
        // Verifica se todas as peças estão em suas posições corretas
        for (int i = 0; i < pecas.Length; i++)
        {
            int linhaCorreta = i / 3;
            int colunaCorreta = i % 3;

            if (pecas[i].linha != linhaCorreta ||
                pecas[i].coluna != colunaCorreta)
            {
                return false;
            }
        }

        return true;
    }

    private Vector3 CalcularPosicao(int linha, int coluna)
    {
        return new Vector3(
            coluna * tamanhoCelula - tamanhoCelula,
            -linha * tamanhoCelula + tamanhoCelula,
            0
        );
    }

    private void AtualizarTextoMovimentos()
    {
        textoMovimentos.text =
            "Movimentos: " + quantidadeMovimentos;
    }

    private void AtualizarTextoTempo()
    {
        textoTempo.text =
            "Tempo: " + Mathf.CeilToInt(tempoRestante);
    }

    public bool TentarMover(Peca peca)
    {
        if (jogoFinalizado || estaEmbaralhando || estaAnimando)
        {
            return false;
        }

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

            Vector3 destino = CalcularPosicao(linhaVazia, colunaVazia);

            // Atualiza a posição lógica antes da animação
            peca.linha = linhaVazia;
            peca.coluna = colunaVazia;

            linhaVazia = linhaAntiga;
            colunaVazia = colunaAntiga;

            quantidadeMovimentos++;
            AtualizarTextoMovimentos();

            StartCoroutine(MoverEVerificarVitoria(peca, destino));

            return true;
        }

        peca.transform.localPosition =
            CalcularPosicao(peca.linha, peca.coluna);

        return false;
    }

    private IEnumerator MoverEVerificarVitoria(Peca peca, Vector3 destino)
    {
        yield return StartCoroutine(AnimarMovimento(peca, destino));

        VerificarVitoria();
    }

    private void VerificarVitoria()
    {
        if (!EstaResolvido())
        {
            return;
        }

        jogoFinalizado = true;
        SceneManager.LoadScene("Vitoria");
    }
}