# 2D文字对话游戏系统使用指南

## 一、系统概述

本系统是一个专为2D文字对话游戏设计的对话系统，具有以下特点：
1. 自动开始对话序列，无需NPC交互触发
2. 支持连续多段对话自动播放
3. 打字机效果显示文本
4. 分支对话选项支持
5. 简单的游戏进度管理
6. 点击屏幕或按空格继续对话

## 二、快速开始

### 1. 创建UI界面

1. 创建一个Canvas作为UI的父对象
2. 添加如下组件到Canvas下：
   - **对话面板 (DialoguePanel)**：包含背景图、说话者名称、对话内容
   - **选项面板 (OptionsPanel)**：包含选项按钮的容器
   - **角色立绘**：左侧和右侧的角色图像

### 2. 设置DialogueManager

1. 在场景中创建一个空对象，命名为"DialogueManager"
2. 添加DialogueManager脚本到该对象
3. 在Inspector中设置以下引用：
   - **对话序列**：拖拽创建好的DialogueSO对话数据资源到dialogueSequence数组
   - **UI组件**：拖拽对应的UI组件到相应字段

### 3. 创建对话数据

1. 在Project面板中右键选择：`Create -> Dialogue System -> Dialogue Data`
2. 设置对话条目：
   - **speaker**：说话者名称
   - **contents**：对话内容列表（每条对话内容）
   - **options**：对话选项数组（用于分支对话）

## 三、详细设置指南

### 对话数据结构

每个DialogueSO包含多个DialogueEntry（对话条目），每个条目包含：
- **speaker**：说话者名称
- **contents**：对话内容列表（一个说话者可以连续说多段话）
- **options**：分支选项（可为空）

每个选项包含：
- **optionText**：选项文本
- **nextDialogueIndex**：选择后跳转到的对话条目索引

### 对话序列设置

DialogueManager中的dialogueSequence数组用于设置对话的顺序：
- 数组中的每个元素为一个DialogueSO资源
- 游戏开始后会按顺序自动播放这些对话
- 每个对话结束后自动进入下一个对话

### UI界面设置

确保设置以下UI组件引用：
1. **dialoguePanel**：整个对话面板
2. **speakerNameText**：显示说话者名称的Text组件
3. **dialogueContentText**：显示对话内容的Text组件
4. **leftCharacterImage/rightCharacterImage**：左右角色立绘Image组件
5. **optionsPanel**：选项面板GameObject
6. **optionButtonPrefab**：选项按钮预制体
7. **optionButtonContainer**：选项按钮的父容器Transform

## 四、游戏流程管理

### 故事标记系统

DialogueManager提供了简单的故事进度跟踪功能：
```csharp
// 设置标记
dialogueManager.SetStoryFlag("已询问线索A", 1);

// 获取标记
int value = dialogueManager.GetStoryFlag("已询问线索A");

// 根据标记跳转对话
if (value > 0) {
    dialogueManager.JumpToDialogue(5);
}
```

### 自定义对话结束行为

可以通过修改HandleDialogueEnd方法来自定义对话结束后的行为：
```csharp
private void HandleDialogueEnd(string dialogueName)
{
    // 根据对话名称执行特定操作
    switch (dialogueName)
    {
        case "开场白":
            // 执行特定操作
            break;
        case "结局":
            // 加载下一个场景
            SceneManager.LoadScene("结局场景");
            break;
    }
    
    // 是否自动开始下一段对话
    if (autoStartNextDialogue)
    {
        StartNextDialogue();
    }
}
```

## 五、扩展功能

### 1. 添加对话音效

修改TypeText方法，在每个字符显示时播放音效：
```csharp
private IEnumerator TypeText(string text)
{
    // ...现有代码
    
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
    
    // ...继续现有代码
}
```

### 2. 添加角色表情变化

在DialogueSO中添加情绪类型，并在显示对话时应用：
```csharp
// 在DialogueSO中添加
public enum EmotionType { Normal, Happy, Sad, Angry }

// 修改DialogueEntry结构
public struct DialogueEntry
{
    // ...现有字段
    public EmotionType emotion; // 角色情绪
}

// 加载不同表情的角色图像
private void UpdateCharacterImages(string speakerName, EmotionType emotion)
{
    bool isLeftSpeaking = DetermineIsLeftSpeaker(speakerName);
    
    // 加载对应表情的角色图像
    string emotionSuffix = emotion.ToString().ToLower();
    string imagePath = $"Characters/{speakerName}_{emotionSuffix}";
    
    Sprite characterSprite = Resources.Load<Sprite>(imagePath);
    if (characterSprite != null)
    {
        if (isLeftSpeaking)
            leftCharacterImage.sprite = characterSprite;
        else
            rightCharacterImage.sprite = characterSprite;
    }
}
```

### 3. 控制对话自动播放

您可以控制对话是否自动连续播放：
```csharp
// 关闭自动播放
dialogueManager.autoStartNextDialogue = false;

// 手动触发下一段对话
public void OnContinueButtonClick()
{
    dialogueManager.StartNextDialogue();
}
```

## 六、常见问题

1. **对话文本显示不全**：检查Text组件是否设置了合适的RectTransform大小和自动换行
2. **选项按钮不显示**：确保optionButtonPrefab和optionButtonContainer正确设置
3. **角色图像不显示**：确保角色图像资源正确放置在Resources文件夹中
4. **对话序列不继续**：检查autoStartNextDialogue是否为true，以及对话数据是否正确设置

----------

如有更多问题，请联系游戏开发团队。 