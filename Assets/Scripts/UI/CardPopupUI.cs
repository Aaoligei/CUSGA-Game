using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 卡牌弹窗界面实现
    /// </summary>
    public class CardPopupUI : BaseUI
    {
        // 界面组件
        private Button _closeButton;           // 关闭按钮
        private Image _cardImage;              // 卡牌图片
        private Text _cardNameText;            // 卡牌名称文本
        private Text _cardDescriptionText;     // 卡牌描述文本
        private Text _cardEffectText;          // 卡牌效果文本
        private Button _useButton;             // 使用按钮

        private Button _weibiButton;           // 威逼按钮
        private Button _liyouButton;           // 利诱按钮
        private Button _chanjiButton;          // 禅机按钮
        private Button _gongqingButton;        // 共情按钮

        private Image _weibiSelectImage;      // 威逼选中图片
        private Image _liyouSelectImage;      // 利诱选中图片
        private Image _chanjiSelectImage;     // 禅机选中图片
        private Image _gongqingSelectImage;   // 共情选中图片

        // 当前显示的卡牌ID
        private string _currentCardId;
        
        // 临时卡牌数据字典（实际应从数据管理器获取）
        private class CardData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Effect { get; set; }
            public bool CanUse { get; set; }
        }
        
        private Dictionary<string, CardData> _cardDataDict = new Dictionary<string, CardData>();
        
        protected override void OnInit()
        {
            base.OnInit();
            
            // 初始化界面组件
            InitComponents();
            
            // 注册事件
            RegisterEvents();
            
            // 初始化卡牌数据
            InitCardData();
        }

        /// <summary>
        /// 初始化界面组件
        /// </summary>
        private void InitComponents()
        {
            // 获取界面组件
            _closeButton = GetButton("关闭按钮");
            _cardImage = GetImage("CardImage");
            _cardNameText = GetText("CardNameText");
            _cardDescriptionText = GetText("CardDescriptionText");
            _cardEffectText = GetText("CardEffectText");
            _useButton = GetButton("UseButton");

            _weibiButton = GetButton("威逼按钮");
            _liyouButton = GetButton("利诱按钮");
            _chanjiButton = GetButton("禅机按钮");
            _gongqingButton = GetButton("共情按钮");

            _weibiSelectImage = GetImage("威逼按钮选中");
            _liyouSelectImage = GetImage("利诱按钮选中");
            _chanjiSelectImage = GetImage("禅机按钮选中");
            _gongqingSelectImage = GetImage("共情按钮选中");

            _weibiSelectImage.gameObject.SetActive(false);
            _liyouSelectImage.gameObject.SetActive(false);
            _chanjiSelectImage.gameObject.SetActive(false);
            _gongqingSelectImage.gameObject.SetActive(false);

            // 添加按钮点击事件
            AddButtonClickListener("关闭按钮", OnCloseButtonClick);
            AddButtonClickListener("UseButton", OnUseButtonClick);

            AddButtonClickListener("威逼按钮", () => { 
                _weibiSelectImage.gameObject.SetActive(true);
                _liyouSelectImage.gameObject.SetActive(false);
                _chanjiSelectImage.gameObject.SetActive(false);
                _gongqingSelectImage.gameObject.SetActive(false);
            });

            AddButtonClickListener("利诱按钮", () =>
            {
                _weibiSelectImage.gameObject.SetActive(false);
                _liyouSelectImage.gameObject.SetActive(true);
                _chanjiSelectImage.gameObject.SetActive(false);
                _gongqingSelectImage.gameObject.SetActive(false);
            });

            AddButtonClickListener("禅机按钮", () =>
            {
                _weibiSelectImage.gameObject.SetActive(false);
                _liyouSelectImage.gameObject.SetActive(false);
                _chanjiSelectImage.gameObject.SetActive(true);
                _gongqingSelectImage.gameObject.SetActive(false);
            });

            AddButtonClickListener("共情按钮", () =>
            {
                _weibiSelectImage.gameObject.SetActive(false);
                _liyouSelectImage.gameObject.SetActive(false);
                _chanjiSelectImage.gameObject.SetActive(false);
                _gongqingSelectImage.gameObject.SetActive(true);
            });

            // 初始隐藏使用按钮
            if (_useButton != null)
            {
                _useButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            // 注册卡牌使用事件
            GEventSystem.Instance.AddListener(GameEvents.CardUsed, OnCardUsed);
        }

        /// <summary>
        /// 初始化卡牌数据
        /// </summary>
        private void InitCardData()
        {
            // 临时卡牌数据，实际应从数据管理器获取
            _cardDataDict.Clear();
            
            // 添加一些测试卡牌
            _cardDataDict.Add("card1", new CardData 
            { 
                Id = "card1", 
                Name = "调查", 
                Description = "调查周围环境，获取更多线索。", 
                Effect = "使用后，搜证范围扩大，可发现隐藏物品。",
                CanUse = true
            });
            
            _cardDataDict.Add("card2", new CardData 
            { 
                Id = "card2", 
                Name = "推理", 
                Description = "根据已有线索进行推理，找出事件关键点。", 
                Effect = "使用后，可解锁特定角色的笔录。",
                CanUse = true
            });
            
            _cardDataDict.Add("card3", new CardData 
            { 
                Id = "card3", 
                Name = "辩论", 
                Description = "与角色进行辩论，揭示矛盾点。", 
                Effect = "使用后，进入辩论小游戏，成功可获得重要信息。",
                CanUse = true
            });
            
            _cardDataDict.Add("card4", new CardData 
            { 
                Id = "card4", 
                Name = "分析", 
                Description = "分析已收集的证据，找出关联。", 
                Effect = "使用后，可发现证据间的关联性，推进剧情。",
                CanUse = true
            });
        }

        /// <summary>
        /// 显示卡牌
        /// </summary>
        /// <param name="cardId">卡牌ID</param>
        public void ShowCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                Debug.LogError("卡牌ID不能为空");
                return;
            }
            
            _currentCardId = cardId;
            
            // 获取卡牌数据
            CardData cardData = null;
            if (_cardDataDict.TryGetValue(cardId, out cardData))
            {
                // 设置卡牌图片
                // _cardImage.sprite = Resources.Load<Sprite>($"Cards/{cardId}");
                
                // 设置卡牌名称
                _cardNameText.text = cardData.Name;
                
                // 设置卡牌描述
                _cardDescriptionText.text = cardData.Description;
                
                // 设置卡牌效果
                _cardEffectText.text = cardData.Effect;
                
                // 更新按钮状态
                UpdateButtons(cardData);
            }
            else
            {
                Debug.LogError($"未找到卡牌数据: {cardId}");
            }
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        /// <param name="cardData">卡牌数据</param>
        private void UpdateButtons(CardData cardData)
        {
            // 更新使用按钮
            if (_useButton != null)
            {
                _useButton.gameObject.SetActive(cardData.CanUse);
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void OnCloseButtonClick()
        {
            Debug.Log("点击关闭按钮");
            // 关闭卡牌弹窗界面
            CloseUI();
        }

        /// <summary>
        /// 使用按钮点击事件
        /// </summary>
        private void OnUseButtonClick()
        {
            Debug.Log($"点击使用按钮: {_currentCardId}");
            
            // 使用卡牌
            UseCard(_currentCardId);
        }

        /// <summary>
        /// 使用卡牌
        /// </summary>
        /// <param name="cardId">卡牌ID</param>
        private void UseCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                return;
            }
            
            // 获取卡牌数据
            if (_cardDataDict.TryGetValue(cardId, out CardData cardData))
            {
                if (cardData.CanUse)
                {
                    // 使用卡牌
                    Debug.Log($"使用卡牌: {cardData.Name}");
                    
                    // 根据卡牌类型执行不同效果
                    switch (cardId)
                    {
                        case "card1": // 调查卡
                            // 在搜证场景中扩大搜查范围，显示更多物品
                            break;
                        case "card2": // 推理卡
                            // 解锁特定角色的笔录
                            DataManager dataManager = DataManager.Instance;
                            if (dataManager != null)
                            {
                                // 这里假设解锁char1的笔录note3
                                // dataManager.UnlockCharacterNote("char1", "note3");
                            }
                            break;
                        case "card3": // 辩论卡
                            // 进入辩论小游戏
                            // UIManager.Instance.OpenUI(UIType.CardGameUI);
                            break;
                        case "card4": // 分析卡
                            // 显示证据关联线索
                            break;
                    }
                    
                    // 触发卡牌使用事件
                    GEventSystem.Instance.TriggerEvent(GameEvents.CardUsed, cardId);
                    
                    // 关闭卡牌弹窗界面
                    CloseUI();
                }
            }
        }

        /// <summary>
        /// 卡牌使用事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnCardUsed(object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is string cardId)
            {
                Debug.Log($"卡牌使用事件回调: {cardId}");
                
                // 如果是当前显示的卡牌，则关闭界面
                if (cardId == _currentCardId)
                {
                    CloseUI();
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            // 取消注册事件
            if (GEventSystem.Instance != null)
            {
                GEventSystem.Instance.RemoveListener(GameEvents.CardUsed, OnCardUsed);
            }
        }
    }
} 