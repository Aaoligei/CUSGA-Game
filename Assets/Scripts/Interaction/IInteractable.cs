using UnityEngine;

/// <summary>
/// 定义可交互对象的接口
/// 所有可以与玩家交互的游戏对象都应实现此接口
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// 当玩家与对象交互时调用
    /// </summary>
    /// <param name="player">与对象交互的玩家</param>
    void Interact(PlayerController player);
    
    /// <summary>
    /// 显示或隐藏交互提示
    /// </summary>
    /// <param name="show">是否显示提示</param>
    void ShowInteractionPrompt(bool show);
}