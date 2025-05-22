using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "new_Item", order = 1)]
public class ItemSO : ScriptableObject {

    [Header("��������")]
    public string itemName; // ��������
    [Header("��������"),TextArea(3, 10)]
    public string itemDescription; // ��������
    [Header("����ID")]
    public int itemID; // ����ID
    [Header("����ͼ��")]
    public Sprite itemIcon; // ����ͼ��
}
