using UnityEngine;
using UnityEngine.UI;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 道具弹窗界面实现
    /// </summary>
    public class ItemPopupUI : BaseUI
    {
        // 界面组件
        private Button _closeButton;           // 关闭按钮
        private Image _itemImage;              // 道具图片
        private Text _itemNameText;            // 道具名称文本
        private Text _itemDescriptionText;     // 道具描述文本
        private Button _useButton;             // 使用按钮
        private Button _equipButton;           // 装备按钮
        private Button _unequipButton;         // 卸下按钮
        
        // 当前显示的道具ID
        private string _currentItemId;
        
        // 临时道具数据字典（实际应从数据管理器获取）
        private class ItemData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool CanUse { get; set; }
            public bool CanEquip { get; set; }
        }
        
        private System.Collections.Generic.Dictionary<string, ItemData> _itemDataDict = new System.Collections.Generic.Dictionary<string, ItemData>();
        
        protected override void OnInit()
        {
            base.OnInit();
            
            // 初始化界面组件
            InitComponents();
            
            // 注册事件
            RegisterEvents();
            
            // 初始化道具数据
            InitItemData();
        }

        /// <summary>
        /// 初始化界面组件
        /// </summary>
        private void InitComponents()
        {
            // 获取界面组件
            _closeButton = GetButton("CloseButton");
            _itemImage = GetImage("ItemImage");
            _itemNameText = GetText("ItemNameText");
            _itemDescriptionText = GetText("ItemDescriptionText");
            _useButton = GetButton("UseButton");
            _equipButton = GetButton("EquipButton");
            _unequipButton = GetButton("UnequipButton");
            
            // 添加按钮点击事件
            AddButtonClickListener("CloseButton", OnCloseButtonClick);
            AddButtonClickListener("UseButton", OnUseButtonClick);
            AddButtonClickListener("EquipButton", OnEquipButtonClick);
            AddButtonClickListener("UnequipButton", OnUnequipButtonClick);
            
            // 初始隐藏按钮
            if (_useButton != null)
            {
                _useButton.gameObject.SetActive(false);
            }
            
            if (_equipButton != null)
            {
                _equipButton.gameObject.SetActive(false);
            }
            
            if (_unequipButton != null)
            {
                _unequipButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            // 注册道具使用事件
            GEventSystem.Instance.AddListener(GameEvents.ItemUsed, OnItemUsed);
        }

        /// <summary>
        /// 初始化道具数据
        /// </summary>
        private void InitItemData()
        {
            // 临时道具数据，实际应从数据管理器获取
            _itemDataDict.Clear();
            
            // 添加一些测试道具
            _itemDataDict.Add("item1", new ItemData 
            { 
                Id = "item1", 
                Name = "信笺", 
                Description = "一封来自南唐国的信笺，上面有着精美的花纹，内容似乎暗示着高平楼倒塌事件与南唐余孽有关。", 
                CanUse = true, 
                CanEquip = false 
            });
            
            _itemDataDict.Add("item2", new ItemData 
            { 
                Id = "item2", 
                Name = "木制机关", 
                Description = "在高平楼废墟中发现的精巧木制机关，做工精细，似乎可以用来操控建筑结构。背面刻有“大唐遗制”几个小字。", 
                CanUse = true, 
                CanEquip = false 
            });
            
            _itemDataDict.Add("item3", new ItemData 
            { 
                Id = "item3", 
                Name = "铜制香囊", 
                Description = "精美的铜制香囊，可以佩戴在身上。囊中装有安神的药草，据说能够增强注意力，有助于发现线索。", 
                CanUse = false, 
                CanEquip = true 
            });
            
            _itemDataDict.Add("item4", new ItemData 
            { 
                Id = "item4", 
                Name = "判官笔", 
                Description = "一支精美的黑色毛笔，笔杆上刻有”判官“二字。使用此笔记录的笔录据说更容易发现矛盾之处。", 
                CanUse = false, 
                CanEquip = true 
            });
        }

        /// <summary>
        /// 显示道具
        /// </summary>
        /// <param name="itemId">道具ID</param>
        public void ShowItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                Debug.LogError("道具ID不能为空");
                return;
            }
            
            _currentItemId = itemId;
            
            // 获取道具数据
            ItemData itemData = null;
            if (_itemDataDict.TryGetValue(itemId, out itemData))
            {
                // 设置道具图片
                // _itemImage.sprite = Resources.Load<Sprite>($"Items/{itemId}");
                
                // 设置道具名称
                _itemNameText.text = itemData.Name;
                
                // 设置道具描述
                _itemDescriptionText.text = itemData.Description;
                
                // 更新按钮状态
                UpdateButtons(itemId, itemData);
            }
            else
            {
                Debug.LogError($"未找到道具数据: {itemId}");
            }
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        /// <param name="itemId">道具ID</param>
        /// <param name="itemData">道具数据</param>
        private void UpdateButtons(string itemId, ItemData itemData)
        {
            // 检查是否已装备
            bool isEquipped = false;
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null)
            {
                isEquipped = dataManager.EquippedItems.Contains(itemId);
            }
            
            // 更新使用按钮
            if (_useButton != null)
            {
                _useButton.gameObject.SetActive(itemData.CanUse);
            }
            
            // 更新装备按钮
            if (_equipButton != null)
            {
                _equipButton.gameObject.SetActive(itemData.CanEquip && !isEquipped);
            }
            
            // 更新卸下按钮
            if (_unequipButton != null)
            {
                _unequipButton.gameObject.SetActive(itemData.CanEquip && isEquipped);
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void OnCloseButtonClick()
        {
            Debug.Log("点击关闭按钮");
            // 关闭道具弹窗界面
            CloseUI();
        }

        /// <summary>
        /// 使用按钮点击事件
        /// </summary>
        private void OnUseButtonClick()
        {
            Debug.Log($"点击使用按钮: {_currentItemId}");
            
            // 使用道具
            UseItem(_currentItemId);
        }

        /// <summary>
        /// 装备按钮点击事件
        /// </summary>
        private void OnEquipButtonClick()
        {
            Debug.Log($"点击装备按钮: {_currentItemId}");
            
            // 装备道具
            EquipItem(_currentItemId);
        }

        /// <summary>
        /// 卸下按钮点击事件
        /// </summary>
        private void OnUnequipButtonClick()
        {
            Debug.Log($"点击卸下按钮: {_currentItemId}");
            
            // 卸下道具
            UnequipItem(_currentItemId);
        }

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="itemId">道具ID</param>
        private void UseItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return;
            }
            
            // 获取道具数据
            if (_itemDataDict.TryGetValue(itemId, out ItemData itemData))
            {
                if (itemData.CanUse)
                {
                    // 使用道具
                    Debug.Log($"使用道具: {itemData.Name}");
                    
                    // 触发道具使用事件
                    GEventSystem.Instance.TriggerEvent(GameEvents.ItemUsed, itemId);
                    
                    // 关闭道具弹窗界面
                    CloseUI();
                }
            }
        }

        /// <summary>
        /// 装备道具
        /// </summary>
        /// <param name="itemId">道具ID</param>
        private void EquipItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return;
            }
            
            // 获取道具数据
            if (_itemDataDict.TryGetValue(itemId, out ItemData itemData))
            {
                if (itemData.CanEquip)
                {
                    // 装备道具
                    Debug.Log($"装备道具: {itemData.Name}");
                    
                    // 使用DataManager装备道具
                    DataManager dataManager = DataManager.Instance;
                    if (dataManager != null)
                    {
                        dataManager.EquipItem(itemId);
                    }
                    
                    // 更新按钮状态
                    UpdateButtons(itemId, itemData);
                }
            }
        }

        /// <summary>
        /// 卸下道具
        /// </summary>
        /// <param name="itemId">道具ID</param>
        private void UnequipItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return;
            }
            
            // 获取道具数据
            if (_itemDataDict.TryGetValue(itemId, out ItemData itemData))
            {
                if (itemData.CanEquip)
                {
                    // 卸下道具
                    Debug.Log($"卸下道具: {itemData.Name}");
                    
                    // 使用DataManager卸下道具
                    DataManager dataManager = DataManager.Instance;
                    if (dataManager != null)
                    {
                        dataManager.UnequipItem(itemId);
                    }
                    
                    // 更新按钮状态
                    UpdateButtons(itemId, itemData);
                }
            }
        }

        /// <summary>
        /// 道具使用事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnItemUsed(object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is string itemId)
            {
                Debug.Log($"道具使用事件回调: {itemId}");
                
                // 如果是当前显示的道具，则关闭界面
                if (itemId == _currentItemId)
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
                GEventSystem.Instance.RemoveListener(GameEvents.ItemUsed, OnItemUsed);
            }
        }
    }
} 