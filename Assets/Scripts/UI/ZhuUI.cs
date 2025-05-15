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
        Debug.Log("����浵��ť");
        // �򿪴浵����
        UIManager.Instance.OpenUI(UIType.SaveUI);
    }

    private void OnBiluButtonClick()
    {
        Debug.Log("�����¼��ť");
        // �򿪱�¼����
        UIManager.Instance.OpenUI(UIType.NotesUI);
    }

    private void OnJinnangButtonClick()
    {
        Debug.Log("������Ұ�ť");
        // �򿪱�¼����
        UIManager.Instance.OpenUI(UIType.TipsPopupUI);
    }

    private void OnKapaiButtonClick()
    {
        Debug.Log("������ư�ť");
        // �򿪿��ƽ���
        UIManager.Instance.OpenUI(UIType.CardPopupUI);
    }

    private void OnDituButtonClick()
    {
        Debug.Log("�����ͼ��ť");
        // �򿪵�ͼ����
        UIManager.Instance.OpenUI(UIType.MapUI);
    }
}
