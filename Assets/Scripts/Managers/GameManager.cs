using UnityEngine;
using Game.Core;
using System;

namespace Game.Managers
{
    /// <summary>
    /// 游戏状态枚举
    /// </summary>
    public enum GameState
    {
        MainMenu,       // 主菜单
        Playing,        // 游戏中
        Paused,         // 暂停
        Dialog,         // 对话中
        Investigation,  // 搜证中
        CardGame,       // 卡牌游戏中
        GameOver        // 游戏结束
    }

    /// <summary>
    /// 游戏管理器，负责控制游戏主要流程和状态
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        // 当前游戏状态
        private GameState _currentState;
        public GameState CurrentState
        {
            get { return _currentState; }
            private set
            {
                if (_currentState != value)
                {
                    GameState oldState = _currentState;
                    _currentState = value;
                    OnGameStateChanged?.Invoke(oldState, _currentState);
                }
            }
        }

        // 游戏状态改变事件
        public event Action<GameState, GameState> OnGameStateChanged;

        // 游戏内时间（游戏内一天划分为若干时间段）
        private int _gameTime;
        public int GameTime
        {
            get { return _gameTime; }
            private set
            {
                if (_gameTime != value)
                {
                    _gameTime = value;
                    OnGameTimeChanged?.Invoke(_gameTime);
                }
            }
        }

        // 游戏时间改变事件
        public event Action<int> OnGameTimeChanged;

        // 行动点数
        private int _actionPoints;
        public int ActionPoints
        {
            get { return _actionPoints; }
            private set
            {
                if (_actionPoints != value)
                {
                    _actionPoints = value;
                    OnActionPointsChanged?.Invoke(_actionPoints);
                    GEventSystem.Instance.TriggerEvent(GameEvents.ActionPointsChanged, _actionPoints);
                }
            }
        }

        // 行动点改变事件
        public event Action<int> OnActionPointsChanged;

        // 游戏进度（百分比）
        private float _gameProgress;
        public float GameProgress
        {
            get { return _gameProgress; }
            private set
            {
                if (_gameProgress != value)
                {
                    _gameProgress = value;
                    OnGameProgressChanged?.Invoke(_gameProgress);
                }
            }
        }

        // 游戏进度改变事件
        public event Action<float> OnGameProgressChanged;

        protected override void Awake()
        {
            base.Awake();
            InitGame();
        }

        /// <summary>
        /// 初始化游戏
        /// </summary>
        private void InitGame()
        {
            // 初始状态设为主菜单
            _currentState = GameState.MainMenu;
            
            // 初始时间设为早上
            _gameTime = 0;
            
            // 初始行动点数
            _actionPoints = 3;
            
            // 初始进度
            _gameProgress = 0f;
            
            Debug.Log("游戏初始化完成");
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void StartGame()
        {
            if (CurrentState == GameState.MainMenu)
            {
                CurrentState = GameState.Playing;
                GEventSystem.Instance.TriggerEvent(GameEvents.GameStart);
                Debug.Log("游戏开始");
            }
        }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            if (CurrentState == GameState.Playing)
            {
                CurrentState = GameState.Paused;
                Time.timeScale = 0f;
                GEventSystem.Instance.TriggerEvent(GameEvents.GamePause);
                Debug.Log("游戏暂停");
            }
        }

        /// <summary>
        /// 恢复游戏
        /// </summary>
        public void ResumeGame()
        {
            if (CurrentState == GameState.Paused)
            {
                CurrentState = GameState.Playing;
                Time.timeScale = 1f;
                GEventSystem.Instance.TriggerEvent(GameEvents.GameResume);
                Debug.Log("游戏恢复");
            }
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public void EndGame()
        {
            CurrentState = GameState.GameOver;
            GEventSystem.Instance.TriggerEvent(GameEvents.GameOver);
            Debug.Log("游戏结束");
        }

        /// <summary>
        /// 进入对话状态
        /// </summary>
        public void EnterDialogState()
        {
            CurrentState = GameState.Dialog;
            Debug.Log("进入对话状态");
        }

        /// <summary>
        /// 进入搜证状态
        /// </summary>
        public void EnterInvestigationState()
        {
            CurrentState = GameState.Investigation;
            Debug.Log("进入搜证状态");
        }

        /// <summary>
        /// 进入卡牌游戏状态
        /// </summary>
        public void EnterCardGameState()
        {
            CurrentState = GameState.CardGame;
            Debug.Log("进入卡牌游戏状态");
        }

        /// <summary>
        /// 消耗行动点
        /// </summary>
        /// <param name="points">消耗的点数</param>
        /// <returns>是否消耗成功</returns>
        public bool ConsumeActionPoints(int points)
        {
            if (ActionPoints >= points)
            {
                ActionPoints -= points;
                Debug.Log($"消耗 {points} 点行动点，剩余 {ActionPoints} 点");
                return true;
            }
            
            Debug.Log($"行动点不足，需要 {points} 点，当前有 {ActionPoints} 点");
            return false;
        }

        /// <summary>
        /// 增加行动点
        /// </summary>
        /// <param name="points">增加的点数</param>
        public void AddActionPoints(int points)
        {
            ActionPoints += points;
            Debug.Log($"增加 {points} 点行动点，当前有 {ActionPoints} 点");
        }

        /// <summary>
        /// 推进游戏时间
        /// </summary>
        /// <param name="increment">时间增量</param>
        public void AdvanceGameTime(int increment = 1)
        {
            GameTime += increment;
            Debug.Log($"游戏时间推进到 {GameTime}");
        }

        /// <summary>
        /// 更新游戏进度
        /// </summary>
        /// <param name="progress">新的进度值（0-1）</param>
        public void UpdateGameProgress(float progress)
        {
            if (progress < 0f) progress = 0f;
            if (progress > 1f) progress = 1f;
            
            GameProgress = progress;
            Debug.Log($"游戏进度更新为 {GameProgress * 100}%");
        }

        internal void SetGameTime(int gameTime)
        {
            throw new NotImplementedException();
        }

        internal void SetActionPoints(int actionPoints)
        {
            throw new NotImplementedException();
        }

        internal void SetGameProgress(float gameProgress)
        {
            throw new NotImplementedException();
        }
    }
} 