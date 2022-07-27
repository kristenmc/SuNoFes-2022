using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CharacterScriptableObject[] characterScenes;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Occurs when the player talks to a character
    public void LoadCharacterScene(int charNum)
    {
        CharacterScriptableObject currentChar = characterScenes[charNum];
        if(currentChar.SceneProgression < currentChar.Scenes.Length)
        {
            currentChar.Scenes[currentChar.SceneProgression].LoadDialogue();
            characterScenes[charNum].SceneProgression++;
        }
        else
        {
            //End their scenes or something;
        }
    }

    //TODO: This function should load the available characters and position them in the scene 
    public void LoadShopDay()
    {

    }
}
