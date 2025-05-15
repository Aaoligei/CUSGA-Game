using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainDialogueRecord : MonoBehaviour
{
    public GameObject XObject; // ������Ҫ���Ƶ�����

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            bool shouldActive = SceneTransitionManager.Instance.previousScene == "GuideLevel2";
            XObject.SetActive(shouldActive);
        }
    }
}
