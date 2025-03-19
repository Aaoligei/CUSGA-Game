using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Game.Core;
using System;

namespace Game.Managers
{
    /// <summary>
    /// 游戏存档数据类
    /// </summary>
    [Serializable]
    public class GameSaveData
    {
        // 基本游戏信息
        public int gameTime;               // 游戏内时间
        public int actionPoints;           // 行动点数
        public float gameProgress;         // 游戏进度
        public string currentSceneName;    // 当前场景名称

        // 角色信息
        public List<string> unlockedCharacters = new List<string>();  // 已解锁角色ID列表
        public Dictionary<string, CharacterData> characterDataDict = new Dictionary<string, CharacterData>();  // 角色详细数据字典

        // 道具信息
        public List<string> collectedItems = new List<string>();  // 已收集道具ID列表
        public List<string> equippedItems = new List<string>();   // 已装备道具ID列表

        // 卡牌信息
        public List<string> unlockedCards = new List<string>();   // 已解锁卡牌ID列表
        
        // 游戏进度信息
        public Dictionary<string, bool> completedEvents = new Dictionary<string, bool>();  // 已完成事件字典
        
        // 其他自定义数据
        public Dictionary<string, string> customData = new Dictionary<string, string>();   // 自定义数据字典
    }

    /// <summary>
    /// 角色数据类
    /// </summary>
    [Serializable]
    public class CharacterData
    {
        public string id;                      // 角色ID
        public string name;                    // 角色名称
        public string description;             // 角色描述
        public bool isUnlocked;                // 是否已解锁
        public List<string> relatedItems;      // 相关道具ID列表
        public Dictionary<string, bool> notes; // 笔录内容字典（ID：是否已解锁）
        public string faction;                 // 阵营（南唐/北宋）
    }

    /// <summary>
    /// 数据管理器，负责游戏数据的保存、加载和管理
    /// </summary>
    public class DataManager : Singleton<DataManager>
    {
        // 当前游戏存档数据
        private GameSaveData _currentSaveData;
        public GameSaveData CurrentSaveData => _currentSaveData;

        // 存档文件名
        private const string SAVE_FILE_NAME = "game_save.json";
        // 存档文件路径
        public string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

        // 已解锁角色列表
        public List<string> UnlockedCharacters => _currentSaveData.unlockedCharacters;
        
        // 已收集道具列表
        public List<string> CollectedItems => _currentSaveData.collectedItems;
        
        // 已装备道具列表
        public List<string> EquippedItems => _currentSaveData.equippedItems;
        
        // 已解锁卡牌列表
        public List<string> UnlockedCards => _currentSaveData.unlockedCards;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        /// <summary>
        /// 初始化数据管理器
        /// </summary>
        private void Init()
        {
            // 尝试加载存档，如果不存在则创建新存档
            if (!LoadGame())
            {
                CreateNewSaveData();
            }
            
            Debug.Log("数据管理器初始化完成");
        }

        /// <summary>
        /// 创建新存档数据
        /// </summary>
        public void CreateNewSaveData()
        {
            _currentSaveData = new GameSaveData
            {
                gameTime = 0,
                actionPoints = 3,
                gameProgress = 0f,
                currentSceneName = "MainMenu",
                unlockedCharacters = new List<string>(),
                characterDataDict = new Dictionary<string, CharacterData>(),
                collectedItems = new List<string>(),
                equippedItems = new List<string>(),
                unlockedCards = new List<string>(),
                completedEvents = new Dictionary<string, bool>(),
                customData = new Dictionary<string, string>()
            };
            
            Debug.Log("创建新存档数据");
        }

        /// <summary>
        /// 保存游戏
        /// </summary>
        /// <returns>是否保存成功</returns>
        public bool SaveGame()
        {
            try
            {
                // 更新存档数据
                UpdateSaveData();
                
                // 将数据序列化为JSON
                string jsonData = JsonUtility.ToJson(_currentSaveData, true);
                
                // 写入文件
                File.WriteAllText(SaveFilePath, jsonData);
                
                Debug.Log($"游戏保存成功：{SaveFilePath}");
                GEventSystem.Instance.TriggerEvent(GameEvents.DataSaved);
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"保存游戏失败：{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 加载游戏
        /// </summary>
        /// <returns>是否加载成功</returns>
        public bool LoadGame()
        {
            try
            {
                // 检查存档文件是否存在
                if (!File.Exists(SaveFilePath))
                {
                    Debug.Log("存档文件不存在，无法加载");
                    return false;
                }
                
                // 读取文件内容
                string jsonData = File.ReadAllText(SaveFilePath);
                
                // 将JSON反序列化为存档数据
                _currentSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);
                
                // 应用加载的数据
                ApplyLoadedData();
                
                Debug.Log($"游戏加载成功：{SaveFilePath}");
                GEventSystem.Instance.TriggerEvent(GameEvents.DataLoaded);
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载游戏失败：{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        /// <returns>是否删除成功</returns>
        public bool DeleteSaveData()
        {
            try
            {
                // 检查存档文件是否存在
                if (!File.Exists(SaveFilePath))
                {
                    Debug.Log("存档文件不存在，无需删除");
                    return true;
                }
                
                // 删除文件
                File.Delete(SaveFilePath);
                
                Debug.Log($"存档删除成功：{SaveFilePath}");
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"删除存档失败：{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 更新存档数据
        /// </summary>
        private void UpdateSaveData()
        {
            GameManager gameManager = GameManager.Instance;
            
            // 更新基本游戏信息
            _currentSaveData.gameTime = gameManager.GameTime;
            _currentSaveData.actionPoints = gameManager.ActionPoints;
            _currentSaveData.gameProgress = gameManager.GameProgress;
            _currentSaveData.currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            
            // 其他数据通过具体调用来更新，不在这里统一处理
        }

        /// <summary>
        /// 应用加载的数据
        /// </summary>
        private void ApplyLoadedData()
        {
            GameManager gameManager = GameManager.Instance;
            
            // 更新游戏管理器的数据
            // 注意：这些方法需要在GameManager中实现
            gameManager.SetGameTime(_currentSaveData.gameTime);
            gameManager.SetActionPoints(_currentSaveData.actionPoints);
            gameManager.SetGameProgress(_currentSaveData.gameProgress);
            
            // 加载场景
            // 如果当前场景不是要加载的场景，则加载对应的场景
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != _currentSaveData.currentSceneName)
            {
                // 这里使用SceneManager加载场景，实际应该通过自己的场景管理器加载
                // SceneLoadManager.Instance.LoadScene(_currentSaveData.currentSceneName);
            }
        }

        /// <summary>
        /// 解锁角色
        /// </summary>
        /// <param name="characterId">角色ID</param>
        public void UnlockCharacter(string characterId)
        {
            if (!_currentSaveData.unlockedCharacters.Contains(characterId))
            {
                _currentSaveData.unlockedCharacters.Add(characterId);
                
                // 如果角色数据字典中没有该角色，则添加基本数据
                if (!_currentSaveData.characterDataDict.ContainsKey(characterId))
                {
                    _currentSaveData.characterDataDict[characterId] = new CharacterData
                    {
                        id = characterId,
                        isUnlocked = true,
                        relatedItems = new List<string>(),
                        notes = new Dictionary<string, bool>()
                    };
                }
                else
                {
                    _currentSaveData.characterDataDict[characterId].isUnlocked = true;
                }
                
                Debug.Log($"解锁角色：{characterId}");
                GEventSystem.Instance.TriggerEvent(GameEvents.CharacterUnlocked, characterId);
            }
        }

        /// <summary>
        /// 收集道具
        /// </summary>
        /// <param name="itemId">道具ID</param>
        public void CollectItem(string itemId)
        {
            if (!_currentSaveData.collectedItems.Contains(itemId))
            {
                _currentSaveData.collectedItems.Add(itemId);
                Debug.Log($"收集道具：{itemId}");
                GEventSystem.Instance.TriggerEvent(GameEvents.ItemCollected, itemId);
            }
        }

        /// <summary>
        /// 装备道具
        /// </summary>
        /// <param name="itemId">道具ID</param>
        public void EquipItem(string itemId)
        {
            // 检查道具是否已收集
            if (_currentSaveData.collectedItems.Contains(itemId) && !_currentSaveData.equippedItems.Contains(itemId))
            {
                _currentSaveData.equippedItems.Add(itemId);
                Debug.Log($"装备道具：{itemId}");
            }
        }

        /// <summary>
        /// 卸下道具
        /// </summary>
        /// <param name="itemId">道具ID</param>
        public void UnequipItem(string itemId)
        {
            if (_currentSaveData.equippedItems.Contains(itemId))
            {
                _currentSaveData.equippedItems.Remove(itemId);
                Debug.Log($"卸下道具：{itemId}");
            }
        }

        /// <summary>
        /// 解锁卡牌
        /// </summary>
        /// <param name="cardId">卡牌ID</param>
        public void UnlockCard(string cardId)
        {
            if (!_currentSaveData.unlockedCards.Contains(cardId))
            {
                _currentSaveData.unlockedCards.Add(cardId);
                Debug.Log($"解锁卡牌：{cardId}");
            }
        }

        /// <summary>
        /// 设置完成事件
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="isCompleted">是否完成</param>
        public void SetEventCompleted(string eventId, bool isCompleted = true)
        {
            _currentSaveData.completedEvents[eventId] = isCompleted;
            Debug.Log($"设置事件 {eventId} 完成状态为：{isCompleted}");
        }

        /// <summary>
        /// 检查事件是否完成
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <returns>是否完成</returns>
        public bool IsEventCompleted(string eventId)
        {
            return _currentSaveData.completedEvents.ContainsKey(eventId) && _currentSaveData.completedEvents[eventId];
        }

        /// <summary>
        /// 保存自定义数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SaveCustomData(string key, string value)
        {
            _currentSaveData.customData[key] = value;
        }

        /// <summary>
        /// 获取自定义数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>获取的值，如果不存在则返回默认值</returns>
        public string GetCustomData(string key, string defaultValue = "")
        {
            return _currentSaveData.customData.ContainsKey(key) ? _currentSaveData.customData[key] : defaultValue;
        }

        /// <summary>
        /// 设置角色阵营
        /// </summary>
        /// <param name="characterId">角色ID</param>
        /// <param name="faction">阵营</param>
        public void SetCharacterFaction(string characterId, string faction)
        {
            if (_currentSaveData.characterDataDict.ContainsKey(characterId))
            {
                _currentSaveData.characterDataDict[characterId].faction = faction;
                Debug.Log($"设置角色 {characterId} 阵营为：{faction}");
            }
        }

        /// <summary>
        /// 获取角色阵营
        /// </summary>
        /// <param name="characterId">角色ID</param>
        /// <returns>角色阵营</returns>
        public string GetCharacterFaction(string characterId)
        {
            if (_currentSaveData.characterDataDict.ContainsKey(characterId))
            {
                return _currentSaveData.characterDataDict[characterId].faction;
            }
            return "";
        }

        /// <summary>
        /// 解锁角色笔录
        /// </summary>
        /// <param name="characterId">角色ID</param>
        /// <param name="noteId">笔录ID</param>
        public void UnlockCharacterNote(string characterId, string noteId)
        {
            if (_currentSaveData.characterDataDict.ContainsKey(characterId))
            {
                _currentSaveData.characterDataDict[characterId].notes[noteId] = true;
                Debug.Log($"解锁角色 {characterId} 的笔录：{noteId}");
            }
        }

        /// <summary>
        /// 检查角色笔录是否解锁
        /// </summary>
        /// <param name="characterId">角色ID</param>
        /// <param name="noteId">笔录ID</param>
        /// <returns>是否解锁</returns>
        public bool IsCharacterNoteUnlocked(string characterId, string noteId)
        {
            if (_currentSaveData.characterDataDict.ContainsKey(characterId) && 
                _currentSaveData.characterDataDict[characterId].notes.ContainsKey(noteId))
            {
                return _currentSaveData.characterDataDict[characterId].notes[noteId];
            }
            return false;
        }

        /// <summary>
        /// 添加角色相关道具
        /// </summary>
        /// <param name="characterId">角色ID</param>
        /// <param name="itemId">道具ID</param>
        public void AddCharacterRelatedItem(string characterId, string itemId)
        {
            if (_currentSaveData.characterDataDict.ContainsKey(characterId) && 
                !_currentSaveData.characterDataDict[characterId].relatedItems.Contains(itemId))
            {
                _currentSaveData.characterDataDict[characterId].relatedItems.Add(itemId);
                Debug.Log($"添加角色 {characterId} 相关道具：{itemId}");
            }
        }

        /// <summary>
        /// 获取角色相关道具列表
        /// </summary>
        /// <param name="characterId">角色ID</param>
        /// <returns>相关道具ID列表</returns>
        public List<string> GetCharacterRelatedItems(string characterId)
        {
            if (_currentSaveData.characterDataDict.ContainsKey(characterId))
            {
                return _currentSaveData.characterDataDict[characterId].relatedItems;
            }
            return new List<string>();
        }
    }
} 