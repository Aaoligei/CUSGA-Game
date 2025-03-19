using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 事件系统，负责游戏内组件间的消息传递
    /// </summary>
    public class GEventSystem : Singleton<GEventSystem>
    {
        // 事件字典，存储所有事件及其对应的监听者
        private Dictionary<string, Action<object[]>> _events = new Dictionary<string, Action<object[]>>();

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="listener">监听者回调</param>
        public void AddListener(string eventName, Action<object[]> listener)
        {
            if (!_events.ContainsKey(eventName))
            {
                _events[eventName] = listener;
            }
            else
            {
                _events[eventName] += listener;
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="listener">监听者回调</param>
        public void RemoveListener(string eventName, Action<object[]> listener)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName] -= listener;

                // 如果没有监听者了，则移除该事件
                if (_events[eventName] == null)
                {
                    _events.Remove(eventName);
                }
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="parameters">参数数组</param>
        public void TriggerEvent(string eventName, params object[] parameters)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName]?.Invoke(parameters);
            }
        }

        /// <summary>
        /// 清空所有事件
        /// </summary>
        public void ClearAllEvents()
        {
            _events.Clear();
        }
    }

    /// <summary>
    /// 游戏中常用事件名称的常量类
    /// </summary>
    public static class GameEvents
    {
        // 游戏状态相关事件
        public const string GameStart = "GameStart";
        public const string GamePause = "GamePause";
        public const string GameResume = "GameResume";
        public const string GameOver = "GameOver";
        
        // 界面相关事件
        public const string OpenUI = "OpenUI";
        public const string CloseUI = "CloseUI";
        
        // 数据相关事件
        public const string DataLoaded = "DataLoaded";
        public const string DataSaved = "DataSaved";
        
        // 角色相关事件
        public const string CharacterUnlocked = "CharacterUnlocked";
        
        // 道具相关事件
        public const string ItemCollected = "ItemCollected";
        public const string ItemUsed = "ItemUsed";
        
        // 卡牌相关事件
        public const string CardUsed = "CardUsed";
        
        // 场景相关事件
        public const string SceneChanged = "SceneChanged";
        
        // 行动点相关事件
        public const string ActionPointsChanged = "ActionPointsChanged";
    }
} 