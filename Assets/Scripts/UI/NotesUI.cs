using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Managers;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 笔录界面实现
    /// </summary>
    public class NotesUI : BaseUI
    {
        // 一级页面
        [SerializeField] private Button _backButton;                 // 返回按钮
        [SerializeField]private Transform _characterList;           // 角色列表容器

        // 二级页面
        [SerializeField] private GameObject _notesPanel;             // 笔录面板
        [SerializeField] private Text _characterNameText;            // 角色名称文本
        [SerializeField] private Image _characterImage;              // 角色图像
        [SerializeField] private Transform _notesList;               // 笔录列表容器
        [SerializeField] private Button _closeNotesButton;           // 关闭笔录按钮

        // 三级页面
        [SerializeField] private GameObject _noteDetailPanel;        // 笔录详情面板
        [SerializeField] private Text _noteTitleText;                // 笔录标题文本
        [SerializeField] private Text _noteContentText;              // 笔录内容文本
        [SerializeField] private Button _closeNoteDetailButton;      // 关闭笔录详情按钮

        // 预制体
        [SerializeField] private GameObject _characterItemPrefab;    // 角色项预制体
        [SerializeField] private GameObject _noteItemPrefab;         // 笔录项预制体

        // 当前选中的角色ID
        [SerializeField] private string _currentCharacterId;

        // 当前选中的笔录ID
        [SerializeField] private string _currentNoteId;

        // 笔录内容字典（临时，实际应从数据管理器获取）
        [SerializeField] private Dictionary<string, string> _noteContents = new Dictionary<string, string>();
        
        protected override void OnInit()
        {
            base.OnInit();
            
            // 初始化界面组件
            InitComponents();
            
            // 注册事件
            RegisterEvents();
            
            // 初始化笔录内容
            InitNoteContents();
            
            // 更新界面
            UpdateUI();
        }

        /// <summary>
        /// 初始化界面组件
        /// </summary>
        private void InitComponents()
        {
            // 一级页面
            RectTransform rectTransform = GetComponent<RectTransform>();
            _backButton = GetButton("关闭按钮");
            _characterList = transform.Find("人物滑条/视口/内容");
            Debug.Log("sadad");
            
            // 二级页面
            _notesPanel = transform.Find("NotesPanel").gameObject;
            _characterNameText = _notesPanel.transform.Find("CharacterInfo/NameText").GetComponent<Text>();
            _characterImage = _notesPanel.transform.Find("CharacterInfo/CharacterImage").GetComponent<Image>();
            _notesList = _notesPanel.transform.Find("NotesList");
            _closeNotesButton = _notesPanel.transform.Find("CloseButton").GetComponent<Button>();
            
            // 三级页面
            _noteDetailPanel = transform.Find("NoteDetailPanel").gameObject;
            _noteTitleText = _noteDetailPanel.transform.Find("TitleText").GetComponent<Text>();
            _noteContentText = _noteDetailPanel.transform.Find("ContentText").GetComponent<Text>();
            _closeNoteDetailButton = _noteDetailPanel.transform.Find("CloseButton").GetComponent<Button>();
            
            // 加载预制体
            _characterItemPrefab = Resources.Load<GameObject>("Prefabs/UI/CharacterItem");
            _noteItemPrefab = Resources.Load<GameObject>("Prefabs/UI/NoteItem");
            
            // 添加按钮点击事件
            AddButtonClickListener("关闭按钮", OnBackButtonClick);
            //_closeNotesButton.onClick.AddListener(OnCloseNotesButtonClick);
            _closeNoteDetailButton.onClick.AddListener(OnCloseNoteDetailButtonClick);
            
            // 初始隐藏二级和三级页面
            _notesPanel.SetActive(false);
            _noteDetailPanel.SetActive(false);
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
        /// 初始化笔录内容
        /// </summary>
        private void InitNoteContents()
        {
            // 临时笔录内容，实际应从数据管理器获取
            _noteContents.Clear();
            
            // 角色1的笔录
            _noteContents.Add("char1_note1", "【初次见面】\n\n我是南唐的王审琦，字仲雅，旧南唐宁国军节度使王审知之侄，前南唐左卫将军，今为北宋枢密院编修。大宋建隆元年，太祖皇帝遣兵伐唐，我叔父举国请降，后我有幸受学士院征辟，来到东京汴梁。");
            _noteContents.Add("char1_note2", "【关于高平楼倒塌案】\n\n我虽为南唐降臣，但对大宋忠心耿耿。此次高平楼倒塌案，死伤众多，疑点颇多。我将全力协助调查，还死者一个公道，保大宋太平。");
            
            // 角色2的笔录
            _noteContents.Add("char2_note1", "【初次见面】\n\n在下李继勋，字德懋，祖籍太原，为大宋开国功臣、定国军节度使李筠之子。亡父曾被太祖皇帝亲封为彭城郡王，可惜后来因谋反被诛。我虽为谋反之人后代，但蒙太宗皇帝不弃，得以为官。");
            _noteContents.Add("char2_note2", "【关于高平楼倒塌案】\n\n此案关系重大，不仅死伤众多，还可能涉及到朝堂上的党争。作为北宋官员，我有责任查明此案，无论牵涉到谁，都要一查到底。");
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
                    // 检查角色是否有笔录
                    if (HasCharacterNotes(characterId))
                    {
                        // 创建角色项
                        if (_characterItemPrefab != null)
                        {
                            GameObject characterObj = Instantiate(_characterItemPrefab, _characterList);
                            
                            // 设置角色数据
                            NoteCharacterItem characterItem = characterObj.GetComponent<NoteCharacterItem>();
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
        }

        /// <summary>
        /// 检查角色是否有笔录
        /// </summary>
        /// <param name="characterId">角色ID</param>
        /// <returns>是否有笔录</returns>
        private bool HasCharacterNotes(string characterId)
        {
            // 临时实现，实际应从数据管理器获取
            // 这里假设char1和char2有笔录
            return characterId == "char1" || characterId == "char2";
        }

        /// <summary>
        /// 更新笔录列表
        /// </summary>
        /// <param name="characterId">角色ID</param>
        private void UpdateNotesList(string characterId)
        {
            if (_notesList == null)
            {
                return;
            }
            
            // 清空笔录列表
            foreach (Transform child in _notesList)
            {
                Destroy(child.gameObject);
            }
            
            // 设置当前选中的角色ID
            _currentCharacterId = characterId;
            
            // 获取角色数据
            DataManager dataManager = DataManager.Instance;
            if (dataManager != null && dataManager.CurrentSaveData.characterDataDict.TryGetValue(characterId, out var characterData))
            {
                // 设置角色名称
                _characterNameText.text = characterData.name;
                
                // 设置角色图像
                // _characterImage.sprite = Resources.Load<Sprite>($"Characters/{characterId}");
                
                // 获取角色笔录
                Dictionary<string, bool> notes = characterData.notes;
                
                // 临时逻辑，实际应从数据管理器获取
                // 这里假设char1有两条笔录，char2有两条笔录
                List<string> noteIds = new List<string>();
                
                if (characterId == "char1")
                {
                    noteIds.Add("char1_note1");
                    noteIds.Add("char1_note2");
                }
                else if (characterId == "char2")
                {
                    noteIds.Add("char2_note1");
                    noteIds.Add("char2_note2");
                }
                
                // 创建笔录项
                foreach (string noteId in noteIds)
                {
                    // 检查笔录是否已解锁
                    bool isUnlocked = true; // 临时设置为真，实际应从数据管理器获取
                    
                    // 只显示已解锁的笔录
                    if (isUnlocked)
                    {
                        // 创建笔录项
                        if (_noteItemPrefab != null)
                        {
                            GameObject noteObj = Instantiate(_noteItemPrefab, _notesList);
                            
                            // 设置笔录数据
                            NoteItem noteItem = noteObj.GetComponent<NoteItem>();
                            if (noteItem != null)
                            {
                                // 从笔录内容字典获取标题（简单处理，实际应该有专门的标题字段）
                                string noteTitle = _noteContents.TryGetValue(noteId, out string content) ? 
                                    content.Split('\n')[0] : noteId;
                                
                                noteItem.Init(noteId, noteTitle);
                                
                                // 添加点击事件
                                Button button = noteObj.GetComponent<Button>();
                                if (button != null)
                                {
                                    button.onClick.AddListener(() => OnNoteItemClick(noteId));
                                }
                            }
                        }
                    }
                }
                
                // 显示笔录面板
                _notesPanel.SetActive(true);
            }
        }

        /// <summary>
        /// 显示笔录详情
        /// </summary>
        /// <param name="noteId">笔录ID</param>
        private void ShowNoteDetail(string noteId)
        {
            if (_noteDetailPanel == null)
            {
                return;
            }
            
            // 设置当前选中的笔录ID
            _currentNoteId = noteId;
            
            // 获取笔录内容
            if (_noteContents.TryGetValue(noteId, out string content))
            {
                // 从内容中提取标题（简单处理，实际应该有专门的标题字段）
                string[] lines = content.Split('\n');
                string title = lines[0];
                
                // 设置笔录标题
                _noteTitleText.text = title;
                
                // 设置笔录内容（跳过标题）
                _noteContentText.text = content.Substring(title.Length).Trim();
                
                // 显示笔录详情面板
                _noteDetailPanel.SetActive(true);
            }
        }

        /// <summary>
        /// 返回按钮点击事件
        /// </summary>
        private void OnBackButtonClick()
        {
            Debug.Log("点击返回按钮");
            // 关闭笔录界面
            CloseUI();
        }

        /// <summary>
        /// 关闭笔录按钮点击事件
        /// </summary>
        private void OnCloseNotesButtonClick()
        {
            Debug.Log("点击关闭笔录按钮");
            // 隐藏笔录面板
            _notesPanel.SetActive(false);
            _currentCharacterId = null;
        }

        /// <summary>
        /// 关闭笔录详情按钮点击事件
        /// </summary>
        private void OnCloseNoteDetailButtonClick()
        {
            Debug.Log("点击关闭笔录详情按钮");
            // 隐藏笔录详情面板
            _noteDetailPanel.SetActive(false);
            _currentNoteId = null;
        }

        /// <summary>
        /// 角色项点击事件
        /// </summary>
        /// <param name="characterId">角色ID</param>
        private void OnCharacterItemClick(string characterId)
        {
            Debug.Log($"点击角色项: {characterId}");
            // 更新笔录列表
            UpdateNotesList(characterId);
        }

        /// <summary>
        /// 笔录项点击事件
        /// </summary>
        /// <param name="noteId">笔录ID</param>
        private void OnNoteItemClick(string noteId)
        {
            Debug.Log($"点击笔录项: {noteId}");
            // 显示笔录详情
            ShowNoteDetail(noteId);
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
    /// 笔录界面的角色项组件
    /// </summary>
    public class NoteCharacterItem : MonoBehaviour
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
    /// 笔录项组件
    /// </summary>
    public class NoteItem : MonoBehaviour
    {
        // 笔录ID
        private string _noteId;
        
        // 笔录标题
        private Text _titleText;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="noteId">笔录ID</param>
        /// <param name="title">笔录标题</param>
        public void Init(string noteId, string title)
        {
            _noteId = noteId;
            
            // 获取组件
            _titleText = transform.Find("TitleText").GetComponent<Text>();
            
            // 设置数据
            _titleText.text = title;
        }
    }
}