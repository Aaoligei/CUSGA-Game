using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 存档界面实现
    /// </summary>
    public class SaveUI : BaseUI
    {
        // 界面组件
        private Button _backButton;                // 返回按钮
        private Button _newSaveButton;             // 新建存档按钮
        private Transform _saveItemContainer;      // 存档项容器
        private GameObject _confirmPanel;          // 确认面板
        private Text _confirmText;                 // 确认文本
        private Button _confirmYesButton;          // 确认是按钮
        private Button _confirmNoButton;           // 确认否按钮
        
        // 存档项预制体
        private GameObject _saveItemPrefab;
        
        // 当前选中的存档索引
        private int _selectedSaveIndex = -1;
        
        // 存档确认操作类型
        private enum ConfirmType
        {
            None,
            Load,   // 加载存档
            Save,   // 保存存档
            Delete  // 删除存档
        }
        
        // 当前确认操作类型
        private ConfirmType _currentConfirmType = ConfirmType.None;
        
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
            // 获取界面组件
            _backButton = GetButton("取消按钮");
            _newSaveButton = GetButton("新存档按钮");
            _saveItemContainer = transform.Find("SaveItemContainer");
            _confirmPanel = transform.Find("ConfirmPanel").gameObject;
            _confirmText = _confirmPanel.transform.Find("ConfirmText").GetComponent<Text>();
            _confirmYesButton = _confirmPanel.transform.Find("YesButton").GetComponent<Button>();
            _confirmNoButton = _confirmPanel.transform.Find("NoButton").GetComponent<Button>();
            
            // 加载预制体
            _saveItemPrefab = Resources.Load<GameObject>("Prefabs/UI/SaveItem");
            
            // 添加按钮点击事件
            AddButtonClickListener("取消按钮", OnBackButtonClick);
            AddButtonClickListener("新存档按钮", OnNewSaveButtonClick);
            _confirmYesButton.onClick.AddListener(OnConfirmYesButtonClick);
            _confirmNoButton.onClick.AddListener(OnConfirmNoButtonClick);
            
            // 初始隐藏确认面板
            _confirmPanel.SetActive(false);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            // 注册数据保存事件
            GEventSystem.Instance.AddListener(GameEvents.DataSaved, OnDataSaved);
            
            // 注册数据加载事件
            GEventSystem.Instance.AddListener(GameEvents.DataLoaded, OnDataLoaded);
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        private void UpdateUI()
        {
            // 更新存档列表
            UpdateSaveList();
        }

        /// <summary>
        /// 更新存档列表
        /// </summary>
        private void UpdateSaveList()
        {
            if (_saveItemContainer == null)
            {
                return;
            }
            
            // 清空存档列表
            foreach (Transform child in _saveItemContainer)
            {
                Destroy(child.gameObject);
            }
            
            // 获取存档数据
            // 这里应该从保存系统获取所有存档信息，但由于当前框架只支持单一存档
            // 所以这里只是判断是否存在存档文件
            if (System.IO.File.Exists(DataManager.Instance.SaveFilePath))
            {
                // 创建存档项
                if (_saveItemPrefab != null)
                {
                    GameObject saveItemObj = Instantiate(_saveItemPrefab, _saveItemContainer);
                    
                    // 设置存档数据
                    SaveItem saveItem = saveItemObj.GetComponent<SaveItem>();
                    if (saveItem != null)
                    {
                        saveItem.Init(0, "存档 #1", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "当前游戏进度：" + (DataManager.Instance.CurrentSaveData.gameProgress * 100) + "%");
                        
                        // 添加点击事件
                        Button loadButton = saveItemObj.transform.Find("加载按钮").GetComponent<Button>();
                        Button saveButton = saveItemObj.transform.Find("保存按钮").GetComponent<Button>();
                        Button deleteButton = saveItemObj.transform.Find("DeleteButton").GetComponent<Button>();
                        
                        loadButton.onClick.AddListener(() => OnLoadButtonClick(0));
                        saveButton.onClick.AddListener(() => OnSaveButtonClick(0));
                        deleteButton.onClick.AddListener(() => OnDeleteButtonClick(0));
                    }
                }
            }
        }

        /// <summary>
        /// 显示确认面板
        /// </summary>
        /// <param name="message">确认消息</param>
        /// <param name="type">确认类型</param>
        private void ShowConfirmPanel(string message, ConfirmType type)
        {
            if (_confirmPanel == null)
            {
                return;
            }
            
            // 设置确认消息
            _confirmText.text = message;
            
            // 设置确认类型
            _currentConfirmType = type;
            
            // 显示确认面板
            _confirmPanel.SetActive(true);
        }

        /// <summary>
        /// 隐藏确认面板
        /// </summary>
        private void HideConfirmPanel()
        {
            if (_confirmPanel == null)
            {
                return;
            }
            
            // 隐藏确认面板
            _confirmPanel.SetActive(false);
            
            // 重置确认类型
            _currentConfirmType = ConfirmType.None;
        }

        /// <summary>
        /// 返回按钮点击事件
        /// </summary>
        private void OnBackButtonClick()
        {
            Debug.Log("点击返回按钮");
            // 关闭存档界面
            CloseUI();
        }

        /// <summary>
        /// 新建存档按钮点击事件
        /// </summary>
        private void OnNewSaveButtonClick()
        {
            Debug.Log("点击新建存档按钮");
            // 显示确认面板
            ShowConfirmPanel("确定要创建新存档吗？当前游戏进度将会丢失！", ConfirmType.Save);
        }

        /// <summary>
        /// 加载按钮点击事件
        /// </summary>
        /// <param name="saveIndex">存档索引</param>
        private void OnLoadButtonClick(int saveIndex)
        {
            Debug.Log($"点击加载按钮：存档索引 {saveIndex}");
            // 设置当前选中的存档索引
            _selectedSaveIndex = saveIndex;
            // 显示确认面板
            ShowConfirmPanel("确定要加载该存档吗？当前游戏进度将会丢失！", ConfirmType.Load);
        }

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        /// <param name="saveIndex">存档索引</param>
        private void OnSaveButtonClick(int saveIndex)
        {
            Debug.Log($"点击保存按钮：存档索引 {saveIndex}");
            // 设置当前选中的存档索引
            _selectedSaveIndex = saveIndex;
            // 显示确认面板
            ShowConfirmPanel("确定要保存游戏吗？当前存档将会被覆盖！", ConfirmType.Save);
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        /// <param name="saveIndex">存档索引</param>
        private void OnDeleteButtonClick(int saveIndex)
        {
            Debug.Log($"点击删除按钮：存档索引 {saveIndex}");
            // 设置当前选中的存档索引
            _selectedSaveIndex = saveIndex;
            // 显示确认面板
            ShowConfirmPanel("确定要删除该存档吗？删除后将无法恢复！", ConfirmType.Delete);
        }

        /// <summary>
        /// 确认是按钮点击事件
        /// </summary>
        private void OnConfirmYesButtonClick()
        {
            Debug.Log("点击确认是按钮");
            
            // 根据确认类型执行相应操作
            switch (_currentConfirmType)
            {
                case ConfirmType.Load:
                    LoadSaveData();
                    break;
                case ConfirmType.Save:
                    SaveGameData();
                    break;
                case ConfirmType.Delete:
                    DeleteSaveData();
                    break;
            }
            
            // 隐藏确认面板
            HideConfirmPanel();
        }

        /// <summary>
        /// 确认否按钮点击事件
        /// </summary>
        private void OnConfirmNoButtonClick()
        {
            Debug.Log("点击确认否按钮");
            
            // 隐藏确认面板
            HideConfirmPanel();
            
            // 重置选中的存档索引
            _selectedSaveIndex = -1;
        }

        /// <summary>
        /// 加载存档数据
        /// </summary>
        private void LoadSaveData()
        {
            if (_selectedSaveIndex >= 0)
            {
                Debug.Log($"加载存档：索引 {_selectedSaveIndex}");
                
                // 使用DataManager加载存档
                DataManager.Instance.LoadGame();
                
                // 关闭存档界面
                CloseUI();
            }
        }

        /// <summary>
        /// 保存游戏数据
        /// </summary>
        private void SaveGameData()
        {
            if (_currentConfirmType == ConfirmType.Save && _selectedSaveIndex < 0)
            {
                // 新建存档
                Debug.Log("创建新存档");
                
                // 使用DataManager创建新存档
                DataManager.Instance.CreateNewSaveData();
            }
            else if (_selectedSaveIndex >= 0)
            {
                // 保存到现有存档
                Debug.Log($"保存到存档：索引 {_selectedSaveIndex}");
            }
            
            // 使用DataManager保存游戏
            DataManager.Instance.SaveGame();
        }

        /// <summary>
        /// 删除存档数据
        /// </summary>
        private void DeleteSaveData()
        {
            if (_selectedSaveIndex >= 0)
            {
                Debug.Log($"删除存档：索引 {_selectedSaveIndex}");
                
                // 使用DataManager删除存档
                DataManager.Instance.DeleteSaveData();
                
                // 更新存档列表
                UpdateSaveList();
            }
        }

        /// <summary>
        /// 数据保存事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnDataSaved(object[] parameters)
        {
            Debug.Log("数据保存事件回调");
            
            // 更新存档列表
            UpdateSaveList();
        }

        /// <summary>
        /// 数据加载事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnDataLoaded(object[] parameters)
        {
            Debug.Log("数据加载事件回调");
            
            // 更新存档列表
            UpdateSaveList();
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
                GEventSystem.Instance.RemoveListener(GameEvents.DataSaved, OnDataSaved);
                GEventSystem.Instance.RemoveListener(GameEvents.DataLoaded, OnDataLoaded);
            }
        }
    }

    /// <summary>
    /// 存档项组件
    /// </summary>
    public class SaveItem : MonoBehaviour
    {
        // 存档索引
        private int _saveIndex;
        
        // 存档标题
        private Text _titleText;
        
        // 存档时间
        private Text _timeText;
        
        // 存档描述
        private Text _descText;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="saveIndex">存档索引</param>
        /// <param name="title">存档标题</param>
        /// <param name="time">存档时间</param>
        /// <param name="description">存档描述</param>
        public void Init(int saveIndex, string title, string time, string description)
        {
            _saveIndex = saveIndex;
            
            // 获取组件
            _titleText = transform.Find("TitleText").GetComponent<Text>();
            _timeText = transform.Find("TimeText").GetComponent<Text>();
            _descText = transform.Find("DescText").GetComponent<Text>();
            
            // 设置数据
            _titleText.text = title;
            _timeText.text = time;
            _descText.text = description;
        }
    }
} 