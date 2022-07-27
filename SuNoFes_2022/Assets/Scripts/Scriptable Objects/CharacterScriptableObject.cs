using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Char_Name", menuName = "ScriptableObjects/CharObject", order = 2)]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField] private int _characterAffinity;
    [SerializeField] private int _sceneProgression;
    [SerializeField] private DialogueLoader[] _scenes;
    
    [System.Serializable]
    public class expressionDict
    {
        public string expressionKey;
        public GameObject expressionValue;
    }
    [SerializeField] private expressionDict[] _characterExpressions;
    //TODO: add a list of items the character likes
    //TODO: add a list of items the character needs for story progression
    
    public int CharacterAffinity
    {
        get {return _characterAffinity;}
        set {_characterAffinity = value;}
    }
    public int SceneProgression
    {
        get {return _sceneProgression;}
        set {_sceneProgression = value;}
    }
    public DialogueLoader[] Scenes
    {
        get {return _scenes;}
    }
    public expressionDict[] CharacterExpressions
    {
        get {return _characterExpressions;}
    }
}
