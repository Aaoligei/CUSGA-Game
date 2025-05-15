using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CardRotation cardRotation;

    // Start is called before the first frame update
    void Start()
    {
        cardRotation = GetComponent<CardRotation>();
        if (cardRotation == null)
        {
            cardRotation = GetComponentInParent<CardRotation>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject card = eventData.pointerEnter;
        if (card != null && card.CompareTag("Card"))
        {
            RectTransform cardRect = card.GetComponent<RectTransform>();
            if (cardRect != null && !cardRotation.cards.Contains(cardRect))
            {
                cardRotation.AddCard(cardRect);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject card = eventData.pointerEnter;
        if (card != null && card.CompareTag("Card"))
        {
            RectTransform cardRect = card.GetComponent<RectTransform>();
            if (cardRect != null)
            {
                cardRotation.RemoveCard(cardRect);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
