using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI { 

    public class StartGameUI : BaseUI
    {
        private void Start()
        {
            AddButtonClickListener("开始游戏按钮",()=>
            {
                // 点击开始游戏按钮时，加载主菜单场景
                Game.Managers.SceneManager.Instance.LoadMainMenu();
            });

            AddButtonClickListener("继续游戏按钮", () =>
            {
                Game.Managers.SceneManager.Instance.LoadMainMenu();
                //TODO: 点击继续游戏按钮时，加载上次存档的场景
            });

            AddButtonClickListener("退出游戏按钮", () =>
            {
                // 点击退出游戏按钮时，退出游戏
                Application.Quit();
            });
        }
    }


}
