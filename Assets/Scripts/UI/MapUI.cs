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
        private Button _closeButton;    // πÿ±’∞¥≈•
        private Image _dayMapImage;     // ∞◊ÃÏµÿÕºÕº∆¨
        private Image _nightMapImage;   // “πÕÌµÿÕºÕº∆¨
        private Image _map;             // µÿÕºÕº∆¨

        private Image _catalogButtonOutline;   // Õºº¯∞¥≈•√Ë±ﬂ
        private Image _factionButtonOutline;   // ’Û”™∞¥≈•√Ë±ﬂ
        private Image _noteButtonOutline;      // ± ¬º∞¥≈•√Ë±ﬂ
        private Image _lightOffOutline;        // œ®µ∆∞¥≈•√Ë±ﬂ

        [SerializeField]private bool isDay = true; //  «∑Ò∞◊ÃÏ

        protected override void OnInit()
        {
            base.OnInit();

            InitComponents();
        }

        private void InitComponents()
        {
            _closeButton = GetButton("πÿ±’∞¥≈•");
            _dayMapImage = GetImage("∞◊ÃÏ±≥æ∞");
            _nightMapImage = GetImage("“πÕÌ±≥æ∞");
            _map = GetImage("µÿÕº");

            _catalogButtonOutline = GetImage("Õºº¯∞¥≈•√Ë±ﬂ");
            _factionButtonOutline = GetImage("’Û”™∞¥≈•√Ë±ﬂ");
            _noteButtonOutline = GetImage("± ¬º∞¥≈•√Ë±ﬂ");

            AddButtonClickListener("πÿ±’∞¥≈•", () => CloseUI());
        }

        //÷ÿ–¥ªÿµ˜OnShow
        public override void OnShow()
        {
            base.OnShow();
            // œ‘ æµÿÕº
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


