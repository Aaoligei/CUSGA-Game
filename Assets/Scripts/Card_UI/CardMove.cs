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

        // 只有当父物体还是Canvas时，才还原
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent, true);
            if (handPanel != null)
            {
                handPanel.ArrangeCards();
            }
        }
    }
}
