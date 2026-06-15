using UnityEngine;
using TMPro;

public class GerenciadorTabuleiro : MonoBehaviour
{
    public static GerenciadorTabuleiro instancia;

    public Peca[] pecas;

    public int linhaVazia = 2;
    public int colunaVazia = 2;

    public float tamanhoCelula = 105f;

    public TMP_Text textoMovimentos;
    private int quantidadeMovimentos = 0;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        OrganizarPecas();
        AtualizarTextoMovimentos();
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
        textoMovimentos.text = "Movimentos: " + quantidadeMovimentos;
    }

    public bool TentarMover(Peca peca)
    {
        int diferencaLinha = Mathf.Abs(peca.linha - linhaVazia);
        int diferencaColuna = Mathf.Abs(peca.coluna - colunaVazia);

        bool estaAoLado =
            (diferencaLinha == 1 && diferencaColuna == 0) ||
            (diferencaLinha == 0 && diferencaColuna == 1);

        if (estaAoLado)
        {
            int linhaAntiga = peca.linha;
            int colunaAntiga = peca.coluna;

            peca.transform.localPosition = CalcularPosicao(linhaVazia, colunaVazia);

            peca.linha = linhaVazia;
            peca.coluna = colunaVazia;

            linhaVazia = linhaAntiga;
            colunaVazia = colunaAntiga;

            quantidadeMovimentos++;
            AtualizarTextoMovimentos();

            return true;
        }

        peca.transform.localPosition = CalcularPosicao(peca.linha, peca.coluna);
        return false;
    }
}