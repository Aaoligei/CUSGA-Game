using UnityEngine;
using UnityEngine.UI;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 主界面（我的界面）实现
    /// </summary>
    public class MainUI : BaseUI
    {
        
        // 左侧区域
        private Text _timeText;          // 时间文本
        private Text _actionPointsText;  // 行动点数文本
        [SerializeField] private Button _mapButton;       // 地图按钮

        // 右侧区域
        [SerializeField] private Button _catalogButton;   // 图鉴按钮
        private Button _saveButton;      // 存档按钮
        private Button _notesButton;     // 笔录按钮
        private Button _cardButton;     // 卡牌按钮
        private Button _tipButton;       // 锦囊按钮
        private Button _settingsButton;  // 设置按钮
        
        // 底部区域
        private Transform _itemContainer; // 道具容器

        private void Start()
        {
            Init(UIType.MainUI);
        }

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
            // 顶部区域
            _timeText = GetText("TimeText");
            _actionPointsText = GetText("ActionPointsText");
            _mapButton = GetButton("地图按钮");
            _saveButton = GetButton("SaveButton");
            
            // 中央区域
            _catalogButton = GetButton("CatalogButton");
            _notesButton = GetButton("笔录按钮");
            _cardButton = GetButton("FactionButton");
            _settingsButton = GetButton("SettingsButton");
            
            // 底部区域
            _itemContainer = transform.Find("ItemContainer");
            
            // 添加按钮点击事件
            AddButtonClickListener("地图按钮", OnMapButtonClick);
            AddButtonClickListener("SaveButton", OnSaveButtonClick);
            AddButtonClickListener("CatalogButton", OnCatalogButtonClick);
            AddButtonClickListener("笔录按钮", OnNotesButtonClick);
            AddButtonClickListener("FactionButton", OnFactionButtonClick);
            AddButtonClickListener("SettingsButton", OnSettingsButtonClick);
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
                }
            }
        }

        /// <summary>
        /// 地图按钮点击事件
        /// </summary>
        private void OnMapButtonClick()
        {
            Debug.Log("点击地图按钮");
            // 打开地图界面
            UIManager.Instance.OpenUI(UIType.MapUI);
        }

        /// <summary>
        /// 存档按钮点击事件
        /// </summary>
        private void OnSaveButtonClick()
        {
            Debug.Log("点击存档按钮");
            // 打开存档界面
            UIManager.Instance.OpenUI(UIType.SaveUI);
        }

        /// <summary>
        /// 图鉴按钮点击事件
        /// </summary>
        private void OnCatalogButtonClick()
        {
            Debug.Log("点击图鉴按钮");
            // 打开图鉴界面
            UIManager.Instance.OpenUI(UIType.CatalogUI);
        }

        /// <summary>
        /// 笔录按钮点击事件
        /// </summary>
        private void OnNotesButtonClick()
        {
            Debug.Log("点击笔录按钮");
            // 打开笔录界面
            UIManager.Instance.OpenUI(UIType.NotesUI);
        }

        /// <summary>
        /// 阵营按钮点击事件
        /// </summary>
        private void OnFactionButtonClick()
        {
            Debug.Log("点击阵营按钮");
            // 打开阵营界面
            UIManager.Instance.OpenUI(UIType.FactionUI);
        }

        /// <summary>
        /// 设置按钮点击事件
        /// </summary>
        private void OnSettingsButtonClick()
        {
            Debug.Log("点击设置按钮");
            // 打开设置界面
            UIManager.Instance.OpenUI(UIType.SettingsUI);
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
        /// 界面销毁回调
        /// </summary>
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
            }
        }

        public void ShowOutline(Image image)
        {
            image.gameObject.SetActive(true);
        }

        public void HideOutline(Image image)
        {
            image.gameObject.SetActive(false);
        }
    }
} 