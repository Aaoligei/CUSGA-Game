using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 搜证界面实现
    /// </summary>
    public class InvestigationUI : BaseUI
    {
        // 顶部区域
        private Button _mapButton;           // 地图按钮
        private Text _timeText;              // 时间文本
        private Text _actionPointsText;      // 行动点数文本
        
        // 场景切换按钮
        private Button _prevSceneButton;     // 上一个场景按钮
        private Button _nextSceneButton;     // 下一个场景按钮
        
        // 道具栏
        private Transform _itemContainer;    // 道具容器
        
        // 折叠菜单
        private Button _menuButton;          // 菜单按钮
        private GameObject _menuPanel;       // 菜单面板
        private Button _notesButton;         // 笔录按钮
        private Button _tipsButton;          // 锦囊按钮
        private Button _cardsButton;         // 卡牌按钮
        private Button _saveButton;          // 存档按钮
        
        // 场景热点
        private Transform _hotspotContainer; // 热点容器
        
        // 当前场景名称
        private string _currentSceneName;
        
        // 当前场景索引
        private int _currentSceneIndex;
        
        // 场景列表
        private List<string> _sceneList = new List<string>();

        protected override void OnInit()
        {
            base.OnInit();
            
            // 初始化界面组件
            InitComponents();
            
            // 注册事件
            RegisterEvents();
            
            // 初始化场景列表
            InitSceneList();
            
            // 更新界面
            UpdateUI();
        }

        /// <summary>
        /// 初始化界面组件
        /// </summary>
        private void InitComponents()
        {
            // 顶部区域
            _mapButton = GetButton("MapButton");
            _timeText = GetText("TimeText");
            _actionPointsText = GetText("ActionPointsText");
            
            // 场景切换按钮
            _prevSceneButton = GetButton("PrevSceneButton");
            _nextSceneButton = GetButton("NextSceneButton");
            
            // 道具栏
            _itemContainer = transform.Find("ItemContainer");
            
            // 折叠菜单
            _menuButton = GetButton("MenuButton");
            _menuPanel = transform.Find("MenuPanel").gameObject;
            _notesButton = GetButton("NotesButton");
            _tipsButton = GetButton("TipsButton");
            _cardsButton = GetButton("CardsButton");
            _saveButton = GetButton("SaveButton");
            
            // 热点容器
            _hotspotContainer = transform.Find("HotspotContainer");
            
            // 添加按钮点击事件
            AddButtonClickListener("MapButton", OnMapButtonClick);
            AddButtonClickListener("PrevSceneButton", OnPrevSceneButtonClick);
            AddButtonClickListener("NextSceneButton", OnNextSceneButtonClick);
            AddButtonClickListener("MenuButton", OnMenuButtonClick);
            AddButtonClickListener("NotesButton", OnNotesButtonClick);
            AddButtonClickListener("TipsButton", OnTipsButtonClick);
            AddButtonClickListener("CardsButton", OnCardsButtonClick);
            AddButtonClickListener("SaveButton", OnSaveButtonClick);
            
            // 初始隐藏菜单面板
            _menuPanel.SetActive(false);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            // 注册游戏时间改变事件
            GameManager.Instance.OnGameTimeChanged += OnGameTimeChanged;
            
            // 注册行动点改变事件
            GameManager.Instance.OnActionPointsChanged += OnActionPointsChanged;
            
            // 注册道具收集事件
            GEventSystem.Instance.AddListener(GameEvents.ItemCollected, OnItemCollected);
            
            // 注册道具使用事件
            GEventSystem.Instance.AddListener(GameEvents.ItemUsed, OnItemUsed);
        }

        /// <summary>
        /// 初始化场景列表
        /// </summary>
        private void InitSceneList()
        {
            // 初始化搜证场景列表
            // 这里应该从配置文件或游戏管理器获取场景列表
            // 由于没有实际的场景系统，这里只是示例
            _sceneList.Add("RoomScene");
            _sceneList.Add("CorridorScene");
            _sceneList.Add("GardenScene");
            _sceneList.Add("LibraryScene");
            
            // 设置当前场景
            _currentSceneIndex = 0;
            _currentSceneName = _sceneList[_currentSceneIndex];
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        private void UpdateUI()
        {
            // 更新时间文本
            UpdateTimeText(GameManager.Instance.GameTime);
            
            // 更新行动点数文本
            UpdateActionPointsText(GameManager.Instance.ActionPoints);
            
            // 更新道具栏
            UpdateItemContainer();
            
            // 更新热点
            UpdateHotspots();
            
            // 更新场景切换按钮状态
            UpdateSceneButtonState();
        }

        /// <summary>
        /// 更新时间文本
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
        private void UpdateTimeText(int gameTime)
        {
            if (_timeText != null)
            {
                // 根据游戏时间设置对应的时间段
                string timeStr = "";
                
                switch (gameTime)
                {
                    case 0:
                        timeStr = "清晨";
                        break;
                    case 1:
                        timeStr = "上午";
                        break;
                    case 2:
                        timeStr = "中午";
                        break;
                    case 3:
                        timeStr = "下午";
                        break;
                    case 4:
                        timeStr = "傍晚";
                        break;
                    case 5:
                        timeStr = "晚上";
                        break;
                    default:
                        timeStr = "未知";
                        break;
                }
                
                _timeText.text = $"时间：{timeStr}";
            }
        }

        /// <summary>
        /// 更新行动点数文本
        /// </summary>
        /// <param name="actionPoints">行动点数</param>
        private void UpdateActionPointsText(int actionPoints)
        {
            if (_actionPointsText != null)
            {
                _actionPointsText.text = $"行动点：{actionPoints}";
            }
        }

        /// <summary>
        /// 更新道具栏
        /// </summary>
        private void UpdateItemContainer()
        {
            if (_itemContainer == null)
            {
                return;
            }
            
            // 清空道具栏
            foreach (Transform child in _itemContainer)
            {
                Destroy(child.gameObject);
            }
            
            // 获取已装备的道具
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null)
            {
                foreach (string itemId in dataManager.EquippedItems)
                {
                    // 创建道具图标
                    // 这里应该从预制体实例化道具图标，并设置对应的图片和数据
                    // 由于没有实际的道具系统，这里只是示例
                    
                    // GameObject itemObj = Instantiate(itemPrefab, _itemContainer);
                    // ItemIcon itemIcon = itemObj.GetComponent<ItemIcon>();
                    // itemIcon.Init(itemId);
                    // 
                    // 添加点击事件
                    // Button button = itemObj.GetComponent<Button>();
                    // if (button != null)
                    // {
                    //     button.onClick.AddListener(() => OnItemClick(itemId));
                    // }
                }
            }
        }

        /// <summary>
        /// 更新热点
        /// </summary>
        private void UpdateHotspots()
        {
            if (_hotspotContainer == null)
            {
                return;
            }
            
            // 清空热点容器
            foreach (Transform child in _hotspotContainer)
            {
                Destroy(child.gameObject);
            }
            
            // 根据当前场景加载对应的热点
            // 这里应该从配置文件或游戏管理器获取热点数据
            // 由于没有实际的热点系统，这里只是示例
            
            // 假设热点预制体已加载
            // GameObject hotspotPrefab = Resources.Load<GameObject>("Prefabs/UI/Hotspot");
            // 
            // 根据当前场景创建热点
            // List<HotspotData> hotspots = GetHotspotsForScene(_currentSceneName);
            // foreach (HotspotData hotspotData in hotspots)
            // {
            //     GameObject hotspotObj = Instantiate(hotspotPrefab, _hotspotContainer);
            //     Hotspot hotspot = hotspotObj.GetComponent<Hotspot>();
            //     hotspot.Init(hotspotData);
            //     
            //     // 设置位置
            //     hotspotObj.transform.localPosition = hotspotData.position;
            //     
            //     // 添加点击事件
            //     Button button = hotspotObj.GetComponent<Button>();
            //     if (button != null)
            //     {
            //         button.onClick.AddListener(() => OnHotspotClick(hotspotData));
            //     }
            // }
        }

        /// <summary>
        /// 更新场景切换按钮状态
        /// </summary>
        private void UpdateSceneButtonState()
        {
            // 设置上一个场景按钮的可用状态
            _prevSceneButton.interactable = _currentSceneIndex > 0;
            
            // 设置下一个场景按钮的可用状态
            _nextSceneButton.interactable = _currentSceneIndex < _sceneList.Count - 1;
        }

        /// <summary>
        /// 切换到上一个场景
        /// </summary>
        private void SwitchToPrevScene()
        {
            if (_currentSceneIndex > 0)
            {
                _currentSceneIndex--;
                _currentSceneName = _sceneList[_currentSceneIndex];
                
                // 更新界面
                UpdateHotspots();
                UpdateSceneButtonState();
                
                Debug.Log($"切换到场景：{_currentSceneName}");
            }
        }

        /// <summary>
        /// 切换到下一个场景
        /// </summary>
        private void SwitchToNextScene()
        {
            if (_currentSceneIndex < _sceneList.Count - 1)
            {
                _currentSceneIndex++;
                _currentSceneName = _sceneList[_currentSceneIndex];
                
                // 更新界面
                UpdateHotspots();
                UpdateSceneButtonState();
                
                Debug.Log($"切换到场景：{_currentSceneName}");
            }
        }

        /// <summary>
        /// 地图按钮点击事件
        /// </summary>
        private void OnMapButtonClick()
        {
            Debug.Log("点击地图按钮");
            // 打开地图界面
            // UIManager.Instance.OpenUI(UIType.MapUI);
        }

        /// <summary>
        /// 上一个场景按钮点击事件
        /// </summary>
        private void OnPrevSceneButtonClick()
        {
            Debug.Log("点击上一个场景按钮");
            SwitchToPrevScene();
        }

        /// <summary>
        /// 下一个场景按钮点击事件
        /// </summary>
        private void OnNextSceneButtonClick()
        {
            Debug.Log("点击下一个场景按钮");
            SwitchToNextScene();
        }

        /// <summary>
        /// 菜单按钮点击事件
        /// </summary>
        private void OnMenuButtonClick()
        {
            Debug.Log("点击菜单按钮");
            // 切换菜单面板的显示状态
            _menuPanel.SetActive(!_menuPanel.activeSelf);
        }

        /// <summary>
        /// 笔录按钮点击事件
        /// </summary>
        private void OnNotesButtonClick()
        {
            Debug.Log("点击笔录按钮");
            // 打开笔录界面
            UIManager.Instance.OpenUI(UIType.NotesUI);
            // 隐藏菜单面板
            _menuPanel.SetActive(false);
        }

        /// <summary>
        /// 锦囊按钮点击事件
        /// </summary>
        private void OnTipsButtonClick()
        {
            Debug.Log("点击锦囊按钮");
            // 打开锦囊界面
            UIManager.Instance.OpenUI(UIType.TipsPopupUI);
            // 隐藏菜单面板
            _menuPanel.SetActive(false);
        }

        /// <summary>
        /// 卡牌按钮点击事件
        /// </summary>
        private void OnCardsButtonClick()
        {
            Debug.Log("点击卡牌按钮");
            // 打开卡牌界面
            UIManager.Instance.OpenUI(UIType.CardPopupUI);
            // 隐藏菜单面板
            _menuPanel.SetActive(false);
        }

        /// <summary>
        /// 存档按钮点击事件
        /// </summary>
        private void OnSaveButtonClick()
        {
            Debug.Log("点击存档按钮");
            // 打开存档界面
            UIManager.Instance.OpenUI(UIType.SaveUI);
            // 隐藏菜单面板
            _menuPanel.SetActive(false);
        }

        /// <summary>
        /// 道具点击事件
        /// </summary>
        /// <param name="itemId">道具ID</param>
        private void OnItemClick(string itemId)
        {
            Debug.Log($"点击道具：{itemId}");
            // 处理道具点击逻辑
            // ...
        }

        /// <summary>
        /// 热点点击事件
        /// </summary>
        /// <param name="hotspotData">热点数据</param>
        private void OnHotspotClick(object hotspotData)
        {
            Debug.Log($"点击热点：{hotspotData}");
            // 处理热点点击逻辑
            // ...
        }

        /// <summary>
        /// 游戏时间改变事件回调
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
        private void OnGameTimeChanged(int gameTime)
        {
            UpdateTimeText(gameTime);
        }

        /// <summary>
        /// 行动点改变事件回调
        /// </summary>
        /// <param name="actionPoints">行动点数</param>
        private void OnActionPointsChanged(int actionPoints)
        {
            UpdateActionPointsText(actionPoints);
        }

        /// <summary>
        /// 道具收集事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnItemCollected(object[] parameters)
        {
            // 更新道具栏
            UpdateItemContainer();
        }

        /// <summary>
        /// 道具使用事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnItemUsed(object[] parameters)
        {
            // 更新道具栏
            UpdateItemContainer();
            
            // 更新热点
            UpdateHotspots();
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
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameTimeChanged -= OnGameTimeChanged;
                GameManager.Instance.OnActionPointsChanged -= OnActionPointsChanged;
            }
            
            if (GEventSystem.Instance != null)
            {
                GEventSystem.Instance.RemoveListener(GameEvents.ItemCollected, OnItemCollected);
                GEventSystem.Instance.RemoveListener(GameEvents.ItemUsed, OnItemUsed);
            }
        }
    }

    /// <summary>
    /// 热点数据类
    /// </summary>
    [System.Serializable]
    public class HotspotData
    {
        public string id;          // 热点ID
        public string itemId;      // 关联的道具ID
        public Vector2 position;   // 位置
        public string description; // 描述
        public bool isCollected;   // 是否已收集
    }

    /// <summary>
    /// 热点组件
    /// </summary>
    public class Hotspot : MonoBehaviour
    {
        // 热点数据
        private HotspotData _data;
        
        // 热点图标
        private Image _icon;
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="data">热点数据</param>
        public void Init(HotspotData data)
        {
            _data = data;
            
            // 获取组件
            _icon = transform.GetComponent<Image>();
            
            // 根据是否已收集设置图标
            if (_data.isCollected)
            {
                // 设置为已收集的图标
                // _icon.sprite = Resources.Load<Sprite>("UI/hotspot_collected");
                _icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 变暗
            }
            else
            {
                // 设置为未收集的图标
                // _icon.sprite = Resources.Load<Sprite>("UI/hotspot_normal");
                _icon.color = Color.white;
            }
        }
    }
} 