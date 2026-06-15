using UnityEngine;
using UnityEngine.EventSystems;

public class Peca : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int linha;
    public int coluna;

    public float arrasteMinimoParaMover = 5f;

    private Vector2 inicioToque;
    private bool tentouMover;

    public void OnBeginDrag(PointerEventData eventData)
    {
        inicioToque = eventData.position;
        tentouMover = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (tentouMover)
        {
            return;
        }

        Vector2 diferenca = eventData.position - inicioToque;

        if (diferenca.magnitude < arrasteMinimoParaMover)
        {
            return;
        }

        bool moveu = GerenciadorTabuleiro.instancia.TentarMover(this);

        if (moveu)
        {
            tentouMover = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        tentouMover = false;
    }
}