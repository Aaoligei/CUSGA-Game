using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 门类，实现IInteractable接口，可以被玩家交互
/// 门可以被谜题解决后打开，也可以直接交互打开
/// </summary>
public class Door : MonoBehaviour, IInteractable
{
    [Header("门设置")]
    [SerializeField] private bool isLocked = true; // 门是否锁住
    [SerializeField] private bool isOpen = false; // 门是否打开
    [SerializeField] private GameObject visualClosed; // 门关闭时的视觉效果
    [SerializeField] private GameObject visualOpen; // 门打开时的视觉效果
    [SerializeField] private GameObject interactionPrompt; // 交互提示UI
    [SerializeField] private GameObject lockedPrompt; // 锁住提示UI
    
    [Header("谜题关联")]
    [SerializeField] private string requiredPuzzleID; // 需要解决的谜题ID
    
    private void Start()
    {
        // 初始化门的状态
        UpdateVisuals();
        
        // 隐藏交互提示
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        if (lockedPrompt != null)
        {
            lockedPrompt.SetActive(false);
        }
        
        // 检查谜题是否已解决
        CheckPuzzleState();
    }
    
    /// <summary>
    /// 实现IInteractable接口的Interact方法
    /// </summary>
    public void Interact(PlayerController player)
    {
        if (isLocked)
        {
            // 如果门锁住了，显示锁住提示
            if (lockedPrompt != null)
            {
                StartCoroutine(ShowLockedPrompt());
            }
            return;
        }
        
        // 切换门的状态
        isOpen = !isOpen;
        
        // 更新视觉效果
        UpdateVisuals();
        
        // 如果门打开，播放声音
        if (isOpen)
        {
            // 这里可以添加门打开的声音
            // AudioManager.Instance.PlaySound("DoorOpen");
        }
        else
        {
            // 这里可以添加门关闭的声音
            // AudioManager.Instance.PlaySound("DoorClose");
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
    /// 更新门的视觉效果
    /// </summary>
    private void UpdateVisuals()
    {
        if (visualClosed != null)
        {
            visualClosed.SetActive(!isOpen);
        }
        
        if (visualOpen != null)
        {
            visualOpen.SetActive(isOpen);
        }
    }
    
    /// <summary>
    /// 检查谜题状态
    /// </summary>
    private void CheckPuzzleState()
    {
        if (!string.IsNullOrEmpty(requiredPuzzleID) && PuzzleManager.Instance != null)
        {
            if (PuzzleManager.Instance.IsPuzzleSolved(requiredPuzzleID))
            {
                Unlock();
            }
        }
    }
    
    /// <summary>
    /// 解锁门
    /// </summary>
    public void Unlock()
    {
        isLocked = false;
        
        // 这里可以添加解锁的声音或特效
        // AudioManager.Instance.PlaySound("DoorUnlock");
    }
    
    /// <summary>
    /// 显示锁住提示
    /// </summary>
    private IEnumerator ShowLockedPrompt()
    {
        lockedPrompt.SetActive(true);
        yield return new WaitForSeconds(2f); // 显示2秒
        lockedPrompt.SetActive(false);
    }
    
    /// <summary>
    /// 打开门
    /// </summary>
    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            UpdateVisuals();
            
            // 这里可以添加门打开的声音
            // AudioManager.Instance.PlaySound("DoorOpen");
        }
    }
    
    /// <summary>
    /// 关闭门
    /// </summary>
    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            UpdateVisuals();
            
            // 这里可以添加门关闭的声音
            // AudioManager.Instance.PlaySound("DoorClose");
        }
    }
}