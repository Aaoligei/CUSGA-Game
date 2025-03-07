# 简易解谜游戏开发指南

本项目包含了一个简单的2D解谜游戏框架，使用Unity的基本图形和现有脚本即可创建一个完整的游戏。

## 游戏概述

这是一个简单的2D横版解谜游戏，玩家需要通过与环境中的开关交互来解决谜题，打开门，到达终点。

## 创建游戏步骤

### 1. 设置场景

1. 在Unity中创建一个新的2D场景
2. 添加地面和平台（使用Unity默认的方块精灵）
3. 设置适当的光照和相机位置

### 2. 创建玩家角色

1. 创建一个空游戏对象，命名为"Player"
2. 添加以下组件：
   - SpriteRenderer（使用默认方块精灵）
   - BoxCollider2D
   - Rigidbody2D（设置为Dynamic）
   - PlayerController脚本
3. 在Player下创建一个空子对象，命名为"GroundCheck"，并将其位置设置在玩家底部
4. 在PlayerController组件中设置GroundCheck引用和Ground层

### 3. 创建PuzzleManager

1. 创建一个空游戏对象，命名为"GameManager"
2. 添加PuzzleManager脚本

### 4. 创建开关

1. 创建一个空游戏对象，命名为"Switch"
2. 添加以下组件：
   - SpriteRenderer（使用默认方块精灵，可以设置为红色表示关闭状态）
   - BoxCollider2D（设置为IsTrigger）
   - Switch脚本
3. 创建两个子对象，分别命名为"VisualOn"和"VisualOff"，并添加SpriteRenderer组件
   - VisualOn：使用绿色方块表示开启状态
   - VisualOff：使用红色方块表示关闭状态
4. 创建一个子对象，命名为"InteractionPrompt"，添加TextMesh组件，内容为"按E交互"
5. 在Switch组件中设置：
   - SwitchID：一个唯一的标识符，如"switch1"
   - VisualOn和VisualOff引用
   - InteractionPrompt引用
   - PuzzleID：关联的谜题ID

### 5. 创建谜题

1. 创建一个空游戏对象，命名为"Puzzle"
2. 添加SwitchPuzzle脚本
3. 设置PuzzleID为一个唯一标识符，如"puzzle1"
4. 添加需要满足条件的开关列表，指定开关ID和所需状态

### 6. 创建门

1. 创建一个空游戏对象，命名为"Door"
2. 添加以下组件：
   - SpriteRenderer（使用默认方块精灵，可以设置为棕色表示门）
   - BoxCollider2D（设置为IsTrigger）
   - Door脚本
3. 创建两个子对象，分别命名为"VisualClosed"和"VisualOpen"，并添加SpriteRenderer组件
   - VisualClosed：使用棕色方块表示关闭状态
   - VisualOpen：使用透明度较低的棕色方块表示开启状态
4. 创建两个子对象，分别命名为"InteractionPrompt"和"LockedPrompt"，添加TextMesh组件
   - InteractionPrompt：内容为"按E开门"
   - LockedPrompt：内容为"门已锁住，需要解决谜题"
5. 在Door组件中设置：
   - 初始状态为锁住(isLocked = true)
   - VisualClosed和VisualOpen引用
   - InteractionPrompt和LockedPrompt引用
   - RequiredPuzzleID：需要解决的谜题ID，与之前创建的谜题ID对应

### 7. 设置输入系统

项目已经包含了InputSystem_Actions类，无需额外设置。

### 8. 测试游戏

1. 运行游戏
2. 使用WASD键移动玩家
3. 按空格键跳跃
4. 靠近开关时，按E键与开关交互
5. 当所有开关都处于正确状态时，谜题将被解决，门会解锁
6. 靠近解锁的门时，按E键打开门

## 扩展游戏

1. 添加更多的开关和谜题
2. 创建多个房间和关卡
3. 添加收集品和其他交互对象
4. 添加敌人和障碍物
5. 添加音效和背景音乐
6. 添加UI界面和菜单

## 注意事项

- 所有可交互对象都应该实现IInteractable接口
- 所有谜题都应该继承自PuzzleBase类
- 确保所有谜题都在PuzzleManager中注册
- 使用适当的层设置来确保碰撞检测正常工作
CUSGA比赛作品（暂定）
