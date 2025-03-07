using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 谜题管理器，负责管理游戏中的所有谜题
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    // 单例模式
    public static PuzzleManager Instance { get; private set; }
    
    // 所有谜题的字典，键为谜题ID，值为谜题对象
    private Dictionary<string, PuzzleBase> puzzles = new Dictionary<string, PuzzleBase>();
    
    // 已解决的谜题ID列表
    private List<string> solvedPuzzles = new List<string>();
    
    private void Awake()
    {
        // 单例模式初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 注册一个谜题
    /// </summary>
    /// <param name="puzzle">谜题对象</param>
    public void RegisterPuzzle(PuzzleBase puzzle)
    {
        if (!puzzles.ContainsKey(puzzle.PuzzleID))
        {
            puzzles.Add(puzzle.PuzzleID, puzzle);
        }
        else
        {
            Debug.LogWarning($"谜题ID '{puzzle.PuzzleID}' 已存在，无法重复注册");
        }
    }
    
    /// <summary>
    /// 解决一个谜题
    /// </summary>
    /// <param name="puzzleID">谜题ID</param>
    /// <returns>是否成功解决</returns>
    public bool SolvePuzzle(string puzzleID)
    {
        if (puzzles.TryGetValue(puzzleID, out PuzzleBase puzzle))
        {
            if (!solvedPuzzles.Contains(puzzleID))
            {
                puzzle.OnSolved.Invoke();
                solvedPuzzles.Add(puzzleID);
                Debug.Log($"谜题 '{puzzleID}' 已解决");
                return true;
            }
            else
            {
                Debug.Log($"谜题 '{puzzleID}' 已经解决过了");
                return false;
            }
        }
        else
        {
            Debug.LogWarning($"找不到ID为 '{puzzleID}' 的谜题");
            return false;
        }
    }
    
    /// <summary>
    /// 检查谜题是否已解决
    /// </summary>
    /// <param name="puzzleID">谜题ID</param>
    /// <returns>是否已解决</returns>
    public bool IsPuzzleSolved(string puzzleID)
    {
        return solvedPuzzles.Contains(puzzleID);
    }
    
    /// <summary>
    /// 重置一个谜题
    /// </summary>
    /// <param name="puzzleID">谜题ID</param>
    /// <returns>是否成功重置</returns>
    public bool ResetPuzzle(string puzzleID)
    {
        if (puzzles.TryGetValue(puzzleID, out PuzzleBase puzzle))
        {
            if (solvedPuzzles.Contains(puzzleID))
            {
                solvedPuzzles.Remove(puzzleID);
            }
            
            puzzle.Reset();
            Debug.Log($"谜题 '{puzzleID}' 已重置");
            return true;
        }
        else
        {
            Debug.LogWarning($"找不到ID为 '{puzzleID}' 的谜题");
            return false;
        }
    }
    
    /// <summary>
    /// 重置所有谜题
    /// </summary>
    public void ResetAllPuzzles()
    {
        foreach (var puzzle in puzzles.Values)
        {
            puzzle.Reset();
        }
        
        solvedPuzzles.Clear();
        Debug.Log("所有谜题已重置");
    }
    
    /// <summary>
    /// 处理开关状态变化
    /// </summary>
    /// <param name="switchID">开关ID</param>
    /// <param name="isOn">开关状态</param>
    public void OnSwitchStateChanged(string switchID, bool isOn)
    {
        // 查找所有SwitchPuzzle类型的谜题
        foreach (var puzzle in puzzles.Values)
        {
            if (puzzle is SwitchPuzzle switchPuzzle)
            {
                // 更新开关状态
                switchPuzzle.UpdateSwitchState(switchID, isOn);
            }
        }
    }
    
    /// <summary>
    /// 保存谜题状态
    /// </summary>
    public void SavePuzzleState()
    {
        // 这里可以实现保存谜题状态到PlayerPrefs或其他存储系统
        // 示例代码：
        PlayerPrefs.SetInt("SolvedPuzzleCount", solvedPuzzles.Count);
        
        for (int i = 0; i < solvedPuzzles.Count; i++)
        {
            PlayerPrefs.SetString($"SolvedPuzzle_{i}", solvedPuzzles[i]);
        }
        
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 加载谜题状态
    /// </summary>
    public void LoadPuzzleState()
    {
        // 这里可以实现从PlayerPrefs或其他存储系统加载谜题状态
        // 示例代码：
        solvedPuzzles.Clear();
        
        int count = PlayerPrefs.GetInt("SolvedPuzzleCount", 0);
        
        for (int i = 0; i < count; i++)
        {
            string puzzleID = PlayerPrefs.GetString($"SolvedPuzzle_{i}", string.Empty);
            if (!string.IsNullOrEmpty(puzzleID))
            {
                solvedPuzzles.Add(puzzleID);
            }
        }
    }
}