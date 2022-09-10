using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueLoader : MonoBehaviour
{
    //The JSON files are loaded into this class
    //Have fun with all the variables
    //if you ever get a JSON error, its probably because the JSON has an extra comma
    //or its trying to save a variable that doesnt exist in this class
    [System.Serializable]
    public class Dialogue
    {
        public string scene;
        //This var only exists for spreadsheet formatting and has no use in code
        public string startEnd;
        public string speakerName;
        public string displayName;
        //Probably switch this to an enum
        public string speakerExpression;
        public string objectOnScreen;
        public int pointValue;
        public string sfx;
        public string music;
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

    [SerializeField] protected TextAsset[] dialogueJSONs;
    [SerializeField] protected DialogueManager dialogueManager;
        
    [Space(10)]
    [Header("For QoL only, don't input items here")]
    //these variables only exist to allow you to track them in the inspector while the game is running
    [SerializeField] protected DialogueList dialogueList = new DialogueList();
    [SerializeField] protected DialogueList[] dialogueScenes;
    [SerializeField] private int currentScene = 0;

    void Awake()
    {
        LoadDialogueData();
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = DialogueManager.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This loads the JSON file for system scenes
    //Its called at startup as loading the JSON can take a little bit of time
    public virtual void LoadDialogueData()
    {
        dialogueScenes = new DialogueList[dialogueJSONs.Length];
        for(int i = 0; i < dialogueJSONs.Length; i++)
        {
            dialogueList = JsonUtility.FromJson<DialogueList>(dialogueJSONs[i].text);
            dialogueScenes[i] = dialogueList;
        }
    }

    //Starts the dialogue for the current system scene
    //its loader variable for StartDialogue is null
    public virtual void LoadDialogue()
    {
        currentScene++;
        dialogueManager.SetCanGift(false);
        dialogueManager.StartDialogue(dialogueScenes[currentScene - 1 ].dialogue);
    }
}
