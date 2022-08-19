using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DialogueLoader : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string sceneName;
        public string speakerName;
        //Probably switch this to an enum
        public string speakerExpression;
        public string speakerDialogue;
        public string isBranching;
        public string branchingChoice1;
        public string branchingChoice2;
        public string branchingChoice3;
        public int correctChoice;
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

    // Start is called before the first frame update
    void Start()
    {
        LoadDialogueData();
    }

    public void LoadDialogueData()
    {
        dialogueJSONs = character.Scenes;
        dialogueScenes = new DialogueList[dialogueJSONs.Length];
        dialogueManager = DialogueManager.Instance;
        for(int i = 0; i < dialogueJSONs.Length; i++)
        {
            dialogueList = JsonUtility.FromJson<DialogueList>(dialogueJSONs[i].text);
            dialogueScenes[i] = dialogueList;
        }
    }

    private void OnMouseDown() 
    {
        if(!dialogueManager.isDialoguePlaying())
        {
            LoadDialogue();    
            Debug.Log("obj clicked");
        }
    }

    public void LoadDialogue()
    {
        if(character.SceneProgression < dialogueScenes.Length)
        {
            dialogueManager.StartDialogue(dialogueScenes[character.SceneProgression].dialogue, this);
            character.SceneProgression++;
        }
        else
        {
            //Play ending scene or something idk
        }
    }

    public void IncrementSceneProgression(int incrementAmount)
    {
        character.SceneProgression += incrementAmount;
    }
    public void ResetSceneProgression()
    {
        character.SceneProgression = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
