using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int currentGameDay = 0;
    [SerializeField] private int maxGameDays;
    [SerializeField] private DialogueLoader genericDialogueLoader;
    
    [System.Serializable]
    public class CharacterPositioning
    {
        public GameObject character;
        public Transform[] characterPositions;
    }
    [SerializeField] private List<CharacterPositioning> availableCharacters;
    [SerializeField] private DialogueLoader.Dialogue[] warningLoader;
    // Start is called before the first frame update
    void Start()
    {
        LoadShopDay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //There should be a button that the player can press to end the game
    public void EndDay()
    {
        if(!DialogueManager.Instance.IsDialoguePlaying())
        {
            currentGameDay ++;
            if(currentGameDay >= maxGameDays)
            {
                genericDialogueLoader.LoadDialogue();
            }
            else
            {
                LoadShopDay();
            }
        }
    }

    //TODO: This function should load the available characters and position them in the scene 
    public void LoadShopDay()
    {
        UpdateAvailableCharacters();
        for(int i = 0; i < availableCharacters.Count; i++)
        {
            CharacterPositioning positioningData = availableCharacters[i];
            positioningData.character.GetComponent<CharacterDialogueLoader>().SetTalk(true);
            positioningData.character.transform.position = positioningData.characterPositions[currentGameDay].position;
        }
        if(currentGameDay == 0)
        {
            genericDialogueLoader.LoadDialogue();
        }
        else if(currentGameDay > 1)
        {
            //load night order UI here
        }
    }

    public void UpdateAvailableCharacters()
    {
        warningLoader = new DialogueLoader.Dialogue[0];
        for(int i = availableCharacters.Count - 1; i >= 0; i--)
        {
            CharacterDialogueLoader currentCharacter = availableCharacters[i].character.GetComponent<CharacterDialogueLoader>();
            CharacterScriptableObject currentCharacterSO = currentCharacter.GetCharacterSO();
            int scenesLeft = currentCharacterSO.Scenes.Length - currentCharacterSO.SceneProgression;
            Debug.Log("Scenes Left: " + scenesLeft);
            int daysLeft = maxGameDays - currentGameDay;
            Debug.Log("Days Left: " + daysLeft);
            if(scenesLeft == daysLeft)
            {
                //Insert Warning info here
                DialogueLoader.DialogueList currentWarning = currentCharacter.GetWarningScene();
                DialogueLoader.Dialogue[] concatArray = new DialogueLoader.Dialogue[warningLoader.Length + currentWarning.dialogue.Length];
                Debug.Log("new Array created"); 
                warningLoader.CopyTo(concatArray, 0);
                Debug.Log("first copy complete");
                currentWarning.dialogue.CopyTo(concatArray, warningLoader.Length);
                Debug.Log("second copy complete");
                warningLoader = concatArray;
                DialogueManager.Instance.StartDialogue(warningLoader);
                Debug.Log("dialogue started");
            }
            else if(scenesLeft > daysLeft)
            {
                availableCharacters[i].character.SetActive(false);
                availableCharacters.RemoveAt(i);
            }
        }
    }
}
