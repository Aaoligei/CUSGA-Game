using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI { 

    public class MapUI : BaseUI
    {
        private Button _closeButton;    // �رհ�ť
        private Image _dayMapImage;     // �����ͼͼƬ
        private Image _nightMapImage;   // ҹ���ͼͼƬ
        private Image _map;             // ��ͼͼƬ

        private Image _catalogButtonOutline;   // ͼ����ť���
        private Image _factionButtonOutline;   // ��Ӫ��ť���
        private Image _noteButtonOutline;      // ��¼��ť���
        private Image _lightOffOutline;        // Ϩ�ư�ť���

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

            _catalogButtonOutline = GetImage("ͼ����ť���");
            _factionButtonOutline = GetImage("��Ӫ��ť���");
            _noteButtonOutline = GetImage("��¼��ť���");

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


