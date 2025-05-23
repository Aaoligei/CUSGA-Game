using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public struct Item
    {
        public string name;
        public string description;
        public Sprite icon;
        public int id;
        public int count;
    }

    [SerializeField] private Transform contentParent; // 拖入图中的“内容”物体
    [SerializeField] private GameObject itemPrefab; // 道具预制体

    private Vector2 startPosition = Vector2.zero;
    private float spacing = 10f; // 道具间距

    [SerializeField]
    public List<ItemSO> itemSOList = new List<ItemSO>();
    private Dictionary<int, Item> _items = new Dictionary<int, Item>();//道具字典
    public Dictionary<int, Item> Items => _items;
    public Dictionary<string,Item> _itemsStr = new Dictionary<string, Item>();//道具字典名字索引
    public Dictionary<string, Item> ItemsStr => _itemsStr;

    [SerializeField] private List<Item> itemsInBag;//背包中的道具列表
    public List<Item> ItemsInBag
    {
        get => itemsInBag;
        set => itemsInBag = value;
    }

    private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        //从SO中读取道具数据到字典中
        _items.Clear();
        foreach (ItemSO itemSO in itemSOList)
        {
            Item item = new Item
            {
                name = itemSO.itemName,
                description = itemSO.itemDescription,
                icon = itemSO.itemIcon,
                id = itemSO.itemID,
                count = 0
            };
            _items.Add(item.id, item);
        }

        //从SO中读取道具数据到字典中
        _itemsStr.Clear();
        foreach (ItemSO itemSO in itemSOList)
        {
            Item item = new Item
            {
                name = itemSO.itemName,
                description = itemSO.itemDescription,
                icon = itemSO.itemIcon,
                id = itemSO.itemID,
                count = 0
            };
            _itemsStr.Add(item.name, item);
        }
    }

    void Start()
    {
        GenerateBagItems();
    }

    public void GenerateBagItems()
    {
        // 清空现有道具
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (ItemsInBag != null)
        {
            float totalWidth = 0f;

            foreach (ItemManager.Item item in ItemsInBag)
            {
                if (itemPrefab != null)
                {
                    GameObject newItem = Instantiate(itemPrefab, contentParent);
                    newItem.name = item.name;

                    RectTransform newItemRect = newItem.GetComponent<RectTransform>();

                    // 设置横向排列位置
                    if (newItemRect != null)
                    {
                        newItemRect.anchoredPosition = new Vector2(totalWidth, 0f);
                        totalWidth += newItemRect.rect.width + spacing;
                    }
                }
            }
        }
    }

    // 如果需要随时更新背包显示，可以调用此方法
    public void UpdateBagDisplay()
    {
        GenerateBagItems();
    }


}

