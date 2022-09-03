using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager _instance;
    static public GameManager Instance { get { return _instance;}}

    [SerializeField] private int currentGameDay = 0;
    [SerializeField] private int maxGameDays;
    [SerializeField] private int numConversations;
    [SerializeField] private DialogueLoader genericDialogueLoader;
    
    [System.Serializable]
    public class CharacterPositioning
    {
        public GameObject character;
        public Transform[] characterPositions;
    }
    [SerializeField] private List<CharacterPositioning> availableCharacters;
    [SerializeField] private DialogueLoader.Dialogue[] warningLoader;
    [SerializeField] private GameObject shopOrderUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private bool menuOpen;
    // Start is called before the first frame update
    void Awake()
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
    
    void Start()
    {
        LoadShopDay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //There should be a button that the player can press to end the game
    public void EndDay()
    {
        ItemManager.Instance.AddSalary();
        if(!DialogueManager.Instance.IsDialoguePlaying() && !menuOpen)
        {
            if(currentGameDay == 0)
            {
                dialogueCanvas.sortingOrder = 1;
                genericDialogueLoader.LoadDialogue();
            }
            currentGameDay ++;
            if(currentGameDay >= maxGameDays)
            {
                genericDialogueLoader.LoadDialogue();
            }
            else
            {
                menuOpen = true;
                LoadShopDay();
            }
        }
    }

    //TODO: This function should load the available characters and position them in the scene 
    public void LoadShopDay()
    {
        UpdateAvailableCharacters();
        for(int i = 0; i < availableCharacters.Count; i++)
        {
            CharacterPositioning positioningData = availableCharacters[i];
            positioningData.character.GetComponent<CharacterDialogueLoader>().SetTalk(true);
            positioningData.character.transform.position = positioningData.characterPositions[currentGameDay].position;
            positioningData.character.transform.rotation = positioningData.characterPositions[currentGameDay].rotation;
        }
        if(currentGameDay == 0)
        {
            numConversations = 4;
            genericDialogueLoader.LoadDialogue();
        }
        else if(currentGameDay > 0)
        {
            numConversations = 2;
            //load night order UI here
            shopOrderUI.SetActive(true);
            ItemManager.Instance.UpdateShopInventory();
        }
    }

    public void UpdateAvailableCharacters()
    {
        warningLoader = new DialogueLoader.Dialogue[0];
        for(int i = availableCharacters.Count - 1; i >= 0; i--)
        {
            CharacterDialogueLoader currentCharacter = availableCharacters[i].character.GetComponent<CharacterDialogueLoader>();
            currentCharacter.ToggleClickableObject(true);
            CharacterScriptableObject currentCharacterSO = currentCharacter.GetCharacterSO();
            int scenesLeft = currentCharacterSO.Scenes.Length - currentCharacterSO.SceneProgression;
            Debug.Log("Scenes Left: " + scenesLeft);
            int daysLeft = maxGameDays - currentGameDay;
            Debug.Log("Days Left: " + daysLeft);
            if(scenesLeft == daysLeft)
            {
                //Insert Warning info here
                DialogueLoader.DialogueList currentWarning = currentCharacter.GetWarningScene();
                DialogueLoader.Dialogue[] concatArray = new DialogueLoader.Dialogue[warningLoader.Length + currentWarning.dialogue.Length];
                warningLoader.CopyTo(concatArray, 0);
                currentWarning.dialogue.CopyTo(concatArray, warningLoader.Length);
                warningLoader = concatArray;
                DialogueManager.Instance.StartDialogue(warningLoader);
            }
            else if(scenesLeft > daysLeft)
            {
                availableCharacters[i].character.SetActive(false);
                availableCharacters.RemoveAt(i);
            }
        }
    }

    public void HideCharacters()
    {
        for(int i = availableCharacters.Count - 1; i >= 0; i--)
        {
            CharacterDialogueLoader currentCharacter = availableCharacters[i].character.GetComponent<CharacterDialogueLoader>();
            currentCharacter.ToggleClickableObject(false);
        }
    }

    public bool IsMenuOpen()
    {
        return menuOpen;
    }

    public void CloseUI()
    {
        shopOrderUI.SetActive(false);
        inventoryUI.SetActive(false);
        menuOpen = false;
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);
        ItemManager.Instance.UpdateInventory();
        menuOpen = true;
    }

    public bool ConversationAvailable()
    {
        return numConversations > 0;
    }

    public void ReduceNumConversations()
    {
        numConversations--;
    }
}
