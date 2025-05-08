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
                //Game.Managers.SceneManager.Instance.LoadMainMenu();
                //��ʱ��Ϊ������������
                Game.Managers.SceneManager.Instance.LoadScene("GuideLevel");
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
