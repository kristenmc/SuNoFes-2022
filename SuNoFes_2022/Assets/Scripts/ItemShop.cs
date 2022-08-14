using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemShop : MonoBehaviour
{
    public GameObject item;
    public GameObject itemDescription;
    public GameObject itemPurchased;
    public float purchaseTimer;

    public void Description()
    {
        itemDescription.SetActive(true);
    }
    public void Purchase()
    {
        Destroy(item);
        Destroy(itemDescription);itemPurchased.SetActive(true);
    }
    public void ExitScene()
    {
        SceneManager.LoadScene("Phase 1");
    }
    private void Update()
    {
        if (itemPurchased != null)
        {
            if (itemPurchased.activeInHierarchy)
            {
                purchaseTimer += Time.deltaTime;
                if (purchaseTimer >= 3)
                {
                    Destroy(itemPurchased);
                    purchaseTimer = 0;
                }
            }
        }
    }
}