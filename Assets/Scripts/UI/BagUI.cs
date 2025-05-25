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
    private string _prefabPath = "Assets/Prefabs/Item/"; // Ԥ����·��

    private ItemManager _itemManager;

    private void Start()
    {
        _itemManager = ItemManager.Instance;

        RegisterUIComponents();

        InitComponents();
        
        // ��ʼ��ʱ���±���UI
        UpdateItemsInBagUI();
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

        // ����з��ذ�ť����ӵ���¼�
        if (_backPropButton != null)
        {
            _backPropButton.onClick.AddListener(() =>
            {
                _itemProp.SetActive(false);
            });
        }

        foreach (Transform child in _itemList)
        {
            // ��ӵ���¼�
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    //������Ʒ˵������
                    _itemProp.SetActive(true);
                    // ��ʾ��Ʒ˵��
                    _itemName.text = GetComponentInChildren<Text>().text.ToString();
                    string itemNameStr = _itemName.text.ToString();
                    ItemManager.Item displayItem = GetItem(itemNameStr);

                    _itemSprite.sprite = displayItem.icon;
                    _itemDescription.text = displayItem.description;

                    foreach (Transform childchild in _itemList)
                    {
                        // ����������Ʒ��ͼƬ��λ��
                        childchild.GetComponent<Image>().sprite = defaultImage;
                        childchild.transform.position = new Vector3(childchild.transform.position.x, child.transform.position.y, childchild.transform.position.z);
                        childchild.GetComponent<Image>().raycastTarget = true; // ���õ���¼�
                    }

                    child.GetComponent<Image>().sprite = selectImage;
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y + 20, child.transform.position.z);
                    child.GetComponent<Image>().raycastTarget = false; // ���õ���¼�
                });
            }
        }
    }

    //�ӵ����ֵ��л�ȡ��������
    private ItemManager.Item GetItem(string str)
    {
        return _itemManager.ItemsStr[str];
    } 

    private void UpdateItemsInBagUI()
    {
        // ������еĵ����б�
        foreach (Transform child in _itemList)
        {
            Destroy(child.gameObject);
        }

        // ����������е��ߣ������ɶ�Ӧ��UIԪ��
        if (_itemManager.ItemsInBag != null && _itemManager.ItemsInBag.Count > 0)
        {
            float spacing = 10f; // ���߼��
            float totalWidth = 0f;
            
            foreach (ItemManager.Item item in _itemManager.ItemsInBag)
            {
                // ��Resources�ļ��м���Ԥ����
                GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item/" + item.name);
                
                if (itemPrefab != null)
                {
                    // ʵ��������Ԥ����
                    GameObject newItem = Instantiate(itemPrefab, _itemList);
                    newItem.name = item.name;
                    
                    // ���õ���ͼ��
                    Image itemImage = newItem.GetComponent<Image>();
                    if (itemImage != null)
                    {
                        itemImage.sprite = defaultImage;
                    }
                    
                    // ���õ�������
                    Text itemText = newItem.GetComponentInChildren<Text>();
                    if (itemText != null)
                    {
                        itemText.text = item.name;
                    }
                    
                    // ���õ��������������������ʾ�����
                    Text countText = newItem.transform.Find("CountText")?.GetComponent<Text>();
                    if (countText != null && item.count > 1)
                    {
                        countText.text = item.count.ToString();
                    }
                    
                    // ��ӵ���¼�
                    Button button = newItem.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.AddListener(() =>
                        {
                            //������Ʒ˵������
                            _itemProp.SetActive(true);
                            
                            // ��ʾ��Ʒ˵��
                            _itemName.text = item.name;
                            _itemSprite.sprite = item.icon;
                            _itemDescription.text = item.description;
                            
                            // �������е��ߵ�ͼƬ��λ��
                            foreach (Transform child in _itemList)
                            {
                                child.GetComponent<Image>().sprite = defaultImage;
                                child.transform.localPosition = new Vector3(child.transform.localPosition.x, _yPos, child.transform.localPosition.z);
                                child.GetComponent<Image>().raycastTarget = true;
                            }
                            
                            // ����ѡ��״̬
                            newItem.GetComponent<Image>().sprite = selectImage;
                            newItem.transform.localPosition = new Vector3(newItem.transform.localPosition.x, _yPos + 20, newItem.transform.localPosition.z);
                            newItem.GetComponent<Image>().raycastTarget = false;
                        });
                    }
                    
                    // ���ú�������λ��
                    RectTransform newItemRect = newItem.GetComponent<RectTransform>();
                    if (newItemRect != null)
                    {
                        newItemRect.anchoredPosition = new Vector2(totalWidth, 0f);
                        totalWidth += newItemRect.rect.width + spacing;
                    }
                }
                else
                {
                    Debug.LogWarning("�Ҳ���Ԥ����: " + item.name);
                }
            }
        }
    }
}
