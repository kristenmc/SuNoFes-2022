using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] ItemScriptableObject[] items;
    private void Awake() 
    {
        items = Resources.LoadAll<ItemScriptableObject>("ScriptableObjects/Items");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
