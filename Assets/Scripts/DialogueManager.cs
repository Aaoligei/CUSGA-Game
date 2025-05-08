using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// 对话管理器 - 适用于2D文字对话游戏
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("对话数据")]
    public DialogueSO[] dialogueSequence;    // 对话序列，按顺序播放
    private int currentDialogueIndex = 0;    // 当前对话索引
    
    [Header("UI组件")]
    public GameObject dialoguePanel;         // 对话面板
    public Text speakerNameText;             // 说话者名称
    public Text dialogueContentText;         // 对话内容
    public Image characterImage;             // 角色立绘（NPC用）
    public GameObject optionsPanel;          // 选项面板
    public Button topOptionButton;           // 上方选项按钮
    public Button bottomOptionButton;        // 下方选项按钮
    public Text topOptionText;               // 上方选项文本
    public Text bottomOptionText;            // 下方选项文本
    
    [Header("打字机效果")]
    public float typingSpeed = 0.05f;        // 打字速度
    public AudioSource typingSoundSource;    // 打字音效
    
    // 事件
    public System.Action OnAllDialoguesComplete;   // 所有对话序列完成时触发
    
    [Header("弹窗设置")]
    public PopupManager popupManager;         // 弹窗管理器引用
    public bool showPopupsAfterDialogue = true; // 是否在对话结束后显示弹窗
    
    // 对话状态
    private DialogueController dialogueController;
    private bool autoStartNextDialogue = true;   // 自动开始下一段对话
    
    // 游戏流程数据
    private Dictionary<string, int> storyFlags = new Dictionary<string, int>();
    
    // 为外部访问提供对话控制器
    public DialogueController DialogueController => dialogueController;
    
    void Awake()
    {
        // 初始化组件
        if (dialoguePanel == null || speakerNameText == null || dialogueContentText == null)
        {
            Debug.LogError("对话UI组件未设置！请在Inspector中设置对话UI组件引用。");
            enabled = false;
            return;
        }
        
        // 创建对话控制器
        dialogueController = gameObject.AddComponent<DialogueController>();
        
        // 设置控制器引用
        dialogueController.dialoguePanel = dialoguePanel;
        dialogueController.speakerNameText = speakerNameText;
        dialogueController.dialogueContentText = dialogueContentText;
        dialogueController.characterImage = characterImage;
        dialogueController.optionsPanel = optionsPanel;
        dialogueController.topOptionButton = topOptionButton;
        dialogueController.bottomOptionButton = bottomOptionButton;
        dialogueController.topOptionText = topOptionText;
        dialogueController.bottomOptionText = bottomOptionText;
        dialogueController.typingSpeed = typingSpeed;
        dialogueController.typingSoundSource = typingSoundSource;
        
        // 初始隐藏对话界面
        dialoguePanel.SetActive(false);
        optionsPanel.SetActive(false);
        
        // 初始隐藏角色立绘
        if (characterImage != null)
        {
            characterImage.gameObject.SetActive(false);
        }
    }
    
    void Start()
    {
        // 注册对话事件
        dialogueController.OnDialogueEnd += HandleDialogueEnd;
        
        // 如果有弹窗管理器，注册弹窗完成事件
        if (popupManager != null)
        {
            popupManager.OnAllPopupsComplete += HandleAllPopupsComplete;
        }
        
        // 开始第一段对话
        StartNextDialogue();
    }
    
    /// <summary>
    /// 开始下一段对话
    /// </summary>
    public void StartNextDialogue()
    {
        if (currentDialogueIndex < dialogueSequence.Length)
        {
            // 设置当前对话
            dialogueController.currentDialogue = dialogueSequence[currentDialogueIndex];
            
            // 开始对话
            dialogueController.StartDialogue();
            
            // 增加索引
            currentDialogueIndex++;
        }
        else
        {
            // 所有对话结束，触发事件
            Debug.Log("所有对话序列已结束");
            
            // 通知所有对话已完成
            CompleteAllDialogues();
        }
    }
    
    /// <summary>
    /// 处理对话结束事件
    /// </summary>
    private void HandleDialogueEnd(string dialogueName)
    {
        // 可以根据对话名称记录游戏进度
        Debug.Log($"对话 {dialogueName} 已结束");
        
        // 判断是否还有对话
        if (currentDialogueIndex >= dialogueSequence.Length)
        {
            // 对话序列全部完成
            CompleteAllDialogues();
        }
        else if (autoStartNextDialogue)
        {
            // 自动开始下一段对话
            StartNextDialogue();
        }
    }
    
    /// <summary>
    /// 通知所有对话已完成，并根据设置显示弹窗序列
    /// </summary>
    private void CompleteAllDialogues()
    {
        // 触发对话全部完成事件
        OnAllDialoguesComplete?.Invoke();
        
        // 如果设置了在对话结束后显示弹窗且弹窗管理器存在
        if (showPopupsAfterDialogue && popupManager != null)
        {
            // 开始显示弹窗序列
            Debug.Log("开始显示弹窗");
            popupManager.ShowPopupSequence();
        }
    }
    
    /// <summary>
    /// 处理所有弹窗显示完成的事件
    /// </summary>
    private void HandleAllPopupsComplete()
    {
        Debug.Log("所有弹窗已显示完毕");
        // 这里可以进行游戏的下一步操作，比如场景切换等
    }
    
    /// <summary>
    /// 设置故事标记，用于跟踪游戏进度和分支选择
    /// </summary>
    public void SetStoryFlag(string flagName, int value)
    {
        storyFlags[flagName] = value;
    }
    
    /// <summary>
    /// 获取故事标记值
    /// </summary>
    public int GetStoryFlag(string flagName, int defaultValue = 0)
    {
        if (storyFlags.TryGetValue(flagName, out int value))
        {
            return value;
        }
        return defaultValue;
    }
    
    /// <summary>
    /// 根据故事标记跳转到特定对话
    /// </summary>
    public void JumpToDialogue(int dialogueIndex)
    {
        if (dialogueIndex >= 0 && dialogueIndex < dialogueSequence.Length)
        {
            currentDialogueIndex = dialogueIndex;
            StartNextDialogue();
        }
    }
    
    /// <summary>
    /// 在完成当前对话后跳过序列中剩余的对话
    /// </summary>
    public void SkipRemainingDialogues()
    {
        // 将当前索引设置为序列末尾
        currentDialogueIndex = dialogueSequence.Length;
        
        // 结束当前对话（若有）
        if (dialogueController.IsInDialogue)
        {
            dialogueController.EndCurrentDialogue();
        }
        else
        {
            // 如果当前没有进行中的对话，直接完成
            CompleteAllDialogues();
        }
    }
}

/// <summary>
/// 自定义对话控制器，添加事件回调
/// </summary>
public class DialogueController : MonoBehaviour
{
    [Header("对话数据")]
    public DialogueSO currentDialogue;
    
    [Header("UI组件")]
    public GameObject dialoguePanel;         // 对话面板
    public Text speakerNameText;             // 说话者名称
    public Text dialogueContentText;         // 对话内容
    public Image characterImage;             // 角色立绘（NPC用）
    public GameObject optionsPanel;          // 选项面板
    public Button topOptionButton;           // 上方选项按钮
    public Button bottomOptionButton;        // 下方选项按钮
    public Text topOptionText;               // 上方选项文本
    public Text bottomOptionText;            // 下方选项文本
    
    [Header("打字机效果")]
    public float typingSpeed = 0.05f;        // 打字速度
    public AudioSource typingSoundSource;    // 打字音效
    
    // 对话状态
    private int currentEntryIndex = 0;      // 当前对话条目索引
    private bool isInDialogue = false;      // 是否在对话中
    private bool isTyping = false;          // 是否正在打字
    private bool waitingForInput = false;   // 是否等待输入

    public bool isSpecialAction = false; // 是否是特殊事件

    // 公开属性
    public bool IsInDialogue => isInDialogue;
    
    // 打字协程
    private Coroutine typingCoroutine;
    
    // 事件
    public System.Action<string> OnDialogueStart;
    public System.Action<string> OnDialogueEnd;
    public System.Action<int> OnOptionSelected;
    
    // 对话回调系统 - 用于在播放特定对话时调用其他脚本的函数
    private Dictionary<int, System.Action> dialogueCallbacks = new Dictionary<int, System.Action>();
    
    /// <summary>
    /// 注册对话回调 - 在特定索引的对话播放时调用指定函数
    /// </summary>
    /// <param name="entryIndex">对话条目索引</param>
    /// <param name="callback">要调用的回调函数</param>
    public void RegisterDialogueCallback(int entryIndex, System.Action callback)
    {
        dialogueCallbacks[entryIndex] = callback;
    }
    
    /// <summary>
    /// 移除对话回调
    /// </summary>
    /// <param name="entryIndex">对话条目索引</param>
    public void RemoveDialogueCallback(int entryIndex)
    {
        if (dialogueCallbacks.ContainsKey(entryIndex))
        {
            dialogueCallbacks.Remove(entryIndex);
        }
    }
    
    /// <summary>
    /// 清除所有对话回调
    /// </summary>
    public void ClearAllDialogueCallbacks()
    {
        dialogueCallbacks.Clear();
    }

    void Update()
    {
        // 检测鼠标点击或键盘按键继续对话
        if (!isSpecialAction&& isInDialogue && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            // 如果正在打字，立即完成当前文本
            if (isTyping)
            {
                CompleteTyping();
            }
            // 否则继续下一句对话
            else if (waitingForInput && !optionsPanel.activeSelf)
            {
                ContinueDialogue();
            }
        }
    }

    // 开始对话
    public void StartDialogue()
    {
        if (currentDialogue == null)
        {
            Debug.LogError("未设置对话数据！");
            return;
        }
        
        isInDialogue = true;
        currentEntryIndex = 0;
        
        // 显示对话面板
        dialoguePanel.SetActive(true);
        
        // 触发对话开始事件
        OnDialogueStart?.Invoke(currentDialogue.dialogueName);
        
        // 显示当前对话内容
        DisplayCurrentContent();
        
        // 绑定选项按钮事件
        if (topOptionButton != null)
        {
            topOptionButton.onClick.RemoveAllListeners();
            topOptionButton.onClick.AddListener(() => SelectOption(true));
        }
        
        if (bottomOptionButton != null)
        {
            bottomOptionButton.onClick.RemoveAllListeners();
            bottomOptionButton.onClick.AddListener(() => SelectOption(false));
        }
    }

    // 继续对话
    public void ContinueDialogue()
    {
        if (!isInDialogue) return;
        
        waitingForInput = false;
        
        var currentEntry = currentDialogue.entries[currentEntryIndex];
        
        // 如果当前对话有选项，不应该自动继续
        if (currentEntry.hasOptions)
        {
            // 显示选项
            ShowOptions(currentEntry);
            return;
        }
        
        // 无选项，进入下一条对话
        int nextIndex = currentEntry.nextDialogueIndex;
        
        // 检查下一条对话是否有效
        if (nextIndex >= 0 && nextIndex < currentDialogue.entries.Length)
        {
            currentEntryIndex = nextIndex;
            DisplayCurrentContent();
        }
        else
        {
            // 结束对话
            EndDialogue();
        }
    }
    
    /// <summary>
    /// 结束当前对话，用于外部调用
    /// </summary>
    public void EndCurrentDialogue()
    {
        EndDialogue();
    }

    // 显示当前对话内容
    private void DisplayCurrentContent()
    {
        if (currentDialogue == null || currentEntryIndex >= currentDialogue.entries.Length)
        {
            EndDialogue();
            return;
        }
        
        var entry = currentDialogue.entries[currentEntryIndex];
        
        // 设置说话者名称
        speakerNameText.text = entry.speaker;
        
        // 根据说话者类型显示/隐藏立绘
        UpdateCharacterImage(entry.speakerType, entry.speaker);
        
        // 隐藏选项面板
        optionsPanel.SetActive(false);
        
        // 开始打字效果
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        typingCoroutine = StartCoroutine(TypeText(entry.content));
        
        // 检查并执行当前对话索引的回调函数
        if (dialogueCallbacks.TryGetValue(currentEntryIndex, out System.Action callback))
        {
            callback?.Invoke();
        }
    }
    
    // 根据说话者类型更新角色立绘
    private void UpdateCharacterImage(DialogueSO.SpeakerType speakerType, string speakerName)
    {
        if (characterImage == null) return;
        
        // 只有NPC类型才显示立绘，并且在左侧
        if (speakerType == DialogueSO.SpeakerType.NPC)
        {
            characterImage.gameObject.SetActive(true);
            
            // 尝试加载NPC的立绘
            Sprite npcSprite = Resources.Load<Sprite>($"Characters/{speakerName}");
            if (npcSprite != null)
            {
                characterImage.sprite = npcSprite;
            }
            else
            {
                Debug.LogWarning($"未找到角色立绘: {speakerName}");
            }
        }
        else
        {
            // 非NPC不显示立绘
            characterImage.gameObject.SetActive(false);
        }
    }

    // 打字机效果
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        waitingForInput = false;
        dialogueContentText.text = "";
        
        foreach (char c in text)
        {
            dialogueContentText.text += c;
            
            // 播放打字音效
            if (typingSoundSource != null && !char.IsWhiteSpace(c))
            {
                typingSoundSource.Play();
            }
            
            yield return new WaitForSeconds(typingSpeed);
        }
        
        isTyping = false;
        waitingForInput = true;
    }
    
    // 立即完成当前文本显示
    private void CompleteTyping()
    {
        if (!isTyping) return;
        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        
        var entry = currentDialogue.entries[currentEntryIndex];
        dialogueContentText.text = entry.content;
        
        isTyping = false;
        waitingForInput = true;
        
        // 如果有选项，立即显示
        if (entry.hasOptions)
        {
            ShowOptions(entry);
        }
    }

    // 显示对话选项
    private void ShowOptions(DialogueSO.DialogueEntry entry)
    {
        if (!entry.hasOptions) return;
        
        // 设置上方选项
        if (topOptionText != null)
        {
            topOptionText.text = entry.topOption.optionText;
        }
        
        // 设置下方选项
        if (bottomOptionText != null)
        {
            bottomOptionText.text = entry.bottomOption.optionText;
        }
        
        // 显示选项面板
        optionsPanel.SetActive(true);
    }

    // 选择选项
    public void SelectOption(bool isTopOption)
    {
        var entry = currentDialogue.entries[currentEntryIndex];
        int nextIndex;
        
        // 根据选择的选项确定下一个对话索引
        if (isTopOption)
        {
            nextIndex = entry.topOption.nextDialogueIndex;
            OnOptionSelected?.Invoke(0); // 0表示上方选项
        }
        else
        {
            nextIndex = entry.bottomOption.nextDialogueIndex;
            OnOptionSelected?.Invoke(1); // 1表示下方选项
        }
        
        // 隐藏选项面板
        optionsPanel.SetActive(false);
        
        // 跳转到下一个对话
        if (nextIndex >= 0 && nextIndex < currentDialogue.entries.Length)
        {
            currentEntryIndex = nextIndex;
            DisplayCurrentContent();
        }
        else
        {
            // 结束对话
            EndDialogue();
        }
    }

    // 选项被选择 - 仅供内部使用，避免与事件冲突
    public void HandleOptionSelected(int nextIndex)
    {
        // 隐藏选项面板
        optionsPanel.SetActive(false);
        
        // 跳转到指定对话索引
        if (nextIndex >= 0 && nextIndex < currentDialogue.entries.Length)
        {
            currentEntryIndex = nextIndex;
            DisplayCurrentContent();
        }
        else
        {
            // 结束对话
            EndDialogue();
        }
    }

    // 结束对话
    private void EndDialogue()
    {
        isInDialogue = false;
        
        // 触发对话结束事件
        OnDialogueEnd?.Invoke(currentDialogue.dialogueName);
        
        // 隐藏对话面板和选项面板
        dialoguePanel.SetActive(false);
        optionsPanel.SetActive(false);
        
        // 隐藏角色立绘
        if (characterImage != null)
        {
            characterImage.gameObject.SetActive(false);
        }
    }
} 