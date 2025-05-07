using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueSO currentDialogue;
    private int currentEntryIndex = 0;    // ��ǰ�Ի���Ŀ����
    private int currentContentIndex = 0;  // ��ǰ�Ի���������
    private bool isInDialogue = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanInteract())
        {
            if (!isInDialogue)
                StartDialogue();
            else
                ContinueDialogue();
        }
    }

    // ��ʼ�Ի����״δ�����
    void StartDialogue()
    {
        isInDialogue = true;
        currentEntryIndex = 0;
        currentContentIndex = 0;
        DisplayCurrentContent();
    }

    // �����Ի���������ʾ���ݣ�
    void ContinueDialogue()
    {
        currentContentIndex++;
        // �����ǰ��Ŀ����δȫ����ʾ��
        if (currentContentIndex < currentDialogue.entries[currentEntryIndex].contents.Count)
        {
            DisplayCurrentContent();
        }
        else
        {
            // ��ʾѡ�������Ի�
            HandleOptions();
        }
    }

    // ��ʾ��ǰ�Ի�����
    void DisplayCurrentContent()
    {
        var entry = currentDialogue.entries[currentEntryIndex];
        string speaker = entry.speaker;
        string content = entry.contents[currentContentIndex];

        // ���� UI ���£���Խ���� UI ϵͳ������ DialogueUIPro��
        //DialogueUIPro.Instance.UpdateDialogueUI(speaker, content);
    }

    // ����ѡ���֧
    void HandleOptions()
    {
        var entry = currentDialogue.entries[currentEntryIndex];
        if (entry.options.Length > 0)
        {
            // ����ѡ�ť���ο���ҳ1�� OptionUI ʵ�֣�
//DialogueUIPro.Instance.CreateOptions(entry.options);
        }
        else
        {
            // ��ѡ���������ǰ��Ŀ���ƽ�����һ����Ŀ
            currentEntryIndex++;
            if (currentEntryIndex < currentDialogue.entries.Length)
            {
                currentContentIndex = 0;
                DisplayCurrentContent();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    // �����Ի�
    void EndDialogue()
    {
        isInDialogue = false;
        //DialogueUIPro.Instance.HidePanel();
    }

    // �ж��Ƿ�ɽ�����������ײ�����߼����ο���ҳ2�� canTalk��
    bool CanInteract()
    {
        // ʵ����Ľ�������߼�������NPC�ľ��룩
        return true;
    }

    // ѡ��ѡ���Ļص����� OptionUI ������
    public void OnOptionSelected(int nextIndex)
    {
        currentEntryIndex = nextIndex;
        currentContentIndex = 0;
        DisplayCurrentContent();
    }
}
