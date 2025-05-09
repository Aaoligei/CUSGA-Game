using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTrench : MonoBehaviour, IDropHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 检查拖拽过来的对象是否有Card标签
        if (eventData.pointerDrag != null && eventData.pointerDrag.CompareTag("Card"))
        {
            // 将卡牌的父对象设置为当前槽位
            eventData.pointerDrag.transform.SetParent(transform);

            // 对齐到槽位中心
            RectTransform cardRect = eventData.pointerDrag.GetComponent<RectTransform>();
            cardRect.anchoredPosition = Vector2.zero;

            // 移除原手牌区的引用并重新排布
            CardMove move = eventData.pointerDrag.GetComponent<CardMove>();
            if (move != null && move.handPanel != null)
            {
                move.handPanel.RemoveCard(cardRect);
            }
        }
    }
}
