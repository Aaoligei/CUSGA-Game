using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue System/Dialogue Data")]
public class DialogueSO : ScriptableObject
{
    // 说话者类型
    public enum SpeakerType
    {
        Narrator,  // 旁白
        NPC,       // NPC
        Player     // 主角
    }

    // 单条对话条目的结构
    [System.Serializable]
    public struct DialogueEntry
    {
        public string speaker;           // 说话者名称
        public SpeakerType speakerType;  // 说话者类型
        [TextArea(3, 10)]                // 多行文本
        public string content;           // 对话内容
        public bool hasOptions;          // 是否有选项
        public DialogueOption topOption;    // 上方选项
        public DialogueOption bottomOption; // 下方选项
        public int nextDialogueIndex;    // 无选项时下一条对话索引
    }

    [System.Serializable]
    public struct DialogueOption
    {
        public string optionText;       // 选项文本
        public int nextDialogueIndex;   // 跳转到下一个对话索引
    }

    // 对话名称（用于标识）
    public string dialogueName;
    
    // 存储所有对话条目
    public DialogueEntry[] entries;
}