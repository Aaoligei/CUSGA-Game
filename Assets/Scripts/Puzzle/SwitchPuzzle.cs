using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 开关谜题类，继承自PuzzleBase
/// 实现多个开关的谜题逻辑，可以设置需要激活的开关组合
/// </summary>
public class SwitchPuzzle : PuzzleBase
{
    [System.Serializable]
    public class SwitchState
    {
        public string switchID;
        public bool requiredState = true;
    }
    
    [Header("开关谜题设置")]
    [Tooltip("需要满足条件的开关列表")]
    [SerializeField] private List<SwitchState> requiredSwitches = new List<SwitchState>();
    
    // 当前开关状态字典
    private Dictionary<string, bool> currentSwitchStates = new Dictionary<string, bool>();
    
    protected override void Awake()
    {
        base.Awake();
        
        // 初始化开关状态字典
        foreach (var switchState in requiredSwitches)
        {
            currentSwitchStates[switchState.switchID] = false;
        }
    }
    
    /// <summary>
    /// 更新开关状态
    /// </summary>
    /// <param name="switchID">开关ID</param>
    /// <param name="isOn">开关状态</param>
    public void UpdateSwitchState(string switchID, bool isOn)
    {
        // 更新开关状态
        if (currentSwitchStates.ContainsKey(switchID))
        {
            currentSwitchStates[switchID] = isOn;
            
            // 检查是否满足解谜条件
            if (CheckSolution())
            {
                Solve();
            }
        }
    }
    
    /// <summary>
    /// 检查谜题是否可以解决
    /// </summary>
    /// <returns>谜题是否可以解决</returns>
    public override bool CheckSolution()
    {
        // 检查所有开关是否满足要求
        foreach (var requiredSwitch in requiredSwitches)
        {
            // 如果开关不存在或状态不匹配，返回false
            if (!currentSwitchStates.TryGetValue(requiredSwitch.switchID, out bool currentState) ||
                currentState != requiredSwitch.requiredState)
            {
                return false;
            }
        }
        
        // 所有开关都满足要求
        return true;
    }
    
    /// <summary>
    /// 重置谜题
    /// </summary>
    protected override void OnReset()
    {
        // 重置所有开关状态
        foreach (var switchID in currentSwitchStates.Keys)
        {
            currentSwitchStates[switchID] = false;
            
            // 尝试找到场景中的开关并重置
            Switch[] switches = FindObjectsOfType<Switch>();
            foreach (var switchObj in switches)
            {
                if (switchObj.GetSwitchID() == switchID)
                {
                    switchObj.SetState(false);
                    break;
                }
            }
        }
    }
}