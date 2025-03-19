using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 锦囊弹窗界面实现
    /// </summary>
    public class TipsPopupUI : BaseUI
    {
        // 界面组件
        private Button _closeButton;            // 关闭按钮
        private Text _tipTitleText;             // 锦囊标题文本
        private Text _tipContentText;           // 锦囊内容文本
        private Image _tipImage;                // 锦囊图片
        private Button _prevTipButton;          // 上一条锦囊按钮
        private Button _nextTipButton;          // 下一条锦囊按钮
        private Text _tipIndexText;             // 锦囊索引文本
        
        // 锦囊数据
        private List<TipData> _tipsList = new List<TipData>();
        private int _currentTipIndex = 0;
        
        // 锦囊数据类
        private class TipData
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string ImagePath { get; set; }
            public bool IsUnlocked { get; set; }
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            // 初始化界面组件
            InitComponents();
            
            // 初始化锦囊数据
            InitTipsData();
        }

        /// <summary>
        /// 初始化界面组件
        /// </summary>
        private void InitComponents()
        {
            // 获取界面组件
            _closeButton = GetButton("CloseButton");
            _tipTitleText = GetText("TipTitleText");
            _tipContentText = GetText("TipContentText");
            _tipImage = GetImage("TipImage");
            _prevTipButton = GetButton("PrevTipButton");
            _nextTipButton = GetButton("NextTipButton");
            _tipIndexText = GetText("TipIndexText");
            
            // 添加按钮点击事件
            AddButtonClickListener("CloseButton", OnCloseButtonClick);
            AddButtonClickListener("PrevTipButton", OnPrevTipButtonClick);
            AddButtonClickListener("NextTipButton", OnNextTipButtonClick);
        }

        /// <summary>
        /// 初始化锦囊数据
        /// </summary>
        private void InitTipsData()
        {
            // 清空锦囊列表
            _tipsList.Clear();
            
            // 添加锦囊数据
            _tipsList.Add(new TipData
            {
                Id = "tip1",
                Title = "高平楼倒塌案背景",
                Content = "高平楼是北宋开宝年间新建的一座楼阁，位于东京汴梁城内，因地势较高而得名。据史料记载，大宋建隆年间，高平楼曾发生一次严重倒塌事件，造成多人伤亡，其中包括多名南唐降臣和宋朝官员。此案被认为是一起重大政治事件，背后可能牵涉到南唐余孽与北宋朝臣之间的复杂关系。",
                ImagePath = "Images/Tips/tip1",
                IsUnlocked = true
            });
            
            _tipsList.Add(new TipData
            {
                Id = "tip2",
                Title = "搜证技巧",
                Content = "进行搜证时，应注意以下几点：\n1. 仔细观察场景中的异常物品或痕迹\n2. 使用特殊道具可能会发现隐藏线索\n3. 与场景中的人物交谈获取更多信息\n4. 在不同时间段访问同一场景可能会有不同发现\n5. 记得查看背包中收集的物品，它们之间可能存在关联",
                ImagePath = "Images/Tips/tip2",
                IsUnlocked = true
            });
            
            _tipsList.Add(new TipData
            {
                Id = "tip3",
                Title = "笔录分析方法",
                Content = "分析角色笔录时，应注意以下几点：\n1. 关注角色描述事件的时间线是否存在矛盾\n2. 对比不同角色对同一事件的描述，找出差异\n3. 注意角色的情绪变化和语气转变\n4. 细节描述的准确性往往能反映角色是否亲历事件\n5. 多次阅读同一笔录，可能会发现之前忽略的细节",
                ImagePath = "Images/Tips/tip3",
                IsUnlocked = true
            });
            
            _tipsList.Add(new TipData
            {
                Id = "tip4",
                Title = "阵营判断要点",
                Content = "判断角色阵营时，应考虑以下几点：\n1. 角色的出身背景和家族关系\n2. 角色在对话中流露的政治倾向\n3. 角色对特定历史事件的态度\n4. 角色与其他角色的交往关系\n5. 角色持有或使用的特殊物品\n\n记住，外表和言辞可能具有欺骗性，要从多方面证据综合判断。",
                ImagePath = "Images/Tips/tip4",
                IsUnlocked = true
            });
            
            _tipsList.Add(new TipData
            {
                Id = "tip5",
                Title = "北宋与南唐关系概述",
                Content = "南唐，是五代十国时期的割据政权之一，由李氏家族建立，都城建康（今南京）。北宋建立后，太祖赵匡胤实行“攘外安内”政策，逐步统一中国。大宋建隆二年（961年），南唐与北宋签订盟约，奉宋为正朝，成为北宋的藩属国。开宝七年（974年），宋太祖派兵伐唐，南唐灭亡，国主李煜被俘，被封为“违命侯”。然而，南唐遗民对北宋的态度复杂，有归顺者，也有心怀不满者。",
                ImagePath = "Images/Tips/tip5",
                IsUnlocked = true
            });
        }

        /// <summary>
        /// 显示锦囊
        /// </summary>
        /// <param name="tipId">锦囊ID</param>
        public void ShowTip(string tipId)
        {
            // 查找锦囊
            int index = _tipsList.FindIndex(tip => tip.Id == tipId);
            
            if (index >= 0)
            {
                _currentTipIndex = index;
                UpdateTipDisplay();
            }
            else
            {
                Debug.LogError($"未找到锦囊: {tipId}");
                
                // 显示第一条锦囊
                if (_tipsList.Count > 0)
                {
                    _currentTipIndex = 0;
                    UpdateTipDisplay();
                }
            }
        }

        /// <summary>
        /// 更新锦囊显示
        /// </summary>
        private void UpdateTipDisplay()
        {
            if (_tipsList.Count == 0 || _currentTipIndex < 0 || _currentTipIndex >= _tipsList.Count)
            {
                return;
            }
            
            TipData currentTip = _tipsList[_currentTipIndex];
            
            // 更新锦囊标题
            if (_tipTitleText != null)
            {
                _tipTitleText.text = currentTip.Title;
            }
            
            // 更新锦囊内容
            if (_tipContentText != null)
            {
                _tipContentText.text = currentTip.Content;
            }
            
            // 更新锦囊图片
            if (_tipImage != null && !string.IsNullOrEmpty(currentTip.ImagePath))
            {
                Sprite tipSprite = Resources.Load<Sprite>(currentTip.ImagePath);
                if (tipSprite != null)
                {
                    _tipImage.sprite = tipSprite;
                    _tipImage.gameObject.SetActive(true);
                }
                else
                {
                    _tipImage.gameObject.SetActive(false);
                }
            }
            else
            {
                if (_tipImage != null)
                {
                    _tipImage.gameObject.SetActive(false);
                }
            }
            
            // 更新锦囊索引
            if (_tipIndexText != null)
            {
                _tipIndexText.text = $"{_currentTipIndex + 1}/{_tipsList.Count}";
            }
            
            // 更新翻页按钮状态
            UpdatePageButtons();
        }

        /// <summary>
        /// 更新翻页按钮状态
        /// </summary>
        private void UpdatePageButtons()
        {
            // 更新上一页按钮
            if (_prevTipButton != null)
            {
                _prevTipButton.interactable = _currentTipIndex > 0;
            }
            
            // 更新下一页按钮
            if (_nextTipButton != null)
            {
                _nextTipButton.interactable = _currentTipIndex < _tipsList.Count - 1;
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void OnCloseButtonClick()
        {
            Debug.Log("点击关闭按钮");
            // 关闭锦囊弹窗界面
            CloseUI();
        }

        /// <summary>
        /// 上一条锦囊按钮点击事件
        /// </summary>
        private void OnPrevTipButtonClick()
        {
            if (_currentTipIndex > 0)
            {
                _currentTipIndex--;
                UpdateTipDisplay();
            }
        }

        /// <summary>
        /// 下一条锦囊按钮点击事件
        /// </summary>
        private void OnNextTipButtonClick()
        {
            if (_currentTipIndex < _tipsList.Count - 1)
            {
                _currentTipIndex++;
                UpdateTipDisplay();
            }
        }

        public override void OnShow()
        {
            base.OnShow();
            
            // 显示第一条锦囊
            if (_tipsList.Count > 0 && _currentTipIndex < 0)
            {
                _currentTipIndex = 0;
                UpdateTipDisplay();
            }
            else
            {
                // 更新当前显示的锦囊
                UpdateTipDisplay();
            }
        }
    }
} 