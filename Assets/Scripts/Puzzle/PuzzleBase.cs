using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 谜题基类，所有具体谜题类型都应继承自此类
/// </summary>
public abstract class PuzzleBase : MonoBehaviour
{
    [Header("谜题基本设置")]
    [Tooltip("谜题的唯一标识符")]
    [SerializeField] private string puzzleID;
    
    [Tooltip("谜题解决时触发的事件")]
    [SerializeField] private UnityEvent onSolved;
    
    [Tooltip("谜题是否已经解决")]
    [SerializeField] private bool isSolved = false;
    
    /// <summary>
    /// 谜题的唯一标识符
    /// </summary>
    public string PuzzleID => puzzleID;
    
    /// <summary>
    /// 谜题解决时触发的事件
    /// </summary>
    public UnityEvent OnSolved => onSolved;
    
    /// <summary>
    /// 谜题是否已经解决
    /// </summary>
    public bool IsSolved => isSolved;
    
    protected virtual void Awake()
    {
        // 确保谜题ID不为空
        if (string.IsNullOrEmpty(puzzleID))
        {
            puzzleID = System.Guid.NewGuid().ToString();
            Debug.LogWarning($"谜题ID为空，已自动生成ID: {puzzleID}");
        }
        
        // 注册到谜题管理器
        if (PuzzleManager.Instance != null)
        {
            PuzzleManager.Instance.RegisterPuzzle(this);
        }
        else
        {
            Debug.LogError("PuzzleManager实例不存在，无法注册谜题");
        }
    }
    
    /// <summary>
    /// 解决谜题
    /// </summary>
    public virtual void Solve()
    {
        if (!isSolved)
        {
            isSolved = true;
            
            // 通知谜题管理器
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.SolvePuzzle(puzzleID);
            }
            else
            {
                // 如果管理器不存在，直接触发事件
                onSolved?.Invoke();
            }
        }
    }
    
    /// <summary>
    /// 重置谜题
    /// </summary>
    public virtual void Reset()
    {
        isSolved = false;
        OnReset();
    }
    
    /// <summary>
    /// 子类重写此方法以实现特定的重置逻辑
    /// </summary>
    protected abstract void OnReset();
    
    /// <summary>
    /// 检查谜题是否可以解决
    /// </summary>
    /// <returns>谜题是否可以解决</returns>
    public abstract bool CheckSolution();
}