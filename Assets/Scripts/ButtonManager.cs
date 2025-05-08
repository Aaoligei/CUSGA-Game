using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header("���水ť")]
    public Button kapai_button;
    public Button liaotian_button;

    public GameObject kapai_zhezhao;
    public DialogueManager dialogueManager;

    private void Start()
    {
        // ���Ӱ�ť����¼�
        kapai_button.onClick.AddListener(OnKapaiButtonClick);
        liaotian_button.onClick.AddListener(OnLiaotianButtonClick);

        kapai_button.gameObject.SetActive(false);
        liaotian_button.gameObject.SetActive(false);

        dialogueManager.DialogueController.RegisterDialogueCallback(4, () => {
            ShowKaPaiButton();
            dialogueManager.DialogueController.isSpecialAction = true;
        });

    }

    private void OnLiaotianButtonClick()
    {
        throw new NotImplementedException();
    }

    private void OnKapaiButtonClick()
    {
        throw new NotImplementedException();
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


}
