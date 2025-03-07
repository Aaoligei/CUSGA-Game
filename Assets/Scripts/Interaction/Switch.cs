using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 开关类，实现IInteractable接口，可以被玩家交互
/// </summary>
public class Switch : MonoBehaviour, IInteractable
{
    [Header("开关设置")]
    [SerializeField] private string switchID; // 开关的唯一标识符
    [SerializeField] private bool isOn = false; // 开关的状态
    [SerializeField] private GameObject visualOn; // 开关打开时的视觉效果
    [SerializeField] private GameObject visualOff; // 开关关闭时的视觉效果
    [SerializeField] private GameObject interactionPrompt; // 交互提示UI
    
    [Header("事件触发")]
    [SerializeField] private string puzzleID; // 关联的谜题ID
    
    private void Start()
    {
        // 初始化开关状态
        UpdateVisuals();
        
        // 隐藏交互提示
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
    
    /// <summary>
    /// 实现IInteractable接口的Interact方法
    /// </summary>
    public void Interact(PlayerController player)
    {
        // 切换开关状态
        isOn = !isOn;
        
        // 更新视觉效果
        UpdateVisuals();
        
        // 如果关联了谜题，尝试解决
        if (!string.IsNullOrEmpty(puzzleID) && PuzzleManager.Instance != null)
        {
            // 通知谜题管理器开关状态已改变
            PuzzleManager.Instance.OnSwitchStateChanged(switchID, isOn);
        }
    }
    
    /// <summary>
    /// 实现IInteractable接口的ShowInteractionPrompt方法
    /// </summary>
    public void ShowInteractionPrompt(bool show)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(show);
        }
    }
    
    /// <summary>
    /// 更新开关的视觉效果
    /// </summary>
    private void UpdateVisuals()
    {
        if (visualOn != null)
        {
            visualOn.SetActive(isOn);
        }
        
        if (visualOff != null)
        {
            visualOff.SetActive(!isOn);
        }
    }
    
    /// <summary>
    /// 获取开关的ID
    /// </summary>
    public string GetSwitchID()
    {
        return switchID;
    }
    
    /// <summary>
    /// 获取开关的状态
    /// </summary>
    public bool IsOn()
    {
        return isOn;
    }
    
    /// <summary>
    /// 设置开关的状态
    /// </summary>
    public void SetState(bool state)
    {
        if (isOn != state)
        {
            isOn = state;
            UpdateVisuals();
        }
    }
}