using System;
using UnityEngine;

/// <summary>
/// 对话数据类，包含一段完整对话的所有信息
/// </summary>
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    /// <summary>
    /// 对话ID，用于在游戏中唯一标识一段对话
    /// </summary>
    public string dialogueID;
    
    /// <summary>
    /// 对话的所有句子
    /// </summary>
    public DialogueSentence[] sentences;
}

/// <summary>
/// 对话句子类，表示对话中的一句话
/// </summary>
[Serializable]
public class DialogueSentence
{
    /// <summary>
    /// 说话者的名字
    /// </summary>
    public string speakerName;
    
    /// <summary>
    /// 说话者的头像
    /// </summary>
    public Sprite speakerSprite;
    
    /// <summary>
    /// 对话文本
    /// </summary>
    [TextArea(3, 10)]
    public string text;
    
    /// <summary>
    /// 对话选项，如果为空则没有选项
    /// </summary>
    public DialogueChoice[] choices;
}

/// <summary>
/// 对话选项类，表示玩家可以选择的对话选项
/// </summary>
[Serializable]
public class DialogueChoice
{
    /// <summary>
    /// 选项文本
    /// </summary>
    [TextArea(1, 3)]
    public string text;
    
    /// <summary>
    /// 选择此选项后触发的事件
    /// </summary>
    public DialogueEvent onSelected;
    
    /// <summary>
    /// 选择此选项后跳转到的下一段对话，如果为空则继续当前对话
    /// </summary>
    public DialogueData nextDialogue;
}

/// <summary>
/// 对话事件，用于在对话过程中触发游戏事件
/// </summary>
[Serializable]
public class DialogueEvent : UnityEngine.Events.UnityEvent
{
    // 使用Unity的事件系统，可以在Inspector中设置回调
}