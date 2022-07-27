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
        public string branchingChoice4;
        
    }
    
    [System.Serializable]
    public class DialogueList
    {
        public Dialogue[] dialogue;
    }

    [SerializeField] private TextAsset dialogueJSON;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueList dialogueList = new DialogueList();

    // Start is called before the first frame update
    void Start()
    {
        dialogueList = JsonUtility.FromJson<DialogueList>(dialogueJSON.text);
        dialogueManager = DialogueManager.Instance;
    }

    public void LoadDialogue()
    {
        dialogueManager.StartDialogue(dialogueList.dialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
