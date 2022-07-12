using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DialogueLoader : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string speakerName;
        //Probably switch this to an enum
        public string speakerExpression;
        public string speakerDialogue;
    }
    
    [System.Serializable]
    public class DialogueList
    {
        public Dialogue[] dialogue;
    }

    [SerializeField] TextAsset dialogueJSON;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] DialogueList dialogueList = new DialogueList();

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
