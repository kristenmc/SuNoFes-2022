using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    static private DialogueManager _instance;
    static public DialogueManager Instance { get { return _instance;}}
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Queue<CharacterDialogueLoader.Dialogue> dialogueQueue;

    [SerializeField] private TextMeshProUGUI displayName;
    [SerializeField] private GameObject nameBackground;
    [SerializeField] private TextMeshProUGUI displayDialogue;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private CharacterDialogueLoader[] clickableCharacters;
    [SerializeField] private bool dialogueInProgress = false;
    [SerializeField] private CharacterDialogueLoader nathanSO;
    [SerializeField] private CharacterDialogueLoader drewSO;
    [SerializeField] private bool canGift;

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

#region Speaker Expression Vars
    [SerializeField] private GameObject[] expressionList;
    [SerializeField] private RectTransform displayLocation;
    [SerializeField] private RectTransform hideLocation;
#endregion

#region Saved Variables
    private int playerChoice;
    private CharacterDialogueLoader.Dialogue dialoguePointValues;    
    private CharacterDialogueLoader currentCharacter;
    private int numSkipBefore;
    private int numSkipAfter;
    private int numChoices;
    [SerializeField] private bool sharedScene1Played = false;
    [SerializeField] private bool sharedScene2Played = false;
    private string currentlyPlayingMusic;
    private string currentSpeakerExpression;
    private GameObject currentSpeakerExpressionObject;
    private int elijahChoice;
    public DialogueLoader.Dialogue[] DebugThing;
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
        dialogueQueue = new Queue<CharacterDialogueLoader.Dialogue>();
    }

    private void Start() 
    {
        availableChoices = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Starts and sets up the dialogue system
    public void StartDialogue(CharacterDialogueLoader.Dialogue[] dialogue, CharacterDialogueLoader loader = null, bool isGiftScene = false)
    {
        DebugThing = dialogue;
        //Check to make sure this isnt a shared scene
        //If it is and it has already been played, skip this scene
        if(loader != null && !isGiftScene)
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
        }
        currentCharacter = loader;
        dialogueInProgress = true;
        dialogueBoxAnimator.Play(dialogueBoxReveal);
        dialogueQueue.Clear();
        foreach(CharacterDialogueLoader.Dialogue sentence in dialogue)
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
        CharacterDialogueLoader.Dialogue sentence = dialogueQueue.Dequeue();
        if(sentence.lineSkip > 0)
        {
            sentence = LineSkipHelper(sentence);
            if(sentence == null)
            {
                EndDialogue();
                return;
            }
        }
        if(sentence.scene != null && sentence.scene != "")
        {
            //TODO:: Load scene background
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
        if(displayName.text == null || displayName.text == "")
        {
            nameBackground.SetActive(false);
        }
        if(sentence.sfx != null && sentence.sfx != "")
        {
            //Add sfx code here based on the string sentence.sfx
            AkSoundEngine.PostEvent("Play_" + sentence.sfx.Replace(" ", "_"), this.gameObject);
            
        }
        if(sentence.music != null && sentence.music != "" && sentence.music!=currentlyPlayingMusic && sentence.music!="whicheverMusicHasBeenSetThusFar")
        {
            //@Kristen TODO:: Add music code here based on the string sentence.music
            if(currentlyPlayingMusic != null && currentlyPlayingMusic != "noMusic")
                AkSoundEngine.PostEvent("Stop_"+currentlyPlayingMusic, this.gameObject);
            if (sentence.music != "noMusic")    
                AkSoundEngine.PostEvent("Play_"+sentence.music, this.gameObject);
        
            //stop music using the string var currentlyPlayingMusic
            currentlyPlayingMusic = sentence.music; 
        }
        SpeakerExpressionHelper(sentence.speakerExpression);
        //#ToDo: speaker expression stuff goes here 
        if(sentence.isBranching != null && sentence.isBranching != "")
        {
            numSkipBefore = 0;
            numSkipAfter = 0;
            availableChoices.Clear();
            if(sentence.branchingChoice1 != null && sentence.branchingChoice1 != "")
            {
                availableChoices.Add(sentence.branchingChoice1);
            }
            if(sentence.branchingChoice2 != null && sentence.branchingChoice2 != "")
            {
                availableChoices.Add(sentence.branchingChoice2);
            }
            if(sentence.branchingChoice3 != null && sentence.branchingChoice3 != "")
            {
                availableChoices.Add(sentence.branchingChoice3);
            }
            if(sentence.isBranching == "isPlayerChoice" || sentence.isBranching == "elijahChoice")
            {
                if(availableChoices.Count > 0)
                {
                    continueButton.SetActive(false);                
                    displayDialogue.text += sentence.speakerDialogue;
                    dialoguePointValues = sentence;
                    SetUpChoices(availableChoices);
                }
                else if(sentence.isBranching == "elijahChoice")
                {
                    if(elijahChoice == 0)
                    {
                        numSkipBefore = 0;
                        numSkipAfter = numChoices - 1;
                    }
                    else if(elijahChoice == 1)
                    {
                        numSkipBefore = 1;
                        numSkipAfter = numChoices - 2;
                    }
                    else
                    {
                        numSkipBefore = 2;
                        numSkipAfter = 0;
                    }
                }
            }
        }
        else
        {
            displayDialogue.text += sentence.speakerDialogue;
        }
    }

    public CharacterDialogueLoader.Dialogue LineSkipHelper(CharacterDialogueLoader.Dialogue sentence)
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
    
    public void SpeakerExpressionHelper(string expression)
    {
        if(currentSpeakerExpression == expression)
        {
            return;
        }
        if(currentSpeakerExpressionObject != null)
        {
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = hideLocation.position; 
        }
        currentSpeakerExpression = expression;
        if(expression == "List")
        {
            currentSpeakerExpressionObject = expressionList[0];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "nathanDefault")
        {
            currentSpeakerExpressionObject = expressionList[1];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "nathanHappyGrinning")
        {
            currentSpeakerExpressionObject = expressionList[2];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "nathanHappyLaughing")
        {
            currentSpeakerExpressionObject = expressionList[3];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "nathanSadSmiling")
        {
            currentSpeakerExpressionObject = expressionList[4];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "nathanSad")
        {
            currentSpeakerExpressionObject = expressionList[5];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "nathanWearingShibaEarrings")
        {
            currentSpeakerExpressionObject = expressionList[6];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "rainaNeutralHappyRobot")
        {
            currentSpeakerExpressionObject = expressionList[7];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "rainaNeutralNervous")
        {
            currentSpeakerExpressionObject = expressionList[8];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "rainahappyPassionate")
        {
            currentSpeakerExpressionObject = expressionList[9];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "rainaCrying")
        {
            currentSpeakerExpressionObject = expressionList[10];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "stellaNeutralHappyCute")
        {
            currentSpeakerExpressionObject = expressionList[11];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "stellaCrying")
        {
            currentSpeakerExpressionObject = expressionList[12];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "stellaAngryCute")
        {
            currentSpeakerExpressionObject = expressionList[13];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "stellaExtraHappy")
        {
            currentSpeakerExpressionObject = expressionList[14];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "drewNeutralHappy")
        {
            currentSpeakerExpressionObject = expressionList[15];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "drewVeryHappy")
        {
            currentSpeakerExpressionObject = expressionList[16];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "drewSadScared")
        {
            currentSpeakerExpressionObject = expressionList[17];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "drewCrying")
        {
            currentSpeakerExpressionObject = expressionList[18];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        else if(expression == "drewAngryCute")
        {
            currentSpeakerExpressionObject = expressionList[19];
            currentSpeakerExpressionObject.GetComponent<RectTransform>().position = displayLocation.position;
        }
        //Add more expressions here
        else
        {
            Debug.Log("Missing Expression: " + expression);
        }
    }

    //Ends the dialogue
    public void EndDialogue()
    {
        dialogueCanvas.sortingOrder = 0;
        //Add any following occurences here
        if(canGift)
        {
            canGift = false;
            //Pull up the gifting UI here
            GameManager.Instance.OpenInventory();
        }
        dialogueBoxAnimator.Play(dialogueBoxHide);
        dialogueInProgress = false;
        if(currentCharacter != null && currentCharacter.GetSceneProgression() > currentCharacter.GetCharacterSO().Scenes.Length - 1)
        {
            currentCharacter.LoadDialogue();
        }
    }

    //@Molina TODO:: This should be called by the gifting UI Button
    //Gives an item to a NPC the player is talking to
    public void GiveItem(int itemID)
    {
        if(currentCharacter != null)
        {
            currentCharacter.LoadGiftDialogue(itemID);
        }
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
        if(dialoguePointValues.isBranching == "elijahChoice")
        {
            elijahChoice = choiceNumber;
        }
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
            if(dialoguePointValues.c1nathanpv == 0 && dialoguePointValues.c1drewpv == 0 && currentCharacter != null)
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
            if(dialoguePointValues.c2nathanpv == 0 && dialoguePointValues.c2drewpv == 0 && currentCharacter != null)
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
            if(currentCharacter != null)
            {
                currentCharacter.IncrementCharAffinity(dialoguePointValues.c3pv);
            }
            ContinueDialogue();
        }
    }

    public void ResetAllSceneProgression()
    {
        foreach(CharacterDialogueLoader character in clickableCharacters)
        {
            character.ResetSceneProgression();
        }
    }

    public bool IsDialoguePlaying()
    {
        return dialogueInProgress;
    }

    public void SetCanGift(bool var)
    {
        canGift = var;
    }
}
