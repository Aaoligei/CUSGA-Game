using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public Button kapai_button;
    public Button liaotian_button;

    public GameObject kapai_zhezhao;
    public DialogueManager dialogueManager;

    public GameObject kaipai_UI;

    private void Start()
    {
        
        kapai_button.onClick.AddListener(OnKapaiButtonClick);
        liaotian_button.onClick.AddListener(OnLiaotianButtonClick);

        kapai_button.gameObject.SetActive(false);
        liaotian_button.gameObject.SetActive(false);

        dialogueManager.DialogueController.RegisterDialogueCallback(4, () => {
            ShowKaPaiButton();
            dialogueManager.DialogueController.isSpecialAction = true;
        });

        dialogueManager.DialogueController.RegisterDialogueCallback(5, () => {
            ShowLiaoTianButton();
        });

    }

    private void OnLiaotianButtonClick()
    {
        kapai_zhezhao.SetActive(false);
    }

    private void OnKapaiButtonClick()
    {
        kapai_zhezhao.SetActive(false);
        kaipai_UI.SetActive(true);
    }

    public void ShowKaPaiButton()
    {
        kapai_button.gameObject.SetActive(true);
        kapai_zhezhao.SetActive(true);
        kapai_zhezhao.GetComponent<Image>().raycastTarget = true;
    }

    public void ShowLiaoTianButton()
    {
        liaotian_button.gameObject.SetActive(true);
        kapai_zhezhao.SetActive(true);
        kapai_zhezhao.GetComponent<Image>().raycastTarget = true;
    }

    //外界调用卡牌提交接口
    public void KapaiSubmit()
    {
        dialogueManager.DialogueController.isSpecialAction = false;
        kaipai_UI.SetActive(false);
    }

}
