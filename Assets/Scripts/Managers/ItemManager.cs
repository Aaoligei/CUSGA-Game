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

    [SerializeField]
    public List<ItemSO> itemSOList = new List<ItemSO>();
    private Dictionary<int, Item> _items = new Dictionary<int, Item>();//道具字典
    public Dictionary<int, Item> Items => _items;
    [SerializeField] private List<Item> ItemsInBag;//背包中的道具列表

    private void Awake()
    {
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
    }

}
