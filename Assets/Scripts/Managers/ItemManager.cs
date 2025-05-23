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

    [SerializeField] private Transform contentParent; // ����ͼ�еġ����ݡ�����
    [SerializeField] private GameObject itemPrefab; // ����Ԥ����

    private Vector2 startPosition = Vector2.zero;
    private float spacing = 10f; // ���߼��

    [SerializeField]
    public List<ItemSO> itemSOList = new List<ItemSO>();
    private Dictionary<int, Item> _items = new Dictionary<int, Item>();//�����ֵ�
    public Dictionary<int, Item> Items => _items;
    public Dictionary<string,Item> _itemsStr = new Dictionary<string, Item>();//�����ֵ���������
    public Dictionary<string, Item> ItemsStr => _itemsStr;

    [SerializeField] private List<Item> itemsInBag;//�����еĵ����б�
    public List<Item> ItemsInBag
    {
        get => itemsInBag;
        set => itemsInBag = value;
    }

    private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        //��SO�ж�ȡ�������ݵ��ֵ���
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

        //��SO�ж�ȡ�������ݵ��ֵ���
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
        // ������е���
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

                    // ���ú�������λ��
                    if (newItemRect != null)
                    {
                        newItemRect.anchoredPosition = new Vector2(totalWidth, 0f);
                        totalWidth += newItemRect.rect.width + spacing;
                    }
                }
            }
        }
    }

    // �����Ҫ��ʱ���±�����ʾ�����Ե��ô˷���
    public void UpdateBagDisplay()
    {
        GenerateBagItems();
    }


}

