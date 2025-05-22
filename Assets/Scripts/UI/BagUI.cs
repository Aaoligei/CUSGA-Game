using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject _itemProp; // ����˵������
    [SerializeField] private Image _itemSprite;   // ����ͼƬ
    [SerializeField] private Text _itemDescription;// ��������
    [SerializeField] private Text _itemName;       // ��������
    [SerializeField] private Button _backPropButton; // ���ذ�ť


    private void Start()
    {
        RegisterUIComponents();

        InitComponents();
    }

    private void InitComponents()
    {
        _itemProp.SetActive(false); // ���ص���˵������
        _itemList = transform.Find("������/���߻���/�ӿ�/����");

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

                        //������Ʒ˵������
                        _itemProp.SetActive(true);
                        Sprite selectSprite = childchild.GetComponentInChildren<Image>().sprite;
                        _itemSprite.sprite = selectSprite;
                        _itemName.text = childchild.GetComponentInChildren<Text>().text;
                    }
                    child.GetComponent<Image>().sprite = selectImage;
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y + 20, child.transform.position.z);
                    child.GetComponent<Image>().raycastTarget = false; // ���õ���¼�
                });
            }
        }
    }

}
