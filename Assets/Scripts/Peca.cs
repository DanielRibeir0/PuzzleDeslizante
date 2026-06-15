using UnityEngine;
using UnityEngine.EventSystems;

public class Peca : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int linha;
    public int coluna;

    private Vector3 posicaoInicial;
    private RectTransform rectTransform;
    private float deslocamento;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posicaoInicial = rectTransform.localPosition;
        deslocamento = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        int diferencaLinha = GerenciadorTabuleiro.instancia.linhaVazia - linha;
        int diferencaColuna = GerenciadorTabuleiro.instancia.colunaVazia - coluna;

        bool estaAoLado =
            (Mathf.Abs(diferencaLinha) == 1 && diferencaColuna == 0) ||
            (Mathf.Abs(diferencaColuna) == 1 && diferencaLinha == 0);

        if (!estaAoLado)
        {
            rectTransform.localPosition = posicaoInicial;
            return;
        }

        Vector3 novaPosicao = posicaoInicial;

        if (diferencaColuna != 0)
        {
            deslocamento += eventData.delta.x;
            deslocamento = Mathf.Clamp(deslocamento, -105f, 105f);

            if (Mathf.Sign(deslocamento) == Mathf.Sign(diferencaColuna))
                novaPosicao.x += deslocamento;
        }
        else if (diferencaLinha != 0)
        {
            deslocamento += eventData.delta.y;
            deslocamento = Mathf.Clamp(deslocamento, -105f, 105f);

            if (Mathf.Sign(deslocamento) == -Mathf.Sign(diferencaLinha))
                novaPosicao.y += deslocamento;
        }

        rectTransform.localPosition = novaPosicao;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool moveu = GerenciadorTabuleiro.instancia.TentarMover(this);

        if (!moveu)
        {
            rectTransform.localPosition = posicaoInicial;
        }
    }
}