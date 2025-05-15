using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Game.UI { 

    public class MapUI : BaseUI
    {
        private Button _closeButton;    // �رհ�ť
        private Image _dayMapImage;     // �����ͼͼƬ
        private Image _nightMapImage;   // ҹ���ͼͼƬ
        private Image _map;             // ��ͼͼƬ

        private Button _hanxizaiButton; // �����ط��䰴ť
        private Button _langcanButton;  // ���ӷ��䰴ť
        private Button _zhuxianButton;  // ���ȷ��䰴ť
        private Button _lijiButton;     // ����䰴ť
        private Button _demingButton;   // �������з��䰴ť
        private Button _lijiamingButton; // ��������䰴ť
        private Button _shuyaButton;    // ���ŷ��䰴ť
        private Button _wangwushanButton; //����ɽ���䰴ť
        private Button _ruolanButton;   //�������䰴ť
        private Button _chenzhiyongButton;//����Ӻ���䰴ť
        private Button _mainButton;     //�������水ť
        private Button _guanjiaButton;  //�ܼҷ��䰴ť

        [SerializeField]private bool isDay = true; // �Ƿ����

        protected override void OnInit()
        {
            base.OnInit();

            InitComponents();
        }

        private void InitComponents()
        {
            _closeButton = GetButton("�رհ�ť");
            _dayMapImage = GetImage("���챳��");
            _nightMapImage = GetImage("ҹ����");
            _map = GetImage("��ͼ");

            _hanxizaiButton = GetButton("�����ط��䰴ť");
            _langcanButton = GetButton("���ӷ��䰴ť");
            _zhuxianButton = GetButton("���ȷ��䰴ť");
            _lijiButton = GetButton("����䰴ť");
            _demingButton = GetButton("�������з��䰴ť");
            _lijiamingButton = GetButton("��������䰴ť");
            _shuyaButton = GetButton("���ŷ��䰴ť");
            _wangwushanButton = GetButton("����ɽ���䰴ť");
            _ruolanButton = GetButton("�������䰴ť");
            _chenzhiyongButton = GetButton("����Ӻ���䰴ť");
            _mainButton = GetButton("�������水ť");
            _guanjiaButton = GetButton("�ܼҷ��䰴ť");
            // ��Ӱ�ť����¼�,�����Ӧ��ť�л�����Ӧ����
            AddButtonClickListener("�����ط��䰴ť", () =>{UnitySceneManager.LoadScene("������"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("���ӷ��䰴ť", () => { UnitySceneManager.LoadScene("����"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("���ȷ��䰴ť", () => { UnitySceneManager.LoadScene("��ϳ"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("����䰴ť", () => { UnitySceneManager.LoadScene("�"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("�������з��䰴ť", () => { UnitySceneManager.LoadScene("��������"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("��������䰴ť", () => { UnitySceneManager.LoadScene("�����"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("���ŷ��䰴ť", () => { UnitySceneManager.LoadScene("����"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("����ɽ���䰴ť", () => { UnitySceneManager.LoadScene("����ɽ"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("�������䰴ť", () => { UnitySceneManager.LoadScene("����"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("����Ӻ���䰴ť", () => { UnitySceneManager.LoadScene("����Ӻ"); UIManager.Instance.CloseUI(UIType.MapUI); });
            //AddButtonClickListener("�������水ť", () => { UnitySceneManager.LoadScene("������"); });
            //AddButtonClickListener("�ܼҷ��䰴ť", () => { UnitySceneManager.LoadScene("�ܼ�"); });
            AddButtonClickListener("�رհ�ť", () => CloseUI());
        }

        //��д�ص�OnShow
        public override void OnShow()
        {
            base.OnShow();
            // ��ʾ��ͼ
            if (isDay)
            {
                _dayMapImage.gameObject.SetActive(true);
                _nightMapImage.gameObject.SetActive(false);
            }
            else
            {
                _dayMapImage.gameObject.SetActive(false);
                _nightMapImage.gameObject.SetActive(true);
            }
        }
    }
}


