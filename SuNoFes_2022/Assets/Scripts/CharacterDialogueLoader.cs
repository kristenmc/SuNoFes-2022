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
    [SerializeField] private bool canTalk;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = DialogueManager.Instance;
        LoadDialogueData();
        LoadGiftDialogueData();
        //ResetSceneProgression();
    }

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
    }

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

    private void OnMouseDown() 
    {
        Debug.Log("clicked on person");
        if(!dialogueManager.IsDialoguePlaying() && canTalk && !GameManager.Instance.IsMenuOpen())
        {
            Debug.Log("stargting dialogue");
            canTalk = false;
            LoadDialogue();    
        }
    }

    public override void LoadDialogue()
    {
        if(character.SceneProgression < dialogueScenes.Length)
        {
            Debug.Log("trying cangift");
            if(character.SceneProgression != 0 && character.SceneProgression < character.Scenes.Length - 1)
            {
                Debug.Log("cangift tru");
                dialogueManager.SetCanGift(true);
            }
            else
            {
                Debug.Log("cangift fals");
                dialogueManager.SetCanGift(false);
            }
            //Sending itself as part of the function call is definitely bad practice 
            //Unfortunately it was the best way to get things to work based off time constraints
            Debug.Log("starting dialogue");
            dialogueManager.StartDialogue(GetCurrentScene(), this);
            character.SceneProgression++;
        }
        else
        {
            //Play ending scene or something idk
        }
    }

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
}
