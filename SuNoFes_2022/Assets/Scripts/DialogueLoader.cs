using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DialogueLoader : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string sceneName;
        //This var only exists for spreadsheet formatting and has no use in code
        public string startEnd;
        public string speakerName;
        public string displayName;
        //Probably switch this to an enum
        public string speakerExpression;
        public string sfx;
        public string speakerDialogue;
        public string isBranching;
        public string branchingChoice1;
        public int c1pv;
        public int c1nathanpv;
        public int c1drewpv;
        public string branchingChoice2;
        public int c2pv;
        public int c2nathanpv;
        public int c2drewpv;
        public string branchingChoice3;
        public int c3pv;
        public int lineSkip;
    }
    
    [System.Serializable]
    public class DialogueList
    {
        public Dialogue[] dialogue;
    }

    private TextAsset[] dialogueJSONs;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueList dialogueList = new DialogueList();
    [SerializeField] private DialogueList[] dialogueScenes;
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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = DialogueManager.Instance;
        LoadDialogueData();
        LoadGiftDialogueData();
    }

    public void LoadDialogueData()
    {
        dialogueJSONs = character.Scenes;
        dialogueScenes = new DialogueList[dialogueJSONs.Length];
        for(int i = 0; i < dialogueJSONs.Length; i++)
        {
            dialogueList = JsonUtility.FromJson<DialogueList>(dialogueJSONs[i].text);
            dialogueScenes[i] = dialogueList;
        }
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
        if(!dialogueManager.IsDialoguePlaying())
        {
            LoadDialogue();    
            Debug.Log("obj clicked");
        }
    }

    public void LoadDialogue()
    {
        if(character.SceneProgression < dialogueScenes.Length)
        {
            if(character.SceneProgression != 0 && character.SceneProgression !> character.Scenes.Length - 1)
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
        }
    }

    public void LoadGiftDialogue(int ID)
    {
        dialogueManager.SetCanGift(false);
        Dialogue[] giftSceneDialogue = giftScenes[giftScenes.Length - 1].giftScene.dialogue;
        foreach(giftSceneDict gift in giftScenes)
        {
            if(gift.giftID == ID)
            {
                giftSceneDialogue = gift.giftScene.dialogue;
                IncrementCharAffinity(ItemManager.Instance.ReturnItem(ID).ItemAffinity);
            }
        }
        dialogueManager.StartDialogue(giftSceneDialogue, this);
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
    public DialogueLoader.Dialogue[] GetCurrentScene()
    {
        return dialogueScenes[character.SceneProgression].dialogue;
    }
}
