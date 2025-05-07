using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue System/Dialogue Data")]
public class DialogueSO : ScriptableObject
{
    // ����Ի���Ŀ�Ľṹ
    [System.Serializable]
    public struct DialogueEntry
    {
        public string speaker;    // ˵����
        [TextArea(3, 10)]         // �����ı���
        public List<string> contents;    // �Ի�����
        public DialogueOption[] options; // �Ի�ѡ���ѡ��֧��
    }

    [System.Serializable]
    public struct DialogueOption
    {
        public string optionText; // ѡ���ı�
        public int nextDialogueIndex; // ��ת����һ���Ի�����
    }

    // �洢���жԻ���Ŀ
    public DialogueEntry[] entries;
}