# 2D文字对话游戏系统使用指南（更新版）

## 一、系统更新内容

本次更新主要修改了对话数据结构和UI显示方式：
1. 每个对话条目有单句内容，而非多句内容列表
2. 对话选项固定为上下两个选项位置
3. 角色类型分为三类：旁白、NPC和主角
4. 只有NPC类型的角色会显示立绘且在左侧显示
5. 对话按顺序依次播放
6. **新功能**：对话结束后支持自动显示弹窗序列

## 二、快速开始

### 1. 创建UI界面

1. 创建一个Canvas作为UI的父对象
2. 添加如下组件到Canvas下：
   - **对话面板 (DialoguePanel)**：包含背景图、说话者名称、对话内容
   - **选项面板 (OptionsPanel)**：包含上下两个选项按钮
   - **角色立绘图像**：用于显示NPC角色
   - **弹窗容器（PopupContainer）**：用于存放弹窗预制体实例

UI层级结构示例：
```
Canvas
├── DialoguePanel
│   ├── Background
│   ├── SpeakerNameText
│   └── DialogueContentText
├── OptionsPanel
│   ├── TopOptionButton (带Text组件)
│   └── BottomOptionButton (带Text组件)
├── CharacterImage
└── PopupContainer
```

### 2. 创建管理器对象

1. 在场景中创建一个空对象，命名为"GameManager"
2. 添加GameController脚本到该对象
3. 创建子对象"DialogueManager"，添加DialogueManager脚本
4. 创建子对象"PopupManager"，添加PopupManager脚本
5. 在GameController中设置DialogueManager和PopupManager的引用

### 3. 设置弹窗预制体

1. 创建弹窗预制体，每个预制体需要：
   - 背景图像或面板
   - 内容文本
   - CanvasGroup组件（用于淡入淡出效果）
2. 将预制体拖拽到PopupManager的popupPrefabs数组中
3. 设置popupContainer（通常指向Canvas下的PopupContainer对象）

## 三、完整游戏流程

新系统支持的完整游戏流程如下：

1. 游戏启动 → GameController初始化
2. 自动开始对话序列 → DialogueManager显示对话
3. 玩家点击屏幕或按空格键继续对话
4. 遇到分支选项时，玩家选择上/下选项
5. 所有对话结束 → 触发OnAllDialoguesComplete事件
6. 自动开始显示弹窗序列 → PopupManager依次显示弹窗
7. 玩家点击屏幕或按空格键显示下一个弹窗
8. 所有弹窗显示完毕 → 触发OnAllPopupsComplete事件
9. 可选：加载下一个场景

## 四、弹窗系统设置

### 创建弹窗预制体

弹窗预制体是一个UI面板，通常包含：
1. **背景图**：整个弹窗的背景
2. **内容文本**：要显示的信息
3. **（可选）图片**：相关图像或插图
4. **（可选）按钮**：确认按钮（不是必需的，因为点击任意位置可继续）

每个弹窗预制体在Resources文件夹下创建，例如：`Resources/Prefabs/Popups/Popup1.prefab`

### 设置PopupManager

PopupManager组件需要设置以下属性：
1. **Popup Prefabs**：要显示的弹窗预制体数组
2. **Popup Container**：弹窗的父容器Transform
3. **Fade In Duration**：弹窗淡入动画的时间（秒）

### 连接弹窗系统和对话系统

1. 在DialogueManager中设置弹窗相关属性：
   - **Popup Manager**：拖拽PopupManager引用
   - **Show Popups After Dialogue**：勾选此选项，使对话结束后自动显示弹窗

2. 或者在代码中手动连接：
```csharp
// 获取管理器
DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
PopupManager popupManager = FindObjectOfType<PopupManager>();

// 设置引用
dialogueManager.popupManager = popupManager;
dialogueManager.showPopupsAfterDialogue = true;

// 订阅事件
dialogueManager.OnAllDialoguesComplete += () => {
    Debug.Log("所有对话已完成");
};

popupManager.OnAllPopupsComplete += () => {
    Debug.Log("所有弹窗已完成");
};
```

## 五、弹窗交互方式

### 基本交互

- **点击屏幕**：关闭当前弹窗，显示下一个
- **按空格键**：同上

### 自定义控制

如果需要更复杂的控制，可以使用PopupManager提供的方法：
- **ShowPopupSequence()**：开始显示弹窗序列
- **ShowNextPopup()**：显示下一个弹窗
- **SkipAllPopups()**：跳过所有剩余弹窗

## 六、扩展功能示例

### 1. 在对话选项选择后记录玩家选择

```csharp
// 在DialogueManager或GameController中
dialogueController.OnOptionSelected += (optionIndex) => {
    if (currentDialogue.dialogueName == "重要选择") {
        // 记录选择
        SetStoryFlag("玩家选择", optionIndex);
    }
};
```

### 2. 根据对话内容动态决定显示哪些弹窗

```csharp
// 在对话结束事件中
dialogueManager.OnAllDialoguesComplete += () => {
    // 根据故事标记决定显示哪些弹窗
    if (dialogueManager.GetStoryFlag("已询问线索A") > 0) {
        popupManager.popupPrefabs = new GameObject[] {
            Resources.Load<GameObject>("Prefabs/Popups/线索A弹窗"),
            Resources.Load<GameObject>("Prefabs/Popups/结论弹窗")
        };
    } else {
        popupManager.popupPrefabs = new GameObject[] {
            Resources.Load<GameObject>("Prefabs/Popups/结论弹窗")
        };
    }
    
    popupManager.ShowPopupSequence();
};
```

### 3. 实现自定义弹窗行为

创建继承自PopupClickHandler的自定义类：
```csharp
public class CustomPopupHandler : PopupClickHandler
{
    public string flagName;
    public int flagValue;
    
    private new void Update()
    {
        base.Update();
        
        // 添加额外的交互逻辑
        if (Input.GetKeyDown(KeyCode.S)) {
            // 保存数据
            FindObjectOfType<DialogueManager>().SetStoryFlag(flagName, flagValue);
            // 关闭弹窗
            popupManager.ShowNextPopup();
        }
    }
}
```

## 七、常见问题

1. **弹窗未显示**：
   - 检查popupPrefabs数组是否正确设置
   - 确认popupContainer已正确引用
   - 验证showPopupsAfterDialogue选项已启用

2. **弹窗显示但无法点击**：
   - 确保弹窗预制体没有阻挡射线的元素
   - 检查是否有其他UI元素遮挡了弹窗
   - 验证PopupClickHandler是否正确添加

3. **弹窗淡入淡出效果不正常**：
   - 确保预制体包含CanvasGroup组件
   - 检查fadeInDuration值是否合理（通常0.3-1.0秒）

4. **对话结束后没有自动显示弹窗**：
   - 确认DialogueManager中的popupManager引用已设置
   - 验证showPopupsAfterDialogue选项已启用
   - 检查popupPrefabs数组是否有元素

----------

如有更多问题，请联系游戏开发团队。 