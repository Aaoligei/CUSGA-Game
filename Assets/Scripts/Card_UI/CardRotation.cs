using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotation : MonoBehaviour
{
    public List<RectTransform> cards; // 卡牌RectTransform列表
    public float radius = 240f;   // 扇形半径（UI单位通常是像素，建议调大）
    public float angleRange = 120f; // 扇形总角度
    public float moveDuration = 0.3f; // 移动时间
    public float idealAngleStep = 200f; // 理想的卡牌间隔角度
    public float fixedAngleStep = 5f; // 新增：固定间隔角度

    private int handCount { get { return cards.Count; } }

    // Start is called before the first frame update
    void Start()
    {
        ArrangeCards();
    }

    // 排列卡牌
    public void ArrangeCards()
    {
        StartCoroutine(MoveCardsCoroutine());
    }

    IEnumerator MoveCardsCoroutine()
    {
        // 计算理想总角度
        float idealTotalAngle = fixedAngleStep * (handCount - 1);
        // 实际总角度不能超过angleRange
        float actualAngleRange = Mathf.Min(angleRange, idealTotalAngle);
        float startAngle = -actualAngleRange / 2f;
        float angleStep = handCount > 1 ? actualAngleRange / (handCount - 1) : 0f;

        for (int i = 0; i < handCount; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 targetPos = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * radius;
            Vector3 targetRot = new Vector3(0, 0, -angle);

            StartCoroutine(MoveCard(cards[i], targetPos, targetRot));
        }
        yield return null;
    }

    IEnumerator MoveCard(RectTransform card, Vector2 targetPos, Vector3 targetRot)
    {
        Vector2 startPos = card.anchoredPosition;
        Quaternion startQuat = card.localRotation;
        Quaternion targetQuat = Quaternion.Euler(targetRot);
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            card.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            card.localRotation = Quaternion.Lerp(startQuat, targetQuat, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        card.anchoredPosition = targetPos;
        card.localRotation = targetQuat;
    }

    // 新增：添加卡牌
    public void AddCard(RectTransform card)
    {
        cards.Add(card);
        ArrangeCards();
    }

    // 新增：移除卡牌
    public void RemoveCard(RectTransform card)
    {
        cards.Remove(card);
        ArrangeCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
