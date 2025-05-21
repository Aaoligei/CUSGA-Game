using UnityEngine;
using System.Collections.Generic;
using Game.Core;
using Game.UI;

namespace Game.Managers
{
    /// <summary>
    /// UI界面类型枚举
    /// </summary>
    public enum UIType
    {
        None,
        StartGameUI,      // 开始游戏界面 
        MainUI,             // 主界面
        CatalogUI,          // 图鉴界面
        NotesUI,            // 笔录界面
        FactionUI,          // 阵营界面
        SaveUI,             // 存档界面
        InvestigationUI,    // 搜证界面
        ItemPopupUI,        // 道具弹窗界面
        CardPopupUI,        // 卡牌弹窗界面
        TipsPopupUI,        // 锦囊弹窗界面
        DialogUI,           // 对话界面
        CardGameUI,         // 卡牌游戏界面
        LoadingUI,          // 加载界面
        SettingsUI,         // 设置界面
        PauseUI,            // 暂停界面
        MapUI,              // 地图界面
        BagUI,              // 背包界面
        GetItemUI,          // 获得物品弹窗
    }

    /// <summary>
    /// UI层级枚举
    /// </summary>
    public enum UILayer
    {
        Background = 0,     // 背景层
        Normal = 1,         // 普通层
        Top = 2,            // 顶层
        System = 3,         // 系统层
        Popup = 4           // 弹窗层
    }

    /// <summary>
    /// UI管理器，负责游戏界面的创建、显示、隐藏和管理
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        // UI根节点
        private Transform _uiRoot;
        
        // 各层级的父节点
        private Transform[] _layerParents;
        
        // UI预制体路径
        private const string UI_PREFAB_PATH = "Prefabs/UI/";
        
        // 当前打开的UI列表
        private Dictionary<UIType, BaseUI> _openedUIs = new Dictionary<UIType, BaseUI>();
        
        // UI预制体缓存
        private Dictionary<UIType, GameObject> _uiPrefabCache = new Dictionary<UIType, GameObject>();
        
        // UI类型到预制体名称的映射
        private Dictionary<UIType, string> _uiTypeToName = new Dictionary<UIType, string>
        {
            { UIType.MainUI, "MainUI" },
            { UIType.CatalogUI, "图鉴界面" },
            { UIType.NotesUI, "笔录弹窗" },
            { UIType.FactionUI, "FactionUI" },
            { UIType.SaveUI, "存档弹窗" },
            { UIType.InvestigationUI, "InvestigationUI" },
            { UIType.ItemPopupUI, "ItemPopupUI" },
            { UIType.CardPopupUI, "卡牌弹窗" },
            { UIType.TipsPopupUI, "TipsPopupUI" },
            { UIType.DialogUI, "DialogUI" },
            { UIType.CardGameUI, "CardGameUI" },
            { UIType.LoadingUI, "LoadingUI" },
            { UIType.SettingsUI, "SettingsUI" },
            { UIType.PauseUI, "PauseUI" },
            { UIType.MapUI, "地图弹窗" },
            {UIType.BagUI,"背包弹窗" },
            {UIType.GetItemUI,"获取道具弹窗" }
        };
        
        // UI类型到层级的映射
        private Dictionary<UIType, UILayer> _uiTypeToLayer = new Dictionary<UIType, UILayer>
        {
            { UIType.MainUI, UILayer.Normal },
            { UIType.CatalogUI, UILayer.Normal },
            { UIType.NotesUI, UILayer.Popup },
            { UIType.FactionUI, UILayer.Normal },
            { UIType.SaveUI, UILayer.Popup },
            { UIType.InvestigationUI, UILayer.Normal },
            { UIType.ItemPopupUI, UILayer.Popup },
            { UIType.CardPopupUI, UILayer.Popup },
            { UIType.TipsPopupUI, UILayer.Popup },
            { UIType.DialogUI, UILayer.Top },
            { UIType.CardGameUI, UILayer.Normal },
            { UIType.LoadingUI, UILayer.System },
            { UIType.SettingsUI, UILayer.Popup },
            { UIType.PauseUI, UILayer.System },
            { UIType.MapUI, UILayer.Popup },
            { UIType.BagUI, UILayer.Popup },
            {UIType.GetItemUI, UILayer.Popup }
        };

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        /// <summary>
        /// 初始化UI管理器
        /// </summary>
        private void Init()
        {
            // 创建UI根节点
            GameObject uiRootObj = new GameObject("UI_Root");
            _uiRoot = uiRootObj.transform;
            _uiRoot.position = Vector3.zero;
            DontDestroyOnLoad(_uiRoot.gameObject);
            
            // 创建各层级的父节点
            _layerParents = new Transform[System.Enum.GetValues(typeof(UILayer)).Length];
            for (int i = 0; i < _layerParents.Length; i++)
            {
                GameObject layerObj = new GameObject($"Layer_{(UILayer)i}");
                _layerParents[i] = layerObj.transform;
                _layerParents[i].SetParent(_uiRoot, false);
            }
            
            // 注册事件
            GEventSystem.Instance.AddListener(GameEvents.OpenUI, OnOpenUIEvent);
            GEventSystem.Instance.AddListener(GameEvents.CloseUI, OnCloseUIEvent);
            
            Debug.Log("UI管理器初始化完成");
        }

        /// <summary>
        /// 处理打开UI事件
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnOpenUIEvent(object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is string uiTypeStr && 
                System.Enum.TryParse(uiTypeStr, out UIType uiType))
            {
                OpenUI(uiType);
            }
        }

        /// <summary>
        /// 处理关闭UI事件
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnCloseUIEvent(object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is string uiTypeStr && 
                System.Enum.TryParse(uiTypeStr, out UIType uiType))
            {
                CloseUI(uiType);
            }
        }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <returns>打开的UI实例</returns>
        public BaseUI OpenUI(UIType uiType)
        {
            // 如果已经打开，则直接返回
            if (_openedUIs.TryGetValue(uiType, out BaseUI existingUI) && existingUI != null)
            {
                existingUI.gameObject.SetActive(true);
                existingUI.OnShow();
                return existingUI;
            }
            
            // 创建UI
            BaseUI newUI = CreateUI(uiType);
            if (newUI == null)
            {
                Debug.LogError($"创建UI失败: {uiType}");
                return null;
            }
            
            // 添加到已打开列表
            _openedUIs[uiType] = newUI;
            
            // 显示UI
            newUI.gameObject.SetActive(true);
            newUI.OnShow();
            
            Debug.Log($"打开UI: {uiType}");
            
            return newUI;
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="uiType">UI类型</param>
        public void CloseUI(UIType uiType)
        {
            if (_openedUIs.TryGetValue(uiType, out BaseUI ui) && ui != null)
            {
                ui.OnHide();
                ui.gameObject.SetActive(false);
                
                Debug.Log($"关闭UI: {uiType}");
            }
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <returns>创建的UI实例</returns>
        private BaseUI CreateUI(UIType uiType)
        {
            // 获取预制体名称
            if (!_uiTypeToName.TryGetValue(uiType, out string prefabName))
            {
                Debug.LogError($"未找到UI类型对应的预制体名称: {uiType}");
                return null;
            }
            
            // 获取UI层级
            UILayer layer = UILayer.Normal;
            _uiTypeToLayer.TryGetValue(uiType, out layer);
            
            // 从缓存中获取预制体
            GameObject prefab = null;
            if (!_uiPrefabCache.TryGetValue(uiType, out prefab))
            {
                // 加载预制体
                prefab = Resources.Load<GameObject>(UI_PREFAB_PATH + prefabName);
                if (prefab == null)
                {
                    Debug.LogError($"加载UI预制体失败: {prefabName}");
                    return null;
                }
                
                // 缓存预制体
                _uiPrefabCache[uiType] = prefab;
            }
            
            // 实例化UI
            GameObject uiObj = Instantiate(prefab, _layerParents[(int)layer]);
            uiObj.name = prefabName;
            
            // 获取BaseUI组件
            BaseUI baseUI = uiObj.GetComponent<BaseUI>();
            if (baseUI == null)
            {
                Debug.LogError($"UI预制体没有BaseUI组件: {prefabName}");
                Destroy(uiObj);
                return null;
            }
            
            // 初始化UI
            baseUI.Init(uiType);
            
            return baseUI;
        }

        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <param name="uiType">UI类型</param>
        public void DestroyUI(UIType uiType)
        {
            if (_openedUIs.TryGetValue(uiType, out BaseUI ui) && ui != null)
            {
                // 从已打开列表中移除
                _openedUIs.Remove(uiType);
                
                // 销毁UI
                ui.OnDestroy();
                Destroy(ui.gameObject);
                
                Debug.Log($"销毁UI: {uiType}");
            }
        }

        /// <summary>
        /// 销毁所有UI
        /// </summary>
        public void DestroyAllUI()
        {
            foreach (var kv in _openedUIs)
            {
                if (kv.Value != null)
                {
                    kv.Value.OnDestroy();
                    Destroy(kv.Value.gameObject);
                }
            }
            
            _openedUIs.Clear();
            
            Debug.Log("销毁所有UI");
        }

        /// <summary>
        /// 获取已打开的UI
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <returns>UI实例</returns>
        public BaseUI GetOpenedUI(UIType uiType)
        {
            _openedUIs.TryGetValue(uiType, out BaseUI ui);
            return ui;
        }

        /// <summary>
        /// 隐藏所有UI
        /// </summary>
        public void HideAllUI()
        {
            foreach (var kv in _openedUIs)
            {
                if (kv.Value != null)
                {
                    kv.Value.OnHide();
                    kv.Value.gameObject.SetActive(false);
                }
            }
            
            Debug.Log("隐藏所有UI");
        }

        /// <summary>
        /// 显示所有UI
        /// </summary>
        public void ShowAllUI()
        {
            foreach (var kv in _openedUIs)
            {
                if (kv.Value != null)
                {
                    kv.Value.gameObject.SetActive(true);
                    kv.Value.OnShow();
                }
            }
            
            Debug.Log("显示所有UI");
        }
    }
} 