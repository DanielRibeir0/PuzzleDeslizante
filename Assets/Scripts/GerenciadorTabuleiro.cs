using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorTabuleiro : MonoBehaviour
{
    public static GerenciadorTabuleiro instancia;

    public Peca[] pecas;

    public int linhaVazia = 2;
    public int colunaVazia = 2;

    public float espacoColuna = 105f;
    public float espacoLinha = 105f;

    public float ajusteX = 0f;
    public float ajusteY = -80f;

    public float velocidadeMovimento = 8f;

    public TMP_Text textoMovimentos;
    public TMP_Text textoTempo;

    public AudioSource audioSource;
    public AudioClip somMovimento;

    private int quantidadeMovimentos = 0;
    private float tempoRestante;

    private bool jogoFinalizado = false;
    private bool estaEmbaralhando = false;
    private bool estaAnimando = false;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        tempoRestante = DadosJogo.tempoDaPartida;

        Debug.Log("Tempo recebido na cena Jogo: " + tempoRestante);

        OrganizarPecas();
        StartCoroutine(EmbaralharTabuleiro());

        AtualizarTextoMovimentos();
        AtualizarTextoTempo();
    }

    private void Update()
    {
        if (jogoFinalizado || estaEmbaralhando)
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

            RectTransform rect = pecas[i].GetComponent<RectTransform>();
            rect.anchoredPosition = CalcularPosicao(linha, coluna);
        }

        linhaVazia = 2;
        colunaVazia = 2;
    }

    private IEnumerator EmbaralharTabuleiro()
    {
        estaEmbaralhando = true;

        int quantidadeMisturas = 100;

        for (int i = 0; i < quantidadeMisturas; i++)
        {
            Peca[] pecasPossiveis = System.Array.FindAll(
                pecas,
                p => Mathf.Abs(p.linha - linhaVazia) +
                     Mathf.Abs(p.coluna - colunaVazia) == 1
            );

            Peca pecaEscolhida =
                pecasPossiveis[Random.Range(0, pecasPossiveis.Length)];

            MoverPecaInstantaneamente(pecaEscolhida);
        }

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
        int linhaAntiga = peca.linha;
        int colunaAntiga = peca.coluna;

        RectTransform rect = peca.GetComponent<RectTransform>();
        rect.anchoredPosition = CalcularPosicao(linhaVazia, colunaVazia);

        peca.linha = linhaVazia;
        peca.coluna = colunaVazia;

        linhaVazia = linhaAntiga;
        colunaVazia = colunaAntiga;
    }

    private IEnumerator AnimarMovimento(Peca peca, Vector2 destino)
    {
        estaAnimando = true;

        RectTransform rect = peca.GetComponent<RectTransform>();

        while (Vector2.Distance(rect.anchoredPosition, destino) > 0.1f)
        {
            rect.anchoredPosition = Vector2.Lerp(
                rect.anchoredPosition,
                destino,
                velocidadeMovimento * Time.deltaTime
            );

            yield return null;
        }

        rect.anchoredPosition = destino;
        estaAnimando = false;
    }

    private bool EstaResolvido()
    {
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

    private Vector2 CalcularPosicao(int linha, int coluna)
    {
        return new Vector2(
            coluna * espacoColuna - espacoColuna + ajusteX,
            -linha * espacoLinha + espacoLinha + ajusteY
        );
    }

    private void AtualizarTextoMovimentos()
    {
        textoMovimentos.text = "" + quantidadeMovimentos;
        if (audioSource != null && somMovimento != null &&
            int.Parse(textoMovimentos.text) > 0)
        {
            audioSource.PlayOneShot(somMovimento, 1f);
        }


    }

    private void AtualizarTextoTempo()
    {
        int tempoInteiro = Mathf.CeilToInt(tempoRestante);
        int minutos = tempoInteiro / 60;
        int segundos = tempoInteiro % 60;

        textoTempo.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    public bool TentarMover(Peca peca)
    {
        if (jogoFinalizado || estaEmbaralhando || estaAnimando)
        {
            return false;
        }

        int diferencaLinha = Mathf.Abs(peca.linha - linhaVazia);
        int diferencaColuna = Mathf.Abs(peca.coluna - colunaVazia);

        bool estaAoLado =
            (diferencaLinha == 1 && diferencaColuna == 0) ||
            (diferencaLinha == 0 && diferencaColuna == 1);

        if (estaAoLado)
        {
            int linhaAntiga = peca.linha;
            int colunaAntiga = peca.coluna;

            Vector2 destino = CalcularPosicao(linhaVazia, colunaVazia);

            peca.linha = linhaVazia;
            peca.coluna = colunaVazia;

            linhaVazia = linhaAntiga;
            colunaVazia = colunaAntiga;

            quantidadeMovimentos++;
            AtualizarTextoMovimentos();

            StartCoroutine(MoverEVerificarVitoria(peca, destino));

            return true;
        }

        RectTransform rect = peca.GetComponent<RectTransform>();
        rect.anchoredPosition = CalcularPosicao(peca.linha, peca.coluna);

        return false;
    }

    private IEnumerator MoverEVerificarVitoria(Peca peca, Vector2 destino)
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