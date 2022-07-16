using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    static private DialogueManager _instance;
    static public DialogueManager Instance { get { return _instance;}}
    [SerializeField] private Queue<DialogueLoader.Dialogue> dialogueQueue;
    
    [SerializeField] private TextMeshProUGUI displayName;
    [SerializeField] private TextMeshProUGUI displayDialogue;

    private void Awake()
    {
         if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start() 
    {
        dialogueQueue = new Queue<DialogueLoader.Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(DialogueLoader.Dialogue[] dialogue)
    {
        dialogueQueue.Clear();
        foreach(DialogueLoader.Dialogue sentence in dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }

        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLoader.Dialogue sentence = dialogueQueue.Dequeue();
        displayName.text = sentence.speakerName;
        displayDialogue.text = sentence.speakerDialogue;
        //#ToDo: speaker expression stuff goes here
    }
    

    public void EndDialogue()
    {
        //Add any following occurences here
    }
}
