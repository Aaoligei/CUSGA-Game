using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 弹窗管理器 - 负责在对话结束后显示一系列弹窗
/// </summary>
public class PopupManager : MonoBehaviour
{
    [Header("弹窗设置")]
    public GameObject[] popupPrefabs;        // 要显示的弹窗预制体数组
    public Transform popupContainer;          // 弹窗的父容器
    public float fadeInDuration = 0.5f;       // 弹窗淡入时间
    public float fadeOutDuration = 0.5f;      // 弹窗淡出时间
    
    [Header("当前状态")]
    [SerializeField] private int currentPopupIndex = 0;  // 当前弹窗索引
    private GameObject currentPopup;          // 当前显示的弹窗实例
    private bool isPopupActive = false;       // 是否有弹窗正在显示
    
    // 用于通知所有弹窗都已经显示完毕
    public System.Action OnAllPopupsComplete;
    
    private void Awake()
    {
        // 如果未设置弹窗容器，则使用当前对象的变换
        if (popupContainer == null)
        {
            popupContainer = transform;
        }
        foreach (var item in popupPrefabs)
        {
            item.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 开始显示弹窗序列
    /// </summary>
    public void ShowPopupSequence()
    {
        // 重置索引并显示第一个弹窗
        currentPopupIndex = 0;
        isPopupActive = true;
        ShowCurrentPopup();
    }
    
    /// <summary>
    /// 显示当前索引的弹窗
    /// </summary>
    private void ShowCurrentPopup()
    {
        // 检查是否还有弹窗需要显示
        if (currentPopupIndex < popupPrefabs.Length && popupPrefabs[currentPopupIndex] != null)
        {
            // 创建弹窗实例
            currentPopup = Instantiate(popupPrefabs[currentPopupIndex], popupContainer);
            currentPopup.SetActive(true);
            
            // 设置初始透明度为0
            CanvasGroup canvasGroup = currentPopup.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = currentPopup.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
            
            // 添加点击监听组件
            PopupClickHandler clickHandler = currentPopup.GetComponent<PopupClickHandler>();
            if (clickHandler == null)
            {
                clickHandler = currentPopup.AddComponent<PopupClickHandler>();
            }
            clickHandler.Initialize(this);
            
            // 淡入效果
            StartCoroutine(FadeIn(canvasGroup));
            
            // 增加索引，为下一个弹窗做准备
            currentPopupIndex++;
        }
        else
        {
            // 所有弹窗都已显示完毕
            isPopupActive = false;
            OnAllPopupsComplete?.Invoke();
        }
    }
    
    /// <summary>
    /// 显示下一个弹窗（关闭当前弹窗并显示下一个）
    /// </summary>
    public void ShowNextPopup()
    {
        if (!isPopupActive) return;
        
        // 销毁当前弹窗
        if (currentPopup != null)
        {
            StartCoroutine(FadeOutAndDestroy(currentPopup.GetComponent<CanvasGroup>(), () => {
                ShowCurrentPopup();
            }));
        }
        else
        {
            ShowCurrentPopup();
        }
    }
    
    /// <summary>
    /// 淡入效果
    /// </summary>
    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
    
    /// <summary>
    /// 淡出并销毁弹窗
    /// </summary>
    private IEnumerator FadeOutAndDestroy(CanvasGroup canvasGroup, System.Action onComplete)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(canvasGroup.gameObject);
        currentPopup = null;
        
        onComplete?.Invoke();
    }
    
    /// <summary>
    /// 跳过所有剩余的弹窗
    /// </summary>
    public void SkipAllPopups()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
        }
        
        isPopupActive = false;
        OnAllPopupsComplete?.Invoke();
    }
}

/// <summary>
/// 弹窗点击处理器 - 处理弹窗上的点击事件
/// </summary>
public class PopupClickHandler : MonoBehaviour
{
    private PopupManager popupManager;
    private bool isInitialized = false;
    
    public void Initialize(PopupManager manager)
    {
        popupManager = manager;
        isInitialized = true;
    }
    
    private void Update()
    {
        if (!isInitialized) return;
        
        // 检测点击或触摸输入
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            // 点击任意位置或按空格键进入下一个弹窗
            popupManager.ShowNextPopup();
        }
    }
} 