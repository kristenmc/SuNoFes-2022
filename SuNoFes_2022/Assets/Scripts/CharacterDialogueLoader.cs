using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterDialogueLoader : DialogueLoader
{
    [SerializeField] private CharacterScriptableObject character;

    #region Gift JSON Vars
    private CharacterScriptableObject.giftJSONDict[] giftJSONs;
    
    [System.Serializable]
    public class giftSceneDict
    {
        public int giftID;
        public DialogueList giftScene;
    }
    [SerializeField] private giftSceneDict[] giftScenes;
    [SerializeField] private DialogueList warningScene;
    [SerializeField] private DialogueList finalScene;
    [SerializeField] private bool canTalk;
    [SerializeField] private CapsuleCollider clickableCollider;
    [SerializeField] private GameObject clickableModel;
    [SerializeField] private GameObject[] characterExpressions;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = DialogueManager.Instance;
        LoadDialogueData();
        LoadGiftDialogueData();
        ResetSceneProgression();
    }

    //This loads the JSON file for character scenes
    //Its called at startup as loading the JSON can take a little bit of time
    public override void LoadDialogueData()
    {
        dialogueJSONs = character.Scenes;
        dialogueScenes = new DialogueList[dialogueJSONs.Length];
        for(int i = 0; i < dialogueJSONs.Length; i++)
        {
            dialogueList = JsonUtility.FromJson<DialogueList>(dialogueJSONs[i].text);
            dialogueScenes[i] = dialogueList;
        }
        warningScene = JsonUtility.FromJson<DialogueList>(character.WarningScene.text);
        finalScene = JsonUtility.FromJson<DialogueList>(character.FinalScene.text);
    }

    //This loads the JSON file for character gift responses
    //Its also called at startup 
    public void LoadGiftDialogueData()
    {
        giftJSONs = character.CharacterGifts;
        giftScenes = new giftSceneDict[giftJSONs.Length];

        for(int i = 0; i < giftJSONs.Length; i++)
        {
            giftScenes[i] = new giftSceneDict();
            dialogueList = JsonUtility.FromJson<DialogueList>(giftJSONs[i].giftScene.text);
            giftScenes[i].giftID = giftJSONs[i].giftID;
            giftScenes[i].giftScene = dialogueList;
        }
    }

    //This function reads if the player has clicked on an NPC
    private void OnMouseDown() 
    {
        if(!dialogueManager.IsDialoguePlaying() && canTalk && !GameManager.Instance.IsMenuOpen() && GameManager.Instance.ConversationAvailable())
        {
            ToggleClickableObject(false);
            GameManager.Instance.ReduceNumConversations();
            if(!GameManager.Instance.ConversationAvailable())
            {
                GameManager.Instance.HideCharacters();
            }
            canTalk = false;
            LoadDialogue();    
        }
    }

    //This loader is for normal character dialogue
    public override void LoadDialogue()
    {
        if(character.SceneProgression < dialogueScenes.Length)
        {
            if(character.SceneProgression != 0 && character.SceneProgression < character.Scenes.Length - 1)
            {
                dialogueManager.SetCanGift(true);
            }
            else
            {
                dialogueManager.SetCanGift(false);
            }
            //Sending itself as part of the function call is definitely bad practice 
            //Unfortunately it was the best way to get things to work based off time constraints
            dialogueManager.StartDialogue(GetCurrentScene(), this);
            character.SceneProgression++;
        }
        else
        {
            //Play ending scene or something idk
            if(character.CharacterAffinity > character.AffinityMax)
            {
                dialogueManager.StartDialogue(finalScene.dialogue, this);
            }
        }
    }

    //This loader is for NPC responses to gifts given (or sold if thats what you want to call it)
    public void LoadGiftDialogue(int ID)
    {
        Debug.Log("trying to gift: " + ID);
        dialogueManager.SetCanGift(false);
        Dialogue[] giftSceneDialogue = giftScenes[giftScenes.Length - 1].giftScene.dialogue;
        if(ID == -1)
        {
            giftSceneDialogue = giftScenes[giftScenes.Length - 2].giftScene.dialogue;
        }
        else
        {
            foreach(giftSceneDict gift in giftScenes)
            {
                if(gift.giftID == ID)
                {
                    giftSceneDialogue = gift.giftScene.dialogue;
                    IncrementCharAffinity(ItemManager.Instance.ReturnItem(ID).ItemAffinity);
                }
            }
        }
        dialogueManager.StartDialogue(giftSceneDialogue, this, true);
    }

    public void IncrementSceneProgression(int incrementAmount)
    {
        character.SceneProgression += incrementAmount;
    }

    public void IncrementCharAffinity(int incrementAmount)
    {
        character.CharacterAffinity += incrementAmount;
    }

    //This resets the scriptable objects because their data persists between scene resets
    //As such this must be called at the beginning of the scene if you want a fresh start
    public void ResetSceneProgression()
    {
        character.SceneProgression = 0;
        character.CharacterAffinity = 0;
    }

    public string GetCharacterName()
    {
        return character.CharacterName;
    }

    public int GetSceneProgression()
    {
        return character.SceneProgression;
    }

    //Returns the current scene
    public CharacterDialogueLoader.Dialogue[] GetCurrentScene()
    {
        return dialogueScenes[character.SceneProgression].dialogue;
    }

    public CharacterScriptableObject GetCharacterSO()
    {
        return character;
    }
    
    public void SetTalk(bool var)
    {
        canTalk = var;
    }

    public DialogueList GetWarningScene()
    {
        return warningScene;
    }

    //Turns the model off or one depending on the passed in variable
    public void ToggleClickableObject(bool state)
    {
        clickableCollider.enabled = state;
        clickableModel.SetActive(state);
    }
}
