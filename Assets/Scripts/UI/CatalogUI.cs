using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 图鉴界面实现
    /// </summary>
    public class CatalogUI : BaseUI
    {
        // 一级页面
        private Button _backButton;         // 返回按钮
        private Transform _characterList;   // 角色列表容器
        
        // 二级页面
        private GameObject _detailPanel;    // 详情面板
        private Image _characterImage;      // 角色图片
        private Text _characterName;        // 角色名称
        private Text _characterDesc;        // 角色描述
        private Transform _itemList;        // 道具列表容器
        private Button _closeDetailButton;  // 关闭详情按钮
        
        // 角色项预制体
        private GameObject _characterItemPrefab;
        
        // 道具项预制体
        private GameObject _itemItemPrefab;
        
        // 当前选中的角色ID
        private string _currentCharacterId;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            // 初始化界面组件
            InitComponents();
            
            // 注册事件
            RegisterEvents();
            
            // 更新界面
            UpdateUI();
        }

        /// <summary>
        /// 初始化界面组件
        /// </summary>
        private void InitComponents()
        {
            // 一级页面
            _backButton = GetButton("BackButton");
            _characterList = transform.Find("CharacterList");
            
            // 二级页面
            _detailPanel = transform.Find("DetailPanel").gameObject;
            _characterImage = _detailPanel.transform.Find("CharacterImage").GetComponent<Image>();
            _characterName = _detailPanel.transform.Find("CharacterInfo/NameText").GetComponent<Text>();
            _characterDesc = _detailPanel.transform.Find("CharacterInfo/DescText").GetComponent<Text>();
            _itemList = _detailPanel.transform.Find("ItemList");
            _closeDetailButton = _detailPanel.transform.Find("CloseButton").GetComponent<Button>();
            
            // 加载预制体
            _characterItemPrefab = Resources.Load<GameObject>("Prefabs/UI/CharacterItem");
            _itemItemPrefab = Resources.Load<GameObject>("Prefabs/UI/ItemItem");
            
            // 添加按钮点击事件
            AddButtonClickListener("BackButton", OnBackButtonClick);
            _closeDetailButton.onClick.AddListener(OnCloseDetailButtonClick);
            
            // 初始显示一级页面
            _detailPanel.SetActive(false);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            // 注册角色解锁事件
            GEventSystem.Instance.AddListener(GameEvents.CharacterUnlocked, OnCharacterUnlocked);
            
            // 注册道具收集事件
            GEventSystem.Instance.AddListener(GameEvents.ItemCollected, OnItemCollected);
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        private void UpdateUI()
        {
            // 更新角色列表
            UpdateCharacterList();
        }

        /// <summary>
        /// 更新角色列表
        /// </summary>
        private void UpdateCharacterList()
        {
            if (_characterList == null)
            {
                return;
            }
            
            // 清空角色列表
            foreach (Transform child in _characterList)
            {
                Destroy(child.gameObject);
            }
            
            // 获取已解锁的角色
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null)
            {
                foreach (string characterId in dataManager.UnlockedCharacters)
                {
                    // 创建角色项
                    if (_characterItemPrefab != null)
                    {
                        GameObject characterObj = Instantiate(_characterItemPrefab, _characterList);
                        
                        // 设置角色数据
                        CharacterItem characterItem = characterObj.GetComponent<CharacterItem>();
                        if (characterItem != null)
                        {
                            characterItem.Init(characterId);
                            
                            // 添加点击事件
                            Button button = characterObj.GetComponent<Button>();
                            if (button != null)
                            {
                                button.onClick.AddListener(() => OnCharacterItemClick(characterId));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新角色详情
        /// </summary>
        /// <param name="characterId">角色ID</param>
        private void UpdateCharacterDetail(string characterId)
        {
            if (_detailPanel == null)
            {
                return;
            }
            
            _currentCharacterId = characterId;
            
            // 获取角色数据
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null && dataManager.CurrentSaveData.characterDataDict.TryGetValue(characterId, out var characterData))
            {
                // 设置角色图片
                // _characterImage.sprite = Resources.Load<Sprite>($"Characters/{characterId}");
                
                // 设置角色名称
                _characterName.text = characterData.name;
                
                // 设置角色描述
                _characterDesc.text = characterData.description;
                
                // 更新道具列表
                UpdateItemList(characterId);
                
                // 显示详情面板
                _detailPanel.SetActive(true);
            }
        }

        /// <summary>
        /// 更新道具列表
        /// </summary>
        /// <param name="characterId">角色ID</param>
        private void UpdateItemList(string characterId)
        {
            if (_itemList == null)
            {
                return;
            }
            
            // 清空道具列表
            foreach (Transform child in _itemList)
            {
                Destroy(child.gameObject);
            }
            
            // 获取角色相关道具
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null)
            {
                List<string> relatedItems = dataManager.GetCharacterRelatedItems(characterId);
                
                foreach (string itemId in relatedItems)
                {
                    // 检查道具是否已收集
                    if (dataManager.CollectedItems.Contains(itemId))
                    {
                        // 创建道具项
                        if (_itemItemPrefab != null)
                        {
                            GameObject itemObj = Instantiate(_itemItemPrefab, _itemList);
                            
                            // 设置道具数据
                            ItemItem itemItem = itemObj.GetComponent<ItemItem>();
                            if (itemItem != null)
                            {
                                itemItem.Init(itemId);
                                
                                // 添加点击事件
                                Button button = itemObj.GetComponent<Button>();
                                if (button != null)
                                {
                                    button.onClick.AddListener(() => OnItemItemClick(itemId));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回按钮点击事件
        /// </summary>
        private void OnBackButtonClick()
        {
            Debug.Log("点击返回按钮");
            // 关闭图鉴界面
            CloseUI();
        }

        /// <summary>
        /// 关闭详情按钮点击事件
        /// </summary>
        private void OnCloseDetailButtonClick()
        {
            Debug.Log("点击关闭详情按钮");
            // 隐藏详情面板
            _detailPanel.SetActive(false);
            _currentCharacterId = null;
        }

        /// <summary>
        /// 角色项点击事件
        /// </summary>
        /// <param name="characterId">角色ID</param>
        private void OnCharacterItemClick(string characterId)
        {
            Debug.Log($"点击角色项: {characterId}");
            // 更新角色详情
            UpdateCharacterDetail(characterId);
        }

        /// <summary>
        /// 道具项点击事件
        /// </summary>
        /// <param name="itemId">道具ID</param>
        private void OnItemItemClick(string itemId)
        {
            Debug.Log($"点击道具项: {itemId}");
            // 打开道具弹窗
            // ItemPopupUI itemPopupUI = UIManager.Instance.OpenUI(UIType.ItemPopupUI) as ItemPopupUI;
            // if (itemPopupUI != null)
            // {
            //     itemPopupUI.ShowItem(itemId);
            // }
        }

        /// <summary>
        /// 角色解锁事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnCharacterUnlocked(object[] parameters)
        {
            // 更新角色列表
            UpdateCharacterList();
        }

        /// <summary>
        /// 道具收集事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnItemCollected(object[] parameters)
        {
            // 如果当前有选中的角色，则更新道具列表
            if (!string.IsNullOrEmpty(_currentCharacterId))
            {
                UpdateItemList(_currentCharacterId);
            }
        }

        public override void OnShow()
        {
            base.OnShow();
            
            // 显示时更新界面
            UpdateUI();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            // 取消注册事件
            if (GEventSystem.Instance != null)
            {
                GEventSystem.Instance.RemoveListener(GameEvents.CharacterUnlocked, OnCharacterUnlocked);
                GEventSystem.Instance.RemoveListener(GameEvents.ItemCollected, OnItemCollected);
            }
        }
    }

    /// <summary>
    /// 角色项组件
    /// </summary>
    public class CharacterItem : MonoBehaviour
    {
        // 角色ID
        private string _characterId;
        
        // 角色头像
        private Image _avatar;
        
        // 角色名称
        private Text _nameText;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="characterId">角色ID</param>
        public void Init(string characterId)
        {
            _characterId = characterId;
            
            // 获取组件
            _avatar = transform.Find("Avatar").GetComponent<Image>();
            _nameText = transform.Find("NameText").GetComponent<Text>();
            
            // 设置数据
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null && dataManager.CurrentSaveData.characterDataDict.TryGetValue(characterId, out var characterData))
            {
                // 设置头像
                // _avatar.sprite = Resources.Load<Sprite>($"Characters/{characterId}_Avatar");
                
                // 设置名称
                _nameText.text = characterData.name;
            }
        }
    }

    /// <summary>
    /// 道具项组件
    /// </summary>
    public class ItemItem : MonoBehaviour
    {
        // 道具ID
        private string _itemId;
        
        // 道具图标
        private Image _icon;
        
        // 道具名称
        private Text _nameText;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="itemId">道具ID</param>
        public void Init(string itemId)
        {
            _itemId = itemId;
            
            // 获取组件
            _icon = transform.Find("Icon").GetComponent<Image>();
            _nameText = transform.Find("NameText").GetComponent<Text>();
            
            // 设置数据
            // 这里应该从道具管理器获取道具数据
            // 由于没有实际的道具系统，这里只是示例
            
            // 设置图标
            // _icon.sprite = Resources.Load<Sprite>($"Items/{itemId}");
            
            // 设置名称
            _nameText.text = itemId;
        }
    }
} 