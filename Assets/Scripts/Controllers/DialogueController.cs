using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueSO currentDialogue;
    private int currentEntryIndex = 0;    // 当前对话条目索引
    private int currentContentIndex = 0;  // 当前对话内容索引
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

    // 开始对话（首次触发）
    void StartDialogue()
    {
        isInDialogue = true;
        currentEntryIndex = 0;
        currentContentIndex = 0;
        DisplayCurrentContent();
    }

    // 继续对话（逐条显示内容）
    void ContinueDialogue()
    {
        currentContentIndex++;
        // 如果当前条目内容未全部显示完
        if (currentContentIndex < currentDialogue.entries[currentEntryIndex].contents.Count)
        {
            DisplayCurrentContent();
        }
        else
        {
            // 显示选项或结束对话
            HandleOptions();
        }
    }

    // 显示当前对话内容
    void DisplayCurrentContent()
    {
        var entry = currentDialogue.entries[currentEntryIndex];
        string speaker = entry.speaker;
        string content = entry.contents[currentContentIndex];

        // 调用 UI 更新（需对接你的 UI 系统，例如 DialogueUIPro）
        //DialogueUIPro.Instance.UpdateDialogueUI(speaker, content);
    }

    // 处理选项分支
    void HandleOptions()
    {
        var entry = currentDialogue.entries[currentEntryIndex];
        if (entry.options.Length > 0)
        {
            // 创建选项按钮（参考网页1的 OptionUI 实现）
//DialogueUIPro.Instance.CreateOptions(entry.options);
        }
        else
        {
            // 无选项则结束当前条目，推进到下一个条目
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

    // 结束对话
    void EndDialogue()
    {
        isInDialogue = false;
        //DialogueUIPro.Instance.HidePanel();
    }

    // 判断是否可交互（根据碰撞检测等逻辑，参考网页2的 canTalk）
    bool CanInteract()
    {
        // 实现你的交互检测逻辑（如与NPC的距离）
        return true;
    }

    // 选项选择后的回调（由 OptionUI 触发）
    public void OnOptionSelected(int nextIndex)
    {
        currentEntryIndex = nextIndex;
        currentContentIndex = 0;
        DisplayCurrentContent();
    }
}
