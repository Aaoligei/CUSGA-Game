# 2D文字对话游戏系统使用指南

## 一、系统介绍

本对话系统专为2D文字对话游戏设计，支持以下功能：

1. 自动开始对话序列（无需NPC交互触发）
2. 打字机文字显示效果
3. 支持不同类型说话者（旁白、NPC、玩家）
4. 固定的上下两个选项结构
5. 对话结束后自动显示弹窗序列
6. 完整的事件回调系统

## 二、快速设置

### 场景设置

1. 在场景中创建以下层次结构：
   ```
   - GameController
   - DialogueManager
     - DialoguePanel
       - SpeakerNameText
       - DialogueContentText
       - CharacterImage
       - OptionsPanel
         - TopOptionButton
           - TopOptionText
         - BottomOptionButton
           - BottomOptionText
   - PopupManager
     - PopupContainer
   ```

2. 设置引用：
   - 将 DialogueManager 脚本添加到 DialogueManager 对象
   - 将 GameController 脚本添加到 GameController 对象
   - 将 PopupManager 脚本添加到 PopupManager 对象
   - 设置 DialogueManager 中的 UI 组件引用
   - 设置 GameController 中的管理器引用

### 创建对话数据

1. 在 Project 窗口中右键点击 → Create → Dialogue System → Dialogue Data
2. 设置对话名称和对话条目数组
3. 将创建好的对话数据拖拽到 DialogueManager 的 Dialogue Sequence 数组中

### 创建弹窗预制体

1. 为每个弹窗创建一个预制体，确保它们都有 Canvas Group 组件
2. 将创建好的弹窗预制体拖拽到 PopupManager 的 Popup Prefabs 数组中

## 三、对话数据结构说明

### DialogueSO 结构

```csharp
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
```

### 说话者类型说明

1. **旁白（Narrator）**：
   - 不显示角色立绘
   - 通常说话者名称设为"旁白"或不显示

2. **NPC（NPC）**：
   - 显示角色立绘
   - 立绘图片应放在 Resources/Characters 文件夹中，文件名与说话者名称相同

3. **玩家（Player）**：
   - 不显示角色立绘
   - 说话者名称通常为玩家角色名

### 对话选项设置

每个对话条目可以设置两个选项：

1. 设置 `hasOptions` 为 true
2. 设置上方选项（topOption）和下方选项（bottomOption）的文本和跳转索引
3. 如果不需要选项，则将 `hasOptions` 设为 false，并设置 `nextDialogueIndex` 为下一个对话索引

## 四、弹窗系统

### 弹窗预制体要求

1. 每个弹窗预制体应具有以下结构：
   ```
   - PopupPrefab
     - Background
     - Content
     - (其他UI元素)
   ```

2. 弹窗会自动添加 CanvasGroup 组件用于淡入淡出效果
3. 弹窗会自动添加 PopupClickHandler 组件处理点击事件

### 弹窗交互

1. 玩家可以点击任意位置或按空格键显示下一个弹窗
2. 弹窗按 PopupManager 中设置的顺序依次显示
3. 所有弹窗显示完毕后，系统会触发 OnAllPopupsComplete 事件

## 五、游戏流程控制

默认的游戏流程如下：

1. 游戏启动 → 自动开始对话序列
2. 对话序列结束 → 自动显示弹窗序列
3. 弹窗序列结束 → 加载下一个场景（如果设置了）

### 使用GameController控制游戏流程

GameController 提供以下功能：

1. 自动查找和连接对话管理器和弹窗管理器
2. 处理对话和弹窗完成事件
3. 提供跳过所有内容的功能（按Esc键）
4. 提供重新开始场景的功能（按R键）
5. 支持设置下一个要加载的场景

## 六、资源放置规则

1. 角色立绘：放在 Resources/Characters 文件夹下，文件名与说话者名称相同
2. 对话数据：建议放在 Resources/Dialogues 文件夹下
3. 弹窗预制体：建议放在 Resources/Popups 文件夹下

## 七、扩展与自定义

### 添加自定义事件

可以利用以下事件进行扩展：

1. DialogueController.OnDialogueStart
2. DialogueController.OnDialogueEnd
3. DialogueController.OnOptionSelected
4. DialogueManager.OnAllDialoguesComplete
5. PopupManager.OnAllPopupsComplete

### 自定义UI样式

可以修改 DialoguePanel 和弹窗预制体的UI设计，只要保持组件引用正确即可。

## 八、常见问题解决

1. **对话不自动开始**：检查 GameController 的 startDialogueOnAwake 设置是否为 true

2. **角色立绘不显示**：
   - 确认说话者类型设置为 NPC
   - 检查立绘文件是否放在正确位置（Resources/Characters/[说话者名称].png）
   - 检查立绘文件名是否与说话者名称完全一致（区分大小写）

3. **选项不显示**：确认对话条目的 hasOptions 设置为 true

4. **弹窗不显示**：
   - 检查 DialogueManager 的 showPopupsAfterDialogue 设置是否为 true
   - 确认 PopupManager 中的 popupPrefabs 数组已正确设置

5. **无法跳转到下一个场景**：检查 GameController 的 nextSceneName 设置是否正确 