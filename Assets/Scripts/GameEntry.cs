using UnityEngine;
using Game.Managers;
using Game.Core;

/// <summary>
/// 游戏入口类，负责初始化和启动游戏
/// </summary>
public class GameEntry : MonoBehaviour
{
    // 是否自动加载主菜单
    [SerializeField] private bool _autoLoadMainMenu = true;

    private void Awake()
    {
        // 初始化单例管理器
        InitManagers();
        
        // 注册全局事件
        RegisterEvents();
    }

    private void Start()
    {
        // 如果设置了自动加载主菜单，则加载主菜单场景
        if (_autoLoadMainMenu)
        {
            Game.Managers.SceneManager.Instance.LoadMainMenu();
        }
    }

    /// <summary>
    /// 初始化单例管理器
    /// </summary>
    private void InitManagers()
    {
        // 确保EventSystem实例已创建
        GEventSystem eventSystem = GEventSystem.Instance;
        Debug.Log("EventSystem 初始化完成");
        
        // 确保GameManager实例已创建
        GameManager gameManager = GameManager.Instance;
        Debug.Log("GameManager 初始化完成");
        
        // 确保DataManager实例已创建
        DataManager dataManager = DataManager.Instance;
        Debug.Log("DataManager 初始化完成");
        
        // 确保SceneManager实例已创建
        Game.Managers.SceneManager sceneManager = Game.Managers.SceneManager.Instance;
        Debug.Log("SceneManager 初始化完成");
        
        // 确保UIManager实例已创建
        UIManager uiManager = UIManager.Instance;
        Debug.Log("UIManager 初始化完成");
        
        Debug.Log("所有管理器初始化完成");
    }

    /// <summary>
    /// 注册全局事件
    /// </summary>
    private void RegisterEvents()
    {
        // 注册游戏开始事件
        GEventSystem.Instance.AddListener(GameEvents.GameStart, OnGameStart);
        
        // 注册游戏结束事件
        GEventSystem.Instance.AddListener(GameEvents.GameOver, OnGameOver);
        
        Debug.Log("全局事件注册完成");
    }

    /// <summary>
    /// 游戏开始事件处理
    /// </summary>
    /// <param name="parameters">参数数组</param>
    private void OnGameStart(object[] parameters)
    {
        Debug.Log("游戏开始事件触发");
        
        // 在这里可以执行游戏开始时的逻辑
        // 例如播放开场动画、音乐等
    }

    /// <summary>
    /// 游戏结束事件处理
    /// </summary>
    /// <param name="parameters">参数数组</param>
    private void OnGameOver(object[] parameters)
    {
        Debug.Log("游戏结束事件触发");
        
        // 在这里可以执行游戏结束时的逻辑
        // 例如显示结算界面、保存游戏等
    }

    private void OnDestroy()
    {
        // 取消注册全局事件
        if (GEventSystem.Instance != null)
        {
            GEventSystem.Instance.RemoveListener(GameEvents.GameStart, OnGameStart);
            GEventSystem.Instance.RemoveListener(GameEvents.GameOver, OnGameOver);
        }
        
        Debug.Log("游戏入口销毁");
    }
} 