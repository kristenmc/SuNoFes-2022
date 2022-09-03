using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    static private ItemManager _instance;
    static public ItemManager Instance { get { return _instance;}}

    //List to load all scriptable objects to
    [SerializeField] private ItemScriptableObject[] items;
    //Dictionary holding all item SOs keyed by item ID
    [SerializeField] private Dictionary<int, ItemScriptableObject> itemsDict;
    //IDs of the items players own
#region Night Ordering Shop Variables
    //IDs of the items players can buy
    [SerializeField] private List<int> itemShopInventory;
    //Array of item display slots in the shop UI
    [SerializeField] private GameObject[] itemShopDisplaySlots;
    [SerializeField] private TextMeshProUGUI itemShopDisplayName;
    [SerializeField] private TextMeshProUGUI itemShopDisplayDescription;
    [SerializeField] private TextMeshProUGUI itemShopDisplayCost;
    [SerializeField] private TextMeshProUGUI itemShopBudget;
    [SerializeField] private float playerBudget;
#endregion
#region Inventory Variables
    [SerializeField] private List<int> itemPlayerInventory;
    //Array of item display slots in the shop UI
    [SerializeField] private GameObject[] itemInventoryDisplaySlots;
    [SerializeField] private TextMeshProUGUI itemInventoryDisplayName;
    [SerializeField] private TextMeshProUGUI itemInventoryDisplayDescription;
#endregion
    //Amount of money added to the budget at the end of a day
    [SerializeField] private float playerSalary;
    [SerializeField] private ItemScriptableObject currentItem;
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
        currentItem = null;
        itemShopBudget.text = "$" + playerBudget;
        //@Molina TODO:: This might be useful to you
        //Replace with object pooling if there is time
        //Remove and delete objects from itemDisplay
        foreach(GameObject displaySlot in itemShopDisplaySlots)
        {
            displaySlot.SetActive(false);
        }
        //Add new objects to itemDisplay
        int displaySlotIndex = 0; 
        foreach(int item in itemShopInventory)
        {
            itemShopDisplaySlots[displaySlotIndex].SetActive(true);
            Debug.Log("Sprite: " + itemShopDisplaySlots[displaySlotIndex].GetComponent<Image>().sprite);
            Debug.Log("ItemImage: " + ReturnItem(item).ItemImage);
            itemShopDisplaySlots[displaySlotIndex].GetComponent<Image>().sprite = ReturnItem(item).ItemImage;
            displaySlotIndex++;
        }   
    }

    public void DisplayItemInUI(int displaySlotIndex)
    {
        Debug.Log("Button pressted");
        if(displaySlotIndex < itemShopInventory.Count)
        {
            currentItem = ReturnItem(itemShopInventory[displaySlotIndex]);
            itemShopDisplayName.text = currentItem.ItemName;
            itemShopDisplayDescription.text = currentItem.ItemDescription;
            itemShopDisplayCost.text = "$" + currentItem.ItemCost;
        }
    }

    public void BuyItem()
    {
        if(currentItem != null && playerBudget - currentItem.ItemCost >= 0)
        {
            ModifyBudget(-currentItem.ItemCost);
            itemPlayerInventory.Add(currentItem.ItemID);
            //@Molina TODO:: probably change this for the graying out
            itemShopInventory.Remove(currentItem.ItemID);
            UpdateShopInventory();
        }
    }

    //Similar to UpdateShopInventory except it is specifically for player gifting
    //@Molina TODO:: if you edited UpdateShopInventory you probably should edit this as well
    //Or just let me know
    public void UpdateInventory()
    {
        currentItem = null;
        foreach(GameObject displaySlot in itemInventoryDisplaySlots)
        {
            displaySlot.SetActive(false);
        }
        //Add new objects to itemDisplay
        int displaySlotIndex = 0; 
        foreach(int item in itemPlayerInventory)
        {
            itemInventoryDisplaySlots[displaySlotIndex].SetActive(true);
            itemInventoryDisplaySlots[displaySlotIndex].GetComponent<Image>().sprite = ReturnItem(item).ItemImage;
            displaySlotIndex++;
        }
    }

    public void DisplayItemInInventory(int displaySlotIndex)
    {
        Debug.Log("Button pressted");
        if(displaySlotIndex < itemPlayerInventory.Count)
        {
            currentItem = ReturnItem(itemPlayerInventory[displaySlotIndex]);
            itemInventoryDisplayName.text = currentItem.ItemName;
            itemInventoryDisplayDescription.text = currentItem.ItemDescription;
        }
    }

    public void GiveItem()
    {
        if(currentItem != null)
        {
            //@Molina TODO:: probably change this for the graying out
            itemPlayerInventory.Remove(currentItem.ItemID);
            DialogueManager.Instance.GiveItem(currentItem.ItemID);
            GameManager.Instance.CloseUI();
        }
        else
        {
            DialogueManager.Instance.GiveItem(-1);
            GameManager.Instance.CloseUI();
        }
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

    public void AddSalary()
    {
        playerBudget += playerSalary;
    }
}
