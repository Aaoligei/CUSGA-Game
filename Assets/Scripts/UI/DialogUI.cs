using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 对话界面实现
    /// </summary>
    public class DialogUI : BaseUI
    {
        // 对话组件
        private Image _dialogBackground;          // 对话背景
        private Text _speakerNameText;            // 说话者名称文本
        private Text _dialogContentText;          // 对话内容文本
        private Button _continueButton;           // 继续按钮
        private GameObject _choicesPanel;         // 选项面板
        private Transform _choiceButtonsContainer; // 选项按钮容器
        
        // 角色组件
        private Image _leftCharacterImage;        // 左侧角色图像
        private Image _rightCharacterImage;       // 右侧角色图像
        
        // 配置参数
        private float _typingSpeed = 0.05f;       // 打字速度
        private bool _isTyping = false;           // 是否正在打字
        private bool _isWaitingForInput = false;  // 是否等待输入
        
        // 当前对话数据
        private DialogData _currentDialog;
        private int _currentNodeIndex = 0;
        
        // 选项按钮预制体
        private GameObject _choiceButtonPrefab;
        
        // 临时对话数据类
        [System.Serializable]
        private class DialogNode
        {
            public string SpeakerId;           // 说话者ID
            public string SpeakerName;         // 说话者名称
            public string Content;             // 对话内容
            public bool IsLeft;                // 是否在左侧
            public List<DialogChoice> Choices = new List<DialogChoice>(); // 选项列表
            public int NextNodeId = -1;        // 下一节点ID
        }
        
        [System.Serializable]
        private class DialogChoice
        {
            public string Text;                // 选项文本
            public int NextNodeId;             // 下一节点ID
            public string ActionId;            // 动作ID
        }
        
        [System.Serializable]
        private class DialogData
        {
            public string Id;                  // 对话ID
            public List<DialogNode> Nodes = new List<DialogNode>(); // 节点列表
        }
        
        // 临时对话数据字典
        private Dictionary<string, DialogData> _dialogDataDict = new Dictionary<string, DialogData>();
        
        protected override void OnInit()
        {
            base.OnInit();
            
            // 初始化界面组件
            InitComponents();
            
            // 注册事件
            RegisterEvents();
            
            // 初始化对话数据
            InitDialogData();
        }

        /// <summary>
        /// 初始化界面组件
        /// </summary>
        private void InitComponents()
        {
            // 获取对话组件
            _dialogBackground = GetImage("DialogBackground");
            _speakerNameText = GetText("SpeakerNameText");
            _dialogContentText = GetText("DialogContentText");
            _continueButton = GetButton("ContinueButton");
            _choicesPanel = transform.Find("ChoicesPanel").gameObject;
            _choiceButtonsContainer = _choicesPanel.transform.Find("ChoiceButtonsContainer");
            
            // 获取角色组件
            _leftCharacterImage = GetImage("LeftCharacterImage");
            _rightCharacterImage = GetImage("RightCharacterImage");
            
            // 加载预制体
            _choiceButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/ChoiceButton");
            
            // 添加按钮点击事件
            AddButtonClickListener("ContinueButton", OnContinueButtonClick);
            
            // 初始隐藏选项面板
            _choicesPanel.SetActive(false);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            // 注册事件
        }

        /// <summary>
        /// 初始化对话数据
        /// </summary>
        private void InitDialogData()
        {
            // 临时对话数据，实际应从数据管理器获取
            _dialogDataDict.Clear();
            
            // 创建一个测试对话
            DialogData testDialog = new DialogData
            {
                Id = "dialog1",
                Nodes = new List<DialogNode>
                {
                    new DialogNode
                    {
                        SpeakerId = "char1",
                        SpeakerName = "王审琦",
                        Content = "高平楼倒塌案中，有很多疑点值得深究。首先，倒塌时间是在深夜，为何会有这么多人在场？",
                        IsLeft = true,
                        NextNodeId = 1
                    },
                    new DialogNode
                    {
                        SpeakerId = "char2",
                        SpeakerName = "李继勋",
                        Content = "据说当晚有场私人宴会，宋太宗近臣刘承规在高平楼设宴招待南唐降臣，不知是否与此有关？",
                        IsLeft = false,
                        NextNodeId = 2
                    },
                    new DialogNode
                    {
                        SpeakerId = "char1",
                        SpeakerName = "王审琦",
                        Content = "宴会...可为何要在夜间？且高平楼乃是新建不久，结构应当稳固，何故突然倒塌？",
                        IsLeft = true,
                        NextNodeId = 3
                    },
                    new DialogNode
                    {
                        SpeakerId = "char2",
                        SpeakerName = "李继勋",
                        Content = "阁下有何看法？",
                        IsLeft = false,
                        Choices = new List<DialogChoice>
                        {
                            new DialogChoice
                            {
                                Text = "南唐余孽所为",
                                NextNodeId = 4,
                                ActionId = "accuse_tang"
                            },
                            new DialogChoice
                            {
                                Text = "朝中党争导致",
                                NextNodeId = 5,
                                ActionId = "suggest_conflict"
                            },
                            new DialogChoice
                            {
                                Text = "建筑质量问题",
                                NextNodeId = 6,
                                ActionId = "suggest_accident"
                            }
                        }
                    },
                    new DialogNode
                    {
                        SpeakerId = "char2",
                        SpeakerName = "李继勋",
                        Content = "南唐余孽？阁下可有证据？切勿因出身而妄加揣测。",
                        IsLeft = false,
                        NextNodeId = 7
                    },
                    new DialogNode
                    {
                        SpeakerId = "char2",
                        SpeakerName = "李继勋",
                        Content = "朝中党争？此言有理，近来太宗与太祖旧臣确有龃龉...",
                        IsLeft = false,
                        NextNodeId = 7
                    },
                    new DialogNode
                    {
                        SpeakerId = "char2",
                        SpeakerName = "李继勋",
                        Content = "建筑质量？可高平楼乃朝廷重点工程，监管严格，若真如此，责任重大。",
                        IsLeft = false,
                        NextNodeId = 7
                    },
                    new DialogNode
                    {
                        SpeakerId = "char1",
                        SpeakerName = "王审琦",
                        Content = "无论如何，我们需要更多证据。此案不简单，背后牵连甚广。",
                        IsLeft = true,
                        NextNodeId = -1
                    }
                }
            };
            
            _dialogDataDict.Add(testDialog.Id, testDialog);
        }

        /// <summary>
        /// 开始对话
        /// </summary>
        /// <param name="dialogId">对话ID</param>
        public void StartDialog(string dialogId)
        {
            if (string.IsNullOrEmpty(dialogId))
            {
                Debug.LogError("对话ID不能为空");
                return;
            }
            
            // 获取对话数据
            if (_dialogDataDict.TryGetValue(dialogId, out DialogData dialogData))
            {
                _currentDialog = dialogData;
                _currentNodeIndex = 0;
                
                // 显示初始对话节点
                if (_currentDialog.Nodes.Count > 0)
                {
                    ShowDialogNode(_currentDialog.Nodes[_currentNodeIndex]);
                }
                else
                {
                    Debug.LogError($"对话节点为空: {dialogId}");
                    CloseUI();
                }
            }
            else
            {
                Debug.LogError($"未找到对话数据: {dialogId}");
                CloseUI();
            }
        }

        /// <summary>
        /// 显示对话节点
        /// </summary>
        /// <param name="node">对话节点</param>
        private void ShowDialogNode(DialogNode node)
        {
            // 设置说话者名称
            _speakerNameText.text = node.SpeakerName;
            
            // 清空对话内容
            _dialogContentText.text = "";
            
            // 根据说话者位置显示角色图像
            if (node.IsLeft)
            {
                if (_leftCharacterImage != null && _rightCharacterImage != null)
                {
                    // 设置角色图像
                    // _leftCharacterImage.sprite = Resources.Load<Sprite>($"Characters/{node.SpeakerId}");
                    
                    // 高亮左侧角色，淡化右侧角色
                    _leftCharacterImage.color = Color.white;
                    _rightCharacterImage.color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
            else
            {
                if (_leftCharacterImage != null && _rightCharacterImage != null)
                {
                    // 设置角色图像
                    // _rightCharacterImage.sprite = Resources.Load<Sprite>($"Characters/{node.SpeakerId}");
                    
                    // 高亮右侧角色，淡化左侧角色
                    _rightCharacterImage.color = Color.white;
                    _leftCharacterImage.color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
            
            // 隐藏选项面板
            _choicesPanel.SetActive(false);
            
            // 显示继续按钮
            if (_continueButton != null)
            {
                _continueButton.gameObject.SetActive(true);
            }
            
            // 开始打字效果
            StopAllCoroutines();
            StartCoroutine(TypeText(node.Content));
        }

        /// <summary>
        /// 打字效果协程
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <returns>协程迭代器</returns>
        private IEnumerator TypeText(string text)
        {
            _isTyping = true;
            _isWaitingForInput = false;
            
            // 逐字显示
            for (int i = 0; i < text.Length; i++)
            {
                _dialogContentText.text = text.Substring(0, i + 1);
                yield return new WaitForSeconds(_typingSpeed);
            }
            
            _isTyping = false;
            _isWaitingForInput = true;
        }

        /// <summary>
        /// 显示对话选项
        /// </summary>
        /// <param name="choices">选项列表</param>
        private void ShowDialogChoices(List<DialogChoice> choices)
        {
            if (choices == null || choices.Count == 0)
            {
                return;
            }
            
            // 清空选项按钮容器
            foreach (Transform child in _choiceButtonsContainer)
            {
                Destroy(child.gameObject);
            }
            
            // 创建选项按钮
            for (int i = 0; i < choices.Count; i++)
            {
                DialogChoice choice = choices[i];
                
                if (_choiceButtonPrefab != null)
                {
                    GameObject choiceButtonObj = Instantiate(_choiceButtonPrefab, _choiceButtonsContainer);
                    
                    // 设置选项文本
                    Button choiceButton = choiceButtonObj.GetComponent<Button>();
                    Text choiceText = choiceButtonObj.GetComponentInChildren<Text>();
                    
                    if (choiceText != null)
                    {
                        choiceText.text = choice.Text;
                    }
                    
                    // 添加点击事件
                    if (choiceButton != null)
                    {
                        int index = i; // 创建闭包，防止循环变量问题
                        choiceButton.onClick.AddListener(() => OnChoiceButtonClick(index));
                    }
                }
            }
            
            // 显示选项面板
            _choicesPanel.SetActive(true);
            
            // 隐藏继续按钮
            if (_continueButton != null)
            {
                _continueButton.gameObject.SetActive(false);
            }
            
            _isWaitingForInput = true;
        }

        /// <summary>
        /// 继续按钮点击事件
        /// </summary>
        private void OnContinueButtonClick()
        {
            if (_isTyping)
            {
                // 如果正在打字，则立即显示全部文本
                StopAllCoroutines();
                
                if (_currentDialog != null && _currentNodeIndex < _currentDialog.Nodes.Count)
                {
                    _dialogContentText.text = _currentDialog.Nodes[_currentNodeIndex].Content;
                    _isTyping = false;
                    _isWaitingForInput = true;
                }
                
                return;
            }
            
            if (_isWaitingForInput)
            {
                // 如果当前节点有选项，则显示选项
                if (_currentDialog != null && _currentNodeIndex < _currentDialog.Nodes.Count &&
                    _currentDialog.Nodes[_currentNodeIndex].Choices.Count > 0)
                {
                    ShowDialogChoices(_currentDialog.Nodes[_currentNodeIndex].Choices);
                }
                else
                {
                    // 否则进入下一个节点
                    ContinueToNextNode();
                }
                
                _isWaitingForInput = false;
            }
        }

        /// <summary>
        /// 选项按钮点击事件
        /// </summary>
        /// <param name="choiceIndex">选项索引</param>
        private void OnChoiceButtonClick(int choiceIndex)
        {
            if (_currentDialog != null && _currentNodeIndex < _currentDialog.Nodes.Count)
            {
                DialogNode currentNode = _currentDialog.Nodes[_currentNodeIndex];
                
                if (choiceIndex >= 0 && choiceIndex < currentNode.Choices.Count)
                {
                    DialogChoice choice = currentNode.Choices[choiceIndex];
                    
                    // 执行选项动作
                    if (!string.IsNullOrEmpty(choice.ActionId))
                    {
                        ExecuteChoiceAction(choice.ActionId);
                    }
                    
                    // 根据选项进入下一个节点
                    if (choice.NextNodeId >= 0 && choice.NextNodeId < _currentDialog.Nodes.Count)
                    {
                        _currentNodeIndex = choice.NextNodeId;
                        ShowDialogNode(_currentDialog.Nodes[_currentNodeIndex]);
                    }
                    else
                    {
                        // 结束对话
                        Debug.Log("对话结束");
                        CloseUI();
                    }
                }
            }
            
            _isWaitingForInput = false;
        }

        /// <summary>
        /// 执行选项动作
        /// </summary>
        /// <param name="actionId">动作ID</param>
        private void ExecuteChoiceAction(string actionId)
        {
            Debug.Log($"执行选项动作: {actionId}");
            
            // 根据动作ID执行不同操作
            switch (actionId)
            {
                case "accuse_tang":
                    // 指责南唐余孽
                    // 这里可以触发相关事件或改变游戏状态
                    break;
                case "suggest_conflict":
                    // 暗示朝中党争
                    // 这里可以触发相关事件或改变游戏状态
                    break;
                case "suggest_accident":
                    // 建议可能是意外
                    // 这里可以触发相关事件或改变游戏状态
                    break;
            }
        }

        /// <summary>
        /// 继续到下一个节点
        /// </summary>
        private void ContinueToNextNode()
        {
            if (_currentDialog != null && _currentNodeIndex < _currentDialog.Nodes.Count)
            {
                DialogNode currentNode = _currentDialog.Nodes[_currentNodeIndex];
                
                // 获取下一个节点ID
                int nextNodeId = currentNode.NextNodeId;
                
                if (nextNodeId >= 0 && nextNodeId < _currentDialog.Nodes.Count)
                {
                    // 进入下一个节点
                    _currentNodeIndex = nextNodeId;
                    ShowDialogNode(_currentDialog.Nodes[_currentNodeIndex]);
                }
                else
                {
                    // 结束对话
                    Debug.Log("对话结束");
                    CloseUI();
                }
            }
        }

        public override void OnShow()
        {
            base.OnShow();
            
            // 进入对话状态
            GameManager.Instance.EnterDialogState();
        }

        public override void OnHide()
        {
            base.OnHide();
            
            // 退出对话状态
            if (GameManager.Instance.CurrentState == GameState.Dialog)
            {
                GameManager.Instance.ResumeGame();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            // 停止所有协程
            StopAllCoroutines();
        }
    }
} 