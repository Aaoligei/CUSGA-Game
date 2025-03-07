using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 对话管理器，负责控制游戏中的对话系统
/// </summary>
public class DialogueManager : MonoBehaviour
{
    // 单例模式
    public static DialogueManager Instance { get; private set; }
    
    [Header("UI组件")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image speakerImage;
    [SerializeField] private float typingSpeed = 0.05f;
    
    [Header("选项设置")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    
    // 当前对话数据
    private DialogueData currentDialogue;
    private int currentSentenceIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;
    
    // 对话结束回调
    private System.Action onDialogueEnd;
    
    private void Awake()
    {
        // 单例模式初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // 初始状态为隐藏对话面板
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
    }
    
    /// <summary>
    /// 开始一段对话
    /// </summary>
    /// <param name="dialogue">对话数据</param>
    /// <param name="onEnd">对话结束后的回调</param>
    public void StartDialogue(DialogueData dialogue, System.Action onEnd = null)
    {
        currentDialogue = dialogue;
        currentSentenceIndex = 0;
        onDialogueEnd = onEnd;
        
        // 显示对话面板
        dialoguePanel.SetActive(true);
        
        // 显示第一句对话
        DisplayNextSentence();
    }
    
    /// <summary>
    /// 显示下一句对话
    /// </summary>
    public void DisplayNextSentence()
    {
        // 如果正在打字，则直接显示完整文本
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue.sentences[currentSentenceIndex].text;
            isTyping = false;
            return;
        }
        
        // 检查是否已经到达对话结尾
        if (currentSentenceIndex >= currentDialogue.sentences.Length)
        {
            EndDialogue();
            return;
        }
        
        // 获取当前句子
        DialogueSentence sentence = currentDialogue.sentences[currentSentenceIndex];
        
        // 更新UI
        speakerNameText.text = sentence.speakerName;
        if (sentence.speakerSprite != null)
        {
            speakerImage.sprite = sentence.speakerSprite;
            speakerImage.gameObject.SetActive(true);
        }
        else
        {
            speakerImage.gameObject.SetActive(false);
        }
        
        // 开始打字效果
        typingCoroutine = StartCoroutine(TypeSentence(sentence.text));
        
        // 如果当前句子有选项，显示选项
        if (sentence.choices != null && sentence.choices.Length > 0)
        {
            StartCoroutine(ShowChoicesAfterTyping(sentence));
        }
        
        // 移动到下一句
        currentSentenceIndex++;
    }
    
    /// <summary>
    /// 打字效果协程
    /// </summary>
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        isTyping = false;
    }
    
    /// <summary>
    /// 在打字效果结束后显示选项
    /// </summary>
    private IEnumerator ShowChoicesAfterTyping(DialogueSentence sentence)
    {
        // 等待打字效果结束
        while (isTyping)
        {
            yield return null;
        }
        
        // 显示选项
        choicePanel.SetActive(true);
        
        // 清除旧选项
        foreach (Transform child in choicePanel.transform)
        {
            Destroy(child.gameObject);
        }
        
        // 创建新选项按钮
        foreach (DialogueChoice choice in sentence.choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicePanel.transform);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            
            buttonText.text = choice.text;
            
            // 设置按钮点击事件
            button.onClick.AddListener(() => {
                // 隐藏选项面板
                choicePanel.SetActive(false);
                
                // 执行选项回调
                if (choice.onSelected != null)
                {
                    choice.onSelected.Invoke();
                }
                
                // 如果有下一段对话，则开始新对话
                if (choice.nextDialogue != null)
                {
                    StartDialogue(choice.nextDialogue);
                }
                else
                {
                    // 否则继续当前对话
                    DisplayNextSentence();
                }
            });
        }
    }
    
    /// <summary>
    /// 结束对话
    /// </summary>
    private void EndDialogue()
    {
        // 隐藏对话面板
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        
        // 调用结束回调
        if (onDialogueEnd != null)
        {
            onDialogueEnd.Invoke();
        }
    }
}