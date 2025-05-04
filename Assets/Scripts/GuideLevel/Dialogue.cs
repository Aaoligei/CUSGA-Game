using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dialogue : MonoBehaviour
{
    [Header("按钮绑定")]
    public Button b1;
    public Button b2;

    [Header("显示对象")]
    public GameObject item_Zhubao;
    public GameObject item_Yuanbao;

    [Header("显示设置")]
    [Tooltip("图片显示时长（秒）")]
    public float displayDuration = 1f;

    private Coroutine activeZhubaoCoroutine;
    private Coroutine activeYuanbaoCoroutine;

    private void Start()
    {
        // 确保所有物品初始为隐藏状态
        if (item_Zhubao != null) item_Zhubao.SetActive(false);
        if (item_Yuanbao != null) item_Yuanbao.SetActive(false);
    }

    public void ShowItem_Zhubao()
    {
        // 如果已有正在运行的协程则停止
        if (activeZhubaoCoroutine != null)
        {
            StopCoroutine(activeZhubaoCoroutine);
        }

        item_Zhubao.SetActive(true);
        activeZhubaoCoroutine = StartCoroutine(HideAfterDelay(item_Zhubao));
    }

    public void ShowItem_Yuanbao()
    {
        if (activeYuanbaoCoroutine != null)
        {
            StopCoroutine(activeYuanbaoCoroutine);
        }

        item_Yuanbao.SetActive(true);
        activeYuanbaoCoroutine = StartCoroutine(HideAfterDelay(item_Yuanbao));
    }

    // 自动隐藏协程
    private IEnumerator HideAfterDelay(GameObject target)
    {
        yield return new WaitForSeconds(displayDuration);

        // 安全验证
        if (target != null)
        {
            target.SetActive(false);
            // 如需销毁对象可使用下面这行：
            // Destroy(target); 
        }
    }
}
