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
        private Button _closeButton;    // 关闭按钮
        private Image _dayMapImage;     // 白天地图图片
        private Image _nightMapImage;   // 夜晚地图图片
        private Image _map;             // 地图图片

        private Button _hanxizaiButton; // 韩熙载房间按钮
        private Button _langcanButton;  // 郎粲房间按钮
        private Button _zhuxianButton;  // 朱先房间按钮
        private Button _lijiButton;     // 李姬房间按钮
        private Button _demingButton;   // 德明和尚房间按钮
        private Button _lijiamingButton; // 李嘉明房间按钮
        private Button _shuyaButton;    // 舒雅房间按钮
        private Button _wangwushanButton; //王屋山房间按钮
        private Button _ruolanButton;   //弱兰房间按钮
        private Button _chenzhiyongButton;//陈致雍房间按钮
        private Button _mainButton;     //回主界面按钮
        private Button _guanjiaButton;  //管家房间按钮

        [SerializeField]private bool isDay = true; // 是否白天

        protected override void OnInit()
        {
            base.OnInit();

            InitComponents();
        }

        private void InitComponents()
        {
            _closeButton = GetButton("关闭按钮");
            _dayMapImage = GetImage("白天背景");
            _nightMapImage = GetImage("夜晚背景");
            _map = GetImage("地图");

            _hanxizaiButton = GetButton("韩熙载房间按钮");
            _langcanButton = GetButton("郎粲房间按钮");
            _zhuxianButton = GetButton("朱先房间按钮");
            _lijiButton = GetButton("李姬房间按钮");
            _demingButton = GetButton("德明和尚房间按钮");
            _lijiamingButton = GetButton("李嘉明房间按钮");
            _shuyaButton = GetButton("舒雅房间按钮");
            _wangwushanButton = GetButton("王屋山房间按钮");
            _ruolanButton = GetButton("弱兰房间按钮");
            _chenzhiyongButton = GetButton("陈致雍房间按钮");
            _mainButton = GetButton("回主界面按钮");
            _guanjiaButton = GetButton("管家房间按钮");
            // 添加按钮点击事件,点击对应按钮切换至对应场景
            AddButtonClickListener("韩熙载房间按钮", () =>{UnitySceneManager.LoadScene("韩熙载"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("郎粲房间按钮", () => { UnitySceneManager.LoadScene("郎粲"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("朱先房间按钮", () => { UnitySceneManager.LoadScene("朱铣"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("李姬房间按钮", () => { UnitySceneManager.LoadScene("李姬"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("德明和尚房间按钮", () => { UnitySceneManager.LoadScene("德明和尚"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("李嘉明房间按钮", () => { UnitySceneManager.LoadScene("李嘉明"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("舒雅房间按钮", () => { UnitySceneManager.LoadScene("舒雅"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("王屋山房间按钮", () => { UnitySceneManager.LoadScene("王屋山"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("弱兰房间按钮", () => { UnitySceneManager.LoadScene("弱兰"); UIManager.Instance.CloseUI(UIType.MapUI); });
            AddButtonClickListener("陈致雍房间按钮", () => { UnitySceneManager.LoadScene("陈致雍"); UIManager.Instance.CloseUI(UIType.MapUI); });
            //AddButtonClickListener("回主界面按钮", () => { UnitySceneManager.LoadScene("主界面"); });
            //AddButtonClickListener("管家房间按钮", () => { UnitySceneManager.LoadScene("管家"); });
            AddButtonClickListener("关闭按钮", () => CloseUI());
        }

        //重写回调OnShow
        public override void OnShow()
        {
            base.OnShow();
            // 显示地图
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


