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
    [SerializeField] private int playerChoice;
    [SerializeField] GameObject continueButton;
    
    [Header("Choice Button Locations")]
    [SerializeField] private float topChoiceY;
    [SerializeField] private float botChoiceY;
    [SerializeField] private float choiceX;
    [SerializeField] private GameObject[] choiceButtons;
    private List<string> availableChoices;


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
        availableChoices = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Starts and sets up the dialogue system
    public void StartDialogue(DialogueLoader.Dialogue[] dialogue)
    {
        dialogueQueue.Clear();
        foreach(DialogueLoader.Dialogue sentence in dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }

        ContinueDialogue();
    }

    //Opens the next part of the dialogue
    public void ContinueDialogue()
    {
        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLoader.Dialogue sentence = dialogueQueue.Dequeue();
        displayName.text = sentence.speakerName;
        //#ToDo: speaker expression stuff goes here
        if(sentence.isBranching == "isPlayerChoice")
        {
            availableChoices.Clear();
            continueButton.SetActive(false);
            if(sentence.branchingChoice1 != null)
            {
                availableChoices.Add(sentence.branchingChoice1);
            }
            if(sentence.branchingChoice2 != null)
            {
                availableChoices.Add(sentence.branchingChoice2);
            }
            if(sentence.branchingChoice3 != null)
            {
                availableChoices.Add(sentence.branchingChoice3);
            }
            if(sentence.branchingChoice4 != null)
            {
                availableChoices.Add(sentence.branchingChoice4);
            }
            displayDialogue.text = sentence.speakerDialogue;
            SetUpChoices(availableChoices);
        }
        else if(sentence.isBranching == "isNPCResponse")
        {
            availableChoices.Clear();
            if(sentence.branchingChoice1 != null)
            {
                availableChoices.Add(sentence.branchingChoice1);
            }
            if(sentence.branchingChoice2 != null)
            {
                availableChoices.Add(sentence.branchingChoice2);
            }
            if(sentence.branchingChoice3 != null)
            {
                availableChoices.Add(sentence.branchingChoice3);
            }
            if(sentence.branchingChoice4 != null)
            {
                availableChoices.Add(sentence.branchingChoice4);
            }
            displayDialogue.text = availableChoices[playerChoice];
        }
        else
        {
            displayDialogue.text = sentence.speakerDialogue;
        }
    }
    
    //Ends the dialogue
    public void EndDialogue()
    {
        //Add any following occurences here
    }

    //Gives an item to a NPC the player is talking to
    public void GiveItem(int itemID)
    {

    }

    public void SetUpChoices(List<string> choices)
    {
        int numChoices = choices.Count;
        float buttonSeperation = (topChoiceY - botChoiceY)/(numChoices - 1);
        for(int i = 0; i < numChoices; i++)
        {
            GameObject currentButton = choiceButtons[i];
            currentButton.SetActive(true);
            currentButton.GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
            currentButton.GetComponent<RectTransform>().localPosition = new Vector2(choiceX, topChoiceY - (i * buttonSeperation));
        }
    }

    public void ChooseDialogue(int choiceNumber)
    {
        playerChoice = choiceNumber;
        continueButton.SetActive(true);
        foreach(GameObject button in choiceButtons)
        {
            button.SetActive(false);
        }
        //TODO: check if choice was correct and add or subtract affinity
        ContinueDialogue();
    }
}
