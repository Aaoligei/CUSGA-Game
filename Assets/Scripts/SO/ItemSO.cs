using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "new_Item", order = 1)]
public class ItemSO : ScriptableObject {

    [Header("道具名称")]
    public string itemName; // 道具名称
    [Header("道具描述"),TextArea(3, 10)]
    public string itemDescription; // 道具描述
    [Header("道具ID")]
    public int itemID; // 道具ID
    [Header("道具图标")]
    public Sprite itemIcon; // 道具图标
}
