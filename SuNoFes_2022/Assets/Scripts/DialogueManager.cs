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
    [SerializeField] private GameObject nameBackground;
    [SerializeField] private TextMeshProUGUI displayDialogue;
    [SerializeField] GameObject continueButton;
    [SerializeField] DialogueLoader[] clickableCharacters;
    [SerializeField] private bool dialogueInProgress = false;
    [SerializeField] DialogueLoader nathanSO;
    [SerializeField] DialogueLoader drewSO;

#region Choice Button Vars
    [Header("Choice Button Locations")]
    [SerializeField] private float topChoiceY;
    [SerializeField] private float botChoiceY;
    [SerializeField] private float choiceX;
    [SerializeField] private GameObject[] choiceButtons;
    private List<string> availableChoices;
#endregion

#region DialogueBox Positioning Vars
    [SerializeField] private Animator dialogueBoxAnimator;
    [SerializeField] private string dialogueBoxReveal;
    [SerializeField] private string dialogueBoxHide;
#endregion

#region Saved Variables
    private int playerChoice;
    private DialogueLoader.Dialogue dialoguePointValues;    
    private DialogueLoader currentCharacter;
    private int numSkipBefore;
    private int numSkipAfter;
    private int numChoices;
    [SerializeField] private bool sharedScene1Played = false;
    [SerializeField] private bool sharedScene2Played = false;
#endregion

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
    public void StartDialogue(DialogueLoader.Dialogue[] dialogue, DialogueLoader loader)
    {
        
        if(loader.GetCharacterName() == "Nathan")
        {
            if(loader.GetSceneProgression() == 2 && sharedScene1Played)
            {
                loader.IncrementSceneProgression(1);
            }
            else if(loader.GetSceneProgression() == 2 && !sharedScene1Played)
            {
                sharedScene1Played = true;
            }
            if(loader.GetSceneProgression() == 3 && sharedScene2Played)
            {
                loader.IncrementSceneProgression(1);
            }
            else if(loader.GetSceneProgression() == 3 && !sharedScene2Played)
            {
                sharedScene2Played = true;
            }
            dialogue = loader.GetCurrentScene();
        }
        else if(loader.GetCharacterName() == "Drew")
        {
            if(loader.GetSceneProgression() == 1 && sharedScene1Played)
            {
                loader.IncrementSceneProgression(1);
            }
            else if(loader.GetSceneProgression() == 1 && !sharedScene1Played)
            {
                sharedScene1Played = true;
            }
            if(loader.GetSceneProgression() == 3 && sharedScene2Played)
            {
                loader.IncrementSceneProgression(1);
            }
            else if(loader.GetSceneProgression() == 3 && !sharedScene2Played)
            {
                sharedScene2Played = true;
            }
            dialogue = loader.GetCurrentScene();
        }
        currentCharacter = loader;
        dialogueInProgress = true;
        dialogueBoxAnimator.Play(dialogueBoxReveal);
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
        if(sentence.lineSkip > 0)
        {
            sentence = LineSkipHelper(sentence);
            if(sentence == null)
            {
                EndDialogue();
                return;
            }
        }
        if(sentence.displayName != "Thinking")
        {
            nameBackground.SetActive(true);
            displayName.text = sentence.displayName;
            displayDialogue.text = "";
        }
        else
        {
            displayName.text = null;
            displayDialogue.text = "<i>";
        }
        if(displayName.text == null)
        {
            nameBackground.SetActive(false);
        }
        if(sentence.sfx != null)
        {
            //@Kristen TODO:: Add sfx code here based on the string sentence.sfx 
        }
        //#ToDo: speaker expression stuff goes here 
        if(sentence.isBranching != null)
        {
            numSkipBefore = 0;
            numSkipAfter = 0;
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
            if(sentence.isBranching == "isPlayerChoice")
            {
                continueButton.SetActive(false);                
                displayDialogue.text += sentence.speakerDialogue;
                dialoguePointValues = sentence;
                SetUpChoices(availableChoices);
            }
        }
        else
        {
            
            displayDialogue.text += sentence.speakerDialogue;
        }
    }

    public DialogueLoader.Dialogue LineSkipHelper(DialogueLoader.Dialogue sentence)
    {
        if(numSkipBefore > 0)
            {
                int i = sentence.lineSkip - 1;
                while(i > 1)
                {
                    dialogueQueue.Dequeue();
                    i--;
                }
                //Make sure we can still Dequeue
                if(dialogueQueue.Count == 0)
                {
                    return null;
                }
                //If we can dequeue, save the sentence to display
                sentence = dialogueQueue.Dequeue();
                numSkipBefore--;
            }
            else if(numSkipBefore == 0)
            {
                numSkipBefore--;
            }
            else if(numSkipBefore <0 && numSkipAfter > 0)
            {
                int i = sentence.lineSkip - 1;
                while(i > 1)
                {
                    dialogueQueue.Dequeue();
                    i--;
                }
                //Make sure we can still Dequeue
                if(dialogueQueue.Count == 0)
                {
                    return null;
                }
                //If we can dequeue, save the sentence to display
                sentence = dialogueQueue.Dequeue();
                numSkipAfter--;
            }
            return sentence;
    }
    
    //Ends the dialogue
    public void EndDialogue()
    {
        //Add any following occurences here
        dialogueBoxAnimator.Play(dialogueBoxHide);
        dialogueInProgress = false;
    }

    //Gives an item to a NPC the player is talking to
    public void GiveItem(int itemID)
    {

    }

    public void SetUpChoices(List<string> choices)
    {
        numChoices = choices.Count;
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
        if(playerChoice == 0)
        {
            numSkipBefore = 0;
            numSkipAfter = numChoices - 1;
            if(dialoguePointValues.c1nathanpv == 0 && dialoguePointValues.c1drewpv == 0)
            {
                currentCharacter.IncrementCharAffinity(dialoguePointValues.c1pv);
            }
            else
            {
                nathanSO.IncrementCharAffinity(dialoguePointValues.c1nathanpv);
                drewSO.IncrementCharAffinity(dialoguePointValues.c1drewpv);
            }
            ContinueDialogue();
        }
        else if(playerChoice == 1)
        {
            numSkipBefore = 1;
            numSkipAfter = numChoices - 2;
            if(dialoguePointValues.c2nathanpv == 0 && dialoguePointValues.c2drewpv == 0)
            {
                currentCharacter.IncrementCharAffinity(dialoguePointValues.c2pv);
            }
            else
            {
                nathanSO.IncrementCharAffinity(dialoguePointValues.c1nathanpv);
                drewSO.IncrementCharAffinity(dialoguePointValues.c1drewpv);
            }
            ContinueDialogue();
        }
        else
        {
            numSkipBefore = 2;
            numSkipAfter = 0;
            currentCharacter.IncrementCharAffinity(dialoguePointValues.c3pv);
            ContinueDialogue();
        }
    }

    public void ResetAllSceneProgression()
    {
        foreach(DialogueLoader character in clickableCharacters)
        {
            character.ResetSceneProgression();
        }
    }

    public bool isDialoguePlaying()
    {
        return dialogueInProgress;
    }
}
