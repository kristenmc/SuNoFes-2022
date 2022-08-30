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
    //IDs of the items players own
    [SerializeField] private List<int> itemPlayerInventory;
    //IDs of the items players can buy
    [SerializeField] private List<int> itemShopInventory;
    //List of item GameObjects currently displayed in the shop UI
    [SerializeField] private List<GameObject> itemShopDisplay;
    //Array of item display slots in the shop UI
    [SerializeField] private GameObject[] itemShopDisplaySlots;
    [SerializeField] private float playerBudget;
    //Amount of money added to the budget at the end of a day
    [SerializeField] private float playerSalary;
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
        itemsDict = new Dictionary<int, ItemScriptableObject>();
        foreach(ItemScriptableObject item in items)
        {
            itemsDict.Add(item.ItemID, item);
        }
        /*
        //Debug code
        for(int i = 0; i < itemsDict.Count; i++)
        {
            Debug.Log(itemsDict[i].ItemName);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generates or updates the shop inventory
    //Should be called at the beginning of a day
    public void UpdateShopInventory()
    {
        //@Molina TODO:: This might be useful to you
        //Replace with object pooling if there is time
        //Remove and delete objects from itemDisplay
        for(int i = itemShopDisplay.Count-1; i >= 0; i--)
        {
            GameObject display = itemShopDisplay[i];
            itemShopDisplay.RemoveAt(i);
            Destroy(display);
        }
        //Add new objects to itemDisplay
        int displaySlotIndex = 0; 
        foreach(int item in itemShopInventory)
        {
            GameObject display = Instantiate(ReturnItem(item).ItemImage, itemShopDisplaySlots[displaySlotIndex].transform);
            itemShopDisplay.Add(display);
            displaySlotIndex++;
        }   
    }

    //Similar to UpdateShopInventory except it is specifically for player gifting
    //@Molina TODO:: if you edited UpdateShopInventory you probably should edit this as well
    //Or just let me know
    public void UpdateInventory()
    {

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

    public void ModifyBudget(float money)
    {
        playerBudget += money;
    }

    public void BuyItem(int itemID)
    {
        ItemScriptableObject boughtItem = ReturnItem(itemID);
        ModifyBudget(-boughtItem.ItemCost);
        itemPlayerInventory.Add(itemID);
        //@Molina TODO:: probably change this for the graying out
        itemShopInventory.Remove(itemID);
    }
}
