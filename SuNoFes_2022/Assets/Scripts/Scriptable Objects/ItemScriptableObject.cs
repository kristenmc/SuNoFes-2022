using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Name", menuName = "ScriptableObjects/ItemObject", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
#region SO Backing Fields
    [SerializeField] private string _itemName;
    [SerializeField] private string _itemDescription;
    [Tooltip("Unique ID of this item. Used to better organize and code t he items in.")]
    [SerializeField] private int _itemID = -1;
    [Tooltip("How long this item takes to arrive after ordering, in days.")]
    [SerializeField] private int _itemShippingTime;
    [SerializeField] private float _itemCost;
    [Tooltip("Number of uses this item might hold. Most items will have a charge of 1.")]
    [SerializeField] private int _itemAffinity = 1;
    [SerializeField] private GameObject _itemImage;
#endregion

#region SO Getters
    public string ItemName
    {
        get {return _itemName;}
    }
    public string ItemDescription
    {
        get {return _itemDescription;}
    }
    public int ItemID
    {
        get {return _itemID;}
    }
    public int ItemShippingTime
    {
        get {return _itemShippingTime;}
    }
    public float ItemCost
    {
        get {return _itemCost;}
    }
    public int ItemAffinity
    {
        get {return _itemAffinity;}
    }
    public GameObject ItemImage
    {
        get {return _itemImage;}
    }
#endregion
}
