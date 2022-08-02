using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] availableCharacters;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //TODO: This function should load the available characters and position them in the scene 
    public void LoadShopDay()
    {
        foreach(GameObject character in availableCharacters)
        {
            Debug.Log("Loading character");
        }
    }
}
