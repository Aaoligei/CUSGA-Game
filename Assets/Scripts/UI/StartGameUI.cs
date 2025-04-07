using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI { 

    public class StartGameUI : BaseUI
    {
        private void Start()
        {
            AddButtonClickListener("��ʼ��Ϸ��ť",()=>
            {
                // �����ʼ��Ϸ��ťʱ���������˵�����
                Game.Managers.SceneManager.Instance.LoadMainMenu();
            });

            AddButtonClickListener("������Ϸ��ť", () =>
            {
                Game.Managers.SceneManager.Instance.LoadMainMenu();
                //TODO: ���������Ϸ��ťʱ�������ϴδ浵�ĳ���
            });

            AddButtonClickListener("�˳���Ϸ��ť", () =>
            {
                // ����˳���Ϸ��ťʱ���˳���Ϸ
                Application.Quit();
            });
        }
    }


}
