using Game.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    public string previousScene; // ��¼��Դ��������

    public void LoadSceneWithRecord(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
