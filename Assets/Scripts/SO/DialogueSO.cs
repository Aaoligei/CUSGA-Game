using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue System/Dialogue Data")]
public class DialogueSO : ScriptableObject
{
    // 定义对话条目的结构
    [System.Serializable]
    public struct DialogueEntry
    {
        public string speaker;    // 说话人
        [TextArea(3, 10)]         // 多行文本框
        public List<string> contents;    // 对话内容
        public DialogueOption[] options; // 对话选项（可选分支）
    }

    [System.Serializable]
    public struct DialogueOption
    {
        public string optionText; // 选项文本
        public int nextDialogueIndex; // 跳转的下一条对话索引
    }

    // 存储所有对话条目
    public DialogueEntry[] entries;
}