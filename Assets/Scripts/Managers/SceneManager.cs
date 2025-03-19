using UnityEngine;
using System.Collections;
using Game.Core;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Game.Managers
{
    /// <summary>
    /// 场景管理器，负责游戏场景的加载、卸载和切换
    /// </summary>
    public class SceneManager : Singleton<SceneManager>
    {
        // 当前加载的场景名称
        private string _currentSceneName;
        public string CurrentSceneName => _currentSceneName;

        // 是否正在加载场景
        private bool _isLoading = false;
        public bool IsLoading => _isLoading;

        // 场景加载进度
        private float _loadingProgress = 0f;
        public float LoadingProgress => _loadingProgress;

        // 加载界面场景名称
        private const string LOADING_SCENE_NAME = "LoadingScene";

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        /// <summary>
        /// 初始化场景管理器
        /// </summary>
        private void Init()
        {
            // 获取当前场景名称
            _currentSceneName = UnitySceneManager.GetActiveScene().name;
            
            // 注册场景加载完成事件
            UnitySceneManager.sceneLoaded += OnSceneLoaded;
            
            Debug.Log("场景管理器初始化完成");
        }

        /// <summary>
        /// 场景加载完成回调
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _currentSceneName = scene.name;
            
            // 如果不是加载界面，则触发场景切换事件
            if (_currentSceneName != LOADING_SCENE_NAME)
            {
                _isLoading = false;
                GEventSystem.Instance.TriggerEvent(GameEvents.SceneChanged, _currentSceneName);
                Debug.Log($"场景 {_currentSceneName} 加载完成");
            }
        }

        protected override void OnDestroy()
        {
            // 取消注册场景加载完成事件
            UnitySceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// 加载场景（带加载界面）
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void LoadScene(string sceneName)
        {
            if (_isLoading)
            {
                Debug.LogWarning("正在加载场景，请稍后再试");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("场景名称不能为空");
                return;
            }

            StartCoroutine(LoadSceneAsync(sceneName));
        }

        /// <summary>
        /// 异步加载场景（带加载界面）
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            _isLoading = true;
            _loadingProgress = 0f;
            
            // 加载加载界面
            AsyncOperation loadingOperation = UnitySceneManager.LoadSceneAsync(LOADING_SCENE_NAME);
            
            // 等待加载界面加载完成
            while (!loadingOperation.isDone)
            {
                yield return null;
            }
            
            // 加载实际场景
            AsyncOperation operation = UnitySceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            
            // 等待场景加载到90%
            while (operation.progress < 0.9f)
            {
                _loadingProgress = operation.progress;
                yield return null;
            }
            
            // 等待一段时间，确保UI过渡效果完成
            yield return new WaitForSeconds(0.5f);
            
            // 激活场景
            operation.allowSceneActivation = true;
            _loadingProgress = 1f;
            
            // 等待场景完全加载
            while (!operation.isDone)
            {
                yield return null;
            }
        }

        /// <summary>
        /// 直接加载场景（不使用加载界面）
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void LoadSceneDirectly(string sceneName)
        {
            if (_isLoading)
            {
                Debug.LogWarning("正在加载场景，请稍后再试");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("场景名称不能为空");
                return;
            }

            StartCoroutine(LoadSceneDirectlyAsync(sceneName));
        }

        /// <summary>
        /// 异步直接加载场景（不使用加载界面）
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        private IEnumerator LoadSceneDirectlyAsync(string sceneName)
        {
            _isLoading = true;
            _loadingProgress = 0f;
            
            // 加载场景
            AsyncOperation operation = UnitySceneManager.LoadSceneAsync(sceneName);
            
            // 等待场景加载完成
            while (!operation.isDone)
            {
                _loadingProgress = operation.progress;
                yield return null;
            }
            
            _loadingProgress = 1f;
        }

        /// <summary>
        /// 添加场景（不卸载当前场景）
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void AddScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("场景名称不能为空");
                return;
            }

            StartCoroutine(AddSceneAsync(sceneName));
        }

        /// <summary>
        /// 异步添加场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        private IEnumerator AddSceneAsync(string sceneName)
        {
            // 加载场景
            AsyncOperation operation = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            
            // 等待场景加载完成
            while (!operation.isDone)
            {
                yield return null;
            }
            
            Debug.Log($"场景 {sceneName} 添加完成");
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void UnloadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("场景名称不能为空");
                return;
            }

            StartCoroutine(UnloadSceneAsync(sceneName));
        }

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        private IEnumerator UnloadSceneAsync(string sceneName)
        {
            // 卸载场景
            AsyncOperation operation = UnitySceneManager.UnloadSceneAsync(sceneName);
            
            // 等待场景卸载完成
            while (operation != null && !operation.isDone)
            {
                yield return null;
            }
            
            Debug.Log($"场景 {sceneName} 卸载完成");
        }

        /// <summary>
        /// 重新加载当前场景
        /// </summary>
        public void ReloadCurrentScene()
        {
            LoadScene(_currentSceneName);
        }

        /// <summary>
        /// 加载主菜单场景
        /// </summary>
        public void LoadMainMenu()
        {
            LoadScene("MainMenu");
        }

        /// <summary>
        /// 加载搜证场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void LoadInvestigationScene(string sceneName)
        {
            LoadScene(sceneName);
            GameManager.Instance.EnterInvestigationState();
        }

        /// <summary>
        /// 加载卡牌游戏场景
        /// </summary>
        public void LoadCardGameScene()
        {
            LoadScene("CardGame");
            GameManager.Instance.EnterCardGameState();
        }
    }
} 