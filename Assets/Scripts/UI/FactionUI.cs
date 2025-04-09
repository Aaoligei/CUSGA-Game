using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 阵营界面实现
    /// </summary>
    public class FactionUI : BaseUI
    {
        // 界面组件
        private Button _backButton;                    // 返回按钮
        private Transform _characterContainer;         // 角色容器
        private Transform _tangFactionSlot;            // 南唐阵营槽位
        private Transform _songFactionSlot;            // 北宋阵营槽位
        private Button _submitButton;                  // 提交按钮
        
        // 当前槽位中的角色
        private Dictionary<string, string> _slotCharacters = new Dictionary<string, string>();
        
        // 角色卡片预制体
        private GameObject _characterCardPrefab;
        
        // 拖拽相关
        private GameObject _dragObject;                // 当前拖拽的对象
        private Vector3 _dragOriginalPosition;         // 拖拽对象的原始位置
        private Transform _dragOriginalParent;         // 拖拽对象的原始父节点

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
            // 获取组件
            _backButton = GetButton("BackButton");
            _characterContainer = transform.Find("CharacterContainer");
            _tangFactionSlot = transform.Find("TangFactionSlot");
            _songFactionSlot = transform.Find("SongFactionSlot");
            _submitButton = GetButton("提交按钮");
            
            // 加载预制体
            _characterCardPrefab = Resources.Load<GameObject>("Prefabs/UI/CharacterCard");
            
            // 添加按钮点击事件
            AddButtonClickListener("BackButton", OnBackButtonClick);
            AddButtonClickListener("提交按钮", OnSubmitButtonClick);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            // 注册角色解锁事件
            GEventSystem.Instance.AddListener(GameEvents.CharacterUnlocked, OnCharacterUnlocked);
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        private void UpdateUI()
        {
            // 更新角色卡片
            UpdateCharacterCards();
            
            // 清空槽位
            ClearSlots();
        }

        /// <summary>
        /// 更新角色卡片
        /// </summary>
        private void UpdateCharacterCards()
        {
            if (_characterContainer == null)
            {
                return;
            }
            
            // 清空角色容器
            foreach (Transform child in _characterContainer)
            {
                Destroy(child.gameObject);
            }
            
            // 获取已解锁的角色
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null)
            {
                foreach (string characterId in dataManager.UnlockedCharacters)
                {
                    // 创建角色卡片
                    if (_characterCardPrefab != null)
                    {
                        GameObject cardObj = Instantiate(_characterCardPrefab, _characterContainer);
                        
                        // 设置角色数据
                        CharacterCard characterCard = cardObj.GetComponent<CharacterCard>();
                        if (characterCard != null)
                        {
                            characterCard.Init(characterId);
                        }
                        
                        // 添加拖拽组件
                        DragHandler dragHandler = cardObj.GetComponent<DragHandler>();
                        if (dragHandler == null)
                        {
                            dragHandler = cardObj.AddComponent<DragHandler>();
                        }
                        
                        dragHandler.dragBeginCallback = OnDragBegin;
                        dragHandler.dragCallback = OnDrag;
                        dragHandler.dragEndCallback = OnDragEnd;
                    }
                }
            }
        }

        /// <summary>
        /// 清空槽位
        /// </summary>
        private void ClearSlots()
        {
            // 清空南唐阵营槽位
            foreach (Transform child in _tangFactionSlot)
            {
                Destroy(child.gameObject);
            }
            
            // 清空北宋阵营槽位
            foreach (Transform child in _songFactionSlot)
            {
                Destroy(child.gameObject);
            }
            
            // 清空槽位角色记录
            _slotCharacters.Clear();
        }

        /// <summary>
        /// 返回按钮点击事件
        /// </summary>
        private void OnBackButtonClick()
        {
            Debug.Log("点击返回按钮");
            // 关闭阵营界面
            CloseUI();
        }

        /// <summary>
        /// 提交按钮点击事件
        /// </summary>
        private void OnSubmitButtonClick()
        {
            Debug.Log("点击提交按钮");
            
            // 检查槽位中的角色
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null)
            {
                // 设置角色阵营
                foreach (var pair in _slotCharacters)
                {
                    string slotName = pair.Key;
                    string characterId = pair.Value;
                    
                    if (slotName == "TangFactionSlot")
                    {
                        dataManager.SetCharacterFaction(characterId, "Tang");
                        Debug.Log($"角色 {characterId} 设置为南唐阵营");
                    }
                    else if (slotName == "SongFactionSlot")
                    {
                        dataManager.SetCharacterFaction(characterId, "Song");
                        Debug.Log($"角色 {characterId} 设置为北宋阵营");
                    }
                }
                
                // 保存游戏
                dataManager.SaveGame();
                
                // 显示提示
                Debug.Log("阵营选择已保存");
            }
        }

        /// <summary>
        /// 拖拽开始回调
        /// </summary>
        /// <param name="eventData">事件数据</param>
        /// <param name="dragObject">拖拽对象</param>
        private void OnDragBegin(PointerEventData eventData, GameObject dragObject)
        {
            // 记录当前拖拽的对象
            _dragObject = dragObject;
            
            // 记录原始位置和父节点
            _dragOriginalPosition = dragObject.transform.position;
            _dragOriginalParent = dragObject.transform.parent;
            
            // 将拖拽对象置于顶层
            dragObject.transform.SetParent(transform);
            dragObject.transform.SetAsLastSibling();
        }

        /// <summary>
        /// 拖拽中回调
        /// </summary>
        /// <param name="eventData">事件数据</param>
        /// <param name="dragObject">拖拽对象</param>
        private void OnDrag(PointerEventData eventData, GameObject dragObject)
        {
            // 更新拖拽对象的位置
            dragObject.transform.position = eventData.position;
        }

        /// <summary>
        /// 拖拽结束回调
        /// </summary>
        /// <param name="eventData">事件数据</param>
        /// <param name="dragObject">拖拽对象</param>
        private void OnDragEnd(PointerEventData eventData, GameObject dragObject)
        {
            // 检查是否放置在槽位上
            bool placed = false;
            
            // 射线检测，获取鼠标下的UI元素
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            
            foreach (RaycastResult result in results)
            {
                // 检查是否是槽位
                if (result.gameObject.transform == _tangFactionSlot || result.gameObject.transform == _songFactionSlot)
                {
                    // 获取角色ID
                    CharacterCard characterCard = dragObject.GetComponent<CharacterCard>();
                    if (characterCard != null)
                    {
                        string characterId = characterCard.CharacterId;
                        
                        // 检查角色是否已经在其他槽位中
                        foreach (var pair in _slotCharacters)
                        {
                            if (pair.Value == characterId)
                            {
                                // 如果角色已经在槽位中，则移除
                                _slotCharacters.Remove(pair.Key);
                                break;
                            }
                        }
                        
                        // 将角色放入槽位
                        string slotName = result.gameObject.transform.name;
                        _slotCharacters[slotName] = characterId;
                        
                        // 设置卡片的父节点为槽位
                        dragObject.transform.SetParent(result.gameObject.transform);
                        dragObject.transform.localPosition = Vector3.zero;
                        
                        placed = true;
                        
                        Debug.Log($"将角色 {characterId} 放入槽位 {slotName}");
                        break;
                    }
                }
            }
            
            // 如果没有放置在槽位上，则返回原位置
            if (!placed)
            {
                dragObject.transform.SetParent(_dragOriginalParent);
                dragObject.transform.position = _dragOriginalPosition;
            }
            
            // 清空拖拽相关变量
            _dragObject = null;
            _dragOriginalParent = null;
            _dragOriginalPosition = Vector3.zero;
        }

        /// <summary>
        /// 角色解锁事件回调
        /// </summary>
        /// <param name="parameters">参数数组</param>
        private void OnCharacterUnlocked(object[] parameters)
        {
            // 更新角色卡片
            UpdateCharacterCards();
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
            }
        }
    }

    /// <summary>
    /// 角色卡片组件
    /// </summary>
    public class CharacterCard : MonoBehaviour
    {
        // 角色ID
        private string _characterId;
        public string CharacterId => _characterId;
        
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
    /// 拖拽处理组件
    /// </summary>
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // 拖拽开始回调
        public System.Action<PointerEventData, GameObject> dragBeginCallback;
        
        // 拖拽中回调
        public System.Action<PointerEventData, GameObject> dragCallback;
        
        // 拖拽结束回调
        public System.Action<PointerEventData, GameObject> dragEndCallback;

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            dragBeginCallback?.Invoke(eventData, gameObject);
        }

        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnDrag(PointerEventData eventData)
        {
            dragCallback?.Invoke(eventData, gameObject);
        }

        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            dragEndCallback?.Invoke(eventData, gameObject);
        }
    }
} 