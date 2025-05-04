using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dialogue : MonoBehaviour
{
    [Header("��ť��")]
    public Button b1;
    public Button b2;

    [Header("��ʾ����")]
    public GameObject item_Zhubao;
    public GameObject item_Yuanbao;

    [Header("��ʾ����")]
    [Tooltip("ͼƬ��ʾʱ�����룩")]
    public float displayDuration = 1f;

    private Coroutine activeZhubaoCoroutine;
    private Coroutine activeYuanbaoCoroutine;

    private void Start()
    {
        // ȷ��������Ʒ��ʼΪ����״̬
        if (item_Zhubao != null) item_Zhubao.SetActive(false);
        if (item_Yuanbao != null) item_Yuanbao.SetActive(false);
    }

    public void ShowItem_Zhubao()
    {
        // ��������������е�Э����ֹͣ
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

    // �Զ�����Э��
    private IEnumerator HideAfterDelay(GameObject target)
    {
        yield return new WaitForSeconds(displayDuration);

        // ��ȫ��֤
        if (target != null)
        {
            target.SetActive(false);
            // �������ٶ����ʹ���������У�
            // Destroy(target); 
        }
    }
}
