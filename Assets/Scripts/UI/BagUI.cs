using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BagUI : BaseUI
{
    private Button _openBagButton;
    private Animator _ac;
    private bool _isOpen=false;
    public Sprite selectImage; // ѡ����Ʒ��ͼƬ
    public Sprite defaultImage; // Ĭ����Ʒ��ͼƬ
    private float _yPos= -65.73845f; // ��Ʒ�ĳ�ʼy����

    private Transform _itemList; // �����б�



    private void Start()
    {
        RegisterUIComponents();

        InitComponents();
    }

    private void InitComponents()
    {
        _itemList= transform.Find("������/���߻���/�ӿ�/����");

        _openBagButton = GetButton("������ť");
        _ac = GetComponentInChildren<Animator>();

        // ע�ᰴť����¼�
        _openBagButton.onClick.AddListener(() =>
        {
            _isOpen = !_isOpen;
            _ac.SetBool("isOpen", _isOpen);
        });

        foreach (Transform child in _itemList)
        {
            // ��ӵ���¼�
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    foreach (Transform childchild in _itemList)
                    {
                        // ����������Ʒ��ͼƬ��λ��
                        childchild.GetComponent<Image>().sprite = defaultImage;
                        childchild.transform.position = new Vector3(childchild.transform.position.x, child.transform.position.y, childchild.transform.position.z);
                        childchild.GetComponent<Image>().raycastTarget = true; // ���õ���¼�
                    }
                    child.GetComponent<Image>().sprite = selectImage;
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y + 100, child.transform.position.z);
                    child.GetComponent<Image>().raycastTarget = false; // ���õ���¼�
                });
            }
        }
    }

}
