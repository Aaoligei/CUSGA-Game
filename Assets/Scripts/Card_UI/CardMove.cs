using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMove : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Transform originalParent;
    private Vector3 originalRotation;
    public CardRotation handPanel; // 拖动前所在的手牌管理器

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        // 找到最近的Canvas
        canvas = GetComponentInParent<Canvas>();
        // 可以在Inspector里拖拽赋值handPanel，或者在Start里自动查找
        if (handPanel == null)
            handPanel = GetComponentInParent<CardRotation>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 选中卡牌时可以做高亮等操作
        // Debug.Log("Card Selected");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 拖拽开始时让卡牌不再阻挡射线（方便拖到目标区域）
        canvasGroup.alpha = 0.9f;
        canvasGroup.blocksRaycasts = false;

        // 记录原父物体
        originalParent = transform.parent;
        // 设置为Canvas的子物体，保证在最上层
        transform.SetParent(canvas.transform, true);

        originalRotation = rectTransform.localEulerAngles;
        rectTransform.localEulerAngles = Vector3.zero; // 角度正过来
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 跟随鼠标移动
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 检查是否被放到了其他卡槽上
        bool droppedOnTrench = transform.parent != canvas.transform;

        // 如果没有放到卡槽上，检查是否在手牌区域范围内
        if (!droppedOnTrench)
        {
            // 找到带有HandCard组件的UI对象
            HandCard handCardUI = FindObjectOfType<HandCard>();
            if (handCardUI != null && handPanel != null)
            {
                // 检查鼠标位置是否在HandCard的UI区域内
                if (RectTransformUtility.RectangleContainsScreenPoint(
                    handCardUI.GetComponent<RectTransform>(), 
                    eventData.position, 
                    eventData.pressEventCamera))
                {
                    Debug.Log("卡牌回到手牌区");
                    // 卡牌拖回了手牌区域，设置为HandPanel的子物体
                    transform.SetParent(handPanel.transform);
                    
                    // 添加到手牌区并排列
                    if (!handPanel.cards.Contains(rectTransform))
                    {
                        handPanel.AddCard(rectTransform);
                        Debug.Log("添加卡牌到手牌列表");
                    }
                    else
                    {
                        handPanel.ArrangeCards();
                        Debug.Log("重新排列手牌");
                    }
                    return; // 已处理，退出方法
                }
            }
            
            // 如果不在手牌区域内，返回原来的位置
            transform.SetParent(originalParent);
            if (handPanel != null && handPanel.cards.Contains(rectTransform))
            {
                handPanel.ArrangeCards();
            }
        }
    }
}
