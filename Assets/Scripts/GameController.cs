using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏控制器 - 整合管理对话系统和弹窗系统
/// </summary>
public class GameController : MonoBehaviour
{
    [Header("系统引用")]
    public DialogueManager dialogueManager;   // 对话管理器
    public PopupManager popupManager;         // 弹窗管理器
    
    [Header("游戏流程")]
    public bool startDialogueOnAwake = true;  // 是否在启动时自动开始对话
    public string nextSceneName;              // 所有内容结束后加载的下一个场景
    
    [Header("UI交互")]
    public KeyCode skipKey = KeyCode.Escape;  // 跳过键
    public KeyCode restartKey = KeyCode.R;    // 重新开始键
    
    private void Awake()
    {
        // 查找管理器（如果未设置）
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }
        
        if (popupManager == null)
        {
            popupManager = FindObjectOfType<PopupManager>();
        }
        
        // 确保至少有对话管理器
        if (dialogueManager == null)
        {
            Debug.LogError("未找到DialogueManager！请确保场景中存在DialogueManager。");
            enabled = false;
            return;
        }
    }
    
    private void Start()
    {
        // 设置事件监听
        SetupEventListeners();
        
        // 如果设置了自动开始对话
        if (startDialogueOnAwake)
        {
            // 对话系统已在其Start方法中自动开始第一个对话
            // 无需再次调用StartDialogue
        }
    }
    
    private void Update()
    {
        // 检测跳过键
        if (Input.GetKeyDown(skipKey))
        {
            SkipAllContent();
        }
        
        // 检测重新开始键
        if (Input.GetKeyDown(restartKey))
        {
            RestartScene();
        }
    }
    
    /// <summary>
    /// 设置事件监听
    /// </summary>
    private void SetupEventListeners()
    {
        // 监听对话完成事件
        if (dialogueManager != null)
        {
            dialogueManager.OnAllDialoguesComplete += HandleAllDialoguesComplete;
        }
        
        // 监听弹窗完成事件
        if (popupManager != null)
        {
            popupManager.OnAllPopupsComplete += HandleAllPopupsComplete;
        }
    }
    
    /// <summary>
    /// 处理所有对话完成事件
    /// </summary>
    private void HandleAllDialoguesComplete()
    {
        Debug.Log("游戏控制器：所有对话已完成");
        
        // 弹窗显示由DialogueManager直接控制，此处无需额外处理
    }
    
    /// <summary>
    /// 处理所有弹窗完成事件
    /// </summary>
    private void HandleAllPopupsComplete()
    {
        Debug.Log("游戏控制器：所有弹窗已完成");
        
        // 所有内容都已完成，可以执行下一步操作
        CompleteGameSequence();
    }
    
    /// <summary>
    /// 完成整个游戏序列，执行后续操作
    /// </summary>
    private void CompleteGameSequence()
    {
        // 如果设置了下一个场景，则加载
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"加载下一个场景: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("所有内容已完成，但未设置下一个场景");
        }
    }
    
    /// <summary>
    /// 跳过当前所有内容（对话和弹窗）
    /// </summary>
    public void SkipAllContent()
    {
        // 跳过对话
        if (dialogueManager != null)
        {
            dialogueManager.SkipRemainingDialogues();
        }
        
        // 跳过弹窗
        if (popupManager != null)
        {
            popupManager.SkipAllPopups();
        }
    }
    
    /// <summary>
    /// 重新开始当前场景
    /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void OnDestroy()
    {
        // 移除事件监听
        if (dialogueManager != null)
        {
            dialogueManager.OnAllDialoguesComplete -= HandleAllDialoguesComplete;
        }
        
        if (popupManager != null)
        {
            popupManager.OnAllPopupsComplete -= HandleAllPopupsComplete;
        }
    }
} 