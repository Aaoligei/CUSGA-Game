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
    public Sprite selectImage; // 选中物品的图片
    public Sprite defaultImage; // 默认物品的图片
    private float _yPos= -65.73845f; // 物品的初始y坐标

    private Transform _itemList; // 道具列表
    [SerializeField] private GameObject _itemProp; // 道具说明界面
    [SerializeField] private Image _itemSprite;   // 道具图片
    [SerializeField] private Text _itemDescription;// 道具描述
    [SerializeField] private Text _itemName;       // 道具名称
    [SerializeField] private Button _backPropButton; // 返回按钮


    private void Start()
    {
        RegisterUIComponents();

        InitComponents();
    }

    private void InitComponents()
    {
        _itemProp.SetActive(false); // 隐藏道具说明界面
        _itemList = transform.Find("背包栏/道具滑条/视口/内容");

        _openBagButton = GetButton("包袱按钮");
        _ac = GetComponentInChildren<Animator>();

        // 注册按钮点击事件
        _openBagButton.onClick.AddListener(() =>
        {
            _isOpen = !_isOpen;
            _ac.SetBool("isOpen", _isOpen);
        });

        foreach (Transform child in _itemList)
        {
            // 添加点击事件
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    foreach (Transform childchild in _itemList)
                    {
                        // 重置所有物品的图片和位置
                        childchild.GetComponent<Image>().sprite = defaultImage;
                        childchild.transform.position = new Vector3(childchild.transform.position.x, child.transform.position.y, childchild.transform.position.z);
                        childchild.GetComponent<Image>().raycastTarget = true; // 启用点击事件

                        //弹出物品说明界面
                        _itemProp.SetActive(true);
                        Sprite selectSprite = childchild.GetComponentInChildren<Image>().sprite;
                        _itemSprite.sprite = selectSprite;
                        _itemName.text = childchild.GetComponentInChildren<Text>().text;
                    }
                    child.GetComponent<Image>().sprite = selectImage;
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y + 20, child.transform.position.z);
                    child.GetComponent<Image>().raycastTarget = false; // 禁用点击事件
                });
            }
        }
    }

}
