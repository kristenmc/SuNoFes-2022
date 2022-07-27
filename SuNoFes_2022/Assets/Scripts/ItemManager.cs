using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    static private ItemManager _instance;
    static public ItemManager Instance { get { return _instance;}}

    //List to load all scriptable objects to
    [SerializeField] private ItemScriptableObject[] items;
    //Dictionary holding all item SOs keyed by item ID
    [SerializeField] private Dictionary<int, ItemScriptableObject> itemsDict;
    //Items players own
    [SerializeField] private int[] itemPlayerInventory;
    //Items players can buy
    [SerializeField] private int[] itemShopInventory;
    [SerializeField] private int playerBudget;
    private void Awake() 
    {
        if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this.gameObject);
            } 
        items = Resources.LoadAll<ItemScriptableObject>("ScriptableObjects/Items");
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach(ItemScriptableObject item in items)
        {
            itemsDict.Add(item.ItemID, item);
        }
        itemPlayerInventory = new int[0];
        itemShopInventory = new int[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generates or updates the shop inventory
    //Should be called at the beginning of a day
    public void UpdateShopInventory()
    {
        //Insert code to generate shop items    
    }

    //Returns the SO for an item given its itemID
    public ItemScriptableObject ReturnItem(int itemID)
    {
        if(itemsDict.ContainsKey(itemID))
        {
            return itemsDict[itemID];
        }
        Debug.Log("Invalid item ID.");
        return null;
    }
}
