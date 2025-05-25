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
    private string _prefabPath = "Assets/Prefabs/Item/"; // 预制体路径

    private ItemManager _itemManager;

    private void Start()
    {
        _itemManager = ItemManager.Instance;

        RegisterUIComponents();

        InitComponents();
        
        // 初始化时更新背包UI
        UpdateItemsInBagUI();
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

        // 如果有返回按钮，添加点击事件
        if (_backPropButton != null)
        {
            _backPropButton.onClick.AddListener(() =>
            {
                _itemProp.SetActive(false);
            });
        }

        foreach (Transform child in _itemList)
        {
            // 添加点击事件
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    //弹出物品说明界面
                    _itemProp.SetActive(true);
                    // 显示物品说明
                    _itemName.text = GetComponentInChildren<Text>().text.ToString();
                    string itemNameStr = _itemName.text.ToString();
                    ItemManager.Item displayItem = GetItem(itemNameStr);

                    _itemSprite.sprite = displayItem.icon;
                    _itemDescription.text = displayItem.description;

                    foreach (Transform childchild in _itemList)
                    {
                        // 重置所有物品的图片和位置
                        childchild.GetComponent<Image>().sprite = defaultImage;
                        childchild.transform.position = new Vector3(childchild.transform.position.x, child.transform.position.y, childchild.transform.position.z);
                        childchild.GetComponent<Image>().raycastTarget = true; // 启用点击事件
                    }

                    child.GetComponent<Image>().sprite = selectImage;
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y + 20, child.transform.position.z);
                    child.GetComponent<Image>().raycastTarget = false; // 禁用点击事件
                });
            }
        }
    }

    //从道具字典中获取道具数据
    private ItemManager.Item GetItem(string str)
    {
        return _itemManager.ItemsStr[str];
    } 

    private void UpdateItemsInBagUI()
    {
        // 清空现有的道具列表
        foreach (Transform child in _itemList)
        {
            Destroy(child.gameObject);
        }

        // 如果背包中有道具，则生成对应的UI元素
        if (_itemManager.ItemsInBag != null && _itemManager.ItemsInBag.Count > 0)
        {
            float spacing = 10f; // 道具间距
            float totalWidth = 0f;
            
            foreach (ItemManager.Item item in _itemManager.ItemsInBag)
            {
                // 从Resources文件夹加载预制体
                GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item/" + item.name);
                
                if (itemPrefab != null)
                {
                    // 实例化道具预制体
                    GameObject newItem = Instantiate(itemPrefab, _itemList);
                    newItem.name = item.name;
                    
                    // 设置道具图标
                    Image itemImage = newItem.GetComponent<Image>();
                    if (itemImage != null)
                    {
                        itemImage.sprite = defaultImage;
                    }
                    
                    // 设置道具名称
                    Text itemText = newItem.GetComponentInChildren<Text>();
                    if (itemText != null)
                    {
                        itemText.text = item.name;
                    }
                    
                    // 设置道具数量（如果有数量显示组件）
                    Text countText = newItem.transform.Find("CountText")?.GetComponent<Text>();
                    if (countText != null && item.count > 1)
                    {
                        countText.text = item.count.ToString();
                    }
                    
                    // 添加点击事件
                    Button button = newItem.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.AddListener(() =>
                        {
                            //弹出物品说明界面
                            _itemProp.SetActive(true);
                            
                            // 显示物品说明
                            _itemName.text = item.name;
                            _itemSprite.sprite = item.icon;
                            _itemDescription.text = item.description;
                            
                            // 重置所有道具的图片和位置
                            foreach (Transform child in _itemList)
                            {
                                child.GetComponent<Image>().sprite = defaultImage;
                                child.transform.localPosition = new Vector3(child.transform.localPosition.x, _yPos, child.transform.localPosition.z);
                                child.GetComponent<Image>().raycastTarget = true;
                            }
                            
                            // 设置选中状态
                            newItem.GetComponent<Image>().sprite = selectImage;
                            newItem.transform.localPosition = new Vector3(newItem.transform.localPosition.x, _yPos + 20, newItem.transform.localPosition.z);
                            newItem.GetComponent<Image>().raycastTarget = false;
                        });
                    }
                    
                    // 设置横向排列位置
                    RectTransform newItemRect = newItem.GetComponent<RectTransform>();
                    if (newItemRect != null)
                    {
                        newItemRect.anchoredPosition = new Vector2(totalWidth, 0f);
                        totalWidth += newItemRect.rect.width + spacing;
                    }
                }
                else
                {
                    Debug.LogWarning("找不到预制体: " + item.name);
                }
            }
        }
    }
}
