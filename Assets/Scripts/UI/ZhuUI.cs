using Game.Core;
using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZhuUI : Singleton<ZhuUI>
{
    public Button ditu;
    public Button kapai;
    public Button jinnang;
    public Button bilu;
    public Button cundang;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(EventSystem.current.gameObject);
        ditu.onClick.AddListener(OnDituButtonClick);
        kapai.onClick.AddListener(OnKapaiButtonClick);
        jinnang.onClick.AddListener(OnJinnangButtonClick);
        bilu.onClick.AddListener(OnBiluButtonClick);
        cundang.onClick.AddListener(OnCundangButtonClick);
    }

    private void OnCundangButtonClick()
    {
        Debug.Log("点击存档按钮");
        // 打开存档界面
        UIManager.Instance.OpenUI(UIType.SaveUI);
    }

    private void OnBiluButtonClick()
    {
        Debug.Log("点击笔录按钮");
        // 打开笔录界面
        UIManager.Instance.OpenUI(UIType.NotesUI);
    }

    private void OnJinnangButtonClick()
    {
        Debug.Log("点击锦囊按钮");
        // 打开笔录界面
        UIManager.Instance.OpenUI(UIType.TipsPopupUI);
    }

    private void OnKapaiButtonClick()
    {
        Debug.Log("点击卡牌按钮");
        // 打开卡牌界面
        UIManager.Instance.OpenUI(UIType.CardPopupUI);
    }

    private void OnDituButtonClick()
    {
        Debug.Log("点击地图按钮");
        // 打开地图界面
        UIManager.Instance.OpenUI(UIType.MapUI);
    }
}
